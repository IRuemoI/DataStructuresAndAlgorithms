//测试通过

namespace Algorithms.Lesson26;

// 测试链接：https://leetcode.cn/problems/sum-of-subarray-minimums/
// subArrayMinSum1是暴力解
// subArrayMinSum2是最优解的思路
// sumSubarrayMins是最优解思路下的单调栈优化
// Leetcode上只提交sumSubarrayMins方法，时间复杂度O(N)，可以直接通过
public class SumOfSubarrayMinimums
{
    private static int SubArrayMinSum1(int[] arr)
    {
        var ans = 0;
        for (var i = 0; i < arr.Length; i++)
        for (var j = i; j < arr.Length; j++)
        {
            var min = arr[i];
            for (var k = i + 1; k <= j; k++) min = Math.Min(min, arr[k]);

            ans += min;
        }

        return ans;
    }

    // 没有用单调栈
    private static int SubArrayMinSum2(int[] arr)
    {
        // left[i] = x : arr[i]左边，离arr[i]最近，<=arr[i]，位置在x
        var left = LeftNearLessEqual2(arr);
        // right[i] = y : arr[i]右边，离arr[i]最近，< arr[i],的数，位置在y
        var right = RightNearLess2(arr);
        var ans = 0;
        for (var i = 0; i < arr.Length; i++)
        {
            var start = i - left[i];
            var end = right[i] - i;
            ans += start * end * arr[i];
        }

        return ans;
    }

    private static int[] LeftNearLessEqual2(int[] arr)
    {
        var n = arr.Length;
        var left = new int[n];
        for (var i = 0; i < n; i++)
        {
            var ans = -1;
            for (var j = i - 1; j >= 0; j--)
                if (arr[j] <= arr[i])
                {
                    ans = j;
                    break;
                }

            left[i] = ans;
        }

        return left;
    }

    private static int[] RightNearLess2(int[] arr)
    {
        var n = arr.Length;
        var right = new int[n];
        for (var i = 0; i < n; i++)
        {
            var ans = n;
            for (var j = i + 1; j < n; j++)
                if (arr[i] > arr[j])
                {
                    ans = j;
                    break;
                }

            right[i] = ans;
        }

        return right;
    }

    private static int SumSubarrayMinus(int[] arr)
    {
        var left = NearLessEqualLeft(arr);
        var right = NearLessRight(arr);
        long ans = 0;
        for (var i = 0; i < arr.Length; i++)
        {
            long start = i - left[i];
            long end = right[i] - i;
            ans += start * end * arr[i];
            ans %= 1000000007;
        }

        return (int)ans;
    }

    private static int[] NearLessEqualLeft(int[] arr)
    {
        var n = arr.Length;
        var left = new int[n];
        Stack<int> stack = new();
        for (var i = n - 1; i >= 0; i--)
        {
            while (stack.Count != 0 && arr[i] <= arr[stack.Peek()]) left[stack.Pop()] = i;

            stack.Push(i);
        }

        while (stack.Count != 0) left[stack.Pop()] = -1;

        return left;
    }

    private static int[] NearLessRight(int[] arr)
    {
        var n = arr.Length;
        var right = new int[n];
        Stack<int> stack = new();
        for (var i = 0; i < n; i++)
        {
            while (stack.Count != 0 && arr[stack.Peek()] > arr[i]) right[stack.Pop()] = i;

            stack.Push(i);
        }

        while (stack.Count != 0) right[stack.Pop()] = n;

        return right;
    }

    private static int[] RandomArray(int len, int maxValue)
    {
        var ans = new int[len];
        for (var i = 0; i < len; i++) ans[i] = (int)(new Random().NextDouble() * maxValue) + 1;

        return ans;
    }

    private static void PrintArray(int[] arr)
    {
        foreach (var element in arr) Console.Write(element + " ");

        Console.WriteLine();
    }

    public static void Run()
    {
        var maxLen = 100;
        var maxValue = 50;
        var testTime = 100000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var len = (int)(new Random().NextDouble() * maxLen);
            var arr = RandomArray(len, maxValue);
            var ans1 = SubArrayMinSum1(arr);
            var ans2 = SubArrayMinSum2(arr);
            var ans3 = SumSubarrayMinus(arr);
            if (ans1 != ans2 || ans1 != ans3)
            {
                PrintArray(arr);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                Console.WriteLine(ans3);
                Console.WriteLine("出错了！");
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}