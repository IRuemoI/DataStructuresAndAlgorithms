#region

using Common.Utilities;

#endregion

namespace Common.Algorithms.Sort;

public class QuickSort
{
    private static void Code(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return;

        Process(arr, 0, arr.Length - 1);
    }

    private static void Process(int[] arr, int leftEdge, int rightEdge)
    {
        if (leftEdge >= rightEdge) return;


        Utility.Swap(arr, leftEdge + (int)(Utility.getRandomDouble * (rightEdge - leftEdge + 1)), rightEdge);
        var equalArea = NetherlandsFlag(arr, leftEdge, rightEdge);
        Process(arr, leftEdge, equalArea[0] - 1);
        Process(arr, equalArea[1] + 1, rightEdge);
    }

    private static int[] NetherlandsFlag(int[] arr, int leftEdge, int rightEdge)
    {
        if (leftEdge > rightEdge)
            // L...R L>R
            return new[] { -1, -1 };

        if (leftEdge == rightEdge) return new[] { leftEdge, rightEdge };

        var less = leftEdge - 1; // < 区 右边界
        var more = rightEdge; // > 区 左边界
        var index = leftEdge;
        while (index < more)
            // 当前位置，不能和 >区的左边界撞上
            if (arr[index] == arr[rightEdge])
                index++;
            else if (arr[index] < arr[rightEdge])
                Utility.Swap(arr, index++, ++less);
            else
                Utility.Swap(arr, index, --more);

        (arr[more], arr[rightEdge]) = (arr[rightEdge], arr[more]); // <[R]   =[R]   >[R]
        return [less + 1, more];
    }

    public static void Run()
    {
        int[] testList = [54, 26, 93, 17, 77, 31, 44, 55, 20];
        Console.WriteLine("快速排序升序：");
        Code(testList);
        Console.WriteLine(string.Join(",", testList));
    }
}