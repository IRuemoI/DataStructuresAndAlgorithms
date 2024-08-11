//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson47;

// 整型数组arr长度为n(3 <= n <= 10^4)，最初每个数字是<=200的正数且满足如下条件：
// 1. 0位置的要求：arr[0]<=arr[1] 
// 2. n-1位置的要求：arr[n-1]<=arr[n-2]
// 3. 中间i位置的要求：arr[i]<=max(arr[i-1],arr[i+1]) 
// 但是在arr有些数字丢失了，比如k位置的数字之前是正数，丢失之后k位置的数字为0
// 请你根据上述条件，计算可能有多少种不同的arr可以满足以上条件
// 比如 [6,0,9] 只有还原成 [6,9,9]满足全部三个条件，所以返回1种，即[6,9,9]达标
public class RestoreWays
{
    private static int Ways0(int[] arr)
    {
        return Process0(arr, 0);
    }

    private static int Process0(int[] arr, int index)
    {
        if (index == arr.Length) return IsValid(arr) ? 1 : 0;

        if (arr[index] != 0) return Process0(arr, index + 1);

        var ways = 0;
        for (var v = 1; v < 201; v++)
        {
            arr[index] = v;
            ways += Process0(arr, index + 1);
        }

        arr[index] = 0;
        return ways;
    }

    private static bool IsValid(int[] arr)
    {
        if (arr[0] > arr[1]) return false;

        if (arr[^1] > arr[^2]) return false;

        for (var i = 1; i < arr.Length - 1; i++)
            if (arr[i] > Math.Max(arr[i - 1], arr[i + 1]))
                return false;

        return true;
    }

    private static int Ways1(int[] arr)
    {
        var n = arr.Length;
        if (arr[n - 1] != 0) return Process1(arr, n - 1, arr[n - 1], 2);

        var ways = 0;
        for (var v = 1; v < 201; v++) ways += Process1(arr, n - 1, v, 2);

        return ways;
    }

    // 如果i位置的数字变成了v,
    // 并且arr[i]和arr[i+1]的关系为s，
    // s==0，代表arr[i] < arr[i+1] 右大
    // s==1，代表arr[i] == arr[i+1] 右=当前
    // s==2，代表arr[i] > arr[i+1] 右小
    // 返回0...i范围上有多少种有效的转化方式？
    private static int Process1(int[] arr, int i, int v, int s)
    {
        if (i == 0)
            // 0...i 只剩一个数了，0...0
            return (s == 0 || s == 1) && (arr[0] == 0 || v == arr[0]) ? 1 : 0;

        // i > 0
        if (arr[i] != 0 && v != arr[i]) return 0;

        // i>0 ，并且， i位置的数真的可以变成V，
        var ways = 0;
        if (s == 0 || s == 1)
            // [i] -> V <= [i+1]
            for (var pre = 1; pre < 201; pre++)
                ways += Process1(arr, i - 1, pre, pre < v ? 0 : pre == v ? 1 : 2);
        else
            // ? 当前 > 右 当前 <= max{左，右}
            for (var pre = v; pre < 201; pre++)
                ways += Process1(arr, i - 1, pre, pre == v ? 1 : 2);

        return ways;
    }

    private static int Zuo(int[] arr, int i, int v, int s)
    {
        if (i == 0)
            // 0...i 只剩一个数了，0...0
            return (s == 0 || s == 1) && (arr[0] == 0 || v == arr[0]) ? 1 : 0;

        // i > 0
        if (arr[i] != 0 && v != arr[i]) return 0;

        // i>0 ，并且， i位置的数真的可以变成V，
        var ways = 0;
        if (s == 0 || s == 1)
            // [i] -> V <= [i+1]
            for (var pre = 1; pre < v; pre++)
                ways += Zuo(arr, i - 1, pre, 0);

        ways += Zuo(arr, i - 1, v, 1);
        for (var pre = v + 1; pre < 201; pre++) ways += Zuo(arr, i - 1, pre, 2);

        return ways;
    }

    private static int Ways2(int[] arr)
    {
        var n = arr.Length;
        var dp = new int[n, 201, 3];
        if (arr[0] != 0)
        {
            dp[0, arr[0], 0] = 1;
            dp[0, arr[0], 1] = 1;
        }
        else
        {
            for (var v = 1; v < 201; v++)
            {
                dp[0, v, 0] = 1;
                dp[0, v, 1] = 1;
            }
        }

        for (var i = 1; i < n; i++)
        for (var v = 1; v < 201; v++)
        for (var s = 0; s < 3; s++)
            if (arr[i] == 0 || v == arr[i])
            {
                if (s == 0 || s == 1)
                    for (var pre = 1; pre < v; pre++)
                        dp[i, v, s] += dp[i - 1, pre, 0];

                dp[i, v, s] += dp[i - 1, v, 1];
                for (var pre = v + 1; pre < 201; pre++) dp[i, v, s] += dp[i - 1, pre, 2];
            }

        if (arr[n - 1] != 0) return dp[n - 1, arr[n - 1], 2];

        var ways = 0;
        for (var v = 1; v < 201; v++) ways += dp[n - 1, v, 2];

        return ways;
    }

    private static int Ways3(int[] arr)
    {
        var n = arr.Length;
        var dp = new int [n, 201, 3];
        if (arr[0] != 0)
        {
            dp[0, arr[0], 0] = 1;
            dp[0, arr[0], 1] = 1;
        }
        else
        {
            for (var v = 1; v < 201; v++)
            {
                dp[0, v, 0] = 1;
                dp[0, v, 1] = 1;
            }
        }

        var preSum = new int [201, 3];
        for (var v = 1; v < 201; v++)
        for (var s = 0; s < 3; s++)
            preSum[v, s] = preSum[v - 1, s] + dp[0, v, s];

        for (var i = 1; i < n; i++)
        {
            for (var v = 1; v < 201; v++)
            for (var s = 0; s < 3; s++)
                if (arr[i] == 0 || v == arr[i])
                {
                    if (s == 0 || s == 1) dp[i, v, s] += Sum(1, v - 1, 0, preSum);

                    dp[i, v, s] += dp[i - 1, v, 1];
                    dp[i, v, s] += Sum(v + 1, 200, 2, preSum);
                }

            for (var v = 1; v < 201; v++)
            for (var s = 0; s < 3; s++)
                preSum[v, s] = preSum[v - 1, s] + dp[i, v, s];
        }

        if (arr[n - 1] != 0)
            return dp[n - 1, arr[n - 1], 2];
        return Sum(1, 200, 2, preSum);
    }

    private static int Sum(int begin, int end, int relation, int[,] preSum)
    {
        return preSum[end, relation] - preSum[begin - 1, relation];
    }

    //用于测试
    private static int[] GenerateRandomArray(int len)
    {
        var ans = new int[len];
        for (var i = 0; i < ans.Length; i++)
            if (Utility.GetRandomDouble < 0.5)
                ans[i] = 0;
            else
                ans[i] = (int)(Utility.GetRandomDouble * 200) + 1;

        return ans;
    }

    //用于测试
    private static void PrintArray(int[] arr)
    {
        Console.WriteLine("arr size : " + arr.Length);
        foreach (var element in arr)
            Console.Write(element + " ");

        Console.WriteLine();
    }

    public static void Run()
    {
        var len = 4;
        var testTime = 15;
        Console.WriteLine("功能测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var n1 = (int)(Utility.GetRandomDouble * len) + 2;
            var arrA = GenerateRandomArray(n1);
            var ans0 = Ways0(arrA);
            var ans1 = Ways1(arrA);
            var ans2 = Ways2(arrA);
            var ans3 = Ways3(arrA);
            if (ans0 != ans1 || ans2 != ans3 || ans0 != ans2) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("功能测试结束");
        Console.WriteLine("===========");
        var n2 = 100000;
        var arr = GenerateRandomArray(n2);

        Utility.RestartStopwatch();
        Ways3(arr);
        Console.WriteLine("run time : " + Utility.GetStopwatchElapsedMilliseconds() + " ms");
    }
}