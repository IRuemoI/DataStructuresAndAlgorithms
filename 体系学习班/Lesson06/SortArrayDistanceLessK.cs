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

        // 创建小根堆
        var minHeap = new Heap<int>((x, y) => x.CompareTo(y));
        var heapRightEdge = 0;
        // 把下标为0到K-1的元素先先放到小根堆中
        while (heapRightEdge <= Math.Min(arr.Length - 1, k - 1))
        {
            minHeap.Push(arr[heapRightEdge]);
            heapRightEdge++;
        }

        var current = 0;
        //剩下的元素依次进入小根堆作为的窗口中
        while (heapRightEdge < arr.Length)
        {
            minHeap.Push(arr[heapRightEdge]);
            arr[current] = minHeap.Pop();
            current++;
            heapRightEdge++;
        }

        // 将窗口内剩下的值全部取出
        while (minHeap.count != 0) arr[current++] = minHeap.Pop();
    }

    public static void Run()
    {
        Console.WriteLine("测试开始");
        var testTime = 10000;
        var maxSize = 100;
        var maxValue = 100;
        var succeed = true;

        for (var i = 0; i < testTime; i++)
        {
            var k = (int)(Utility.getRandomDouble * maxSize) + 1;
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

    #region 用于测试

    private static void Comparator(int[] arr)
    {
        Array.Sort(arr);
    }

    private static int[] RandomArrayNoMoveMoreK(int maxSize, int maxValue, int k)
    {
        var arr = new int[(int)((maxSize + 1) * Utility.getRandomDouble)];
        for (var i = 0; i < arr.Length; i++)
            arr[i] = (int)((maxValue + 1) * Utility.getRandomDouble) - (int)(maxValue * Utility.getRandomDouble);

        // 先排个序
        Array.Sort(arr);
        // 然后开始随意交换，但是保证每个数距离不超过K
        // swap[i] == true, 表示i位置已经参与过交换
        // swap[i] == false, 表示i位置没有参与过交换
        var isSwap = new bool[arr.Length];
        for (var i = 0; i < arr.Length; i++)
        {
            var j = Math.Min(i + (int)(Utility.getRandomDouble * (k + 1)), arr.Length - 1);
            if (!isSwap[i] && !isSwap[j])
            {
                isSwap[i] = true;
                isSwap[j] = true;
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }
        }

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
}