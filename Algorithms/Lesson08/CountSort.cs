//passed

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson08;

public class CountSort
{
    // only for 0~200 value
    private static void Code(int[] arr)
    {
        if (arr.Length < 2) return;

        //找出数组中的最大值和最小值
        var max = int.MinValue;
        var min = int.MaxValue;
        foreach (var element in arr)
        {
            max = Math.Max(max, element);
            min = Math.Min(min, element);
        }

        //申请合适大小的"桶"，作为计数数组
        var bucket = new int[max - min + 1];
        //把数组中的所有元素放入桶中
        foreach (var element in arr)
        {
            bucket[element - min]++;//可以处理负数的情况
        }

        var index = 0; //定义用于写入数组时所使用的指针
        //把桶中的元素按照从小到大的索引从桶中取出放入元素数组(降序排序时逆向写入)
        for (var i = 0; i < bucket.Length; i++)
        {
            while (bucket[i]-- > 0)
            {
                arr[index++] = min + i;
            }
        }
    }

    #region 用于测试

    private static void Comparator(int[] arr)
    {
        Array.Sort(arr);
    }

    private static int[] GenerateRandomArray(int maxSize, int minValue, int maxValue)
    {
        var arr = new int[(int)((maxSize + 1) * Utility.GetRandomDouble)];
        for (var i = 0; i < arr.Length; i++) arr[i] = (int)((maxValue + 1) * Utility.GetRandomDouble) + minValue;

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

    #endregion

    public static void Run()
    {
        var testTime = 500000;
        var maxSize = 100;
        var minValue = -20;
        var maxValue = 60;
        var succeed = true;
        for (var i = 0; i < testTime; i++)
        {
            var arr1 = GenerateRandomArray(maxSize, minValue, maxValue);
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