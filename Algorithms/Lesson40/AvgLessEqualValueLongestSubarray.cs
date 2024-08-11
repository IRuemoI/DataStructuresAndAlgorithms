#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson40;

public class AvgLessEqualValueLongestSubarray
{
    // 暴力解，时间复杂度O(N^3)，用于做对数器
    private static int Ways1(int[] arr, int v)
    {
        var ans = 0;
        for (var l = 0; l < arr.Length; l++)
        for (var r = l; r < arr.Length; r++)
        {
            var sum = 0;
            var k = r - l + 1;
            for (var i = l; i <= r; i++) sum += arr[i];

            var avg = (double)sum / k;
            if (avg <= v) ans = Math.Max(ans, k);
        }

        return ans;
    }

    // 想实现的解法2，时间复杂度O(N*logN)
    private static int Ways2(int[]? arr, int v)
    {
        if (arr == null || arr.Length == 0) return 0;

        var origins = new SortedDictionary<int, int?>();
        var ans = 0;
        var modify = 0;
        for (var i = 0; i < arr.Length; i++)
        {
            var p1 = arr[i] <= v ? 1 : 0;
            var p2 = 0;
            var query = -arr[i] - modify;
            var tempFloorKey = origins.LastOrDefault(x => x.Key <= query);
            if (tempFloorKey.Value != null) p2 = (int)(i - origins[tempFloorKey.Key] + 1)!;

            ans = Math.Max(ans, Math.Max(p1, p2));
            var curOrigin = -modify - v;
            tempFloorKey = origins.LastOrDefault(x => x.Key <= curOrigin);
            if (tempFloorKey.Value == null) origins[curOrigin] = i;

            modify += arr[i] - v;
        }

        return ans;
    }

    // 想实现的解法3，时间复杂度O(N)
    private static int Ways3(int[]? arr, int v)
    {
        if (arr == null || arr.Length == 0) return 0;

        for (var i = 0; i < arr.Length; i++) arr[i] -= v;

        return MaxLengthAwesome(arr, 0);
    }

    // 找到数组中累加和<=k的最长子数组
    private static int MaxLengthAwesome(int[] arr, int k)
    {
        var n = arr.Length;
        var sums = new int[n];
        var ends = new int[n];
        sums[n - 1] = arr[n - 1];
        ends[n - 1] = n - 1;
        for (var i = n - 2; i >= 0; i--)
            if (sums[i + 1] < 0)
            {
                sums[i] = arr[i] + sums[i + 1];
                ends[i] = ends[i + 1];
            }
            else
            {
                sums[i] = arr[i];
                ends[i] = i;
            }

        var end = 0;
        var sum = 0;
        var res = 0;
        for (var i = 0; i < n; i++)
        {
            while (end < n && sum + sums[end] <= k)
            {
                sum += sums[end];
                end = ends[end] + 1;
            }

            res = Math.Max(res, end - i);
            if (end > i)
                sum -= arr[i];
            else
                end = i + 1;
        }

        return res;
    }

    // 用于测试
    private static int[] RandomArray(int maxLen, int maxValue)
    {
        var len = (int)(Utility.GetRandomDouble * maxLen) + 1;
        var ans = new int[len];
        for (var i = 0; i < len; i++) ans[i] = (int)(Utility.GetRandomDouble * maxValue);

        return ans;
    }

    // 用于测试
    private static int[] CopyArray(int[] arr)
    {
        var ans = new int[arr.Length];
        for (var i = 0; i < arr.Length; i++) ans[i] = arr[i];

        return ans;
    }

    // 用于测试
    private static void PrintArray(int[] arr)
    {
        foreach (var item in arr) Console.Write(item + " ");

        Console.WriteLine();
    }

    // 用于测试
    public static void Run()
    {
        Console.WriteLine("测试开始");
        var maxLen = 20;
        var maxValue = 100;
        var testTime = 500000;
        for (var i = 0; i < testTime; i++)
        {
            var arr = RandomArray(maxLen, maxValue);
            var value = (int)(Utility.GetRandomDouble * maxValue);
            var arr1 = CopyArray(arr);
            var arr2 = CopyArray(arr);
            var arr3 = CopyArray(arr);
            var ans1 = Ways1(arr1, value);
            var ans2 = Ways2(arr2, value);
            var ans3 = Ways3(arr3, value);
            if (ans1 != ans2 || ans1 != ans3)
            {
                Console.WriteLine("测试出错！");
                Console.Write("测试数组：");
                PrintArray(arr);
                Console.WriteLine("子数组平均值不小于 ：" + value);
                Console.WriteLine("方法1得到的最大长度：" + ans1);
                Console.WriteLine("方法2得到的最大长度：" + ans2);
                Console.WriteLine("方法3得到的最大长度：" + ans3);
                Console.WriteLine("=========================");
            }
        }

        Console.WriteLine("测试结束");
    }
}