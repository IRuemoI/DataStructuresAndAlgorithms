#region

using Common.Utilities;

#endregion

namespace Common.Algorithms.Sort;

public class MethodChecker
{
    private static void Execute(CustomSortMethod customSortMethod, int testTime = 10, int maxListSize = 20,
        int maxValue = 100)
    {
        var succeed = true;
        for (var i = 0; i < testTime; i++)
        {
            var arr1 = GenerateRandomArray(maxListSize, maxValue);
            var arr2 = CopyArray(arr1);

            customSortMethod(arr1);
            if (arr2 != null)
            {
                ContrastMethod(arr2);
                if (!IsResultEqual(arr1, arr2))
                {
                    succeed = false;
                    Console.WriteLine(string.Join(",", arr1));
                    Console.WriteLine(string.Join(",", arr2));
                    break;
                }
            }
        }

        Console.WriteLine("测试结果:" + (succeed ? "成功" : "失败"));
    }

    private static void ContrastMethod(int[] arr)
    {
        Array.Sort(arr);
    }

    private static int[] GenerateRandomArray(int maxSize, int maxValue)
    {
        var arr = new int[(int)((maxSize + 1) * Utility.GetRandomDouble)];
        for (var i = 0; i < arr.Length; i++)
            arr[i] = (int)((maxValue + 1) * Utility.GetRandomDouble) - (int)(maxValue * Utility.GetRandomDouble);

        return arr;
    }

    private static int[]? CopyArray(int[]? arr)
    {
        if (arr == null) return null;

        var res = new int[arr.Length];
        for (var i = 0; i < arr.Length; i++) res[i] = arr[i];

        return res;
    }

    private static bool IsResultEqual(int[]? arr1, int[]? arr2)
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

    private delegate void CustomSortMethod(int[] testList);
}