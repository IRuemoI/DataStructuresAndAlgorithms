//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson01;

public class BsExist
{
    private static bool Exist(IReadOnlyList<int>? sortedArr, int num)
    {
        if (sortedArr == null || sortedArr.Count == 0) return false;

        var leftEdge = 0;
        var rightEdge = sortedArr.Count - 1;
        // L..R
        while (leftEdge < rightEdge)
        {
            // L..R 至少两个数的时候
            var middle = leftEdge + ((rightEdge - leftEdge) >> 1);
            if (sortedArr[middle] == num) return true;

            if (sortedArr[middle] > num)
                rightEdge = middle - 1;
            else
                leftEdge = middle + 1;
        }

        return sortedArr[leftEdge] == num;
    }

    //用于测试
    private static bool Test(int[] sortedArr, int num)
    {
        foreach (var cur in sortedArr)
            if (cur == num)
                return true;

        return false;
    }


    //用于测试
    private static int[] GenerateRandomArray(int maxSize, int maxValue)
    {
        var arr = new int[(int)((maxSize + 1) * Utility.GetRandomDouble)];
        for (var i = 0; i < arr.Length; i++)
            arr[i] = (int)((maxValue + 1) * Utility.GetRandomDouble) - (int)(maxValue * Utility.GetRandomDouble);

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
            var value = (int)((maxValue + 1) * Utility.GetRandomDouble) - (int)(maxValue * Utility.GetRandomDouble);
            if (Test(arr, value) != Exist(arr, value))
            {
                succeed = false;
                break;
            }
        }

        Console.WriteLine(succeed ? "测试通过" : "出现错误");
    }
}