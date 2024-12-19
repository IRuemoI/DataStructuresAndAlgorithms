//测试通过

namespace Algorithms.Lesson25;

// 测试链接：https://leetcode.cn/problems/count-submatrices-with-all-ones
public static class CountSubMatricesWithAllOnes
{
    public static int NumSubMatrix(int[,]? mat)
    {
        if (mat == null || mat.GetLength(0) == 0 || mat.GetLength(1) == 0) return 0;

        var nums = 0;
        var height = new int[mat.GetLength(1)];
        for (var i = 0; i < mat.GetLength(0); i++)
        {
            for (var j = 0; j < mat.GetLength(1); j++) height[j] = mat[i, j] == 0 ? 0 : height[j] + 1;

            nums += CountFromBottom(height);
        }

        return nums;
    }

    private static int CountFromBottom(int[]? height)
    {
        if (height == null || height.Length == 0) return 0;

        var nums = 0;
        var stack = new int[height.Length];
        var si = -1;
        for (var i = 0; i < height.Length; i++)
        {
            while (si != -1 && height[stack[si]] >= height[i])
            {
                var cur = stack[si--];
                if (height[cur] > height[i])
                {
                    var left = si == -1 ? -1 : stack[si];
                    var n = i - left - 1;
                    var down = Math.Max(left == -1 ? 0 : height[left], height[i]);
                    nums += (height[cur] - down) * Num(n);
                }
            }

            stack[++si] = i;
        }

        while (si != -1)
        {
            var cur = stack[si--];
            var left = si == -1 ? -1 : stack[si];
            var n = height.Length - left - 1;
            var down = left == -1 ? 0 : height[left];
            nums += (height[cur] - down) * Num(n);
        }

        return nums;
    }

    private static int Num(int n)
    {
        return (n * (1 + n)) >> 1;
    }
}

public class CountSubMatricesWithAllOnesTest
{
    public static void Run()
    {
        int[,] mat1 =
        {
            {
                1, 0, 1
            },
            {
                1, 1, 0
            },
            {
                1, 1, 0
            }
        };
        int[,] mat2 =
        {
            {
                0, 1, 1, 0
            },
            {
                0, 1, 1, 1
            },
            {
                1, 1, 1, 0
            }
        };
        Console.WriteLine(CountSubMatricesWithAllOnes.NumSubMatrix(mat1)); //13
        Console.WriteLine(CountSubMatricesWithAllOnes.NumSubMatrix(mat2)); //24
    }
}