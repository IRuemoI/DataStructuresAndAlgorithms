#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson48;

// 来自学员问题
// 比如{ 5, 3, 1, 4 }
// 全部数字对是：(5,3)、(5,1)、(5,4)、(3,1)、(3,4)、(1,4)
// 数字对的差值绝对值： 2、4、1、2、1、3
// 差值绝对值排序后：1、1、2、2、3、4
// 给定一个数组arr，和一个正数k
// 返回arr中所有数字对差值的绝对值，第k小的是多少
// arr = { 5, 3, 1, 4 }, k = 4
// 返回2
public class MinKthPairMinusAbs
{
    // 暴力解，生成所有数字对差值绝对值，排序，拿出第k个，k从1开始
    private static int KthAbs1(int[] arr, int k)
    {
        var n = arr.Length;
        var m = ((n - 1) * n) >> 1;
        if (m == 0 || k < 1 || k > m) return -1;
        var abs = new int[m];
        var size = 0;
        for (var i = 0; i < n; i++)
        for (var j = i + 1; j < n; j++)
            abs[size++] = Math.Abs(arr[i] - arr[j]);
        Array.Sort(abs);
        return abs[k - 1];
    }

    // 二分 + 不回退
    private static int KthAbs2(int[] arr, int k)
    {
        var n = arr.Length;
        if (n < 2 || k < 1 || k > (n * (n - 1)) >> 1) return -1;
        Array.Sort(arr);
        // 0 ~ 大-小 二分
        // l  ~  r
        var left = 0;
        var right = arr[n - 1] - arr[0];
        var rightest = -1;
        while (left <= right)
        {
            var mid = (left + right) / 2;
            // 数字对差值的绝对值<=mid的数字对个数，是不是 < k个的！
            if (Valid(arr, mid, k))
            {
                rightest = mid;
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        return rightest + 1;
    }

    // 假设arr中的所有数字对，差值绝对值<=limit的个数为x
    // 如果 x < k，达标，返回true
    // 如果 x >= k，不达标，返回false
    private static bool Valid(int[] arr, int limit, int k)
    {
        var x = 0;
        for (int l = 0, r = 1; l < arr.Length; r = Math.Max(r, ++l))
        {
            while (r < arr.Length && arr[r] - arr[l] <= limit) r++;
            x += r - l - 1;
        }

        return x < k;
    }

    // 为了测试
    private static int[] randomArray(int n, int v)
    {
        var ans = new int[n];
        for (var i = 0; i < ans.Length; i++) ans[i] = (int)(Utility.GetRandomDouble * v);
        return ans;
    }

    // 为了测试
    public static void Run()
    {
        var size = 100;
        var value = 100;
        var testTime = 10000;

        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var n1 = (int)(Utility.GetRandomDouble * size);
            var k1 = (int)(Utility.GetRandomDouble * (n1 * (n1 - 1) / 2)) + 1;
            var arr1 = randomArray(n1, value);
            var ans1 = KthAbs1(arr1, k1);
            var ans2 = KthAbs2(arr1, k1);
            if (ans1 != ans2)
            {
                Console.Write("arr : ");
                foreach (var num in arr1) Console.Write(num + " ");
                Console.WriteLine();
                Console.WriteLine("k : " + k1);
                Console.WriteLine("ans1 : " + ans1);
                Console.WriteLine("ans2 : " + ans2);
                Console.WriteLine("出错了！");
                break;
            }
        }

        Console.WriteLine("测试结束");
        var n = 500000;
        var v = 50000000;
        var arr = randomArray(n, v);
        var k = (int)(Utility.GetRandomDouble * (n * (n - 1) / 2)) + 1;
        Utility.RestartStopwatch();
        KthAbs2(arr, k);

        Console.WriteLine("大数据量运行时间（毫秒）：" + Utility.GetStopwatchElapsedMilliseconds());
    }
}