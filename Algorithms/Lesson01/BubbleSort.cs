//测试通过

#region

using Common;
using Common.Utilities;

#endregion

namespace Algorithms.Lesson01;

public class BubbleSort : QuestionTemplate<int[]?, int[]?>
{
    //用于测试
    public static void Run()
    {
        new BubbleSort().RunTests();
    }

    protected override int[] GenerateInputArgs()
    {
        var maxSize = 50;
        var maxValue = 100;
        var arr = new int[(int)((maxSize + 1) * Utility.GetRandomDouble)];
        for (var i = 0; i < arr.Length; i++)
            arr[i] = (int)((maxValue + 1) * Utility.GetRandomDouble) - (int)(maxValue * Utility.GetRandomDouble);

        return arr;
    }

    protected override int[]? QuestionSolution(int[]? list)
    {
        if (list == null || list.Length == 1) return list;
        //对于冒泡排序(升序)：
        //将数组分为左侧未排序的部分(0~i-1)，和右侧已排序的部分(i~list.Length-1)
        //每次比较时将较大值交换到右侧，完成一趟排序后使得最大值位于未排序部分的最右侧，之后向左扩大已排序的部分。
        for (var i = list.Length - 1; i > 0; i--)
        for (var j = 0; j < i; j++)
            //如果未排序的部分中当前数小于下一个数
            if (list[j] > list[j + 1])
                (list[j], list[j + 1]) = (list[j + 1], list[j]); //让较大的数字右移
        return list;
    }

    protected override int[]? ExpectedResult(int[]? testData)
    {
        if (testData == null) return null;
        //Console.WriteLine(string.Join(",", testData));
        Array.Sort(testData);
        return testData;
    }

    protected override bool CheckResult(int[]? arr1, int[]? arr2)
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
}