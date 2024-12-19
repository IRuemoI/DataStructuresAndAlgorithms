//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson24;

public class MinCoinsOnePaper
{
    private static int MinCoins(int[] arr, int aim)
    {
        return Process(arr, 0, aim);
    }

    private static int Process(int[] arr, int index, int rest)
    {
        if (rest < 0) return int.MaxValue;

        if (index == arr.Length) return rest == 0 ? 0 : int.MaxValue;

        var p1 = Process(arr, index + 1, rest);
        var p2 = Process(arr, index + 1, rest - arr[index]);
        if (p2 != int.MaxValue) p2++;

        return Math.Min(p1, p2);
    }

    // dp1时间复杂度为：O(arr长度 * aim)
    private static int Dp1(int[] arr, int aim)
    {
        if (aim == 0) return 0;

        var n = arr.Length;
        var dp = new int[n + 1, aim + 1];
        dp[n, 0] = 0;
        for (var j = 1; j <= aim; j++) dp[n, j] = int.MaxValue;

        for (var index = n - 1; index >= 0; index--)
        for (var rest = 0; rest <= aim; rest++)
        {
            var p1 = dp[index + 1, rest];
            var p2 = rest - arr[index] >= 0 ? dp[index + 1, rest - arr[index]] : int.MaxValue;
            if (p2 != int.MaxValue) p2++;

            dp[index, rest] = Math.Min(p1, p2);
        }

        return dp[0, aim];
    }

    private static Info GetInfo(int[] arr)
    {
        Dictionary<int, int> counts = new();
        foreach (var value in arr)
            if (!counts.ContainsKey(value))
                counts.Add(value, 1);
            else
                counts[value] += 1;

        var n = counts.Count;
        var coins = new int[n];
        var count = new int[n];
        var index = 0;
        foreach (var entry in counts)
        {
            coins[index] = entry.Key;
            count[index++] = entry.Value;
        }

        return new Info(coins, count);
    }

    // dp2时间复杂度为：O(arr长度) + O(货币种数 * aim * 每种货币的平均张数)
    private static int Dp2(int[] arr, int aim)
    {
        if (aim == 0) return 0;

        // 得到info时间复杂度O(arr长度)
        var info = GetInfo(arr);
        var coins = info.Coins;
        var count = info.Count;
        var n = coins.Length;
        var dp = new int[n + 1, aim + 1];
        dp[n, 0] = 0;
        for (var j = 1; j <= aim; j++) dp[n, j] = int.MaxValue;

        // 这三层for循环，时间复杂度为O(货币种数 * aim * 每种货币的平均张数)
        for (var index = n - 1; index >= 0; index--)
        for (var rest = 0; rest <= aim; rest++)
        {
            dp[index, rest] = dp[index + 1, rest];
            for (var zhang = 1; zhang * coins[index] <= aim && zhang <= count[index]; zhang++)
                if (rest - zhang * coins[index] >= 0
                    && dp[index + 1, rest - zhang * coins[index]] != int.MaxValue)
                    dp[index, rest] = Math.Min(dp[index, rest], zhang + dp[index + 1, rest - zhang * coins[index]]);
        }

        return dp[0, aim];
    }

    private static int Dp3(int[] arr, int aim)
    {
        if (aim == 0) return 0;

        // 得到info时间复杂度O(arr长度)
        var info = GetInfo(arr);
        var c = info.Coins;
        var z = info.Count;
        var n = c.Length;
        var dp = new int[n + 1, aim + 1];
        dp[n, 0] = 0;
        for (var j = 1; j <= aim; j++) dp[n, j] = int.MaxValue;

        // 虽然是嵌套了很多循环，但是时间复杂度为O(货币种数 * aim)
        // 因为用了窗口内最小值的更新结构
        for (var i = n - 1; i >= 0; i--)
        for (var mod = 0; mod < Math.Min(aim + 1, c[i]); mod++)
        {
            // 当前面值 X
            // mod  mod + x   mod + 2*x   mod + 3 * x
            LinkedList<int> w = new();
            w.AddLast(mod);
            dp[i, mod] = dp[i + 1, mod];
            for (var r = mod + c[i]; r <= aim; r += c[i])
            {
                while (w.Last != null &&
                       w.Count != 0 &&
                       (dp[i + 1, w.Last.Value] == int.MaxValue ||
                        dp[i + 1, w.Last.Value] +
                        Compensate(w.Last.Value, r, c[i]) >= dp[i + 1, r]))
                    w.RemoveLast();

                w.AddLast(r);
                var overdue = r - c[i] * (z[i] + 1);
                if (w.First != null && w.First.Value == overdue) w.RemoveFirst();
                if (w.First != null) dp[i, r] = dp[i + 1, w.First.Value] + Compensate(w.First.Value, r, c[i]);
            }
        }

        return dp[0, aim];
    }


    private static int Compensate(int pre, int cur, int coin)
    {
        return (cur - pre) / coin;
    }

    // 为了测试
    private static int[] RandomArray(int n, int maxValue)
    {
        var arr = new int[n];
        for (var i = 0; i < n; i++) arr[i] = (int)(new Random().NextDouble() * maxValue) + 1;

        return arr;
    }

    // 为了测试
    private static void PrintArray(int[] arr)
    {
        foreach (var element in arr) Console.Write(element + " ");

        Console.WriteLine();
    }

    // 为了测试
    public static void Run()
    {
        var maxLen = 20;
        var maxValue = 30;
        var testTime = 300000;

        Utility.RestartStopwatch();
        Console.WriteLine("功能测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var n = (int)(new Random().NextDouble() * maxLen);
            var arr = RandomArray(n, maxValue);
            var aim = (int)(new Random().NextDouble() * maxValue);
            var ans1 = MinCoins(arr, aim);
            var ans2 = Dp1(arr, aim);
            var ans3 = Dp2(arr, aim);
            var ans4 = Dp3(arr, aim);
            if (ans1 != ans2 || ans3 != ans4 || ans1 != ans3)
            {
                Console.WriteLine("出错啦！");
                PrintArray(arr);
                Console.WriteLine(aim);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                Console.WriteLine(ans3);
                Console.WriteLine(ans4);
                break;
            }
        }

        Console.WriteLine("功能测试结束");

        Console.WriteLine("==========");
        Console.WriteLine("性能测试开始");
        maxLen = 30000;
        maxValue = 20;
        var aim6 = 60000;
        var arr1 = RandomArray(maxLen, maxValue);

        Utility.RestartStopwatch();
        var ans7 = Dp2(arr1, aim6);
        Console.WriteLine("dp2答案 : " + ans7 + ", dp2运行时间 : " + Utility.GetStopwatchElapsedMilliseconds() + " ms");

        Utility.RestartStopwatch();
        var ans8 = Dp3(arr1, aim6);
        Console.WriteLine("dp3答案 : " + ans8 + ", dp3运行时间 : " + Utility.GetStopwatchElapsedMilliseconds() + " ms");
        Console.WriteLine("性能测试结束");
        Console.WriteLine("===========");
        Console.WriteLine("货币大量重复出现情况下，大数据量测试dp3开始");
        maxLen = 20000000;
        aim6 = 10000;
        maxValue = 10000;
        arr1 = RandomArray(maxLen, maxValue);
        Utility.RestartStopwatch();
        Dp3(arr1, aim6);
        Console.WriteLine("dp3运行" + testTime + "次所用时间 : " + Utility.GetStopwatchElapsedMilliseconds() + " ms");
        Console.WriteLine("大数据量测试dp3结束");
        Console.WriteLine("===========");

        //当货币很少出现重复，dp2比dp3有常数时间优势"
        //当货币大量出现重复，dp3时间复杂度明显优于dp2"
        //dp3的优化用到了窗口内最小值的更新结构"
    }

    private class Info
    {
        public readonly int[] Coins;
        public readonly int[] Count;

        public Info(int[] c, int[] z)
        {
            Coins = c;
            Count = z;
        }
    }
}