//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson40;

public class LongestSumSubArrayLengthInPositiveArray
{
    private static int GetMaxLength(int[]? arr, int k)
    {
        if (arr == null || arr.Length == 0 || k <= 0) return 0;

        var left = 0;
        var right = 0;
        var sum = arr[0];
        var len = 0;
        while (right < arr.Length)
            if (sum == k)
            {
                len = Math.Max(len, right - left + 1);
                sum -= arr[left++];
            }
            else if (sum < k)
            {
                right++;
                if (right == arr.Length) break;

                sum += arr[right];
            }
            else
            {
                sum -= arr[left++];
            }

        return len;
    }

    //用于测试
    private static int Right(int[] arr, int k)
    {
        var max = 0;
        for (var i = 0; i < arr.Length; i++)
        for (var j = i; j < arr.Length; j++)
            if (Valid(arr, i, j, k))
                max = Math.Max(max, j - i + 1);

        return max;
    }

    //用于测试
    private static bool Valid(int[] arr, int l, int r, int k)
    {
        var sum = 0;
        for (var i = l; i <= r; i++) sum += arr[i];

        return sum == k;
    }

    //用于测试
    private static int[] GeneratePositiveArray(int size, int value)
    {
        var ans = new int[size];
        for (var i = 0; i != size; i++) ans[i] = (int)(Utility.GetRandomDouble * value) + 1;

        return ans;
    }

    //用于测试
    private static void PrintArray(int[] arr)
    {
        for (var i = 0; i != arr.Length; i++) Console.Write(arr[i] + " ");

        Console.WriteLine();
    }

    public static void Run()
    {
        var len = 50;
        var value = 100;
        var testTime = 500000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var arr = GeneratePositiveArray(len, value);
            var k = (int)(Utility.GetRandomDouble * value) + 1;
            var ans1 = GetMaxLength(arr, k);
            var ans2 = Right(arr, k);
            if (ans1 != ans2)
            {
                Console.WriteLine("出错啦！");
                PrintArray(arr);
                Console.WriteLine("K : " + k);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}