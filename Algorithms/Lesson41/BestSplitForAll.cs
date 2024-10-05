//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson41;

public class BestSplitForAll
{
    private static int BestSplit1(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;
        var n = arr.Length;
        var ans = 0;
        for (var s = 0; s < n - 1; s++)
        {
            var sumL = 0;
            for (var l = 0; l <= s; l++) sumL += arr[l];
            var sumR = 0;
            for (var r = s + 1; r < n; r++) sumR += arr[r];
            ans = Math.Max(ans, Math.Min(sumL, sumR));
        }

        return ans;
    }

    private static int BestSplit2(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;
        var n = arr.Length;
        var sumAll = 0;
        foreach (var num in arr) sumAll += num;
        var ans = 0;
        var sumL = 0;
        // [0...s]  [s+1...N-1]
        for (var s = 0; s < n - 1; s++)
        {
            sumL += arr[s];
            var sumR = sumAll - sumL;
            ans = Math.Max(ans, Math.Min(sumL, sumR));
        }

        return ans;
    }

    private static int[] RandomArray(int len, int max)
    {
        var ans = new int[len];
        for (var i = 0; i < len; i++) ans[i] = (int)(Utility.getRandomDouble * max);
        return ans;
    }

    public static void Run()
    {
        const int n = 20;
        const int max = 30;
        const int testTime = 1000000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var len = (int)(Utility.getRandomDouble * n);
            var arr = RandomArray(len, max);
            var ans1 = BestSplit1(arr);
            var ans2 = BestSplit2(arr);
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