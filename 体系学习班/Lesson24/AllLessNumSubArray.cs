//通过

namespace Algorithms.Lesson24;

public class AllLessNumSubArray
{
    // 暴力的对数器方法
    private static int Right(int[]? arr, int sum)
    {
        if (arr == null || arr.Length == 0 || sum < 0) return 0;

        var n = arr.Length;
        var count = 0;
        for (var l = 0; l < n; l++)
        for (var r = l; r < n; r++)
        {
            var max = arr[l];
            var min = arr[l];
            for (var i = l + 1; i <= r; i++)
            {
                max = Math.Max(max, arr[i]);
                min = Math.Min(min, arr[i]);
            }

            if (max - min <= sum) count++;
        }

        return count;
    }

    private static int Num(int[]? arr, int sum)
    {
        if (arr == null || arr.Length == 0 || sum < 0) return 0;

        var n = arr.Length;
        var count = 0;
        var maxWindow = new LinkedList<int>();
        var minWindow = new LinkedList<int>();
        var r = 0;
        for (var l = 0; l < n; l++)
        {
            while (r < n)
            {
                while (maxWindow.Count > 0 && arr[maxWindow.Last()] <= arr[r]) maxWindow.RemoveLast();

                maxWindow.AddLast(r);
                while (minWindow.Count > 0 && arr[minWindow.Last()] >= arr[r]) minWindow.RemoveLast();

                minWindow.AddLast(r);
                if (arr[maxWindow.First()] - arr[minWindow.First()] > sum)
                    break;
                r++;
            }

            count += r - l;
            if (maxWindow.First() == l) maxWindow.RemoveFirst();

            if (minWindow.First() == l) minWindow.RemoveFirst();
        }

        return count;
    }


    //用于测试
    private static int[] GenerateRandomArray(int maxLen, int maxValue)
    {
        var len = (int)(new Random().NextDouble() * (maxLen + 1));
        var arr = new int[len];
        for (var i = 0; i < len; i++)
            arr[i] = (int)(new Random().NextDouble() * (maxValue + 1)) -
                     (int)(new Random().NextDouble() * (maxValue + 1));

        return arr;
    }

    //用于测试
    private static void PrintArray(int[]? arr)
    {
        if (arr != null)
        {
            foreach (var element in arr) Console.Write(element + " ");

            Console.WriteLine();
        }
    }

    public static void Run()
    {
        var maxLen = 100;
        var maxValue = 200;
        var testTime = 10000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var arr = GenerateRandomArray(maxLen, maxValue);
            var sum = (int)(new Random().NextDouble() * (maxValue + 1));
            var ans1 = Right(arr, sum);
            var ans2 = Num(arr, sum);
            if (ans1 != ans2)
            {
                Console.WriteLine("出错啦！");
                PrintArray(arr);
                Console.WriteLine(sum);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}