#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson41;

// 来自小红书
// 一个无序数组长度为n，所有数字都不一样，并且值都在[0...n-1]范围上
// 返回让这个无序数组变成有序数组的最小交换次数
public class MinSwapTimes
{
    // 纯暴力，arr长度大一点都会超时
    // 但是绝对正确
    private static int MinSwap1(int[] arr)
    {
        return Process1(arr, 0);
    }

    // 让arr变有序，最少的交换次数是多少！返回
    // times, 之前已经做了多少次交换
    private static int Process1(int[] arr, int times)
    {
        var sorted = true;
        for (var i = 1; i < arr.Length; i++)
            if (arr[i - 1] > arr[i])
            {
                sorted = false;
                break;
            }

        if (sorted) return times;
        // 数组现在是无序的状态！
        if (times >= arr.Length - 1) return int.MaxValue;
        var ans = int.MaxValue;
        for (var i = 0; i < arr.Length; i++)
        for (var j = i + 1; j < arr.Length; j++)
        {
            Swap(arr, i, j);
            ans = Math.Min(ans, Process1(arr, times + 1));
            Swap(arr, i, j);
        }

        return ans;
    }

    private static void Swap(int[] arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    // 已知arr中，只有0~n-1这些值，并且都出现1次
    private static int MinSwap2(int[] arr)
    {
        var ans = 0;
        for (var i = 0; i < arr.Length; i++)
            while (i != arr[i])
            {
                Swap(arr, i, arr[i]);
                ans++;
            }

        return ans;
    }

    // 为了测试
    private static int[] randomArray(int len)
    {
        var arr = new int[len];
        for (var i = 0; i < len; i++) arr[i] = i;
        for (var i = 0; i < len; i++) Swap(arr, i, (int)(Utility.GetRandomDouble * len));
        return arr;
    }

    public static void Run()
    {
        var n = 6;
        var testTime = 2000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var len = (int)(Utility.GetRandomDouble * n) + 1;
            var arr = randomArray(len);
            var ans1 = MinSwap1(arr);
            var ans2 = MinSwap2(arr);
            if (ans1 != ans2) Console.WriteLine("出错了!");
        }

        Console.WriteLine("测试结束");
    }
}