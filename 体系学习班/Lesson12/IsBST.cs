//通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson12;

//二叉树是否是二叉搜索树
public class IsBst
{
    private static bool IsBst1(Node? head)
    {
        if (head == null) return true;

        List<Node> arr = new();
        In1(head, arr);
        for (var i = 1; i < arr.Count; i++)
            if (arr[i].Value <= arr[i - 1].Value)
                return false;

        return true;
    }

    private static void In1(Node? head, List<Node> arr)
    {
        if (head == null) return;

        In1(head.Left, arr);
        arr.Add(head);
        In1(head.Right, arr);
    }

    private static bool IsBst2(Node? head)
    {
        if (head == null) return true;

        return Process(head)!.IsBst;
    }

    private static Info? Process(Node? x)
    {
        if (x == null) return null;

        var leftInfo = Process(x.Left);
        var rightInfo = Process(x.Right);
        var max = x.Value;
        if (leftInfo != null) max = Math.Max(max, leftInfo.Max);

        if (rightInfo != null) max = Math.Max(max, rightInfo.Max);

        var min = x.Value;
        if (leftInfo != null) min = Math.Min(min, leftInfo.Min);

        if (rightInfo != null) min = Math.Min(min, rightInfo.Min);

        var isBst = !(leftInfo is { IsBst: false });
        if (rightInfo is { IsBst: false }) isBst = false;

        if (leftInfo != null && leftInfo.Max >= x.Value) isBst = false;

        if (rightInfo != null && rightInfo.Min <= x.Value) isBst = false;

        return new Info(isBst, max, min);
    }
    
    private static Node? GenerateRandomBst(int maxLevel, int maxValue)
    {
        return Generate(1, maxLevel, maxValue);
    }
    
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
            if (IsBst1(head) != IsBst2(head)) Console.WriteLine("出错了！");
        }

        Console.WriteLine("测试完成");
    }

    private class Node(int data)
    {
        public readonly int Value = data;
        public Node? Left;
        public Node? Right;
    }

    private class Info(bool i, int ma, int mi)
    {
        public readonly bool IsBst = i;
        public readonly int Max = ma;
        public readonly int Min = mi;
    }
}