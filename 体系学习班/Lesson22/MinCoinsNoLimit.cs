//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson22;

public class MinCoinsNoLimit
{
    private static int MinCoins(int[] arr, int aim)
    {
        return Process(arr, 0, aim);
    }

    // arr[index...]面值，每种面值张数自由选择，
    // 搞出rest正好这么多钱，返回最小张数
    private static int Process(int[] arr, int index, int rest)
    {
        //rest不可能小于0，因为上游已经控制了
        //if (rest < 0) { //钱数小于0时，认为无穷多张才能组成它，用最大值标记无效解
        //    return Integer.MAX_VALUE;
        //}

        //没钱了，且rest=0，那么只需要0张组成rest
        if (index == arr.Length) return rest == 0 ? 0 : int.MaxValue;

        var ans = int.MaxValue;
        //index位置的面值从0张开始尝试
        for (var zhang = 0; zhang * arr[index] <= rest; zhang++)
        {
            //当前index位置已经做了决定，接下来该index+1位置及其往后的面值做决定
            //剩下的面值搞定剩下的钱的最小张数
            var next = Process(arr, index + 1, rest - zhang * arr[index]);
            if (next != int.MaxValue) ans = Math.Min(ans, zhang + next);
        }

        return ans;
    }

    private static int Dp1(int[] arr, int aim)
    {
        if (aim == 0) return 0;

        //index: 0~n
        //rest: 0~aim
        var n = arr.Length;
        var dp = new int[n + 1, aim + 1];
        dp[n, 0] = 0;
        for (var j = 1; j <= aim; j++) dp[n, j] = int.MaxValue;

        for (var index = n - 1; index >= 0; index--)
        for (var rest = 0; rest <= aim; rest++)
        {
            var ans = int.MaxValue;
            for (var zhang = 0; zhang * arr[index] <= rest; zhang++)
            {
                var next = dp[index + 1, rest - zhang * arr[index]];
                if (next != int.MaxValue) ans = Math.Min(ans, zhang + next);
            }

            dp[index, rest] = ans;
        }

        return dp[0, aim];
    }

    private static int Dp2(int[] arr, int aim)
    {
        if (aim == 0) return 0;

        var n = arr.Length;
        var dp = new int[n + 1, aim + 1];
        dp[n, 0] = 0;
        for (var j = 1; j <= aim; j++) dp[n, j] = int.MaxValue;

        for (var index = n - 1; index >= 0; index--)
        for (var rest = 0; rest <= aim; rest++)
        {
            dp[index, rest] = dp[index + 1, rest];
            if (rest - arr[index] >= 0 && dp[index, rest - arr[index]] != int.MaxValue)
                dp[index, rest] = Math.Min(dp[index, rest], dp[index, rest - arr[index]] + 1);
        }

        return dp[0, aim];
    }

    // 为了测试
    private static int[] RandomArray(int maxLen, int maxValue)
    {
        var n = (int)(Utility.getRandomDouble * maxLen);
        var arr = new int[n];
        var has = new bool[maxValue + 1];
        for (var i = 0; i < n; i++)
        {
            do
            {
                arr[i] = (int)(Utility.getRandomDouble * maxValue) + 1;
            } while (has[arr[i]]);

            has[arr[i]] = true;
        }

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

        Console.WriteLine("功能测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var n = (int)(Utility.getRandomDouble * maxLen);
            var arr = RandomArray(n, maxValue);
            var aim = (int)(Utility.getRandomDouble * maxValue);
            var ans1 = MinCoins(arr, aim);
            var ans2 = Dp1(arr, aim);
            var ans3 = Dp2(arr, aim);
            if (ans1 != ans2 || ans1 != ans3)
            {
                Console.WriteLine("出错啦！");
                PrintArray(arr);
                Console.WriteLine(aim);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                break;
            }
        }

        Console.WriteLine("功能测试结束");
    }
}