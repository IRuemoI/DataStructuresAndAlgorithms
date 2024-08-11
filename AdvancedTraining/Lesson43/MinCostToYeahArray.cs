#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson43;

// 来自360笔试
// 给定一个正数数组arr，长度为n，下标0~n-1
// arr中的0、n-1位置不需要达标，它们分别是最左、最右的位置
// 中间位置i需要达标，达标的条件是 : arr[i-1] > arr[i] 或者 arr[i+1] > arr[i]哪个都可以
// 你每一步可以进行如下操作：对任何位置的数让其-1
// 你的目的是让arr[1~n-2]都达标，这时arr称之为yeah！数组
// 返回至少要多少步可以让arr变成yeah！数组
// 数据规模 : 数组长度 <= 10000，数组中的值<=500
public class MinCostToYeahArray
{
    private const int Invalid = int.MaxValue;

    // 纯暴力方法，只是为了结果对
    // 时间复杂度极差
    private static int MinCost0(int[]? arr)
    {
        if (arr == null || arr.Length < 3) return 0;

        var n = arr.Length;
        var min = Invalid;
        foreach (var num in arr) min = Math.Min(min, num);

        var @base = min - n;
        return Process0(arr, @base, 0);
    }

    private static int Process0(int[] arr, int @base, int index)
    {
        if (index == arr.Length)
        {
            for (var i = 1; i < arr.Length - 1; i++)
                if (arr[i - 1] <= arr[i] && arr[i] >= arr[i + 1])
                    return Invalid;

            return 0;
        }

        var ans = Invalid;
        var tmp = arr[index];
        for (var cost = 0; arr[index] >= @base; cost++, arr[index]--)
        {
            var next = Process0(arr, @base, index + 1);
            if (next != Invalid) ans = Math.Min(ans, cost + next);
        }

        arr[index] = tmp;
        return ans;
    }

    // 递归方法，已经把尝试写出
    private static int MinCost1(int[]? arr)
    {
        if (arr == null || arr.Length < 3) return 0;

        var min = Invalid;
        foreach (var num in arr) min = Math.Min(min, num);

        for (var i = 0; i < arr.Length; i++) arr[i] += arr.Length - min;

        return Process1(arr, 1, arr[0], true);
    }

    // 当前来到index位置，值arr[index]
    // pre : 前一个位置的值，可能减掉了一些，所以不能用arr[index-1]
    // preOk : 前一个位置的值，是否被它左边的数变有效了
    // 返回 : 让arr都变有效，最小代价是什么？
    private static int Process1(int[] arr, int index, int pre, bool preOk)
    {
        if (index == arr.Length - 1)
            // 已经来到最后一个数了
            return preOk || pre < arr[index] ? 0 : Invalid;

        // 当前index，不是最后一个数！
        var ans = Invalid;
        if (preOk)
            for (var cur = arr[index]; cur >= 0; cur--)
            {
                var next = Process1(arr, index + 1, cur, cur < pre);
                if (next != Invalid) ans = Math.Min(ans, arr[index] - cur + next);
            }
        else
            for (var cur = arr[index]; cur > pre; cur--)
            {
                var next = Process1(arr, index + 1, cur, false);
                if (next != Invalid) ans = Math.Min(ans, arr[index] - cur + next);
            }

        return ans;
    }

    // 初改动态规划方法，就是参考minCost1，改出来的版本
    private static int MinCost2(int[]? arr)
    {
        if (arr == null || arr.Length < 3) return 0;

        var min = Invalid;
        foreach (var num in arr) min = Math.Min(min, num);

        var n = arr.Length;
        for (var i = 0; i < n; i++) arr[i] += n - min;
        var dp = RectangularArrays.RectangularIntArray(n, 2, -1);
        for (var i = 1; i < n; i++)
        {
            dp[i][0] = new int[arr[i - 1] + 1];
            dp[i][1] = new int[arr[i - 1] + 1];
            Array.Fill(dp[i][0], Invalid);
            Array.Fill(dp[i][1], Invalid);
        }

        for (var pre = 0; pre <= arr[n - 2]; pre++)
        {
            dp[n - 1][0][pre] = pre < arr[n - 1] ? 0 : Invalid;
            dp[n - 1][1][pre] = 0;
        }

        for (var index = n - 2; index >= 1; index--)
        for (var pre = 0; pre <= arr[index - 1]; pre++)
        {
            for (var cur = arr[index]; cur > pre; cur--)
            {
                var next = dp[index + 1][0][cur];
                if (next != Invalid) dp[index][0][pre] = Math.Min(dp[index][0][pre], arr[index] - cur + next);
            }

            for (var cur = arr[index]; cur >= 0; cur--)
            {
                var next = dp[index + 1][cur < pre ? 1 : 0][cur];
                if (next != Invalid) dp[index][1][pre] = Math.Min(dp[index][1][pre], arr[index] - cur + next);
            }
        }

        return dp[1][1][arr[0]];
    }

    // minCost2动态规划 + 枚举优化
    // 改出的这个版本，需要一些技巧，但很可惜不是最优解
    // 虽然不是最优解，也足以通过100%的case了，
    // 这种技法的练习，非常有意义
    private static int MinCost3(int[]? arr)
    {
        if (arr == null || arr.Length < 3) return 0;

        var min = Invalid;
        foreach (var num in arr) min = Math.Min(min, num);

        var n = arr.Length;
        for (var i = 0; i < n; i++) arr[i] += n - min;

        // JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
// ORIGINAL LINE: int[][][] dp = new int[n][2][];
        var dp = RectangularArrays.RectangularIntArray(n, 2, -1);
        for (var i = 1; i < n; i++)
        {
            dp[i][0] = new int[arr[i - 1] + 1];
            dp[i][1] = new int[arr[i - 1] + 1];
        }

        for (var p = 0; p <= arr[n - 2]; p++) dp[n - 1][0][p] = p < arr[n - 1] ? 0 : Invalid;

        var best = MinCostToYeahArray.best(dp, n - 1, arr[n - 2]);
        for (var i = n - 2; i >= 1; i--)
        {
            for (var p = 0; p <= arr[i - 1]; p++)
            {
                if (arr[i] < p)
                    dp[i][1][p] = best[1][arr[i]];
                else
                    dp[i][1][p] = Math.Min(best[0][p], p > 0 ? best[1][p - 1] : Invalid);

                dp[i][0][p] = arr[i] <= p ? Invalid : best[0][p + 1];
            }

            best = MinCostToYeahArray.best(dp, i, arr[i - 1]);
        }

        return dp[1][1][arr[0]];
    }

    private static int[][] best(int[][][] dp, int i, int v)
    {
        int[][] best = [new int[v + 1], new int[v + 1]];
        best[0][v] = dp[i][0][v];
        for (var p = v - 1; p >= 0; p--)
        {
            best[0][p] = dp[i][0][p] == Invalid ? Invalid : v - p + dp[i][0][p];
            best[0][p] = Math.Min(best[0][p], best[0][p + 1]);
        }

        best[1][0] = dp[i][1][0] == Invalid ? Invalid : v + dp[i][1][0];
        for (var p = 1; p <= v; p++)
        {
            best[1][p] = dp[i][1][p] == Invalid ? Invalid : v - p + dp[i][1][p];
            best[1][p] = Math.Min(best[1][p], best[1][p - 1]);
        }

        return best;
    }

    // 最终的最优解，贪心
    // 时间复杂度O(N)
    // 请注意，重点看上面的方法
    // 这个最优解容易理解，但让你学到的东西不是很多
    private static int Yeah(int[]? arr)
    {
        if (arr == null || arr.Length < 3) return 0;

        var n = arr.Length;
        var nums = new int[n + 2];
        nums[0] = int.MaxValue;
        nums[n + 1] = int.MaxValue;
        for (var i = 0; i < arr.Length; i++) nums[i + 1] = arr[i];

        var leftCost = new int[n + 2];
        var pre = nums[0];
        int change;
        for (var i = 1; i <= n; i++)
        {
            change = Math.Min(pre - 1, nums[i]);
            leftCost[i] = nums[i] - change + leftCost[i - 1];
            pre = change;
        }

        var rightCost = new int[n + 2];
        pre = nums[n + 1];
        for (var i = n; i >= 1; i--)
        {
            change = Math.Min(pre - 1, nums[i]);
            rightCost[i] = nums[i] - change + rightCost[i + 1];
            pre = change;
        }

        var ans = int.MaxValue;
        for (var i = 1; i <= n; i++) ans = Math.Min(ans, leftCost[i] + rightCost[i + 1]);

        return ans;
    }

    // 为了测试
    private static int[] randomArray(int len, int v)
    {
        var arr = new int[len];
        for (var i = 0; i < len; i++) arr[i] = (int)(MathHelper.NextDouble * v) + 1;

        return arr;
    }

    // 为了测试
    private static int[] copyArray(int[] arr)
    {
        var ans = new int[arr.Length];
        for (var i = 0; i < arr.Length; i++) ans[i] = arr[i];

        return ans;
    }

    // 为了测试
    public static void Run()
    {
        var len = 7;
        var v = 10;
        var testTime = 100;
        Console.WriteLine("==========");
        Console.WriteLine("功能测试开始");

        for (var i = 0; i < testTime; i++)
        {
            var n = (int)(MathHelper.NextDouble * len) + 1;
            var arr = randomArray(n, v);
            var arr0 = copyArray(arr);
            var arr1 = copyArray(arr);
            var arr2 = copyArray(arr);
            var arr3 = copyArray(arr);
            var arr4 = copyArray(arr);
            var ans0 = MinCost0(arr0);
            var ans1 = MinCost1(arr1);
            var ans2 = MinCost2(arr2);
            var ans3 = MinCost3(arr3);
            var ans4 = Yeah(arr4);
            if (ans0 != ans1 || ans0 != ans2 || ans0 != ans3 || ans0 != ans4) Console.WriteLine("出错了！");
        }

        Console.WriteLine("功能测试结束");
        Console.WriteLine("==========");

        Console.WriteLine("性能测试开始");

        len = 10000;
        v = 500;
        Console.WriteLine("生成随机数组长度：" + len);
        Console.WriteLine("生成随机数组值的范围：[1, " + v + "]");
        var arr31 = randomArray(len, v);
        var arr32 = copyArray(arr31);
        var arrYeah = copyArray(arr31);

        Utility.RestartStopwatch();
        var ans34 = MinCost3(arr32);

        Console.WriteLine("minCost3方法:");
        Console.WriteLine("运行结果: " + ans34 + ", 时间(毫秒) : " + Utility.GetStopwatchElapsedMilliseconds());

        Utility.RestartStopwatch();
        var ansYeah = Yeah(arrYeah);

        Console.WriteLine("yeah方法:");
        Console.WriteLine("运行结果: " + ansYeah + ", 时间(毫秒) : " + Utility.GetStopwatchElapsedMilliseconds());

        Console.WriteLine("性能测试结束");
        Console.WriteLine("==========");
    }
}

internal static class MathHelper
{
    private static Random? _randomInstance;

    public static double NextDouble
    {
        get
        {
            if (_randomInstance == null)
                _randomInstance = new Random();

            return _randomInstance.NextDouble();
        }
    }

    private static double Expm1(double x)
    {
        if (Math.Abs(x) < 1e-5)
            return x + 0.5 * x * x;
        return Math.Exp(x) - 1.0;
    }

    private static double Log1p(double x)
    {
        var y = x;
        return 1 + y == 1 ? y : y * (Math.Log(1 + y) / (1 + y - 1));
    }

    public static void Run()
    {
        //todo:待整理
    }
}

internal static class RectangularArrays
{
    public static int[][][] RectangularIntArray(int size1, int size2, int size3)
    {
        var newArray = new int[size1][][];
        for (var array1 = 0; array1 < size1; array1++)
        {
            newArray[array1] = new int[size2][];
            if (size3 > -1)
                for (var array2 = 0; array2 < size2; array2++)
                    newArray[array1][array2] = new int[size3];
        }

        return newArray;
    }
}