//测试通过

#region

using Common.DataStructures.Heap;
using Common.Utilities;

#endregion

namespace Algorithms.Lesson14;

public class LessMoneySplitGold
{
    // 纯暴力！
    private static int LessMoney1(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;

        return Process(arr, 0);
    }

    // 等待合并的数都在arr里，pre之前的合并行为产生了多少总代价
    // arr中只剩一个数字的时候，停止合并，返回最小的总代价
    private static int Process(int[] arr, int pre)
    {
        if (arr.Length == 1) return pre;

        var ans = int.MaxValue;
        for (var i = 0; i < arr.Length; i++)
        for (var j = i + 1; j < arr.Length; j++)
            ans = Math.Min(ans, Process(CopyAndMergeTwo(arr, i, j), pre + arr[i] + arr[j]));

        return ans;
    }

    private static int[] CopyAndMergeTwo(int[] arr, int i, int j)
    {
        var ans = new int[arr.Length - 1];
        var ansi = 0;
        for (var arri = 0; arri < arr.Length; arri++)
            if (arri != i && arri != j)
                ans[ansi++] = arr[arri];

        ans[ansi] = arr[i] + arr[j];
        return ans;
    }

    private static int LessMoney2(int[] arr)
    {
        var pQ = new Heap<int>((x, y) => x.CompareTo(y));
        foreach (var element in arr) pQ.Push(element);

        var sum = 0;
        while (pQ.Count > 1)
        {
            var cur = pQ.Pop() + pQ.Pop();
            sum += cur;
            pQ.Push(cur);
        }

        return sum;
    }

    //用于测试
    private static int[] GenerateRandomArray(int maxSize, int maxValue)
    {
        var arr = new int[(int)((maxSize + 1) * Utility.GetRandomDouble)];
        for (var i = 0; i < arr.Length; i++) arr[i] = (int)(Utility.GetRandomDouble * (maxValue + 1));

        return arr;
    }

    public static void Run()
    {
        var testTime = 100000;
        var maxSize = 6;
        var maxValue = 1000;
        for (var i = 0; i < testTime; i++)
        {
            var arr = GenerateRandomArray(maxSize, maxValue);
            if (LessMoney1(arr) != LessMoney2(arr)) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("finish!");
    }
}