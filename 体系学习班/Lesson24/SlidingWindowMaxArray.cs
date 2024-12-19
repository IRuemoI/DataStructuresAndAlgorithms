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
        LinkedList<int> queueMax = new();
        var result = new int[arr.Length - w + 1];
        var index = 0;
        for (var r = 0; r < arr.Length; r++)
        {
            //如果新的要添加到双向链表的新元素大于链表下标代表的原始值。那么将这些值对应的下标以从尾部移除方式进行移除
            while (queueMax.Count != 0 && arr[queueMax.Last()] <= arr[r]) queueMax.RemoveLast();
            //将新元素的下标添加到双向链表尾部
            queueMax.AddLast(r);
            //如果双向链表头部下标对应的元素已经过期，移除过期的所引
            if (queueMax.First() == r - w) queueMax.RemoveFirst();
            //只有在窗口的大小达到规定的大小时才会收集答案
            if (r >= w - 1) result[index++] = arr[queueMax.First()];
        }

        return result;
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