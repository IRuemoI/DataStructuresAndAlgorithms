//测试通过

#region

using Common.DataStructures.Heap;
using Common.Utilities;

#endregion

namespace Algorithms.Lesson06;

public class SortArrayDistanceLessK
{
    private static void SortedArrDistanceLessK(int[] arr, int k)
    {
        if (k == 0) return;

        // 默认小根堆
        var minHeap = new Heap<int>((x, y) => x.CompareTo(y));
        var index = 0;
        // 0...K-1
        for (; index <= Math.Min(arr.Length - 1, k - 1); index++) minHeap.Push(arr[index]);

        var i = 0;
        for (; index < arr.Length; i++, index++)
        {
            minHeap.Push(arr[index]);
            arr[i] = minHeap.Pop();
        }

        while (minHeap.Count != 0) arr[i++] = minHeap.Pop();
    }

    //用于测试
    private static void Comparator(int[] arr)
    {
        Array.Sort(arr);
    }

    //用于测试
    private static int[] RandomArrayNoMoveMoreK(int maxSize, int maxValue, int k)
    {
        var arr = new int[(int)((maxSize + 1) * Utility.GetRandomDouble)];
        for (var i = 0; i < arr.Length; i++)
            arr[i] = (int)((maxValue + 1) * Utility.GetRandomDouble) - (int)(maxValue * Utility.GetRandomDouble);

        // 先排个序
        Array.Sort(arr);
        // 然后开始随意交换，但是保证每个数距离不超过K
        // swap[i] == true, 表示i位置已经参与过交换
        // swap[i] == false, 表示i位置没有参与过交换
        var isSwap = new bool[arr.Length];
        for (var i = 0; i < arr.Length; i++)
        {
            var j = Math.Min(i + (int)(Utility.GetRandomDouble * (k + 1)), arr.Length - 1);
            if (!isSwap[i] && !isSwap[j])
            {
                isSwap[i] = true;
                isSwap[j] = true;
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }
        }

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

        if (arr1 == null) return true;
        for (var i = 0; i < arr1.Length; i++)
            if (arr2 != null && arr1[i] != arr2[i])
                return false;

        return true;
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
        Console.WriteLine("测试开始");
        var testTime = 500000;
        var maxSize = 100;
        var maxValue = 100;
        var succeed = true;

        for (var i = 0; i < testTime; i++)
        {
            var k = (int)(Utility.GetRandomDouble * maxSize) + 1;
            var arr = RandomArrayNoMoveMoreK(maxSize, maxValue, k);
            var arr1 = CopyArray(arr);
            var arr2 = CopyArray(arr);
            if (arr1 != null && arr2 != null)
            {
                SortedArrDistanceLessK(arr1, k);
                Comparator(arr2);
                if (!IsEqual(arr1, arr2))
                {
                    succeed = false;
                    Console.WriteLine("K : " + k);
                    PrintArray(arr);
                    PrintArray(arr1);
                    PrintArray(arr2);
                    break;
                }
            }
        }

        Console.WriteLine(succeed ? "测试通过" : "出现错误");
    }
}