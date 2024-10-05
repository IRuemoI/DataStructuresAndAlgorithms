//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson12;

public class MaxSubBstSize
{
    private static int GetBstSize(Node? head)
    {
        if (head == null) return 0;

        List<Node> arr = new();
        In1(head, arr);
        for (var i = 1; i < arr.Count; i++)
            if (arr[i].Value <= arr[i - 1].Value)
                return 0;

        return arr.Count;
    }

    private static void In1(Node? head, List<Node> arr)
    {
        if (head == null) return;

        In1(head.Left, arr);
        arr.Add(head);
        In1(head.Right, arr);
    }

    private static int MaxSubBstSize1(Node? head)
    {
        if (head == null) return 0;

        var h = GetBstSize(head);
        if (h != 0) return h;

        return Math.Max(MaxSubBstSize1(head.Left), MaxSubBstSize1(head.Right));
    }

    // private static int maxSubBSTSize3(Node head)
    // {
    //     if (head == null)
    //     {
    //         return 0;
    //     }
    //
    //     return process(head).maxSubBSTSize;
    // }
    //
    //
    // // 任何子树
    // public class Info
    // {
    //     public bool isAllBST;
    //     public int maxSubBSTSize;
    //     public int min;
    //     public int max;
    //
    //     public Info(bool is1, int size, int mi, int ma)
    //     {
    //         isAllBST = is1;
    //         maxSubBSTSize = size;
    //         min = mi;
    //         max = ma;
    //     }
    // }
    //
    //
    // private static Info process1(Node X)
    // {
    //     if (X == null)
    //     {
    //         return null;
    //     }
    //
    //     Info leftInfo = process1(X.left);
    //     Info rightInfo = process1(X.right);
    //
    //
    //     int min = X.value;
    //     int max = X.value;
    //
    //     if (leftInfo != null)
    //     {
    //         min = Math.Min(min, leftInfo.min);
    //         max = Math.Max(max, leftInfo.max);
    //     }
    //
    //     if (rightInfo != null)
    //     {
    //         min = Math.Min(min, rightInfo.min);
    //         max = Math.Max(max, rightInfo.max);
    //     }
    //
    //
    //     int maxSubBSTSize = 0;
    //     if (leftInfo != null)
    //     {
    //         maxSubBSTSize = leftInfo.maxSubBSTSize;
    //     }
    //
    //     if (rightInfo != null)
    //     {
    //         maxSubBSTSize = Math.Max(maxSubBSTSize, rightInfo.maxSubBSTSize);
    //     }
    //
    //     bool isAllBST = false;
    //
    //
    //     if (
    //         // 左树整体需要是搜索二叉树
    //         (leftInfo == null ? true : leftInfo.isAllBST)
    //         &&
    //         (rightInfo == null ? true : rightInfo.isAllBST)
    //         &&
    //         // 左树最大值<x
    //         (leftInfo == null ? true : leftInfo.max < X.value)
    //         &&
    //         (rightInfo == null ? true : rightInfo.min > X.value)
    //     )
    //     {
    //         maxSubBSTSize =
    //             (leftInfo == null ? 0 : leftInfo.maxSubBSTSize)
    //             +
    //             (rightInfo == null ? 0 : rightInfo.maxSubBSTSize)
    //             +
    //             1;
    //         isAllBST = true;
    //     }
    //
    //     return new Info(isAllBST, maxSubBSTSize, min, max);
    // }

    private static int MaxSubBstSize2(Node? head)
    {
        if (head == null) return 0;

        return Process(head)!.MaxBstSubtreeSize;
    }

    private static Info? Process(Node? x)
    {
        if (x == null) return null;

        var leftInfo = Process(x.Left);
        var rightInfo = Process(x.Right);
        var max = x.Value;
        var min = x.Value;
        var allSize = 1;
        if (leftInfo != null)
        {
            max = Math.Max(leftInfo.Max, max);
            min = Math.Min(leftInfo.Min, min);
            allSize += leftInfo.AllSize;
        }

        if (rightInfo != null)
        {
            max = Math.Max(rightInfo.Max, max);
            min = Math.Min(rightInfo.Min, min);
            allSize += rightInfo.AllSize;
        }

        var p1 = -1;
        if (leftInfo != null) p1 = leftInfo.MaxBstSubtreeSize;

        var p2 = -1;
        if (rightInfo != null) p2 = rightInfo.MaxBstSubtreeSize;

        var p3 = -1;
        var leftBst = leftInfo == null || leftInfo.MaxBstSubtreeSize == leftInfo.AllSize;
        var rightBst = rightInfo == null || rightInfo.MaxBstSubtreeSize == rightInfo.AllSize;
        if (leftBst && rightBst)
        {
            var leftMaxLessX = leftInfo == null || leftInfo.Max < x.Value;
            var rightMinMoreX = rightInfo == null || x.Value < rightInfo.Min;
            if (leftMaxLessX && rightMinMoreX)
            {
                var leftSize = leftInfo?.AllSize ?? 0;
                var rightSize = rightInfo?.AllSize ?? 0;
                p3 = leftSize + rightSize + 1;
            }
        }

        return new Info(Math.Max(p1, Math.Max(p2, p3)), allSize, max, min);
    }

    //用于测试
    private static Node? GenerateRandomBst(int maxLevel, int maxValue)
    {
        return Generate(1, maxLevel, maxValue);
    }

    //用于测试
    private static Node? Generate(int level, int maxLevel, int maxValue)
    {
        if (level > maxLevel || Utility.getRandomDouble < 0.5) return null;

        var head = new Node((int)(Utility.getRandomDouble * maxValue))
        {
            Left = Generate(level + 1, maxLevel, maxValue),
            Right = Generate(level + 1, maxLevel, maxValue)
        };
        return head;
    }

    public static void Run()
    {
        var maxLevel = 4;
        var maxValue = 100;
        var testTimes = 1000000;
        for (var i = 0; i < testTimes; i++)
        {
            var head = GenerateRandomBst(maxLevel, maxValue);
            if (MaxSubBstSize1(head) != MaxSubBstSize2(head)) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");
    }

    private class Node(int data)
    {
        public readonly int Value = data;
        public Node? Left;
        public Node? Right;
    }

    private class Info(int m, int a, int ma, int mi)
    {
        public readonly int AllSize = a;
        public readonly int Max = ma;
        public readonly int MaxBstSubtreeSize = m;
        public readonly int Min = mi;
    }
}