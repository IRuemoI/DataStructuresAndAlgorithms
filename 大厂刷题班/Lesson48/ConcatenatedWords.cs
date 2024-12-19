namespace AdvancedTraining.Lesson48;

//todo:待整理
public class ConcatenatedWords //leetcode_0472
{
    // 提前准备好动态规划表
    private static readonly int[] Dp = new int[1000];

    private static void Insert(TrieNode? root, char[] s)
    {
        if (root == null) throw new Exception();
        foreach (var c in s)
        {
            var path = c - 'a';
            if (root.NextList[path] == null) root.NextList[path] = new TrieNode();
            root = root.NextList[path];
        }

        root.End = true;
    }

    // 方法1：前缀树优化
    private static IList<string> findAllConcatenatedWordsInADict1(string[]? words)
    {
        IList<string> ans = new List<string>();
        if (words == null || words.Length < 3) return ans;
        // 字符串数量 >= 3个
        Array.Sort(words, (str1, str2) => str1.Length - str2.Length);
        var root = new TrieNode();
        foreach (var str in words)
        {
            var s = str.ToCharArray(); // "" 题目要求
            if (s.Length > 0 && Split1(s, root, 0))
                ans.Add(str);
            else
                Insert(root, s);
        }

        return ans;
    }

    // 字符串s[i....]能不能被分解？
    // 之前的元件，全在前缀树上，r就是前缀树头节点
    private static bool Split1(char[] s, TrieNode r, int i)
    {
        var ans = false;
        if (i == s.Length)
        {
            // 没字符了！
            ans = true;
        }
        else
        {
            // 还有字符
            var c = r;
            // s[i.....]
            // s[i..end]作前缀，看看是不是一个元件！f(end+1)...
            for (var end = i; end < s.Length; end++)
            {
                var path = s[end] - 'a';
                if (c.NextList[path] == null) break;
                c = c.NextList[path];
                if (c.End && Split1(s, r, end + 1))
                {
                    ans = true;
                    break;
                }
            }
        }

        return ans;
    }

    // 方法二：前缀树优化 + 动态规划优化
    private static IList<string> findAllConcatenatedWordsInADict2(string[]? words)
    {
        IList<string> ans = new List<string>();
        if (words == null || words.Length < 3) return ans;
        Array.Sort(words, (str1, str2) => str1.Length - str2.Length);
        var root = new TrieNode();
        foreach (var str in words)
        {
            var s = str.ToCharArray();
            Array.Fill(Dp, 0, s.Length + 1, 0);
            if (s.Length > 0 && Split2(s, root, 0, Dp))
                ans.Add(str);
            else
                Insert(root, s);
        }

        return ans;
    }

    private static bool Split2(char[] s, TrieNode r, int i, int[] dp)
    {
        if (dp[i] != 0) return dp[i] == 1;
        var ans = false;
        if (i == s.Length)
        {
            ans = true;
        }
        else
        {
            var c = r;
            for (var end = i; end < s.Length; end++)
            {
                var path = s[end] - 'a';
                if (c.NextList[path] == null) break;
                c = c.NextList[path];
                if (c.End && Split2(s, r, end + 1, dp))
                {
                    ans = true;
                    break;
                }
            }
        }

        dp[i] = ans ? 1 : -1;
        return ans;
    }

    public class TrieNode
    {
        public readonly TrieNode?[] NextList = new TrieNode[26];
        public bool End;
    }
}