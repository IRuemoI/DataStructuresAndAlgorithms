namespace AdvancedTraining.Lesson31;

public class SurroundedRegions //Problem_0130
{
    //	// m -> 二维数组， 不是0就是1
    //	//
    //	private static void Infect(int[][] m, int i, int j) {
    //		if (i < 0 || i == m.length || j < 0 || j == m[0].length || m[i][j] != 1) {
    //			return;
    //		}
    //		// m[i][j] == 1
    //		m[i][j] = 2;
    //		Infect(m, i - 1, j);
    //		Infect(m, i + 1, j);
    //		Infect(m, i, j - 1);
    //		Infect(m, i, j + 1);
    //	}

    private static void Solve1(char[][] board)
    {
        var ans = new bool[1];
        for (var i = 0; i < board.Length; i++)
        for (var j = 0; j < board[0].Length; j++)
            if (board[i][j] == 'O')
            {
                ans[0] = true;
                Can(board, i, j, ans);
                board[i][j] = ans[0] ? 'T' : 'F';
            }

        for (var i = 0; i < board.Length; i++)
        for (var j = 0; j < board[0].Length; j++)
        {
            var can = board[i][j];
            if (can == 'T' || can == 'F')
            {
                board[i][j] = '.';
                Change(board, i, j, can);
            }
        }
    }

    private static void Can(char[][] board, int i, int j, bool[] ans)
    {
        if (i < 0 || i == board.Length || j < 0 || j == board[0].Length)
        {
            ans[0] = false;
            return;
        }

        if (board[i][j] == 'O')
        {
            board[i][j] = '.';
            Can(board, i - 1, j, ans);
            Can(board, i + 1, j, ans);
            Can(board, i, j - 1, ans);
            Can(board, i, j + 1, ans);
        }
    }

    private static void Change(char[][] board, int i, int j, char can)
    {
        if (i < 0 || i == board.Length || j < 0 || j == board[0].Length) return;
        if (board[i][j] == '.')
        {
            board[i][j] = can == 'T' ? 'X' : 'O';
            Change(board, i - 1, j, can);
            Change(board, i + 1, j, can);
            Change(board, i, j - 1, can);
            Change(board, i, j + 1, can);
        }
    }

    // 从边界开始感染的方法，比第一种方法更好
    private static void Solve2(char[][]? board)
    {
        if (board == null || board.Length == 0 || board[0] == null || board[0].Length == 0) return;
        var n = board.Length;
        var m = board[0].Length;
        for (var j = 0; j < m; j++)
        {
            if (board[0][j] == 'O') Free(board, 0, j);
            if (board[n - 1][j] == 'O') Free(board, n - 1, j);
        }

        for (var i = 1; i < n - 1; i++)
        {
            if (board[i][0] == 'O') Free(board, i, 0);
            if (board[i][m - 1] == 'O') Free(board, i, m - 1);
        }

        for (var i = 0; i < n; i++)
        for (var j = 0; j < m; j++)
        {
            if (board[i][j] == 'O') board[i][j] = 'X';
            if (board[i][j] == 'F') board[i][j] = 'O';
        }
    }

    private static void Free(char[][] board, int i, int j)
    {
        if (i < 0 || i == board.Length || j < 0 || j == board[0].Length || board[i][j] != 'O') return;
        board[i][j] = 'F';
        Free(board, i + 1, j);
        Free(board, i - 1, j);
        Free(board, i, j + 1);
        Free(board, i, j - 1);
    }
}