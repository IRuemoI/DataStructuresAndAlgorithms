//pass
namespace AdvancedTraining.Lesson26;

// 本题测试链接 : https://leetcode.cn/problems/word-search-ii/
public class WordSearchIi
{
    private static void FillWord(TrieNode head, string word)
    {
        head.Pass++;
        var chs = word.ToCharArray();
        var node = head;
        foreach (var item in chs)
        {
            var index = item - 'a';
            if (node != null)
            {
                node.NextList[index] ??= new TrieNode();
                node = node.NextList[index];
                if (node != null) node.Pass++;
            }
        }

        if (node != null) node.End = true;
    }

    private static string GeneratePath(LinkedList<char> path)
    {
        var str = new char[path.Count];
        var index = 0;
        foreach (char? cha in path) str[index++] = cha.Value;
        return new string(str);
    }

    private static IList<string> FindWords(char[][] board, string[] words)
    {
        var head = new TrieNode(); // 前缀树最顶端的头
        var set = new HashSet<string>();
        foreach (var word in words)
            if (!set.Contains(word))
            {
                FillWord(head, word);
                set.Add(word);
            }

        // 答案
        IList<string> ans = new List<string>();
        // 沿途走过的字符，收集起来，存在path里
        var path = new LinkedList<char>();
        for (var row = 0; row < board.Length; row++)
        for (var col = 0; col < board[0].Length; col++)
            // 枚举在board中的所有位置
            // 每一个位置出发的情况下，答案都收集
            Process(board, row, col, path, head, ans);
        return ans;
    }

    // 从board[row][col]位置的字符出发，
    // 之前的路径上，走过的字符，记录在path里
    // cur还没有登上，有待检查能不能登上去的前缀树的节点
    // 如果找到words中的某个str，就记录在 res里
    // 返回值，从row,col 出发，一共找到了多少个str
    private static int Process(char[][] board, int row, int col, LinkedList<char> path, TrieNode? cur,
        IList<string> res)
    {
        var cha = board[row][col];
        if (cha == (char)0)
            // 这个row col位置是之前走过的位置
            return 0;
        // (row,col) 不是回头路 cha 有效

        var index = cha - 'a';
        // 如果没路，或者这条路上最终的字符串之前加入过结果里
        if (cur?.NextList[index] == null || cur.NextList[index]!.Pass == 0) return 0;
        // 没有走回头路且能登上去
        cur = cur.NextList[index];
        path.AddLast(cha); // 当前位置的字符加到路径里去
        var fix = 0; // 从row和col位置出发，后续一共搞定了多少答案
        // 当我来到row col位置，如果决定不往后走了。是不是已经搞定了某个字符串了
        if (cur is { End: true })
        {
            res.Add(GeneratePath(path));
            cur.End = false;
            fix++;
        }

        // 往上、下、左、右，四个方向尝试
        board[row][col] = (char)0;
        if (row > 0) fix += Process(board, row - 1, col, path, cur, res);
        if (row < board.Length - 1) fix += Process(board, row + 1, col, path, cur, res);
        if (col > 0) fix += Process(board, row, col - 1, path, cur, res);
        if (col < board[0].Length - 1) fix += Process(board, row, col + 1, path, cur, res);
        board[row][col] = cha;
        path.RemoveLast();
        if (cur != null) cur.Pass -= fix;
        return fix;
    }

    public static void Run()
    {
        char[][] board = [['o', 'a', 'a', 'n'], ['e', 't', 'a', 'e'], ['i', 'h', 'k', 'r'], ['i', 'f', 'l', 'v']];
        string[] words = ["oath", "pea", "eat", "rain"];
        foreach (var item in FindWords(board, words)) Console.Write(item + ","); //输出：["eat","oath"]
    }

    public class TrieNode
    {
        public readonly TrieNode?[] NextList = new TrieNode[26];
        public bool End;
        public int Pass;
    }
}