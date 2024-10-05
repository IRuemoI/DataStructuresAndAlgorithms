#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson39;

// 来自腾讯
// 给定一个长度为n的数组arr，求有多少个子数组满足 : 
// 子数组两端的值，是这个子数组的最小值和次小值，最小值和次小值谁在最左和最右无所谓
// n<=100000（10^5） n*logn  O(N)
public class ValidSequence
{
    private static int Numbers(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;
        var n = arr.Length;
        var values = new int[n];
        var times = new int[n];
        var size = 0;
        var ans = 0;
        foreach (var item in arr)
        {
            while (size != 0 && values[size - 1] > item)
            {
                size--;
                ans += times[size] + Cn2(times[size]);
            }

            if (size != 0 && values[size - 1] == item)
            {
                times[size - 1]++;
            }
            else
            {
                values[size] = item;
                times[size++] = 1;
            }
        }

        while (size != 0) ans += Cn2(times[--size]);
        for (var i = arr.Length - 1; i >= 0; i--)
        {
            while (size != 0 && values[size - 1] > arr[i]) ans += times[--size];
            if (size != 0 && values[size - 1] == arr[i])
            {
                times[size - 1]++;
            }
            else
            {
                values[size] = arr[i];
                times[size++] = 1;
            }
        }

        return ans;
    }

    private static int Cn2(int n)
    {
        return (n * (n - 1)) >> 1;
    }

    // 为了测试
    // 暴力方法
    private static int Test(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;
        var ans = 0;
        for (var s = 0; s < arr.Length; s++)
        for (var e = s + 1; e < arr.Length; e++)
        {
            var max = Math.Max(arr[s], arr[e]);
            var valid = true;
            for (var i = s + 1; i < e; i++)
                if (arr[i] < max)
                {
                    valid = false;
                    break;
                }

            ans += valid ? 1 : 0;
        }

        return ans;
    }

    // 为了测试
    private static int[] randomArray(int n, int v)
    {
        var arr = new int[n];
        for (var i = 0; i < n; i++) arr[i] = (int)(Utility.getRandomDouble * v);
        return arr;
    }

    // 为了测试
    private static void PrintArray(int[] arr)
    {
        foreach (var num in arr) Console.Write(num + " ");
        Console.WriteLine();
    }

    // 为了测试
    public static void Run()
    {
        const int n = 30;
        const int v = 30;
        const int testTime = 100000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var m = (int)(Utility.getRandomDouble * n);
            var arr = randomArray(m, v);
            var ans1 = Numbers(arr);
            var ans2 = Test(arr);
            if (ans1 != ans2)
            {
                Console.WriteLine("出错了!");
                PrintArray(arr);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
            }
        }

        Console.WriteLine("测试结束");
    }
}