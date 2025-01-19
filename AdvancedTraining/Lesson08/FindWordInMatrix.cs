namespace AdvancedTraining.Lesson08;

/*
 * 给定一个char[][] matrix，也就是char类型的二维数组，再给定一个字符串word，
 * 可以从任何一个某个位置出发，可以走上下左右，能不能找到word？
 * 比如：
 * char[][] m = {
 *     { 'a', 'b', 'z' },
 *     { 'c', 'd', 'o' },
 *     { 'f', 'e', 'o' },
 * };
 * word = "zooe"
 * 是可以找到的
 *
 * 设定1：可以走重复路的情况下，返回能不能找到
 * 比如，word = "zoooz"，是可以找到的，z -> o -> o -> o -> z，因为允许走一条路径中已经走过的字符
 *
 * 设定2：不可以走重复路的情况下，返回能不能找到
 * 比如，word = "zoooz"，是不可以找到的，因为允许走一条路径中已经走过的字符不能重复走
 *
 * 写出两种设定下的code
 *
 * */
//todo:待修复
public class FindWordInMatrix
{
    // 可以走重复的设定
    private static bool FindWord1(char[]?[]? m, string word)
    {
        if (word is null or "") return true;

        if (m == null || m.Length == 0 || m[0] == null || m[0].Length == 0) return false;

        var w = word.ToCharArray();
        var row = m.GetLength(0);
        var column = m.GetLength(1);
        var len = w.Length;
        // dp[i][j][k]表示：必须以m[i][j]这个字符结尾的情况下，能不能找到w[0...k]这个前缀串
        // ORIGINAL LINE: bool[][][] dp = new bool[N][M][len];
        var dp = new bool[row][][];

        for (var i = 0; i < row; i++)
        {
            dp[i] = new bool[column][];
            for (var j = 0; j < column; j++)
            {
                dp[i][j] = new bool[len];
                for (var k = 0; k < len; k++) dp[i][j][k] = false;
            }
        }


        for (var i = 0; i < row; i++)
        for (var j = 0; j < column; j++)
            dp[i][j][0] = m[i][j] == w[0];

        for (var k = 1; k < len; k++)
        for (var i = 0; i < row; i++)
        for (var j = 0; j < column; j++)
            dp[i][j][k] = m[i][j] == w[k] && CheckPrevious(dp, i, j, k);

        for (var i = 0; i < row; i++)
        for (var j = 0; j < column; j++)
            if (dp[i][j][len - 1])
                return true;

        return false;
    }

    // 可以走重复路
    // 从m[i][j]这个字符出发，能不能找到str[k...]这个后缀串
    private static bool CanLoop(char[][] m, int i, int j, char[] str, int k)
    {
        if (k == str.Length) return true;

        if (i == -1 || i == m.Length || j == -1 || j == m[0].Length || m[i][j] != str[k]) return false;

        // 不越界！m[i][j] == str[k] 对的上的！
        // str[k+1....]
        var ans = CanLoop(m, i + 1, j, str, k + 1) || CanLoop(m, i - 1, j, str, k + 1) ||
                  CanLoop(m, i, j + 1, str, k + 1) || CanLoop(m, i, j - 1, str, k + 1);

        return ans;
    }

    // 不能走重复路
    // 从m[i][j]这个字符出发，能不能找到str[k...]这个后缀串
    private static bool NoLoop(char[][] m, int i, int j, char[] str, int k)
    {
        if (k == str.Length) return true;

        if (i == -1 || i == m.Length || j == -1 || j == m[0].Length || m[i][j] != str[k]) return false;

        // 不越界！也不是回头路！m[i][j] == str[k] 也对的上！
        m[i][j] = (char)0;
        var ans = NoLoop(m, i + 1, j, str, k + 1) || NoLoop(m, i - 1, j, str, k + 1) ||
                  NoLoop(m, i, j + 1, str, k + 1) || NoLoop(m, i, j - 1, str, k + 1);

        m[i][j] = str[k];
        return ans;
    }

    private static bool CheckPrevious(bool[][][] dp, int i, int j, int k)
    {
        var up = i > 0 && dp[i - 1][j][k - 1];
        var down = i < dp.Length - 1 && dp[i + 1][j][k - 1];
        var left = j > 0 && dp[i][j - 1][k - 1];
        var right = j < dp[0].Length - 1 && dp[i][j + 1][k - 1];
        return up || down || left || right;
    }

    // 不可以走重复路的设定
    private static bool FindWord2(char[][] m, string word)
    {
        if (ReferenceEquals(word, null) || word.Equals("")) return true;

        if (m == null || m.Length == 0 || m[0] == null || m[0].Length == 0) return false;

        var w = word.ToCharArray();
        for (var i = 0; i < m.Length; i++)
        for (var j = 0; j < m[0].Length; j++)
            if (Process(m, i, j, w, 0))
                return true;

        return false;
    }

    // 从m[i][j]这个字符出发，能不能找到w[k...]这个后缀串
    private static bool Process(char[][] m, int i, int j, char[] str, int k)
    {
        if (k == str.Length) return true;

        if (i == -1 || i == m.Length || j == -1 || j == m[0].Length || m[i][j] == (char)0 ||
            m[i][j] != str[k]) return false;

        m[i][j] = (char)0;
        var ans = Process(m, i + 1, j, str, k + 1) || Process(m, i - 1, j, str, k + 1) ||
                  Process(m, i, j + 1, str, k + 1) || Process(m, i, j - 1, str, k + 1);

        m[i][j] = str[k];
        return ans;
    }

    public static void Run()
    {
        char[][] m =
        [
            ['a', 'b', 'z'],
            ['c', 'd', 'o'],
            ['f', 'e', 'o']
        ];
        var word1 = "zoooz";
        var word2 = "zoo";
        // 可以走重复路的设定
        Console.WriteLine(FindWord1(m, word1));
        Console.WriteLine(FindWord1(m, word2));
        // 不可以走重复路的设定
        Console.WriteLine(FindWord2(m, word1));
        Console.WriteLine(FindWord2(m, word2));
    }
}