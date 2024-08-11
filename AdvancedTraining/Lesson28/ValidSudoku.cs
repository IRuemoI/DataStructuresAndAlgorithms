namespace AdvancedTraining.Lesson28;

public class ValidSudoku //Problem_0036
{
    private static bool IsValidSudoku(char[,] board)
    {
        var row = new bool[9, 10];
        var col = new bool[9, 10];
        var bucket = new bool[9, 10];
        for (var i = 0; i < 9; i++)
        for (var j = 0; j < 9; j++)
        {
            var bid = 3 * (i / 3) + j / 3;
            if (board[i, j] != '.')
            {
                var num = board[i, j] - '0';
                if (row[i, num] || col[j, num] || bucket[bid, num]) return false;
                row[i, num] = true;
                col[j, num] = true;
                bucket[bid, num] = true;
            }
        }

        return true;
    }

    public static void Run()
    {
        char[,] board =
        {
            { '5', '3', '.', '.', '7', '.', '.', '.', '.' },
            { '6', '.', '.', '1', '9', '5', '.', '.', '.' },
            { '.', '9', '8', '.', '.', '.', '.', '6', '.' },
            { '8', '.', '.', '.', '6', '.', '.', '.', '3' },
            { '4', '.', '.', '8', '.', '3', '.', '.', '1' },
            { '7', '.', '.', '.', '2', '.', '.', '.', '6' },
            { '.', '6', '.', '.', '.', '.', '2', '8', '.' },
            { '.', '.', '.', '4', '1', '9', '.', '.', '5' },
            { '.', '.', '.', '.', '8', '.', '.', '7', '9' }
        };

        Console.WriteLine(IsValidSudoku(board)); //输出true
    }
}