//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson39;

// 这道题是一个小小的补充，课上没有讲
// 但是如果你听过体系学习班动态规划专题和本节课的话
// 这道题就是一道水题
public static class IsSum
{
    // arr中的值可能为正，可能为负，可能为0
    // 自由选择arr中的数字，能不能累加得到sum
    // 暴力递归方法
    public static bool IsSum1(int[]? arr, int sum)
    {
        if (sum == 0) return true;
        if (arr == null || arr.Length == 0) return false;
        return Process1(arr, arr.Length - 1, sum);
    }

    // 可以自由使用arr[0...i]上的数字，能不能累加得到sum
    private static bool Process1(int[] arr, int i, int sum)
    {
        if (sum == 0) return true;
        if (i == -1) return false;
        return Process1(arr, i - 1, sum) || Process1(arr, i - 1, sum - arr[i]);
    }

    // arr中的值可能为正，可能为负，可能为0
    // 自由选择arr中的数字，能不能累加得到sum
    // 记忆化搜索方法
    // 从暴力递归方法来，加了记忆化缓存，就是动态规划了
    public static bool IsSum2(int[]? arr, int sum)
    {
        if (sum == 0) return true;
        if (arr == null || arr.Length == 0) return false;
        return Process2(arr, arr.Length - 1, sum, new Dictionary<int, Dictionary<int, bool>>());
    }

    private static bool Process2(int[] arr, int i, int sum, Dictionary<int, Dictionary<int, bool>> dp)
    {
        if (dp.ContainsKey(i) && dp[i].ContainsKey(sum)) return dp[i][sum];
        var ans = false;
        if (sum == 0)
            ans = true;
        else if (i != -1) ans = Process2(arr, i - 1, sum, dp) || Process2(arr, i - 1, sum - arr[i], dp);
        if (!dp.ContainsKey(i)) dp[i] = new Dictionary<int, bool>();
        dp[i][sum] = ans;
        return ans;
    }

    // arr中的值可能为正，可能为负，可能为0
    // 自由选择arr中的数字，能不能累加得到sum
    // 经典动态规划
    public static bool IsSum3(int[]? arr, int sum)
    {
        if (sum == 0) return true;
        if (arr == null || arr.Length == 0) return false;
        var min = 0;
        var max = 0;
        foreach (var num in arr)
        {
            min += num < 0 ? num : 0;
            max += num > 0 ? num : 0;
        }

        if (sum < min || sum > max) return false;
        var n = arr.Length;
        var dp = new bool[n, max - min + 1];
        dp[0, -min] = true;
        dp[0, arr[0] - min] = true;
        for (var i = 1; i < n; i++)
        for (var j = min; j <= max; j++)
        {
            dp[i, j - min] = dp[i - 1, j - min];
            var next = j - min - arr[i];
            dp[i, j - min] |= next >= 0 && next <= max - min && dp[i - 1, next];
        }

        return dp[n - 1, sum - min];
    }

    // arr中的值可能为正，可能为负，可能为0
    // 自由选择arr中的数字，能不能累加得到sum
    // 分治的方法
    // 如果arr中的数值特别大，动态规划方法依然会很慢
    // 此时如果arr的数字个数不算多(40以内)，哪怕其中的数值很大，分治的方法也将是最优解
    public static bool IsSum4(int[]? arr, int sum)
    {
        if (sum == 0) return true;
        if (arr == null || arr.Length == 0) return false;
        if (arr.Length == 1) return arr[0] == sum;
        var n = arr.Length;
        var mid = n >> 1;
        var leftSum = new HashSet<int>();
        var rightSum = new HashSet<int>();
        Process4(arr, 0, mid, 0, leftSum);
        Process4(arr, mid, n, 0, rightSum);
        foreach (var l in leftSum)
            if (rightSum.Contains(sum - l))
                return true;
        return false;
    }

    private static void Process4(int[] arr, int i, int end, int pre, HashSet<int> ans)
    {
        if (i == end)
        {
            ans.Add(pre);
        }
        else
        {
            Process4(arr, i + 1, end, pre, ans);
            Process4(arr, i + 1, end, pre + arr[i], ans);
        }
    }
}

public class IsSumTestTest
{
    // 为了测试
    // 生成长度为len的随机数组
    // 值在[-max, max]上随机
    private static int[] RandomArray(int len, int max)
    {
        var arr = new int[len];
        for (var i = 0; i < len; i++) arr[i] = (int)(Utility.GetRandomDouble * ((max << 1) + 1)) - max;
        return arr;
    }

    // 对数器验证所有方法
    public static void Run()
    {
        const int n = 20;
        const int m = 100;
        var testTime = 100000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var size = (int)(Utility.GetRandomDouble * (n + 1));
            var arr = RandomArray(size, m);
            var sum = (int)(Utility.GetRandomDouble * ((m << 1) + 1)) - m;
            var ans1 = IsSum.IsSum1(arr, sum);
            var ans2 = IsSum.IsSum2(arr, sum);
            var ans3 = IsSum.IsSum3(arr, sum);
            var ans4 = IsSum.IsSum4(arr, sum);
            if (ans1 ^ ans2 || ans3 ^ ans4 || ans1 ^ ans3)
            {
                Console.WriteLine("出错了！");
                Console.Write("arr : ");
                foreach (var num in arr) Console.Write(num + " ");
                Console.WriteLine();
                Console.WriteLine("sum : " + sum);
                Console.WriteLine("方法一答案 : " + ans1);
                Console.WriteLine("方法二答案 : " + ans2);
                Console.WriteLine("方法三答案 : " + ans3);
                Console.WriteLine("方法四答案 : " + ans4);
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}