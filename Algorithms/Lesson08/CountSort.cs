//passed

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson08;

public class CountSort
{
    // only for 0~200 value
    private static void Code(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return;

        var max = int.MinValue;
        foreach (var element in arr) max = Math.Max(max, element);

        var bucket = new int[max + 1];
        foreach (var element in arr) bucket[element]++;

        var i1 = 0;
        for (var j = 0; j < bucket.Length; j++)
            while (bucket[j]-- > 0)
                arr[i1++] = j;
    }

    private static void Comparator(int[] arr)
    {
        Array.Sort(arr);
    }

    private static int[] GenerateRandomArray(int maxSize, int maxValue)
    {
        var arr = new int[(int)((maxSize + 1) * Utility.GetRandomDouble)];
        for (var i = 0; i < arr.Length; i++) arr[i] = (int)((maxValue + 1) * Utility.GetRandomDouble);

        return arr;
    }

    private static int[]? CopyArray(int[]? arr)
    {
        if (arr == null) return null;

        var res = new int[arr.Length];
        for (var i = 0; i < arr.Length; i++) res[i] = arr[i];

        return res;
    }

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

    private static void PrintArray(int[]? arr)
    {
        if (arr == null) return;

        foreach (var element in arr) Console.Write(element + " ");

        Console.WriteLine();
    }

    public static void Run()
    {
        var testTime = 500000;
        var maxSize = 100;
        var maxValue = 150;
        var succeed = true;
        for (var i = 0; i < testTime; i++)
        {
            var arr1 = GenerateRandomArray(maxSize, maxValue);
            var arr2 = CopyArray(arr1);
            Code(arr1);
            if (arr2 != null)
            {
                Comparator(arr2);
                if (!IsEqual(arr1, arr2))
                {
                    succeed = false;
                    PrintArray(arr1);
                    PrintArray(arr2);
                    break;
                }
            }
        }

        Console.WriteLine(succeed ? "测试通过" : "出现错误");
    }
}