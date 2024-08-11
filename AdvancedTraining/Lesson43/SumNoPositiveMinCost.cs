#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson43;

// 来自微软面试
// 给定一个正数数组arr长度为n、正数x、正数y
// 你的目标是让arr整体的累加和<=0
// 你可以对数组中的数num执行以下三种操作中的一种，且每个数最多能执行一次操作 : 
// 1）不变
// 2）可以选择让num变成0，承担x的代价
// 3）可以选择让num变成-num，承担y的代价
// 返回你达到目标的最小代价
// 数据规模 : 面试时面试官没有说数据规模
public class SumNoPositiveMinCost
{
    // 动态规划
    private static int MinOpStep1(int[] arr, int x, int y)
    {
        var sum = 0;
        foreach (var num in arr) sum += num;
        return Process1(arr, x, y, 0, sum);
    }

    // arr[i...]自由选择，每个位置的数可以执行三种操作中的一种！
    // 执行变0的操作，x操作，代价 -> x
    // 执行变相反数的操作，y操作，代价 -> y
    // 还剩下sum这么多累加和，需要去搞定！
    // 返回搞定了sum，最低代价是多少？
    private static int Process1(int[] arr, int x, int y, int i, int sum)
    {
        if (sum <= 0) return 0;
        // sum > 0 没搞定
        if (i == arr.Length) return int.MaxValue;
        // 第一选择，什么也不干！
        var p1 = Process1(arr, x, y, i + 1, sum);
        // 第二选择，执行x的操作，变0 x + 后续
        var p2 = int.MaxValue;
        var next2 = Process1(arr, x, y, i + 1, sum - arr[i]);
        if (next2 != int.MaxValue) p2 = x + next2;
        // 第三选择，执行y的操作，变相反数 x + 后续 7 -7 -14
        var p3 = int.MaxValue;
        var next3 = Process1(arr, x, y, i + 1, sum - (arr[i] << 1));
        if (next3 != int.MaxValue) p3 = y + next3;
        return Math.Min(p1, Math.Min(p2, p3));
    }

    // 贪心（最优解）
    private static int MinOpStep2(int[] arr, int x, int y)
    {
        Array.Sort(arr); // 小 -> 大
        var n = arr.Length;
        for (int l = 0, r = n - 1; l <= r; l++, r--) (arr[l], arr[r]) = (arr[r], arr[l]);

        // arr 大 -> 小
        if (x >= y)
        {
            // 没有任何必要执行x操作
            var sum = 0;
            foreach (var num in arr) sum += num;
            var cost = 0;
            for (var i = 0; i < n && sum > 0; i++)
            {
                sum -= arr[i] << 1;
                cost += y;
            }

            return cost;
        }
        else
        {
            for (var i = n - 2; i >= 0; i--) arr[i] += arr[i + 1];
            var benefit = 0;
            // 注意，可以不二分，用不回退的方式！
            // 执行Y操作的数，有0个的时候
            var left = MostLeft(arr, 0, benefit);
            var cost = left * x;
            for (var i = 0; i < n - 1; i++)
            {
                // 0..i 这些数，都执行Y
                benefit += arr[i] - arr[i + 1];
                left = MostLeft(arr, i + 1, benefit);
                cost = Math.Min(cost, (i + 1) * y + (left - i - 1) * x);
            }

            return cost;
        }
    }

    // arr是后缀和数组， arr[l...]中找到值<=v的最左位置
    private static int MostLeft(int[] arr, int l, int v)
    {
        var r = arr.Length - 1;
        var ans = arr.Length;
        while (l <= r)
        {
            var m = (l + r) / 2;
            if (arr[m] <= v)
            {
                ans = m;
                r = m - 1;
            }
            else
            {
                l = m + 1;
            }
        }

        return ans;
    }

    // 不回退
    private static int MinOpStep3(int[] arr, int x, int y)
    {
        // 系统排序，小 -> 大
        Array.Sort(arr);
        var n = arr.Length;
        // 如何变成 大 -> 小
        for (int l = 0, r = n - 1; l <= r; l++, r--) (arr[l], arr[r]) = (arr[r], arr[l]);

        if (x >= y)
        {
            var sum = 0;
            foreach (var num in arr) sum += num;
            var cost = 0;
            for (var i = 0; i < n && sum > 0; i++)
            {
                sum -= arr[i] << 1;
                cost += y;
            }

            return cost;
        }
        else
        {
            // 0个数执行Y
            var benefit = 0;
            // 全部的数都需要执行x，才能让累加和<=0
            var cost = arr.Length * x;
            var holdSum = 0;
            for (int yRight = 0, holdLeft = n; yRight < holdLeft - 1; yRight++)
            {
                benefit += arr[yRight];
                while (holdLeft - 1 > yRight && holdSum + arr[holdLeft - 1] <= benefit)
                {
                    holdSum += arr[holdLeft - 1];
                    holdLeft--;
                }

                // 0...yRight x holdLeft....
                cost = Math.Min(cost, (yRight + 1) * y + (holdLeft - yRight - 1) * x);
            }

            return cost;
        }
    }

    // 为了测试
    private static int[] randomArray(int len, int v)
    {
        var arr = new int[len];
        for (var i = 0; i < len; i++) arr[i] = (int)(Utility.GetRandomDouble * v) + 1;
        return arr;
    }

    // 为了测试
    private static int[] copyArray(int[] arr)
    {
        var ans = new int[arr.Length];
        for (var i = 0; i < arr.Length; i++) ans[i] = arr[i];
        return ans;
    }

    // 为了测试
    public static void Run()
    {
        var n = 12;
        var v = 20;
        var c = 10;
        var testTime = 10000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var len = (int)(Utility.GetRandomDouble * n);
            var arr = randomArray(len, v);
            var arr1 = copyArray(arr);
            var arr2 = copyArray(arr);
            var arr3 = copyArray(arr);
            var x = (int)(Utility.GetRandomDouble * c);
            var y = (int)(Utility.GetRandomDouble * c);
            var ans1 = MinOpStep1(arr1, x, y);
            var ans2 = MinOpStep2(arr2, x, y);
            var ans3 = MinOpStep3(arr3, x, y);
            if (ans1 != ans2 || ans1 != ans3) Console.WriteLine("出错了!");
        }

        Console.WriteLine("测试结束");
    }
}