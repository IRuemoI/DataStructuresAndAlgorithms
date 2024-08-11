//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson04;

public class SmallSum
{
    private static int SmallSumCode(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;

        return Process(arr, 0, arr.Length - 1);
    }

    // arr[L..R]既要排好序，也要求小和返回
    // 所有merge时，产生的小和，累加
    // 左 排序   merge
    // 右 排序  merge
    // merge
    private static int Process(int[] arr, int l, int r)
    {
        if (l == r) return 0;

        // l < r
        var mid = l + ((r - l) >> 1);
        return
            Process(arr, l, mid)
            +
            Process(arr, mid + 1, r)
            +
            Merge(arr, l, mid, r);
    }

    private static int Merge(int[] arr, int leftEdge, int middle, int rightEdge)
    {
        var help = new int[rightEdge - leftEdge + 1];
        var i = 0;
        var p1 = leftEdge;
        var p2 = middle + 1;
        var res = 0;
        while (p1 <= middle && p2 <= rightEdge)
        {
            res += arr[p1] < arr[p2] ? (rightEdge - p2 + 1) * arr[p1] : 0;
            help[i++] = arr[p1] < arr[p2] ? arr[p1++] : arr[p2++];
        }

        while (p1 <= middle) help[i++] = arr[p1++];

        while (p2 <= rightEdge) help[i++] = arr[p2++];

        for (i = 0; i < help.Length; i++) arr[leftEdge + i] = help[i];

        return res;
    }

    //用于测试
    private static int Comparator(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;

        var res = 0;
        for (var i = 1; i < arr.Length; i++)
        for (var j = 0; j < i; j++)
            res += arr[j] < arr[i] ? arr[j] : 0;

        return res;
    }

    //用于测试
    private static int[] GenerateRandomArray(int maxSize, int maxValue)
    {
        var arr = new int[(int)((maxSize + 1) * Utility.GetRandomDouble)];
        for (var i = 0; i < arr.Length; i++)
            arr[i] = (int)((maxValue + 1) * Utility.GetRandomDouble) - (int)(maxValue * Utility.GetRandomDouble);

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
    private static bool IsEqual(int[]? arr1, int[]? arr2)
    {
        if ((arr1 == null && arr2 != null) || (arr1 != null && arr2 == null)) return false;

        if (arr1 == null && arr2 == null) return true;

        if (arr1 != null && arr2 != null)
        {
            if (arr1.Length != arr2.Length) return false;

            for (var i = 0; i < arr1.Length; i++)
                if (arr1[i] != arr2[i])
                    return false;
        }

        return true;
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
        var succeed = true;
        for (var i = 0; i < testTime; i++)
        {
            var arr1 = GenerateRandomArray(maxSize, maxValue);
            var arr2 = CopyArray(arr1);
            if (SmallSumCode(arr1) != Comparator(arr2))
            {
                succeed = false;
                PrintArray(arr1);
                PrintArray(arr2);
                break;
            }
        }

        Console.WriteLine(succeed ? "测试通过" : "出现错误");
    }
}