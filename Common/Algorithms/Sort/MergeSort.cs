#region

using Common.Utilities;

#endregion

namespace Common.Algorithms.Sort;

public class MergeSort
{
    private static void MergeSort1(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return;
        Process(arr, 0, arr.Length - 1);
    }

    // 请把arr[L..R]排有序
    // l...r N
    // T(N) = 2 * T(N / 2) + O(N)
    // O(N * logN)
    private static void Process(int[] arr, int l, int r)
    {
        if (l == r)
            // base case
            return;
        var mid = l + ((r - l) >> 1);
        Process(arr, l, mid);
        Process(arr, mid + 1, r);
        Merge(arr, l, mid, r);
    }

    private static void Merge(int[] arr, int l, int m, int r)
    {
        var help = new int[r - l + 1];
        var i = 0;
        var p1 = l;
        var p2 = m + 1;
        while (p1 <= m && p2 <= r) help[i++] = arr[p1] <= arr[p2] ? arr[p1++] : arr[p2++];
        // 要么p1越界了，要么p2越界了
        while (p1 <= m) help[i++] = arr[p1++];
        while (p2 <= r) help[i++] = arr[p2++];
        for (i = 0; i < help.Length; i++) arr[l + i] = help[i];
    }

    // 非递归方法实现
    private static void MergeSort2(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return;
        var n = arr.Length;
        // 步长
        var mergeSize = 1;
        while (mergeSize < n)
        {
            // log N
            // 当前左组的，第一个位置
            var l = 0;
            while (l < n)
            {
                if (mergeSize >= n - l) break;
                var m = l + mergeSize - 1;
                var r = m + Math.Min(mergeSize, n - m - 1);
                Merge(arr, l, m, r);
                l = r + 1;
            }

            // 防止溢出
            if (mergeSize > n / 2) break;
            mergeSize <<= 1;
        }
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
        if (arr2 != null && arr1 != null && arr1.Length != arr2.Length) return false;
        for (var i = 0; i < arr1?.Length; i++)
            if (arr1[i] != arr2?[i])
                return false;
        return true;
    }

    //用于测试
    private static void PrintArray(int[]? arr)
    {
        if (arr == null) return;
        foreach (var item in arr)
            Console.Write(item + " ");

        Console.WriteLine();
    }

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
            MergeSort1(arr1);
            MergeSort2(arr2);
            if (!IsEqual(arr1, arr2))
            {
                Console.WriteLine("出错了！");
                PrintArray(arr1);
                PrintArray(arr2);
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}