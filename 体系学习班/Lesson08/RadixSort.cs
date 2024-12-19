//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson08;

public class RadixSort
{
    private static void Sort(int[] arr, int radix = 10)
    {
        if (arr.Length < 2) return;

        //获取原数组中的最小值
        var min = int.MaxValue;
        foreach (var element in arr)
        {
            min = Math.Min(min, element);
        }

        //将数组中的所有元素都加上最小值的负数，使得数组中的元素都变为非负数
        if (min < 0)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] += -min;
            }
        }
        
        //获取修正后数组中的最小值
        var max = int.MinValue;
        foreach (var element in arr)
        {
            max = Math.Max(max, element);
        }

        //获取数组中最大值的位数
        var maxValuePosition = 0;
        while (max != 0)
        {
            maxValuePosition++;
            max /= 10;
        }

        //创建原数组大小的额外临时数组用于统计每个数字的出现次数
        var tempArray = new int[arr.Length];
        //进行最大值位数次的入桶和出桶
        for (var position = 1; position <= maxValuePosition; position++)
        {
            //统计在第position位上每个数字的出现次数
            var count = new int[radix];
            foreach (var element in arr)
            {
                count[GetDigitAtPosition(element, position)]++;
            }

            //将count数组转换为前缀和数组用来模拟原数组中的元素在放入桶中的操作
            for (int i = 1; i < radix; i++) count[i] += count[i - 1];
            
            //将原数组中的元素按照前缀和数组来模拟把桶中倒出数据到临时空间中的操作
            for (int i = tempArray.Length - 1; i >= 0; i--)
            {
                //获取原数组中下标i的元素的position位置上的数字
                var digit = GetDigitAtPosition(arr[i], position);
                //将原数组中下标i的元素放到临时数组tempArray中下标前缀和数组中指导的位置上。
                //因为原来构成前缀和数组所使用的数据都是自然数构成的所以当前缀和数组中的相邻两个元素表现为上升状态时说明需要从从上一个位置向后放下提升量个元素
                tempArray[count[digit] - 1] = arr[i];
                //表示已经从存放数字digit的桶中倒出一个元素
                count[digit]--;
            }

            //将临时数组中的元素还原到原数组中
            for (int i = 0; i < arr.Length; i++) arr[i] = tempArray[i];
        }

        //将结果还原到负数
        if (min < 0)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] += min;
            }
        }
    }
    
    private static int GetDigitAtPosition(int number, int position)
    {
        //将number向右移动position - 1位并求余来获得第position位上的数字
        return number / (int)Math.Pow(10, position - 1) % 10;
    }

    #region 用于测试

    private static void Comparator(int[] arr)
    {
        Array.Sort(arr);
    }

    private static int[] GenerateRandomArray(int maxSize, int maxValue, int minValue)
    {
        var arr = new int[(int)((maxSize + 1) * Utility.getRandomDouble)];
        for (var i = 0; i < arr.Length; i++) arr[i] = (int)((maxValue + 1) * Utility.getRandomDouble) + minValue;

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

        if (arr1?.Length != arr2?.Length) return false;

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
        var testTime = 5000;
        var maxSize = 100;
        var minxSize = -500;
        var maxValue = 1500;
        var succeed = true;
        for (var i = 0; i < testTime; i++)
        {
            var arr1 = GenerateRandomArray(maxSize, maxValue, minxSize);
            var arr2 = CopyArray(arr1);
            var original = CopyArray(arr1);
            Sort(arr1);
            if (arr2 != null)
            {
                Comparator(arr2);
                if (!IsEqual(arr1, arr2))
                {
                    succeed = false;
                    Console.Write("输入数组：");
                    PrintArray(original);
                    Console.Write("排序结果：");
                    PrintArray(arr1);
                    Console.Write("预期结果：");
                    PrintArray(arr2);
                    break;
                }
            }
        }
        
        Console.WriteLine(succeed ? "测试通过" : "出现错误");
    }
}