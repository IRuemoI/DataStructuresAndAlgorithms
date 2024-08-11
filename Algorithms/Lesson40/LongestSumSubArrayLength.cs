//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson40;

public class LongestSumSubArrayLength
{
    private static int MaxLength(int[]? arr, int k)
    {
        if (arr == null || arr.Length == 0) return 0;

        // key:前缀和
        // value : 0~value这个前缀和是最早出现key这个值的
        var map = new Dictionary<int, int>
        {
            [0] = -1 // important
        };
        var len = 0;
        var sum = 0;
        for (var i = 0; i < arr.Length; i++)
        {
            sum += arr[i];
            if (map.ContainsKey(sum - k)) len = Math.Max(i - map[sum - k], len);

            map.TryAdd(sum, i);
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
    private static int[] GenerateRandomArray(int size, int value)
    {
        var ans = new int[(int)(Utility.GetRandomDouble * size) + 1];
        for (var i = 0; i < ans.Length; i++)
            ans[i] = (int)(Utility.GetRandomDouble * value) - (int)(Utility.GetRandomDouble * value);

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
            var arr = GenerateRandomArray(len, value);
            var k = (int)(Utility.GetRandomDouble * value) - (int)(Utility.GetRandomDouble * value);
            var ans1 = MaxLength(arr, k);
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