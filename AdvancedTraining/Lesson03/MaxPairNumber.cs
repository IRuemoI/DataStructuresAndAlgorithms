//pass
#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson03;

// 给定一个数组arr，代表每个人的能力值。再给定一个非负数k。
// 如果两个人能力差值正好为k，那么可以凑在一起比赛，一局比赛只有两个人
// 返回最多可以同时有多少场比赛
public class MaxPairNumber
{
    // 暴力解
    private static int MaxPairNum1(int[] arr, int k)
    {
        if (k < 0) return -1;
        return Process1(arr, 0, k);
    }

    private static int Process1(int[] arr, int index, int k)
    {
        var ans = 0;
        if (index == arr.Length)
        {
            for (var i = 1; i < arr.Length; i += 2)
                if (arr[i] - arr[i - 1] == k)
                    ans++;
        }
        else
        {
            for (var r = index; r < arr.Length; r++)
            {
                Swap(arr, index, r);
                ans = Math.Max(ans, Process1(arr, index + 1, k));
                Swap(arr, index, r);
            }
        }

        return ans;
    }

    private static void Swap(int[] arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    // 时间复杂度O(N*logN)
    private static int MaxPairNum2(int[]? arr, int k)
    {
        if (k < 0 || arr == null || arr.Length < 2) return 0;
        Array.Sort(arr);
        var ans = 0;
        var n = arr.Length;
        var l = 0;
        var r = 0;
        var usedR = new bool[n];
        while (l < n && r < n)
            if (usedR[l])
            {
                l++;
            }
            else if (l >= r)
            {
                r++;
            }
            else
            {
                // 不止一个数，而且都没用过！
                var distance = arr[r] - arr[l];
                if (distance == k)
                {
                    ans++;
                    usedR[r++] = true;
                    l++;
                }
                else if (distance < k)
                {
                    r++;
                }
                else
                {
                    l++;
                }
            }

        return ans;
    }

    // 为了测试
    private static int[] randomArray(int len, int value)
    {
        var arr = new int[len];
        for (var i = 0; i < arr.Length; i++) arr[i] = (int)(Utility.getRandomDouble * value);
        return arr;
    }

    // 为了测试
    private static void PrintArray(int[] arr)
    {
        foreach (var item in arr) Console.Write(item + " ");

        Console.WriteLine();
    }

    // 为了测试
    private static int[] copyArray(int[] arr)
    {
        var ans = new int[arr.Length];
        for (var i = 0; i < arr.Length; i++) ans[i] = arr[i];
        return ans;
    }

    public static void Run()
    {
        const int maxLen = 10;
        const int maxValue = 20;
        const int maxK = 5;
        const int testTime = 100;
        Console.WriteLine("功能测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var n = (int)(Utility.getRandomDouble * (maxLen + 1));
            var arr = randomArray(n, maxValue);
            var arr1 = copyArray(arr);
            var arr2 = copyArray(arr);
            var k = (int)(Utility.getRandomDouble * (maxK + 1));
            var ans1 = MaxPairNum1(arr1, k);
            var ans2 = MaxPairNum2(arr2, k);
            if (ans1 != ans2)
            {
                Console.WriteLine("出错啦！");
                PrintArray(arr);
                Console.WriteLine(k);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                break;
            }
        }

        Console.WriteLine("功能测试结束");
    }
}