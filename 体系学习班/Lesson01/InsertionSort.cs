//测试通过

#region

using Common;
using Common.Utilities;

#endregion

namespace Algorithms.Lesson01;

public class InsertionSort : QuestionTemplate<int[]?, int[]?>
{
    public static void Run()
    {
        new InsertionSort().RunTests();
    }

    protected override int[] GenerateInputArgs()
    {
        var maxSize = 50;
        var maxValue = 100;
        var arr = new int[(int)((maxSize + 1) * Utility.getRandomDouble)];
        for (var i = 0; i < arr.Length; i++)
            arr[i] = (int)((maxValue + 1) * Utility.getRandomDouble) - (int)(maxValue * Utility.getRandomDouble);

        return arr;
    }

    protected override int[]? QuestionSolution(int[]? list)
    {
        if (list == null || list.Length == 1) return list;
        //对于插入排序(升序)：  
        //将数组分为左右两部分，左侧时已排序的部分[0~i],右侧是未排序的部分[(i+1)~list.Length-1]。  
        //每趟将右侧无序部分的第一个元素向左有序部分移动(交换)直到这个元素不小于左侧的元素停止，将目标值移动到这个索引  
        for (var i = 1; i < list.Length; i++)
        for (var j = i - 1; j >= 0; j--)
            if (list[j] > list[j + 1])
                (list[j], list[j + 1]) = (list[j + 1], list[j]);
        return list;
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