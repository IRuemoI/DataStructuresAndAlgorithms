//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson40;

public class LongestLessSumSubArrayLength
{
    private static int MaxLengthAwesome(int[]? arr, int k)
    {
        if (arr == null || arr.Length == 0) return 0;
        var minSums = new int[arr.Length];
        var minSumEnds = new int[arr.Length];
        minSums[arr.Length - 1] = arr[^1];
        minSumEnds[arr.Length - 1] = arr.Length - 1;
        for (var i = arr.Length - 2; i >= 0; i--)
            if (minSums[i + 1] < 0)
            {
                minSums[i] = arr[i] + minSums[i + 1];
                minSumEnds[i] = minSumEnds[i + 1];
            }
            else
            {
                minSums[i] = arr[i];
                minSumEnds[i] = i;
            }

        // 迟迟扩不进来那一块儿的开头位置
        var end = 0;
        var sum = 0;
        var ans = 0;
        for (var i = 0; i < arr.Length; i++)
        {
            // while循环结束之后：
            // 1) 如果以i开头的情况下，累加和<=k的最长子数组是arr[i..end-1]，看看这个子数组长度能不能更新res；
            // 2) 如果以i开头的情况下，累加和<=k的最长子数组比arr[i..end-1]短，更新还是不更新res都不会影响最终结果；
            while (end < arr.Length && sum + minSums[end] <= k)
            {
                sum += minSums[end];
                end = minSumEnds[end] + 1;
            }

            ans = Math.Max(ans, end - i);
            if (end > i)
                // 还有窗口，哪怕窗口没有数字 [i~end) [4,4)
                sum -= arr[i];
            else
                // i == end,  即将 i++, i > end, 此时窗口概念维持不住了，所以end跟着i一起走
                end = i + 1;
        }

        return ans;
    }

    private static int MaxLength(int[] arr, int k)
    {
        var h = new int[arr.Length + 1];
        var sum = 0;
        h[0] = sum;
        for (var i = 0; i != arr.Length; i++)
        {
            sum += arr[i];
            h[i + 1] = Math.Max(sum, h[i]);
        }

        sum = 0;
        var res = 0;
        for (var i = 0; i != arr.Length; i++)
        {
            sum += arr[i];
            var pre = GetLessIndex(h, sum - k);
            var len = pre == -1 ? 0 : i - pre + 1;
            res = Math.Max(res, len);
        }

        return res;
    }

    private static int GetLessIndex(int[] arr, int num)
    {
        var low = 0;
        var high = arr.Length - 1;
        var res = -1;
        while (low <= high)
        {
            var mid = (low + high) / 2;
            if (arr[mid] >= num)
            {
                res = mid;
                high = mid - 1;
            }
            else
            {
                low = mid + 1;
            }
        }

        return res;
    }

    //用于测试
    private static int[] GenerateRandomArray(int len, int maxValue)
    {
        var res = new int[len];
        for (var i = 0; i != res.Length; i++) res[i] = (int)(Utility.GetRandomDouble * maxValue) - maxValue / 3;
        return res;
    }

    public static void Run()
    {
        Console.WriteLine("测试开始");
        for (var i = 0; i < 10000000; i++)
        {
            var arr = GenerateRandomArray(10, 20);
            var k = (int)(Utility.GetRandomDouble * 20) - 5;
            if (MaxLengthAwesome(arr, k) != MaxLength(arr, k)) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");
    }
}