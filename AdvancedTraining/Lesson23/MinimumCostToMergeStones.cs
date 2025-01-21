#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson23;

// 本题测试链接 : https://leetcode.cn/problems/minimum-cost-to-merge-stones/
public class MinimumCostToMergeStones
{
    //	// arr[L...R]一定要整出P份，合并的最小代价，返回！
    //	private static int F(int[] arr, int K, int L, int R, int P) {
    //		if(从L到R根本不可能弄出P份) {
    //			return Integer.MAX_VALUE;
    //		}
    //		// 真的有可能的！
    //		if(P == 1) {
    //			return L == R ? 0 : (F(arr, K, L, R, K) + 最后一次大合并的代价);
    //		}
    //		int ans = Integer.MAX_VALUE;
    //		// 真的有可能，P > 1
    //		for(int i = L; i < R;i++) {
    //			// L..i(1份)  i+1...R(P-1份)
    //			int left = F(arr, K, L, i, 1);
    //			if(left == Integer.MAX_VALUE) {
    //				continue;
    //			}
    //			int right = F(arr, K, i+1,R,P-1);
    //			if(right == Integer.MAX_VALUE) {
    //				continue;
    //			}
    //			int all = left + right;
    //			ans = Math.min(ans, all);
    //		}
    //		return ans;
    //	}

    private static int MergeStones1(int[] stones, int k)
    {
        var n = stones.Length;
        if ((n - 1) % (k - 1) > 0) return -1;
        var preSum = new int[n + 1];
        for (var i = 0; i < n; i++) preSum[i + 1] = preSum[i] + stones[i];
        return Process1(0, n - 1, 1, stones, k, preSum);
    }

    // part >= 1
    // arr[L..R] 一定要弄出part份，返回最低代价
    // arr、K、preSum（前缀累加和数组，求i..j的累加和，就是O(1)了）
    private static int Process1(int l, int r, int p, int[] arr, int k, int[] preSum)
    {
        if (l == r)
            // arr[L..R]
            return p == 1 ? 0 : -1;
        // L ... R 不只一个数
        if (p == 1)
        {
            var next = Process1(l, r, k, arr, k, preSum);
            if (next == -1)
                return -1;
            return next + preSum[r + 1] - preSum[l];
        } // P > 1

        var ans = int.MaxValue;
        // L...mid是第1块，剩下的是part-1块
        for (var mid = l; mid < r; mid += k - 1)
        {
            // L..mid(一份) mid+1...R(part - 1)
            var next1 = Process1(l, mid, 1, arr, k, preSum);
            var next2 = Process1(mid + 1, r, p - 1, arr, k, preSum);
            if (next1 != -1 && next2 != -1) ans = Math.Min(ans, next1 + next2);
        }

        return ans;
    }

    private static int MergeStones2(int[] stones, int k)
    {
        var n = stones.Length;
        if ((n - 1) % (k - 1) > 0)
            // n个数，到底能不能K个相邻的数合并，最终变成1个数！
            return -1;
        var preSum = new int[n + 1];
        for (var i = 0; i < n; i++) preSum[i + 1] = preSum[i] + stones[i];
        var dp = new int [n, n, k + 1];
        return Process2(0, n - 1, 1, stones, k, preSum, dp);
    }

    private static int Process2(int l, int r, int p, int[] arr, int k, int[] preSum, int[,,] dp)
    {
        if (dp[l, r, p] != 0) return dp[l, r, p];
        if (l == r) return p == 1 ? 0 : -1;
        if (p == 1)
        {
            var next = Process2(l, r, k, arr, k, preSum, dp);
            if (next == -1)
            {
                dp[l, r, p] = -1;
                return -1;
            }

            dp[l, r, p] = next + preSum[r + 1] - preSum[l];
            return next + preSum[r + 1] - preSum[l];
        }

        var ans = int.MaxValue;
        // i...mid是第1块，剩下的是part-1块
        for (var mid = l; mid < r; mid += k - 1)
        {
            var next1 = Process2(l, mid, 1, arr, k, preSum, dp);
            var next2 = Process2(mid + 1, r, p - 1, arr, k, preSum, dp);
            if (next1 == -1 || next2 == -1)
            {
                dp[l, r, p] = -1;
                return -1;
            }

            ans = Math.Min(ans, next1 + next2);
        }

        dp[l, r, p] = ans;
        return ans;
    }

    //用于测试
    private static int[] GetRandomStringArray(int maxSize, int maxValue)
    {
        var arr = new int[(int)(maxSize * Utility.getRandomDouble) + 1];
        for (var i = 0; i < arr.Length; i++) arr[i] = (int)((maxValue + 1) * Utility.getRandomDouble);
        return arr;
    }

    //用于测试
    private static void PrintArray(int[]? arr)
    {
        if (arr == null) return;
        foreach (var item in arr)
            Console.Write(item + " ");

        Console.WriteLine();
    }

    public static void Run()
    {
        const int maxSize = 12;
        const int maxValue = 100;
        Console.WriteLine("测试开始");
        for (var testTime = 0; testTime < 5000; testTime++)
        {
            var arr = GetRandomStringArray(maxSize, maxValue);
            var k = (int)(Utility.getRandomDouble * 7) + 2;
            var ans1 = MergeStones1(arr, k);
            var ans2 = MergeStones2(arr, k);
            if (ans1 != ans2)
            {
                PrintArray(arr);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
            }
        }
    }
}