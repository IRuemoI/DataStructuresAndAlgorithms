namespace AdvancedTraining.Lesson29;

public class SetMatrixZeroes //Problem_0073
{
    private static int[][] Zeroes1(int[][] value)
    {
        var row0Zero = false;
        var col0Zero = false;
        int i;
        int j;
        for (i = 0; i < value[0].Length; i++)
            if (value[0][i] == 0)
            {
                row0Zero = true;
                break;
            }

        for (i = 0; i < value.Length; i++)
            if (value[i][0] == 0)
            {
                col0Zero = true;
                break;
            }

        for (i = 1; i < value.Length; i++)
        for (j = 1; j < value[0].Length; j++)
            if (value[i][j] == 0)
            {
                value[i][0] = 0;
                value[0][j] = 0;
            }

        for (i = 1; i < value.Length; i++)
        for (j = 1; j < value[0].Length; j++)
            if (value[i][0] == 0 || value[0][j] == 0)
                value[i][j] = 0;
        if (row0Zero)
            for (i = 0; i < value[0].Length; i++)
                value[0][i] = 0;
        if (col0Zero)
            for (i = 0; i < value.Length; i++)
                value[i][0] = 0;
        return value;
    }

    private static int[][] Zeroes2(int[][] value)
    {
        var col0 = false;
        int i;
        int j;
        for (i = 0; i < value.Length; i++)
        for (j = 0; j < value[0].Length; j++)
            if (value[i][j] == 0)
            {
                value[i][0] = 0;
                if (j == 0)
                    col0 = true;
                else
                    value[0][j] = 0;
            }

        for (i = value.Length - 1; i >= 0; i--)
        for (j = 1; j < value[0].Length; j++)
            if (value[i][0] == 0 || value[0][j] == 0)
                value[i][j] = 0;
        if (col0)
            for (i = 0; i < value.Length; i++)
                value[i][0] = 0;
        return value;
    }

    public static void Run()
    {
        foreach (var row in Zeroes1([[1, 1, 1], [1, 0, 1], [1, 1, 1]]))
        {
            foreach (var item in row) Console.Write(item + ","); //输出：[[1,0,1],[0,0,0],[1,0,1]]

            Console.WriteLine();
        }

        Console.WriteLine("--------------------------");
        foreach (var row in Zeroes2([[1, 1, 1], [1, 0, 1], [1, 1, 1]]))
        {
            foreach (var item in row) Console.Write(item + ","); //输出：[[1,0,1],[0,0,0],[1,0,1]]

            Console.WriteLine();
        }
    }
}