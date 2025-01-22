namespace AdvancedTraining.Lesson34;

// https://leetcode.cn/problems/game-of-life/description/

// 有关这个游戏更有意思、更完整的内容：
// https://www.bilibili.com/video/BV1rJ411n7ri
// 也推荐这个up主

//pass
public class GameOfLife //leetcode_0289
{
    private static void GameOfLifeCode(int[][] board)
    {
        var n = board.Length;
        var m = board[0].Length;
        for (var i = 0; i < n; i++)
        for (var j = 0; j < m; j++)
        {
            var neighbors = Neighbors(board, i, j);
            if (neighbors == 3 || (board[i][j] == 1 && neighbors == 2)) board[i][j] |= 2;
        }

        for (var i = 0; i < n; i++)
        for (var j = 0; j < m; j++)
            board[i][j] >>= 1;
    }

    // b[i][j] 这个位置的数，周围有几个1
    private static int Neighbors(int[][] b, int i, int j)
    {
        return F(b, i - 1, j - 1) + F(b, i - 1, j) + F(b, i - 1, j + 1) + F(b, i, j - 1) + F(b, i, j + 1) +
               F(b, i + 1, j - 1) + F(b, i + 1, j) + F(b, i + 1, j + 1);
    }

    // b[i][j] 上面有1，就返回1，上面不是1，就返回0
    private static int F(int[][] b, int i, int j)
    {
        return i >= 0 && i < b.Length && j >= 0 && j < b[0].Length && (b[i][j] & 1) == 1 ? 1 : 0;
    }

    public static void Run()
    {
        int[][] board = [[0, 1, 0], [0, 0, 1], [1, 1, 1], [0, 0, 0]];
        GameOfLifeCode(board);
        foreach (var row in board) Console.WriteLine(string.Join(",", row)); // 输出[[0,0,0],[1,0,1],[0,1,1],[0,1,0]]
    }
}