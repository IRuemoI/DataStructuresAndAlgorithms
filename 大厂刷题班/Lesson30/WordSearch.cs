//pass

namespace AdvancedTraining.Lesson30;

public class WordSearch //leetcode_0079
{
    private static bool Exist(char[][] board, string word)
    {
        var w = word.ToCharArray();
        for (var i = 0; i < board.Length; i++)
        for (var j = 0; j < board[0].Length; j++)
            if (F(board, i, j, w, 0))
                return true;
        return false;
    }

    // 目前到达了b[i][j]，word[k....]
    // 从b[i][j]出发，能不能搞定word[k....] true false
    private static bool F(char[][] b, int i, int j, char[] w, int k)
    {
        if (k == w.Length) return true;
        // word[k.....] 有字符
        // 如果(i,j)越界，返回false
        if (i < 0 || i == b.Length || j < 0 || j == b[0].Length) return false;
        if (b[i][j] != w[k]) return false;
        var tmp = b[i][j];
        b[i][j] = (char)0;
        var ans = F(b, i - 1, j, w, k + 1) || F(b, i + 1, j, w, k + 1) || F(b, i, j - 1, w, k + 1) ||
                  F(b, i, j + 1, w, k + 1);
        b[i][j] = tmp;
        return ans;
    }

    public static void Run()
    {
        Console.WriteLine(Exist([['A', 'B', 'C', 'E'], ['S', 'F', 'C', 'S'], ['A', 'D', 'E', 'E']],
            "ABCCED")); // 输出：true
    }
}