//pass

#region

using Common.DataStructures.Heap;

#endregion

namespace AdvancedTraining.Lesson18;
// 牛客的测试链接：
// https://www.nowcoder.com/practice/7201cacf73e7495aa5f88b223bbbf6d1
// 不要提交包信息，把import底下的类名改成Main，提交下面的代码可以直接通过
// 因为测试平台会卡空间，所以把set换成了动态加和减的结构

public class TopKSumCrossTwoArrays
{
    public static void Run()
    {
        const int k = 4;
        var arr1 = new[] { 1, 2, 3, 4, 5 };
        var arr2 = new[] { 3, 5, 7, 9, 11 };

        var topK = topKSum(arr1, arr2, k);
        for (var i = 0; i < k; i++) Console.Write(topK?[i] + " "); //16 15 14 14
    }

    private static int[]? topKSum(int[]? arr1, int[]? arr2, int topK)
    {
        if (arr1 == null || arr2 == null || topK < 1) return null;

        var n = arr1.Length;
        var m = arr2.Length;
        topK = Math.Min(topK, n * m);
        var res = new int[topK];
        var resIndex = 0;
        var maxHeap = new Heap<Node>((x, y) => y.Sum - x.Sum);
        var set = new HashSet<long>();
        var i1 = n - 1;
        var i2 = m - 1;
        maxHeap.Push(new Node(i1, i2, arr1[i1] + arr2[i2]));
        set.Add(X(i1, i2, m));
        while (resIndex != topK)
        {
            var curNode = maxHeap.Pop();
            res[resIndex++] = curNode.Sum;
            i1 = curNode.Index1;
            i2 = curNode.Index2;
            set.Remove(X(i1, i2, m));
            if (i1 - 1 >= 0 && !set.Contains(X(i1 - 1, i2, m)))
            {
                set.Add(X(i1 - 1, i2, m));
                maxHeap.Push(new Node(i1 - 1, i2, arr1[i1 - 1] + arr2[i2]));
            }

            if (i2 - 1 >= 0 && !set.Contains(X(i1, i2 - 1, m)))
            {
                set.Add(X(i1, i2 - 1, m));
                maxHeap.Push(new Node(i1, i2 - 1, arr1[i1] + arr2[i2 - 1]));
            }
        }

        return res;
    }

    private static long X(int i1, int i2, int m)
    {
        return (long)i1 * m + i2;
    }

    // 放入大根堆中的结构
    private class Node(int i1, int i2, int s)
    {
        public readonly int Index1 = i1; // arr1中的位置
        public readonly int Index2 = i2; // arr2中的位置
        public readonly int Sum = s; // arr1[index1] + arr2[index2]的值
    }
}