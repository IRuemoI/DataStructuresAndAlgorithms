//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson01;

public class BsNearLeft
{
    // 在arr上，找满足>=value的最左位置
    private static int NearestIndex(int[] arr, int value)
    {
        var leftEdge = 0;
        var rightEdge = arr.Length - 1;
        var index = -1; // 记录最左的下标
        while (leftEdge <= rightEdge) //使用<=的原因是当left==right时需要保存index的值
        {
            var middle = leftEdge + ((rightEdge - leftEdge) >> 1);
            if (arr[middle] >= value)
            {
                index = middle;
                rightEdge = middle - 1;
            }
            else
            {
                leftEdge = middle + 1;
            }
        }

        return index;
    }

    //用于测试
    private static int Test(int[] arr, int value)
    {
        for (var i = 0; i < arr.Length; i++)
            if (arr[i] >= value)
                return i;

        return -1;
    }

    //用于测试
    private static int[] GenerateRandomArray(int maxSize, int maxValue)
    {
        var arr = new int[(int)((maxSize + 1) * Utility.getRandomDouble)];
        for (var i = 0; i < arr.Length; i++)
            arr[i] = (int)((maxValue + 1) * Utility.getRandomDouble) - (int)(maxValue * Utility.getRandomDouble);

        return arr;
    }

    public static void Run()
    {
        var testTime = 500000;
        var maxSize = 10;
        var maxValue = 100;
        var succeed = true;

        for (var i = 0; i < testTime; i++)
        {
            var arr = GenerateRandomArray(maxSize, maxValue);
            Array.Sort(arr);
            var value = (int)((maxValue + 1) * Utility.getRandomDouble) - (int)(maxValue * Utility.getRandomDouble);
            if (Test(arr, value) != NearestIndex(arr, value))
            {
                Console.WriteLine(string.Join(",", arr));
                Console.WriteLine(value);
                Console.WriteLine(Test(arr, value));
                Console.WriteLine(NearestIndex(arr, value));
                succeed = false;
                break;
            }
        }

        Console.WriteLine(succeed ? "测试通过" : "出现错误");
    }
}