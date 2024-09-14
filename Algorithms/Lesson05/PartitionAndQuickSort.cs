//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson05;

public class PartitionAndQuickSort
{
    private static void Swap(int[] arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    // arr[L..R]上，以arr[R]位置的数做划分值
    // <= X > X
    // <= X X
    private static int Partition(int[] arr, int leftEdge, int rightEdge)
    {
        if (leftEdge > rightEdge) return -1;

        if (leftEdge == rightEdge) return leftEdge;

        var lessEqual = leftEdge - 1;
        var index = leftEdge;
        while (index < rightEdge)
        {
            if (arr[index] <= arr[rightEdge]) Swap(arr, index, ++lessEqual);

            index++;
        }

        Swap(arr, ++lessEqual, rightEdge);
        return lessEqual;
    }

    // arr[L...R] 玩荷兰国旗问题的划分，以arr[R]做划分值
    // <arr[R] ==arr[R] > arr[R]
    private static int[] NetherlandsFlag(int[] arr, int leftEdge, int rightEdge)
    {
        if (leftEdge > rightEdge)
            // L...R L>R
            return [-1, -1];

        if (leftEdge == rightEdge) return [leftEdge, rightEdge];

        var less = leftEdge - 1; // < 区 右边界
        var more = rightEdge; // > 区 左边界
        var index = leftEdge;
        while (index < more)
            // 当前位置，不能和 >区的左边界撞上
            if (arr[index] == arr[rightEdge])
                index++;
            else if (arr[index] < arr[rightEdge])
                //				swap(arr, less + 1, index);
                //				less++;
                //				index++;						
                Swap(arr, index++, ++less);
            else
                // >
                Swap(arr, index, --more);

        Swap(arr, more, rightEdge); // <[R]   =[R]   >[R]
        return [less + 1, more];
    }

    private static void QuickSort1(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return;

        Process1(arr, 0, arr.Length - 1);
    }

    private static void Process1(int[] arr, int leftEdge, int rightEdge)
    {
        if (leftEdge >= rightEdge) return;

        // L..R partition arr[R] [ <=arr[R] arr[R] >arr[R] ]
        var middle = Partition(arr, leftEdge, rightEdge);
        Process1(arr, leftEdge, middle - 1);
        Process1(arr, middle + 1, rightEdge);
    }

    private static void QuickSort2(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return;

        Process2(arr, 0, arr.Length - 1);
    }

    // arr[L...R] 排有序，快排2.0方式
    private static void Process2(int[] arr, int leftEdge, int rightEdge)
    {
        if (leftEdge >= rightEdge) return;

        // [ equalArea[0]  ,  equalArea[0]]
        var equalArea = NetherlandsFlag(arr, leftEdge, rightEdge);
        Process2(arr, leftEdge, equalArea[0] - 1);
        Process2(arr, equalArea[1] + 1, rightEdge);
    }

    private static void QuickSort3(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return;

        Process3(arr, 0, arr.Length - 1);
    }

    private static void Process3(int[] arr, int leftEdge, int rightEdge)
    {
        if (leftEdge >= rightEdge) return;


        Swap(arr, leftEdge + (int)(Utility.GetRandomDouble * (rightEdge - leftEdge + 1)), rightEdge);
        var equalArea = NetherlandsFlag(arr, leftEdge, rightEdge);
        Process3(arr, leftEdge, equalArea[0] - 1);
        Process3(arr, equalArea[1] + 1, rightEdge);
    }

    #region 用于测试

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

        if (arr1 != null && arr2 != null)
        {
            if (arr1.Length != arr2.Length) return false;

            for (var i = 0; i < arr1.Length; i++)
                if (arr1[i] != arr2[i])
                    return false;
        }

        return true;
    }

    #endregion
    public static void Run()
    {
        var testTime = 100000;
        var maxSize = 100;
        var maxValue = 100;
        var succeed = true;
        for (var i = 0; i < testTime; i++)
        {
            var arr1 = GenerateRandomArray(maxSize, maxValue);
            var arr2 = CopyArray(arr1);
            var arr3 = CopyArray(arr1);
            QuickSort1(arr1);
            QuickSort2(arr2);
            QuickSort3(arr3);
            if (!IsEqual(arr1, arr2) || !IsEqual(arr2, arr3))
            {
                succeed = false;
                break;
            }
        }

        Console.WriteLine(succeed ? "通过!" : "出错");
    }
}