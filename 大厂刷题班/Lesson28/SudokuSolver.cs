//pass
namespace AdvancedTraining.Lesson28;

public class SudokuSolver //leetcode_0037
{
    private static void SolveSudoku(char[][] board)
    {
        var row = new bool[9, 10];
        var col = new bool[9, 10];
        var bucket = new bool[9, 10];
        InitMaps(board, row, col, bucket);
        Process(board, 0, 0, row, col, bucket);
    }

    private static void InitMaps(char[][] board, bool[,] row, bool[,] col, bool[,] bucket)
    {
        for (var i = 0; i < 9; i++)
            for (var j = 0; j < 9; j++)
            {
                var bid = 3 * (i / 3) + j / 3;
                if (board[i][j] != '.')
                {
                    var num = board[i][j] - '0';
                    row[i, num] = true;
                    col[j, num] = true;
                    bucket[bid, num] = true;
                }
            }
    }

    //  当前来到(i,j)这个位置，如果已经有数字，跳到下一个位置上
    //                      如果没有数字，尝试1~9，不能和row、col、bucket冲突
    private static bool Process(char[][] board, int i, int j, bool[,] row, bool[,] col, bool[,] bucket)
    {
        if (i == 9) return true;
        // 当离开(i，j)，应该去哪？(nexti, nextj)
        var nexti = j != 8 ? i : i + 1;
        var nextj = j != 8 ? j + 1 : 0;
        if (board[i][j] != '.') return Process(board, nexti, nextj, row, col, bucket);

        // 可以尝试1~9
        var bid = 3 * (i / 3) + j / 3;
        for (var num = 1; num <= 9; num++)
            // 尝试每一个数字1~9
            if (!row[i, num] && !col[j, num] && !bucket[bid, num])
            {
                // 可以尝试num
                row[i, num] = true;
                col[j, num] = true;
                bucket[bid, num] = true;
                board[i][j] = (char)(num + '0');
                if (Process(board, nexti, nextj, row, col, bucket)) return true;
                row[i, num] = false;
                col[j, num] = false;
                bucket[bid, num] = false;
                board[i][j] = '.';
            }

        return false;
    }

    public static void Run()
    {
        char[][] board =
        {
            [ '5', '3', '.', '.', '7', '.', '.', '.', '.' ],
            [ '6', '.', '.', '1', '9', '5', '.', '.', '.' ],
            [ '.', '9', '8', '.', '.', '.', '.', '6', '.' ],
            [ '8', '.', '.', '.', '6', '.', '.', '.', '3' ],
            [ '4', '.', '.', '8', '.', '3', '.', '.', '1' ],
            [ '7', '.', '.', '.', '2', '.', '.', '.', '6' ],
            [ '.', '6', '.', '.', '.', '.', '2', '8', '.' ],
            [ '.', '.', '.', '4', '1', '9', '.', '.', '5' ],
            [ '.', '.', '.', '.', '8', '.', '.', '7', '9' ]
        };
        SolveSudoku(board);
        for (var i = 0; i < board.GetLength(0); i++)
        {
            for (var j = 0; j < board.GetLength(1); j++) Console.Write(board[i][j] + ",");

            Console.WriteLine();
        }

        //输出
        // [
        // ["5","3","4","6","7","8","9","1","2"],
        // ["6","7","2","1","9","5","3","4","8"],
        // ["1","9","8","3","4","2","5","6","7"],
        // ["8","5","9","7","6","1","4","2","3"],
        // ["4","2","6","8","5","3","7","9","1"],
        // ["7","1","3","9","2","4","8","5","6"],
        // ["9","6","1","5","3","7","2","8","4"],
        // ["2","8","7","4","1","9","6","3","5"],
        // ["3","4","5","2","8","6","1","7","9"]]
    }
}