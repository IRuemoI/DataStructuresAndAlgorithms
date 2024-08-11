namespace AdvancedTraining.Lesson22;

// 本题测试链接 : https://leetcode.cn/problems/maximum-sum-of-3-non-overlapping-subarrays/
public class MaximumSumOf3NonOverlappingSubArrays
{
    //	private static int[] maxSumArray1(int[] arr) {
    //		int N = arr.length;
    //		int[] help = new int[N];
    //		// help[i] 子数组必须以i位置结尾的情况下，累加和最大是多少？
    //		help[0] = arr[0];
    //		for (int i = 1; i < N; i++) {
    //			int p1 = arr[i];
    //			int p2 = arr[i] + help[i - 1];
    //			help[i] = Math.max(p1, p2);
    //		}
    //		// dp[i] 在0~i范围上，随意选一个子数组，累加和最大是多少？
    //		int[] dp = new int[N];
    //		dp[0] = help[0];
    //		for (int i = 1; i < N; i++) {
    //			int p1 = help[i];
    //			int p2 = dp[i - 1];
    //			dp[i] = Math.max(p1, p2);
    //		}
    //		return dp;
    //	}
    //
    //	private static int MaxSumLenK(int[] arr, int k) {
    //		int N = arr.length;
    //		// 子数组必须以i位置的数结尾，长度一定要是K，累加和最大是多少？
    //		// help[0] help[k-2] ...
    //		int sum = 0;
    //		for (int i = 0; i < k - 1; i++) {
    //			sum += arr[i];
    //		}
    //		// 0...k-2 k-1 sum
    //		int[] help = new int[N];
    //		for (int i = k - 1; i < N; i++) {
    //			// 0..k-2 
    //			// 01..k-1
    //			sum += arr[i];
    //			help[i] = sum;
    //			// i == k-1  
    //			sum -= arr[i - k + 1];
    //		}
    //		// help[i] - > dp[i]  0-..i  K
    //
    //	}

    private static int[] maxSumOfThreeSubArrays(int[] numbers, int k)
    {
        var n = numbers.Length;
        var range = new int[n];
        var left = new int[n];
        var sum = 0;
        for (var i = 0; i < k; i++) sum += numbers[i];
        range[0] = sum;
        left[k - 1] = 0;
        var max = sum;
        for (var i = k; i < n; i++)
        {
            sum = sum - numbers[i - k] + numbers[i];
            range[i - k + 1] = sum;
            left[i] = left[i - 1];
            if (sum > max)
            {
                max = sum;
                left[i] = i - k + 1;
            }
        }

        sum = 0;
        for (var i = n - 1; i >= n - k; i--) sum += numbers[i];
        max = sum;
        var right = new int[n];
        right[n - k] = n - k;
        for (var i = n - k - 1; i >= 0; i--)
        {
            sum = sum - numbers[i + k] + numbers[i];
            right[i] = right[i + 1];
            if (sum >= max)
            {
                max = sum;
                right[i] = i;
            }
        }

        var a = 0;
        var b = 0;
        var c = 0;
        max = 0;
        for (var i = k; i < n - 2 * k + 1; i++)
        {
            // 中间一块的起始点 (0...k-1)选不了 i == N-1
            var part1 = range[left[i - 1]];
            var part2 = range[i];
            var part3 = range[right[i + k]];
            if (part1 + part2 + part3 > max)
            {
                max = part1 + part2 + part3;
                a = left[i - 1];
                b = i;
                c = right[i + k];
            }
        }

        return [a, b, c];
    }

    public static void Run()
    {
        foreach (var item in maxSumOfThreeSubArrays([1, 2, 1, 2, 6, 7, 5, 1], 2)) Console.Write(item + ","); //输出[0,3,5]
    }
}