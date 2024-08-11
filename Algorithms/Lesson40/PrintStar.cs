//测试通过

namespace Algorithms.Lesson40;

public class PrintStar
{
    private static void Code(int n)
    {
        var leftUp = 0;
        var rightDown = n - 1;
        var m = new char[n, n];
        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
            m[i, j] = ' ';
        while (leftUp <= rightDown)
        {
            Set(m, leftUp, rightDown);
            leftUp += 2;
            rightDown -= 2;
        }

        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++) Console.Write(m[i, j] + " ");
            Console.WriteLine();
        }
    }

    private static void Set(char[,] m, int leftUp, int rightDown)
    {
        for (var col = leftUp; col <= rightDown; col++) m[leftUp, col] = '*';
        for (var row = leftUp + 1; row <= rightDown; row++) m[row, rightDown] = '*';
        for (var col = rightDown - 1; col > leftUp; col--) m[rightDown, col] = '*';
        for (var row = rightDown - 1; row > leftUp + 1; row--) m[row, leftUp + 1] = '*';
    }

    public static void Run()
    {
        Code(10);
    }
}