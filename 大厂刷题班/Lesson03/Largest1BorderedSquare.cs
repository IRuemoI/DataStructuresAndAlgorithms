namespace AdvancedTraining.Lesson03;

// 本题测试链接 : https://leetcode.cn/problems/largest-1-bordered-square/
public class Largest1BorderedSquare
{
    private static int Code(int[,] m)
    {
        var right = new int[m.GetLength(0), m.GetLength(1)];
        var down = new int[m.GetLength(0), m.GetLength(1)];
        SetBorderMap(m, right, down);
        for (var size = Math.Min(m.GetLength(0), m.GetLength(1)); size != 0; size--)
            if (HasSizeOfBorder(size, right, down))
                return size * size;

        return 0;
    }

    private static void SetBorderMap(int[,] m, int[,] right, int[,] down)
    {
        var r = m.GetLength(0);
        var c = m.GetLength(1);
        if (m[r - 1, c - 1] == 1)
        {
            right[r - 1, c - 1] = 1;
            down[r - 1, c - 1] = 1;
        }

        for (var i = r - 2; i != -1; i--)
            if (m[i, c - 1] == 1)
            {
                right[i, c - 1] = 1;
                down[i, c - 1] = down[i + 1, c - 1] + 1;
            }

        for (var i = c - 2; i != -1; i--)
            if (m[r - 1, i] == 1)
            {
                right[r - 1, i] = right[r - 1, i + 1] + 1;
                down[r - 1, i] = 1;
            }

        for (var i = r - 2; i != -1; i--)
        for (var j = c - 2; j != -1; j--)
            if (m[i, j] == 1)
            {
                right[i, j] = right[i, j + 1] + 1;
                down[i, j] = down[i + 1, j] + 1;
            }
    }

    private static bool HasSizeOfBorder(int size, int[,] right, int[,] down)
    {
        for (var i = 0; i != right.GetLength(0) - size + 1; i++)
        for (var j = 0; j != right.GetLength(1) - size + 1; j++)
            if (right[i, j] >= size && down[i, j] >= size && right[i + size - 1, j] >= size &&
                down[i, j + size - 1] >= size)
                return true;

        return false;
    }

    public static void Run()
    {
        int[,] m =
        {
            { 1, 1, 1 },
            { 1, 0, 1 },
            { 1, 1, 1 }
        };
        Console.WriteLine(Code(m)); //输出9
    }
}