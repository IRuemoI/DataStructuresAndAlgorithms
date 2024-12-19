//测试通过

namespace Algorithms.Lesson03;

public class GetMax
{
    // 求arr中的最大值
    private static int GetMaxCode(int[] arr)
    {
        return Process(arr, 0, arr.Length - 1);
    }

    // arr[L..R]范围上求最大值  L ... R   N
    private static int Process(int[] arr, int leftEdge, int rightEdge)
    {
        // arr[L..R]范围上只有一个数，直接返回，base case
        if (leftEdge == rightEdge) return arr[leftEdge];
        // L...R 不只一个数
        // mid = (L + R) / 2
        var middle = leftEdge + ((rightEdge - leftEdge) >> 1); // 中点   	1
        var leftMax = Process(arr, leftEdge, middle);
        var rightMax = Process(arr, middle + 1, rightEdge);
        return Math.Max(leftMax, rightMax);
    }

    public static void Run()
    {
        int[] arr = [3, 1, 5, 7, 2, 4, 8, 6, 9];
        var max = GetMaxCode(arr);
        Console.WriteLine(max);
    }
}