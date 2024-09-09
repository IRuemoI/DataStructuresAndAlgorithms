//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson04;

public class BiggerThanRightTwice
{
    private static int BiggerTwice(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;

        return Process(arr, 0, arr.Length - 1);
    }

    private static int Process(int[] arr, int leftEdge, int rightEdge)
    {
        if (leftEdge == rightEdge) return 0;

        // l < r
        var mid = leftEdge + ((rightEdge - leftEdge) >> 1);
        return Process(arr, leftEdge, mid) + Process(arr, mid + 1, rightEdge) + Merge(arr, leftEdge, mid, rightEdge);
    }

    private static int Merge(int[] arr, int leftEdge, int middle, int rightEdge)
    {
        // [L....M]   [M+1....R]

        var ans = 0;
        // 目前囊括进来的数，是从[M+1, windowR)
        var windowR = middle + 1;
        for (var j = leftEdge; j <= middle; j++)
        {
            while (windowR <= rightEdge && arr[j] > arr[windowR] * 2) windowR++;

            ans += windowR - middle - 1;
        }


        var help = new int[rightEdge - leftEdge + 1];
        var i = 0;
        var p1 = leftEdge;
        var p2 = middle + 1;
        while (p1 <= middle && p2 <= rightEdge) help[i++] = arr[p1] <= arr[p2] ? arr[p1++] : arr[p2++];

        while (p1 <= middle) help[i++] = arr[p1++];

        while (p2 <= rightEdge) help[i++] = arr[p2++];

        for (i = 0; i < help.Length; i++) arr[leftEdge + i] = help[i];

        return ans;
    }

    //用于测试
    private static int Comparator(int[] arr)
    {
        var ans = 0;
        for (var i = 0; i < arr.Length; i++)
        for (var j = i + 1; j < arr.Length; j++)
            if (arr[i] > arr[j] << 1)
                ans++;

        return ans;
    }

    //用于测试
    private static int[] GenerateRandomArray(int maxSize, int maxValue)
    {
        var arr = new int[(int)((maxSize + 1) * Utility.GetRandomDouble)];
        for (var i = 0; i < arr.Length; i++)
            arr[i] = (int)((maxValue + 1) * Utility.GetRandomDouble) - (int)((maxValue + 1) * Utility.GetRandomDouble);

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

        foreach (var element in arr) Console.Write(element + " ");

        Console.WriteLine();
    }

    //用于测试
    public static void Run()
    {
        var testTime = 10000;
        var maxSize = 100;
        var maxValue = 100;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var arr1 = GenerateRandomArray(maxSize, maxValue);
            var arr2 = CopyArray(arr1);
            if (arr2 != null && BiggerTwice(arr1) != Comparator(arr2))
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