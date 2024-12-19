//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson04;

public class ReversePair
{
    private static int ReverseNumberPair(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;

        return Process(arr, 0, arr.Length - 1);
    }

    // arr[L..R]既要排好序，也要求逆序对数量返回
    // 所有merge时，产生的逆序对数量，累加，返回
    // 左 排序 merge并产生逆序对数量
    // 右 排序 merge并产生逆序对数量
    private static int Process(int[] arr, int leftEdge, int rightEdge)
    {
        if (leftEdge == rightEdge) return 0;

        // l < r
        var mid = leftEdge + ((rightEdge - leftEdge) >> 1);
        return Process(arr, leftEdge, mid) + Process(arr, mid + 1, rightEdge) + Merge(arr, leftEdge, mid, rightEdge);
    }

    private static int Merge(int[] arr, int leftEdge, int middle, int rightEdge)
    {
        var help = new int[rightEdge - leftEdge + 1];
        var i = help.Length - 1;
        var p1 = middle;
        var p2 = rightEdge;
        var res = 0;
        while (p1 >= leftEdge && p2 > middle)
        {
            res += arr[p1] > arr[p2] ? p2 - middle : 0;
            help[i--] = arr[p1] > arr[p2] ? arr[p1--] : arr[p2--];
        }

        while (p1 >= leftEdge) help[i--] = arr[p1--];

        while (p2 > middle) help[i--] = arr[p2--];

        for (i = 0; i < help.Length; i++) arr[leftEdge + i] = help[i];

        return res;
    }

    //用于测试
    private static int Comparator(int[] arr)
    {
        var ans = 0;
        for (var i = 0; i < arr.Length; i++)
        for (var j = i + 1; j < arr.Length; j++)
            if (arr[i] > arr[j])
                ans++;

        return ans;
    }

    //用于测试
    private static int[] GenerateRandomArray(int maxSize, int maxValue)
    {
        var arr = new int[(int)((maxSize + 1) * Utility.getRandomDouble)];
        for (var i = 0; i < arr.Length; i++)
            arr[i] = (int)((maxValue + 1) * Utility.getRandomDouble) - (int)(maxValue * Utility.getRandomDouble);

        return arr;
    }

    //用于测试
    private static int[]? CopyArray(int[]? arr)
    {
        if (arr == null) return null;

        var res = new int[arr.Length];
        for (var i = 0; i < arr.Length; i++) res[i] = arr[i];

        return res;
    }

    //用于测试
    private static void PrintArray(int[]? arr)
    {
        if (arr == null) return;

        foreach (var t in arr) Console.Write(t + " ");

        Console.WriteLine();
    }

    //用于测试
    public static void Run()
    {
        var testTime = 500000;
        var maxSize = 100;
        var maxValue = 100;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var arr1 = GenerateRandomArray(maxSize, maxValue);
            var arr2 = CopyArray(arr1);
            if (arr2 != null && ReverseNumberPair(arr1) != Comparator(arr2))
            {
                Console.WriteLine("出错啦！");
                PrintArray(arr1);
                PrintArray(arr2);
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}