//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson05;

public class QuickSortRecursiveAndIteration
{
    // 三分荷兰国旗问题
    private static int[] NetherlandsFlag(int[] arr, int leftEdge, int rightEdge)
    {
        if (leftEdge > rightEdge) return new[] { -1, -1 };

        if (leftEdge == rightEdge) return new[] { leftEdge, rightEdge };

        var less = leftEdge - 1;
        var more = rightEdge;
        var index = leftEdge;
        while (index < more)
            if (arr[index] == arr[rightEdge])
                index++;
            else if (arr[index] < arr[rightEdge])
                Swap(arr, index++, ++less);
            else
                Swap(arr, index, --more);

        Swap(arr, more, rightEdge);
        return new[] { less + 1, more };
    }

    private static void Swap(int[] arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    // 快排递归版本
    private static void QuickSort1(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return;

        Process(arr, 0, arr.Length - 1);
    }

    private static void Process(int[] arr, int leftEdge, int rightEdge)
    {
        if (leftEdge >= rightEdge) return;

        Swap(arr, leftEdge + (int)(Utility.getRandomDouble * (rightEdge - leftEdge + 1)), rightEdge);
        var equalArea = NetherlandsFlag(arr, leftEdge, rightEdge);
        Process(arr, leftEdge, equalArea[0] - 1);
        Process(arr, equalArea[1] + 1, rightEdge);
    }

    // 快排3.0 非递归版本
    private static void QuickSort2(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return;

        var length = arr.Length;

        Swap(arr, (int)(Utility.getRandomDouble * length), length - 1);
        var equalArea = NetherlandsFlag(arr, 0, length - 1);
        var el = equalArea[0];
        var er = equalArea[1];
        Stack<Op> stack = new();
        stack.Push(new Op(0, el - 1));
        stack.Push(new Op(er + 1, length - 1));
        while (stack.Count != 0)
        {
            var op = stack.Pop(); // op.l  ... op.r
            if (op.Left < op.Right)
            {
                Swap(arr, op.Left + (int)(Utility.getRandomDouble * (op.Right - op.Left + 1)), op.Right);
                equalArea = NetherlandsFlag(arr, op.Left, op.Right);
                el = equalArea[0];
                er = equalArea[1];
                stack.Push(new Op(op.Left, el - 1));
                stack.Push(new Op(er + 1, op.Right));
            }
        }
    }

    #region 用于测试

    // 生成随机数组（用于测试）
    private static int[] GenerateRandomArray(int maxSize, int maxValue)
    {
        var arr = new int[(int)((maxSize + 1) * Utility.getRandomDouble)];
        for (var i = 0; i < arr.Length; i++)
            arr[i] = (int)((maxValue + 1) * Utility.getRandomDouble) - (int)(maxValue * Utility.getRandomDouble);

        return arr;
    }

    // 拷贝数组（用于测试）
    private static int[]? CopyArray(int[]? arr)
    {
        if (arr == null) return null;

        var res = new int[arr.Length];
        for (var i = 0; i < arr.Length; i++) res[i] = arr[i];

        return res;
    }

    // 对比两个数组（用于测试）
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
        var testTime = 500000;
        var maxSize = 100;
        var maxValue = 100;
        var succeed = true;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var arr1 = GenerateRandomArray(maxSize, maxValue);
            var arr2 = CopyArray(arr1);
            QuickSort1(arr1);
            QuickSort2(arr2);
            if (!IsEqual(arr1, arr2))
            {
                succeed = false;
                break;
            }
        }

        Console.WriteLine("测试结束");
        Console.WriteLine("测试" + testTime + "组是否全部通过：" + (succeed ? "是" : "否"));
    }

    // 快排非递归版本需要的辅助类
    // 要处理的是什么范围上的排序
    private class Op(int left, int right)
    {
        public readonly int Left = left;
        public readonly int Right = right;
    }
}