//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson08;

public class RadixSort
{
    // only for no-negative value
    private static void Code(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return;

        Process(arr, 0, arr.Length - 1, MaxBits(arr));
    }

    private static int MaxBits(int[] arr)
    {
        var max = int.MinValue;
        foreach (var element in arr) max = Math.Max(max, element);

        var res = 0;
        while (max != 0)
        {
            res++;
            max /= 10;
        }

        return res;
    }

    // arr[L..R]排序  ,  最大值的十进制位数digit
    private static void Process(int[] arr, int l, int r, int digit)
    {
        const int radix = 10;
        // 有多少个数准备多少个辅助空间
        var help = new int[r - l + 1];
        for (var d = 1; d <= digit; d++)
        {
            // 有多少位就进出几次
            // 10个空间
            // count[0] 当前位(d位)是0的数字有多少个
            // count[1] 当前位(d位)是(0和1)的数字有多少个
            // count[2] 当前位(d位)是(0、1和2)的数字有多少个
            // count[i] 当前位(d位)是(0~i)的数字有多少个
            var count = new int[radix]; // count[0..9]
            int i;
            int j;
            for (i = l; i <= r; i++)
            {
                // 103  1   3
                // 209  1   9
                j = GetDigit(arr[i], d);
                count[j]++;
            }

            for (i = 1; i < radix; i++) count[i] = count[i] + count[i - 1];

            for (i = r; i >= l; i--)
            {
                j = GetDigit(arr[i], d);
                help[count[j] - 1] = arr[i];
                count[j]--;
            }

            for (i = l, j = 0; i <= r; i++, j++) arr[i] = help[j];
        }
    }

    private static int GetDigit(int x, int d)
    {
        return x / (int)Math.Pow(10, d - 1) % 10;
    }

    //用于测试
    private static void Comparator(int[] arr)
    {
        Array.Sort(arr);
    }

    //用于测试
    private static int[] GenerateRandomArray(int maxSize, int maxValue)
    {
        var arr = new int[(int)((maxSize + 1) * Utility.GetRandomDouble)];
        for (var i = 0; i < arr.Length; i++) arr[i] = (int)((maxValue + 1) * Utility.GetRandomDouble);

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

        if (arr1?.Length != arr2?.Length) return false;

        for (var i = 0; i < arr1?.Length; i++)
            if (arr1[i] != arr2?[i])
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
        var testTime = 500000;
        var maxSize = 100;
        var maxValue = 100000;
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