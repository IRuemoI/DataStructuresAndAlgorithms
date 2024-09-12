//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson13;

public class MaxSubBstHead
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

    private static Node? MaxSubBstHead1(Node? head)
    {
        if (head == null) return null;

        if (GetBstSize(head) != 0) return head;

        var leftAns = MaxSubBstHead1(head.Left);
        var rightAns = MaxSubBstHead1(head.Right);
        return GetBstSize(leftAns) >= GetBstSize(rightAns) ? leftAns : rightAns;
    }

    private static Node? MaxSubBstHead2(Node? head)
    {
        if (head == null) return null;

        return Process(head)?.MaxSubBstHead;
    }

    private static Info? Process(Node? x)
    {
        if (x == null) return null;

        var leftInfo = Process(x.Left);
        var rightInfo = Process(x.Right);
        var min = x.Value;
        var max = x.Value;
        Node? maxSubBstHead = null;
        var maxSubBstSize = 0;
        if (leftInfo != null)
        {
            min = Math.Min(min, leftInfo.Min);
            max = Math.Max(max, leftInfo.Max);
            maxSubBstHead = leftInfo.MaxSubBstHead;
            maxSubBstSize = leftInfo.MaxSubBstSize;
        }

        if (rightInfo != null)
        {
            min = Math.Min(min, rightInfo.Min);
            max = Math.Max(max, rightInfo.Max);
            if (rightInfo.MaxSubBstSize > maxSubBstSize)
            {
                maxSubBstHead = rightInfo.MaxSubBstHead;
                maxSubBstSize = rightInfo.MaxSubBstSize;
            }
        }

        if ((leftInfo == null || (leftInfo.MaxSubBstHead == x.Left && leftInfo.Max < x.Value))
            && (rightInfo == null || (rightInfo.MaxSubBstHead == x.Right && rightInfo.Min > x.Value)))
        {
            maxSubBstHead = x;
            maxSubBstSize = (leftInfo?.MaxSubBstSize ?? 0) + (rightInfo?.MaxSubBstSize ?? 0) + 1;
        }

        return new Info(maxSubBstHead, maxSubBstSize, min, max);
    }

    //用于测试
    private static Node? GenerateRandomBst(int maxLevel, int maxValue)
    {
        return Generate(1, maxLevel, maxValue);
    }

    //用于测试
    private static Node? Generate(int level, int maxLevel, int maxValue)
    {
        if (level > maxLevel || Utility.GetRandomDouble < 0.5) return null;

        var head = new Node((int)(Utility.GetRandomDouble * maxValue))
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
            if (MaxSubBstHead1(head) != MaxSubBstHead2(head)) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");
    }

    public class Node
    {
        public readonly int Value;
        public Node? Left;
        public Node? Right;

        public Node(int data)
        {
            Value = data;
        }
    }

    // 每一棵子树
    private class Info
    {
        public readonly int Max;
        public readonly Node? MaxSubBstHead;
        public readonly int MaxSubBstSize;
        public readonly int Min;

        public Info(Node? h, int size, int mi, int ma)
        {
            MaxSubBstHead = h;
            MaxSubBstSize = size;
            Min = mi;
            Max = ma;
        }
    }
}