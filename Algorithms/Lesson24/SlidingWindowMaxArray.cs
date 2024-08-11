//测试通过

namespace Algorithms.Lesson24;

public class SlidingWindowMaxArray
{
    // 暴力的对数器方法
    private static int[]? Right(int[]? arr, int w)
    {
        if (arr == null || w < 1 || arr.Length < w) return null;
        var n = arr.Length;
        var res = new int[n - w + 1];
        var index = 0;
        var l = 0;
        var r = w - 1;
        while (r < n)
        {
            var max = arr[l];
            for (var i = l + 1; i <= r; i++) max = Math.Max(max, arr[i]);
            res[index++] = max;
            l++;
            r++;
        }

        return res;
    }

    private static int[]? GetMaxWindow(int[]? arr, int w)
    {
        if (arr == null || w < 1 || arr.Length < w) return null;
        // qmax 窗口最大值的更新结构
        // 放下标
        LinkedList<int> qMax = new();
        var res = new int[arr.Length - w + 1];
        var index = 0;
        for (var r = 0; r < arr.Length; r++)
        {
            while (qMax.Count != 0 && arr[qMax.Last()] <= arr[r]) qMax.RemoveLast();
            qMax.AddLast(r);
            if (qMax.First() == r - w) qMax.RemoveFirst();
            if (r >= w - 1) res[index++] = arr[qMax.First()];
        }

        return res;
    }

    //用于测试
    private static int[] GenerateRandomArray(int maxSize, int maxValue)
    {
        var arr = new int[(int)((maxSize + 1) * new Random().NextDouble())];
        for (var i = 0; i < arr.Length; i++) arr[i] = (int)(new Random().NextDouble() * (maxValue + 1));
        return arr;
    }

    //用于测试
    private static bool IsEqual(int[]? arr1, int[]? arr2)
    {
        if ((arr1 == null && arr2 != null) || (arr1 != null && arr2 == null)) return false;
        if (arr1 == null && arr2 == null) return true;
        if (arr1?.Length != arr2?.Length) return false;
        for (var i = 0; i < arr1?.Length; i++)
            if (arr1[i] != arr2?[i])
                return false;
        return true;
    }

    public static void Run()
    {
        var testTime = 100000;
        var maxSize = 100;
        var maxValue = 100;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var arr = GenerateRandomArray(maxSize, maxValue);
            var w = (int)(new Random().NextDouble() * (arr.Length + 1));
            var ans1 = GetMaxWindow(arr, w);
            var ans2 = Right(arr, w);
            if (!IsEqual(ans1, ans2)) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");
    }
}