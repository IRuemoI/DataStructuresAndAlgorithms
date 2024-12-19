#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson40;

// 腾讯
// 分裂问题
// 一个数n，可以分裂成一个数组[n/2, n%2, n/2]
// 这个数组中哪个数不是1或者0，就继续分裂下去
// 比如 n = 5，一开始分裂成[2, 1, 2]
// [2, 1, 2]这个数组中不是1或者0的数，会继续分裂下去，比如两个2就继续分裂
// [2, 1, 2] -> [1, 0, 1, 1, 1, 0, 1]
// 那么我们说，5最后分裂成[1, 0, 1, 1, 1, 0, 1]
// 每一个数都可以这么分裂，在最终分裂的数组中，假设下标从1开始
// 给定三个数n、l、r，返回n的最终分裂数组里[l,r]范围上有几个1
// n <= 2 ^ 50，n是long类型
// r - l <= 50000，l和r是int类型
// 我们的课加个码:
// n是long类型随意多大都行
// l和r也是long类型随意多大都行，但要保证l<=r
public class SplitTo01
{
    //	private static long Nums3(long n, long l, long r) {
    //		HashMap<Long, Long> lenMap = new HashMap<>();
    //		Len(n, lenMap);
    //		HashMap<Long, Long> onesMap = new HashMap<>();
    //		Ones(n, onesMap);
    //	}

    // n = 100
    // n = 100, 最终裂变的数组，长度多少？
    // n = 50, 最终裂变的数组，长度多少？
    // n = 25, 最终裂变的数组，长度多少？
    // ..
    // n = 1 ,.最终裂变的数组，长度多少？
    // 请都填写到lenMap中去！
    private static long Len(long n, Dictionary<long, long> lenMap)
    {
        if (n == 1 || n == 0)
        {
            lenMap[n] = 1L;
            return 1;
        }

        // n > 1
        var half = Len(n / 2, lenMap);
        var all = half * 2 + 1;
        lenMap[n] = all;
        return all;
    }

    // n = 100
    // n = 100, 最终裂变的数组中，一共有几个1
    // n = 50, 最终裂变的数组，一共有几个1
    // n = 25, 最终裂变的数组，一共有几个1
    // ..
    // n = 1 ,.最终裂变的数组，一共有几个1
    // 请都填写到onesMap中去！
    private static long Ones(long num, Dictionary<long, long> onesMap)
    {
        if (num is 1 or 0)
        {
            onesMap[num] = num;
            return num;
        }

        // n > 1
        var half = Ones(num / 2, onesMap);
        long mid = num % 2 == 1 ? 1 : 0;
        var all = half * 2 + mid;
        onesMap[num] = all;
        return all;
    }

    //

    private static long Nums1(long n, long l, long r)
    {
        if (n == 1 || n == 0) return n == 1 ? 1 : 0;
        var half = Size(n / 2);
        var left = l > half ? 0 : Nums1(n / 2, l, Math.Min(half, r));
        var mid = l > half + 1 || r < half + 1 ? 0 : n & 1;
        var right = r > half + 1 ? Nums1(n / 2, Math.Max(l - half - 1, 1), r - half - 1) : 0;
        return left + mid + right;
    }

    private static long Size(long n)
    {
        if (n == 1 || n == 0) return 1;

        var half = Size(n / 2);
        return (half << 1) + 1;
    }

    private static long Nums2(long n, long l, long r)
    {
        var allMap = new Dictionary<long, long>();
        return Dp(n, l, r, allMap);
    }

    private static long Dp(long n, long l, long r, Dictionary<long, long> allMap)
    {
        if (n is 1 or 0) return n == 1 ? 1 : 0;
        var half = Size(n / 2);
        var all = (half << 1) + 1;
        var mid = n & 1;
        if (l == 1 && r >= all)
        {
            if (allMap.TryGetValue(n, out var dp)) return dp;

            var count = Dp(n / 2, 1, half, allMap);
            var ans = (count << 1) + mid;
            allMap[n] = ans;
            return ans;
        }

        mid = l > half + 1 || r < half + 1 ? 0 : mid;
        var left = l > half ? 0 : Dp(n / 2, l, Math.Min(half, r), allMap);
        var right = r > half + 1 ? Dp(n / 2, Math.Max(l - half - 1, 1), r - half - 1, allMap) : 0;
        return left + mid + right;
    }

    // 为了测试
    // 彻底生成n的最终分裂数组返回
    private static List<int> test(long n)
    {
        var arr = new List<int>();
        Process(n, arr);
        return arr;
    }

    private static void Process(long n, List<int> arr)
    {
        if (n is 1 or 0)
        {
            arr.Add((int)n);
        }
        else
        {
            Process(n / 2, arr);
            arr.Add((int)(n % 2));
            Process(n / 2, arr);
        }
    }

    public static void Run()
    {
        long num = 671;
        var ans = test(num);
        var testTime = 10000;
        Console.WriteLine("功能测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var a = (int)(Utility.getRandomDouble * ans.Count) + 1;
            var b = (int)(Utility.getRandomDouble * ans.Count) + 1;
            var l1 = Math.Min(a, b);
            var r1 = Math.Max(a, b);
            var ans1 = 0;
            for (var j = l1 - 1; j < r1; j++)
                if (ans[j] == 1)
                    ans1++;
            var ans2 = Nums1(num, l1, r1);
            var ans3 = Nums2(num, l1, r1);
            if (ans1 != ans2 || ans1 != ans3) Console.WriteLine("出错了!");
        }

        Console.WriteLine("功能测试结束");
        Console.WriteLine("==============");

        Console.WriteLine("性能测试开始");
        num = (2L << 50) + 22517998136L;
        var l = 30000L;
        var r = 800000200L;

        Utility.RestartStopwatch();
        Console.WriteLine("nums1结果 : " + Nums1(num, l, r));

        Console.WriteLine("nums1花费时间(毫秒) : " + Utility.GetStopwatchElapsedMilliseconds());

        Utility.RestartStopwatch();
        Console.WriteLine("nums2结果 : " + Nums2(num, l, r));

        Console.WriteLine("nums2花费时间(毫秒) : " + Utility.GetStopwatchElapsedMilliseconds());
        Console.WriteLine("性能测试结束");
        Console.WriteLine("==============");

        Console.WriteLine("单独展示nums2方法强悍程度测试开始");
        num = (2L << 55) + 22517998136L;
        l = 30000L;
        r = 6431000002000L;
        Utility.RestartStopwatch();
        Console.WriteLine("nums2结果 : " + Nums2(num, l, r));

        Console.WriteLine("nums2花费时间(毫秒) : " + Utility.GetStopwatchElapsedMilliseconds());
        Console.WriteLine("单独展示nums2方法强悍程度测试结束");
        Console.WriteLine("==============");
    }
}