//通过

namespace Algorithms.Lesson26;

public class ZeroLeftOneStringNumber
{
    private static int GetNum1(int n)
    {
        if (n < 1) return 0;

        return Process(1, n);
    }

    private static int Process(int i, int n)
    {
        if (i == n - 1) return 2;

        if (i == n) return 1;

        return Process(i + 1, n) + Process(i + 2, n);
    }

    private static int GetNum2(int n)
    {
        if (n < 1) return 0;

        if (n == 1) return 1;

        var pre = 1;
        var cur = 1;
        for (var i = 2; i < n + 1; i++)
        {
            var tmp = cur;
            cur += pre;
            pre = tmp;
        }

        return cur;
    }

    private static int GetNum3(int n)
    {
        if (n < 1) return 0;

        if (n is 1 or 2) return n;

        int[,] base1 = { { 1, 1 }, { 1, 0 } };
        var res = MatrixPower(base1, n - 2);
        return 2 * res[0, 0] + res[1, 0];
    }


    private static int Fi(int n)
    {
        if (n < 1) return 0;

        if (n is 1 or 2) return 1;

        int[,] base1 =
        {
            { 1, 1 },
            { 1, 0 }
        };
        var res = MatrixPower(base1, n - 2);
        return res[0, 0] + res[1, 0];
    }


    private static int[,] MatrixPower(int[,] m, int p)
    {
        var res = new int[m.GetLength(0), m.GetLength(1)];
        for (var i = 0; i < res.GetLength(0); i++) res[i, i] = 1;

        var tmp = m;
        for (; p != 0; p >>= 1)
        {
            if ((p & 1) != 0) res = MultiplyMatrix(res, tmp);

            tmp = MultiplyMatrix(tmp, tmp);
        }

        return res;
    }

    private static int[,] MultiplyMatrix(int[,] m1, int[,] m2)
    {
        var res = new int[m1.GetLength(0), m2.GetLength(1)];
        for (var i = 0; i < m1.GetLength(0); i++)
        for (var j = 0; j < m2.GetLength(1); j++)
        for (var k = 0; k < m2.GetLength(0); k++)
            res[i, j] += m1[i, k] * m2[k, j];

        return res;
    }

    public static void Run()
    {
        for (var i = 0; i != 20; i++)
        {
            Console.WriteLine(GetNum1(i));
            Console.WriteLine(GetNum2(i));
            Console.WriteLine(GetNum3(i));
            Console.WriteLine("===================");
        }
    }
}