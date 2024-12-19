//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson41;

public class BestSplitForEveryPosition
{
    private static int[] BestSplit1(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return [];

        var n = arr.Length;
        var ans = new int[n];
        ans[0] = 0;
        for (var range = 1; range < n; range++)
        for (var s = 0; s < range; s++)
        {
            var sumL = 0;
            for (var l = 0; l <= s; l++) sumL += arr[l];

            var sumR = 0;
            for (var r = s + 1; r <= range; r++) sumR += arr[r];

            ans[range] = Math.Max(ans[range], Math.Min(sumL, sumR));
        }

        return ans;
    }

    // 求原来的数组arr中，arr[L...R]的累加和
    private static int Sum(int[] sum, int l, int r)
    {
        return sum[r + 1] - sum[l];
    }

    private static int[] BestSplit2(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return [];

        var n = arr.Length;
        var ans = new int[n];
        ans[0] = 0;
        var sum = new int[n + 1];
        for (var i = 0; i < n; i++) sum[i + 1] = sum[i] + arr[i];

        for (var range = 1; range < n; range++)
        for (var s = 0; s < range; s++)
        {
            var sumL = Sum(sum, 0, s);
            var sumR = Sum(sum, s + 1, range);
            ans[range] = Math.Max(ans[range], Math.Min(sumL, sumR));
        }

        return ans;
    }

    private static int[] BestSplit3(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return [];

        var n = arr.Length;
        var ans = new int[n];
        ans[0] = 0;
        // arr =   {5, 3, 1, 3}
        //          0  1  2  3
        // sum ={0, 5, 8, 9, 12}
        //       0  1  2  3   4
        // 0~2 ->  sum[3] - sum[0]
        // 1~3 ->  sum[4] - sum[1]
        var sum = new int[n + 1];
        for (var i = 0; i < n; i++) sum[i + 1] = sum[i] + arr[i];

        // 最优划分
        // 0~range-1上，最优划分是左部分[0~best]  右部分[best+1~range-1]
        var best = 0;
        for (var range = 1; range < n; range++)
        {
            while (best + 1 < range)
            {
                var before = Math.Min(Sum(sum, 0, best), Sum(sum, best + 1, range));
                var after = Math.Min(Sum(sum, 0, best + 1), Sum(sum, best + 2, range));
                // 注意，一定要是>=，只是>会出错
                // 课上会讲解
                if (after >= before)
                    best++;
                else
                    break;
            }

            ans[range] = Math.Min(Sum(sum, 0, best), Sum(sum, best + 1, range));
        }

        return ans;
    }

    private static int[] RandomArray(int len, int max)
    {
        var ans = new int[len];
        for (var i = 0; i < len; i++) ans[i] = (int)(Utility.getRandomDouble * max);

        return ans;
    }

    private static bool IsSameArray(int[] arr1, int[] arr2)
    {
        if (arr1.Length != arr2.Length) return false;

        var n = arr1.Length;
        for (var i = 0; i < n; i++)
            if (arr1[i] != arr2[i])
                return false;

        return true;
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
            var ans3 = BestSplit3(arr);
            if (!IsSameArray(ans1, ans2) || !IsSameArray(ans1, ans3)) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试结束");
    }
}