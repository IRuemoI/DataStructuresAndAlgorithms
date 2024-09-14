//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson06;

public class HeapSort
{
    // 堆排序额外空间复杂度O(1)
    private static void Code(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return;

        // 从上到下建堆O(N*logN)
        // for (int i = 0; i < arr.Length; i++) { // O(N)
        // 	heapInsert(arr, i); // O(logN)
        // }

        // 从下到上建堆O(N)
        for (var i = arr.Length - 1; i >= 0; i--) Heapify(arr, i, arr.Length);

        var heapSize = arr.Length;
        Swap(arr, 0, --heapSize);
        // O(N*logN)
        while (heapSize > 0)
        {
            // O(N)
            Heapify(arr, 0, heapSize); // O(logN)
            Swap(arr, 0, --heapSize); // O(1)
        }
    }

    // arr[index]刚来的数，往上
    private static void HeapInsert(int[] arr, int index)
    {
        while (arr[index] > arr[(index - 1) / 2])
        {
            Swap(arr, index, (index - 1) / 2);
            index = (index - 1) / 2;
        }
    }

    // arr[index]位置的数，能否往下移动
    /// <summary>
    ///     堆化，将index位置的数，调整到对应的位置
    /// </summary>
    /// <param name="arr">目标数组</param>
    /// <param name="index">需要调整的元素下下标</param>
    /// <param name="heapSize">堆大小</param>
    private static void Heapify(int[] arr, int index, int heapSize)
    {
        var left = index * 2 + 1; // 左孩子的下标
        while (left < heapSize)
        {
            // 下方还有孩子的时候
            // 两个孩子中，谁的值大，把下标给largest
            // 1）只有左孩子，left -> largest
            // 2) 同时有左孩子和右孩子，右孩子的值<= 左孩子的值，left -> largest
            // 3) 同时有左孩子和右孩子并且右孩子的值> 左孩子的值， right -> largest
            var largest = left + 1 < heapSize && arr[left + 1] > arr[left] ? left + 1 : left;
            // 父和较大的孩子之间，谁的值大，把下标给largest
            largest = arr[largest] > arr[index] ? largest : index;
            if (largest == index) break;

            Swap(arr, largest, index);
            index = largest;
            left = index * 2 + 1;
        }
    }

    private static void Swap(int[] arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    #region 用于测试

    private static void Comparator(int[]? arr)
    {
        if (arr != null) Array.Sort(arr);
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

    private static bool IsEqual(int[]? arr1, int[]? arr2)
    {
        if ((arr1 == null && arr2 != null) || (arr1 != null && arr2 == null)) return false;

        if (arr1 == null && arr2 == null) return true;

        if (arr2 != null && arr1 != null && arr1.Length != arr2.Length) return false;

        if (arr1 == null) return true;
        for (var i = 0; i < arr1.Length; i++)
            if (arr2 != null && arr1[i] != arr2[i])
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
        var testTime = 10;
        var maxSize = 100;
        var maxValue = 100;
        var succeed = true;
        for (var i = 0; i < testTime; i++)
        {
            var arr1 = GenerateRandomArray(maxSize, maxValue);
            var arr2 = CopyArray(arr1);
            Code(arr1);
            Comparator(arr2);
            if (!IsEqual(arr1, arr2))
            {
                succeed = false;
                break;
            }

            Console.WriteLine("出错的输入:" + string.Join(",", arr1.Select(element => element.ToString())));
        }

        Console.WriteLine(succeed ? "测试通过" : "出现错误");
    }
}