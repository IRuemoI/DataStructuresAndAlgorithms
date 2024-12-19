//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson46;

// 给定一个数组arr，和一个正数M
// 返回在子数组长度不大于M的情况下，最大的子数组累加和
public class MaxSumLengthNoMore
{
    // O(N^2)的解法，暴力解，用作对数器
    private static int Test(int[]? arr, int m)
    {
        if (arr == null || arr.Length == 0 || m < 1) return 0;

        var n = arr.Length;
        var max = int.MinValue;
        for (var l = 0; l < n; l++)
        {
            var sum = 0;
            for (var r = l; r < n; r++)
            {
                if (r - l + 1 > m) break;

                sum += arr[r];
                max = Math.Max(max, sum);
            }
        }

        return max;
    }

    // O(N)的解法，最优解
    private static int MaxSum(int[]? arr, int m)
    {
        if (arr == null || arr.Length == 0 || m < 1) return 0;

        var n = arr.Length;
        var sum = new int[n];
        sum[0] = arr[0];
        for (var i = 1; i < n; i++) sum[i] = sum[i - 1] + arr[i];

        var qMax = new LinkedList<int>();
        var i1 = 0;
        var end = Math.Min(n, m);
        for (; i1 < end; i1++)
        {
            while (qMax.Count > 0 && sum[qMax.Last()] <= sum[i1]) qMax.RemoveLast();

            qMax.AddLast(i1);
        }

        var max = sum[qMax.First()];
        var l = 0;
        for (; i1 < n; l++, i1++)
        {
            if (qMax.First() == l) qMax.RemoveFirst();

            while (qMax.Count > 0 && sum[qMax.Last()] <= sum[i1]) qMax.RemoveLast();

            qMax.AddLast(i1);
            max = Math.Max(max, sum[qMax.First()] - sum[l]);
        }

        for (; l < n - 1; l++)
        {
            if (qMax.First() == l) qMax.RemoveFirst();

            max = Math.Max(max, sum[qMax.First()] - sum[l]);
        }

        return max;
    }

    // 用作测试
    private static int[] RandomArray(int len, int max)
    {
        var arr = new int[len];
        for (var i = 0; i < len; i++)
            arr[i] = (int)(Utility.getRandomDouble * max) - (int)(Utility.getRandomDouble * max);

        return arr;
    }

    // 用作测试
    public static void Run()
    {
        var maxN = 50;
        var maxValue = 100;
        var testTime = 1000000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var n = (int)(Utility.getRandomDouble * maxN);
            var m = (int)(Utility.getRandomDouble * maxN);
            var arr = RandomArray(n, maxValue);
            var ans1 = Test(arr, m);
            var ans2 = MaxSum(arr, m);
            if (ans1 != ans2)
            {
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                Console.WriteLine("出错啦！");
            }
        }

        Console.WriteLine("测试结束");
    }
}