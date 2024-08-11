//测试通过

#region

using Common;
using Common.Utilities;

#endregion

namespace Algorithms.Lesson01;

public class SelectionSort : QuestionTemplate<int[]?, int[]?>
{
    protected override int[]? QuestionSolution(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return arr;

        // 0 ~ N-1  找到最小值，在哪，放到0位置上
        // 1 ~ n-1  找到最小值，在哪，放到1 位置上
        // 2 ~ n-1  找到最小值，在哪，放到2 位置上
        for (var i = 0; i < arr.Length - 1; i++)
        {
            var minIndex = i;
            for (var j = i + 1; j < arr.Length; j++)
                // i ~ N-1 上找最小值的下标 
                minIndex = arr[j] < arr[minIndex] ? j : minIndex;

            (arr[i], arr[minIndex]) = (arr[minIndex], arr[i]);
        }

        return arr;
    }

    public static void Run()
    {
        new SelectionSort().RunTests();
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

    protected override int[]? ExpectedResult(int[]? testData)
    {
        if (testData == null) return null;
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