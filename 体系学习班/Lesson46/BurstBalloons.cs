//测试通过

namespace Algorithms.Lesson46;

// 本题测试链接 : https://leetcode.cn/problems/burst-balloons/
public class BurstBalloons
{
    private static int MaxCoins0(int[] arr)
    {
        // [3,2,1,3]
        // [1,3,2,1,3,1]
        var n = arr.Length;
        var help = new int[n + 2];
        for (var i = 0; i < n; i++) help[i + 1] = arr[i];
        help[0] = 1;
        help[n + 1] = 1;
        return Func(help, 1, n);
    }

    // L-1位置，和R+1位置，永远不越界，并且，[L-1] 和 [R+1] 一定没爆呢！
    // 返回，arr[L...R]打爆所有气球，最大得分是什么
    private static int Func(int[] arr, int l, int r)
    {
        if (l == r) return arr[l - 1] * arr[l] * arr[r + 1];
        // 尝试每一种情况，最后打爆的气球，是什么位置
        // L...R
        // L位置的气球，最后打爆
        var max = Func(arr, l + 1, r) + arr[l - 1] * arr[l] * arr[r + 1];
        // R位置的气球，最后打爆
        max = Math.Max(max, Func(arr, l, r - 1) + arr[l - 1] * arr[r] * arr[r + 1]);
        // 尝试所有L...R，中间的位置，(L,R)
        for (var i = l + 1; i < r; i++)
        {
            // i位置的气球，最后打爆
            var left = Func(arr, l, i - 1);
            var right = Func(arr, i + 1, r);
            var last = arr[l - 1] * arr[i] * arr[r + 1];
            var cur = left + right + last;
            max = Math.Max(max, cur);
        }

        return max;
    }

    private static int MaxCoins1(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        if (arr.Length == 1) return arr[0];
        var n = arr.Length;
        var help = new int[n + 2];
        help[0] = 1;
        help[n + 1] = 1;
        for (var i = 0; i < n; i++) help[i + 1] = arr[i];
        return Process(help, 1, n);
    }

    // 打爆arr[L..R]范围上的所有气球，返回最大的分数
    // 假设arr[L-1]和arr[R+1]一定没有被打爆
    private static int Process(int[] arr, int l, int r)
    {
        if (l == r)
            // 如果arr[L..R]范围上只有一个气球，直接打爆即可
            return arr[l - 1] * arr[l] * arr[r + 1];
        // 最后打爆arr[L]的方案，和最后打爆arr[R]的方案，先比较一下
        var max = Math.Max(arr[l - 1] * arr[l] * arr[r + 1] + Process(arr, l + 1, r),
            arr[l - 1] * arr[r] * arr[r + 1] + Process(arr, l, r - 1));
        // 尝试中间位置的气球最后被打爆的每一种方案
        for (var i = l + 1; i < r; i++)
            max = Math.Max(max, arr[l - 1] * arr[i] * arr[r + 1] + Process(arr, l, i - 1) + Process(arr, i + 1, r));
        return max;
    }

    private static int MaxCoins2(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        if (arr.Length == 1) return arr[0];
        var n = arr.Length;
        var help = new int[n + 2];
        help[0] = 1;
        help[n + 1] = 1;
        for (var i = 0; i < n; i++) help[i + 1] = arr[i];
        var dp = new int[n + 2, n + 2];
        for (var i = 1; i <= n; i++) dp[i, i] = help[i - 1] * help[i] * help[i + 1];
        for (var l = n; l >= 1; l--)
        for (var r = l + 1; r <= n; r++)
        {
            var ans = help[l - 1] * help[l] * help[r + 1] + dp[l + 1, r];
            ans = Math.Max(ans, help[l - 1] * help[r] * help[r + 1] + dp[l, r - 1]);
            for (var i = l + 1; i < r; i++)
                ans = Math.Max(ans, help[l - 1] * help[i] * help[r + 1] + dp[l, i - 1] + dp[i + 1, r]);
            dp[l, r] = ans;
        }

        return dp[1, n];
    }

    public static void Run()
    {
        int[] nums1 = [3, 1, 5, 8];
        int[] nums2 = [1, 5];

        Console.WriteLine(MaxCoins1(nums1));
        Console.WriteLine(MaxCoins2(nums1));

        Console.WriteLine("----------------");

        Console.WriteLine(MaxCoins1(nums2));
        Console.WriteLine(MaxCoins2(nums2));
    }
}