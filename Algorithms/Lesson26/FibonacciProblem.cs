//测试通过

namespace Algorithms.Lesson26;

public class FibonacciProblem
{
    private static int F1(int n)
    {
        if (n < 1) return 0;

        if (n == 1 || n == 2) return 1;

        return F1(n - 1) + F1(n - 2);
    }

    private static int F2(int n)
    {
        if (n < 1) return 0;

        if (n == 1 || n == 2) return 1;

        var res = 1;
        var pre = 1;
        for (var i = 3; i <= n; i++)
        {
            var temp = res;
            res += pre;
            pre = temp;
        }

        return res;
    }

    // O(logN)
    private static int F3(int n)
    {
        if (n < 1) return 0;

        if (n == 1 || n == 2) return 1;

        // [ 1 ,1 ]
        // [ 1, 0 ]
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

        // res = 矩阵中的1
        var t = m; // 矩阵1次方
        for (; p != 0; p >>= 1)
        {
            if ((p & 1) != 0) res = MultipleMatrix(res, t);

            t = MultipleMatrix(t, t);
        }

        return res;
    }

    // 两个矩阵乘完之后的结果返回
    private static int[,] MultipleMatrix(int[,] m1, int[,] m2)
    {
        var res = new int[m1.GetLength(0), m2.GetLength(1)];
        for (var i = 0; i < m1.GetLength(0); i++)
        for (var j = 0; j < m2.GetLength(1); j++)
        for (var k = 0; k < m2.GetLength(0); k++)
            res[i, j] += m1[i, k] * m2[k, j];

        return res;
    }

    private static int S1(int n)
    {
        if (n < 1) return 0;

        if (n == 1 || n == 2) return n;

        return S1(n - 1) + S1(n - 2);
    }

    private static int S2(int n)
    {
        if (n < 1) return 0;

        if (n == 1 || n == 2) return n;

        var res = 2;
        var pre = 1;
        for (var i = 3; i <= n; i++)
        {
            var temp = res;
            res += pre;
            pre = temp;
        }

        return res;
    }

    private static int S3(int n)
    {
        if (n < 1) return 0;

        if (n is 1 or 2) return n;

        int[,] base1 = { { 1, 1 }, { 1, 0 } };
        var res = MatrixPower(base1, n - 2);
        return 2 * res[0, 0] + res[1, 0];
    }

    private static int C1(int n)
    {
        if (n < 1) return 0;

        if (n == 1 || n == 2 || n == 3) return n;

        return C1(n - 1) + C1(n - 3);
    }

    private static int C2(int n)
    {
        if (n < 1) return 0;

        if (n == 1 || n == 2 || n == 3) return n;

        var res = 3;
        var pre = 2;
        var a = 1;
        for (var i = 4; i <= n; i++)
        {
            var tmp1 = res;
            var tmp2 = pre;
            res = res + a;
            pre = tmp1;
            a = tmp2;
        }

        return res;
    }

    private static int C3(int n)
    {
        if (n < 1) return 0;

        if (n == 1 || n == 2 || n == 3) return n;

        int[,] base1 =
        {
            { 1, 1, 0 },
            { 0, 0, 1 },
            { 1, 0, 0 }
        };
        var res = MatrixPower(base1, n - 3);
        return 3 * res[0, 0] + 2 * res[1, 0] + res[2, 0];
    }

    public static void Run()
    {
        var n = 19;
        Console.WriteLine(F1(n));
        Console.WriteLine(F2(n));
        Console.WriteLine(F3(n));
        Console.WriteLine("===");
        Console.WriteLine(S1(n));
        Console.WriteLine(S2(n));
        Console.WriteLine(S3(n));
        Console.WriteLine("===");
        Console.WriteLine(C1(n));
        Console.WriteLine(C2(n));
        Console.WriteLine(C3(n));
        Console.WriteLine("===");
    }
}