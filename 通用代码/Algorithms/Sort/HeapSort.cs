#region

using Common.Utilities;

#endregion

namespace Common.Algorithms.Sort;

public class HeapSort
{
    // 堆排序额外空间复杂度O(1)
    // 使用异或交换会有概率让数字变成0
    private static void Code(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return;

        // O(N*logN)
        // for (var i = 0; i < arr.Length; i++) { // O(N)
        // 	HeapInsert(arr, i); // O(logN)
        // }

        // O(N)
        for (var i = arr.Length - 1; i >= 0; i--) Heapify(arr, i, arr.Length);

        var heapSize = arr.Length;
        Utility.Swap(arr, 0, --heapSize);
        // O(N*logN)
        while (heapSize > 0)
        {
            // O(N)
            Heapify(arr, 0, heapSize); // O(logN)
            Utility.Swap(arr, 0, --heapSize); // O(1)
        }
    }

    // arr[index]刚来的数，往上
    private static void HeapInsert(int[] arr, int index)
    {
        while (arr[index] > arr[(index - 1) / 2])
        {
            Utility.Swap(arr, index, (index - 1) / 2);
            index = (index - 1) / 2;
        }
    }

    // arr[index]位置的数，能否往下移动
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

            Utility.Swap(arr, largest, index);
            index = largest;
            left = index * 2 + 1;
        }
    }


    public static void Run()
    {
        int[] testList = [54, 26, 93, 17, 77, 31, 44, 55, 20];
        Console.WriteLine("堆排序升序：");
        Code(testList);
        Console.WriteLine(string.Join(",", testList));
    }
}