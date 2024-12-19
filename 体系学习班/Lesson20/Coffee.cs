//测试通过

#region

using Common.DataStructures.Heap;
using Common.Utilities;

#endregion

namespace Algorithms.Lesson20;

// 题目
// 数组arr代表每一个咖啡机冲一杯咖啡的时间，每个咖啡机只能串行的制造咖啡。
// 现在有n个人需要喝咖啡，只能用咖啡机来制造咖啡。
// 认为每个人喝咖啡的时间非常短，冲好的时间即是喝完的时间。
// 每个人喝完之后咖啡杯可以选择洗或者自然挥发干净，只有一台洗咖啡杯的机器，只能串行的洗咖啡杯。
// 洗杯子的机器洗完一个杯子时间为a，任何一个杯子自然挥发干净的时间为b。
// 四个参数：arr, n, a, b
// 假设时间点从0开始，返回所有人喝完咖啡并洗完咖啡杯的全部过程结束后，至少来到什么时间点。
public class Coffee
{
    // 验证的方法
    // 彻底的暴力
    // 很慢但是绝对正确
    private static int Right(int[] arr, int n, int a, int b)
    {
        var times = new int[arr.Length];
        var drink = new int[n];
        return ForceMake(arr, times, 0, drink, n, a, b);
    }

    // 每个人暴力尝试用每一个咖啡机给自己做咖啡
    private static int ForceMake(int[] arr, int[] times, int kth, int[] drink, int n, int a, int b)
    {
        if (kth == n)
        {
            var drinkSorted = new int[kth];
            Array.Copy(drink, drinkSorted, kth);
            Array.Sort(drinkSorted);
            return ForceWash(drinkSorted, a, b, 0, 0, 0);
        }

        var time = int.MaxValue;
        for (var i = 0; i < arr.Length; i++)
        {
            var work = arr[i];
            var pre = times[i];
            drink[kth] = pre + work;
            times[i] = pre + work;
            time = Math.Min(time, ForceMake(arr, times, kth + 1, drink, n, a, b));
            drink[kth] = 0;
            times[i] = pre;
        }

        return time;
    }

    private static int ForceWash(int[] drinks, int a, int b, int index, int washLine, int time)
    {
        if (index == drinks.Length) return time;

        // 选择一：当前index号咖啡杯，选择用洗咖啡机刷干净
        var wash = Math.Max(drinks[index], washLine) + a;
        var ans1 = ForceWash(drinks, a, b, index + 1, wash, Math.Max(wash, time));

        // 选择二：当前index号咖啡杯，选择自然挥发
        var dry = drinks[index] + b;
        var ans2 = ForceWash(drinks, a, b, index + 1, washLine, Math.Max(dry, time));
        return Math.Min(ans1, ans2);
    }


    // 优良一点的暴力尝试的方法
    private static int MinTime1(int[] arr, int n, int a, int b)
    {
        Heap<Machine> minHeap = new((x, y) => x.TimePoint + x.WorkTime - (y.TimePoint + y.WorkTime));
        foreach (var element in arr) minHeap.Push(new Machine(0, element));

        var drinks = new int[n];
        for (var i = 0; i < n; i++)
        {
            var cur = minHeap.Pop();
            cur.TimePoint += cur.WorkTime;
            drinks[i] = cur.TimePoint;
            minHeap.Push(cur);
        }

        return BestTime(drinks, a, b, 0, 0);
    }

    // drinks 所有杯子可以开始洗的时间
    // wash 单杯洗干净的时间（串行）
    // air 挥发干净的时间(并行)
    // free 洗的机器什么时候可用
    // drinks[index.....]都变干净，最早的结束时间（返回）
    private static int BestTime(int[] drinks, int wash, int air, int index, int free)
    {
        if (index == drinks.Length) return 0;

        // index号杯子 决定洗
        var selfClean1 = Math.Max(drinks[index], free) + wash;
        var restClean1 = BestTime(drinks, wash, air, index + 1, selfClean1);
        var p1 = Math.Max(selfClean1, restClean1);

        // index号杯子 决定挥发
        var selfClean2 = drinks[index] + air;
        var restClean2 = BestTime(drinks, wash, air, index + 1, free);
        var p2 = Math.Max(selfClean2, restClean2);
        return Math.Min(p1, p2);
    }

    // 贪心+优良尝试改成动态规划
    private static int MinTime2(int[] arr, int n, int a, int b)
    {
        Heap<Machine> heap = new((x, y) => x.TimePoint + x.WorkTime - (y.TimePoint + y.WorkTime));
        foreach (var element in arr) heap.Push(new Machine(0, element));

        var drinks = new int[n];
        for (var i = 0; i < n; i++)
        {
            var cur = heap.Pop();
            cur.TimePoint += cur.WorkTime;
            drinks[i] = cur.TimePoint;
            heap.Push(cur);
        }

        return BestTimeDp(drinks, a, b);
    }

    private static int BestTimeDp(int[] drinks, int wash, int air)
    {
        var n = drinks.Length;
        var maxFree = 0;
        foreach (var element in drinks)
            maxFree = Math.Max(maxFree, element) + wash;

        var dp = new int[n + 1, maxFree + 1];
        for (var index = n - 1; index >= 0; index--)
        for (var free = 0; free <= maxFree; free++)
        {
            var selfClean1 = Math.Max(drinks[index], free) + wash;
            if (selfClean1 > maxFree) break; // 因为后面的也都不用填了

            // index号杯子 决定洗
            var restClean1 = dp[index + 1, selfClean1];
            var p1 = Math.Max(selfClean1, restClean1);
            // index号杯子 决定挥发
            var selfClean2 = drinks[index] + air;
            var restClean2 = dp[index + 1, free];
            var p2 = Math.Max(selfClean2, restClean2);
            dp[index, free] = Math.Min(p1, p2);
        }

        return dp[0, 0];
    }

    //用于测试
    private static int[] RandomArray(int len, int max)
    {
        var arr = new int[len];
        for (var i = 0; i < len; i++) arr[i] = (int)(Utility.getRandomDouble * max) + 1;

        return arr;
    }

    //用于测试
    private static void PrintArray(int[] arr)
    {
        Console.Write("arr : ");
        foreach (var element in arr)
            Console.Write(element + ", ");

        Console.WriteLine();
    }

    public static void Run()
    {
        var len = 10;
        const int max = 10;
        var testTime = 1;
        Console.WriteLine("测试开始");

        for (var i = 0; i < testTime; i++)
        {
            var arr = RandomArray(len, max);
            var n = (int)(Utility.getRandomDouble * 7) + 1;
            var a = (int)(Utility.getRandomDouble * 7) + 1;
            var b = (int)(Utility.getRandomDouble * 10) + 1;
            var ans1 = Right(arr, n, a, b);
            var ans2 = MinTime1(arr, n, a, b);
            var ans3 = MinTime2(arr, n, a, b);
            if (ans1 != ans2 || ans2 != ans3)
            {
                Console.WriteLine("出错了");
                PrintArray(arr);
                Console.WriteLine("n : " + n);
                Console.WriteLine("a : " + a);
                Console.WriteLine("b : " + b);
                Console.WriteLine(ans1 + " , " + ans2 + " , " + ans3);
                Console.WriteLine(ans2 + " , " + ans3);
                Console.WriteLine("===============");
                break;
            }
        }

        Console.WriteLine("测试结束");
    }

    // 以下为贪心+优良暴力
    private class Machine
    {
        public readonly int WorkTime;
        public int TimePoint;

        public Machine(int t, int w)
        {
            TimePoint = t;
            WorkTime = w;
        }
    }
}