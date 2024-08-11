#region

using Common.DataStructures.Heap;

#endregion

namespace AdvancedTraining.Lesson17;

// 本题测试链接 : https://leetcode.cn/problems/kth-smallest-element-in-a-sorted-matrix/
public class KthSmallestElementInSortedMatrix
{
    // 堆的方法
    private static int KthSmallest1(int[,] matrix, int k)
    {
        var n = matrix.GetLength(0);
        var m = matrix.GetLength(1);
        var heap = new Heap<Node>((x, y) => x.Value - y.Value);
        var set = new bool[n, m];
        heap.Push(new Node(matrix[0, 0], 0, 0));
        set[0, 0] = true;
        var count = 0;
        Node? ans = null;
        while (!heap.IsEmpty())
        {
            ans = heap.Pop();
            if (++count == k) break;
            var row = ans.Row;
            var col = ans.Col;
            if (row + 1 < n && !set[row + 1, col])
            {
                heap.Push(new Node(matrix[row + 1, col], row + 1, col));
                set[row + 1, col] = true;
            }

            if (col + 1 < m && !set[row, col + 1])
            {
                heap.Push(new Node(matrix[row, col + 1], row, col + 1));
                set[row, col + 1] = true;
            }
        }

        if (ans != null) return ans.Value;
        throw new Exception();
    }

    // 二分的方法
    private static int KthSmallest2(int[,] matrix, int k)
    {
        var n = matrix.GetLength(0);
        var m = matrix.GetLength(1);
        var left = matrix[0, 0];
        var right = matrix[n - 1, m - 1];
        var ans = 0;
        while (left <= right)
        {
            var mid = left + ((right - left) >> 1);
            // <=mid 有几个 <= mid 在矩阵中真实出现的数，谁最接近mid
            var info = NoMoreNum(matrix, mid);
            if (info.Num < k)
            {
                left = mid + 1;
            }
            else
            {
                ans = info.Near;
                right = mid - 1;
            }
        }

        return ans;
    }

    private static Info NoMoreNum(int[,] matrix, int value)
    {
        var near = int.MinValue;
        var num = 0;
        var n = matrix.GetLength(0);
        var m = matrix.GetLength(1);
        var row = 0;
        var col = m - 1;
        while (row < n && col >= 0)
            if (matrix[row, col] <= value)
            {
                near = Math.Max(near, matrix[row, col]);
                num += col + 1;
                row++;
            }
            else
            {
                col--;
            }

        return new Info(near, num);
    }

    public static void Run()
    {
        int[,] matrix =
        {
            { 1, 5, 9 },
            { 10, 11, 13 },
            { 12, 13, 15 }
        };
        const int k = 8;

        Console.WriteLine(KthSmallest1(matrix, k)); //输出13
        Console.WriteLine(KthSmallest2(matrix, k)); //输出13
    }

    public class Node(int v, int r, int c)
    {
        public readonly int Col = c;
        public readonly int Row = r;
        public readonly int Value = v;
    }

    public class NodeComparator : IComparer<Node>
    {
        public virtual int Compare(Node? o1, Node? o2)
        {
            if (o1 == null || o2 == null) throw new Exception();
            return o1.Value - o2.Value;
        }
    }

    private class Info(int n1, int n2)
    {
        public readonly int Near = n1;
        public readonly int Num = n2;
    }
}