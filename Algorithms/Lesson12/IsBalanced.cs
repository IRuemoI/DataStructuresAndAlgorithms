//通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson12;

public class IsBalanced
{
    private static bool IsBalanced1(Node head)
    {
        var ans = new bool[1];
        ans[0] = true;
        Process1(head, ans);
        return ans[0];
    }

    private static int Process1(Node? head, bool[] ans)
    {
        if (!ans[0] || head == null) return -1;

        var leftHeight = Process1(head.Left, ans);
        var rightHeight = Process1(head.Right, ans);
        if (Math.Abs(leftHeight - rightHeight) > 1) ans[0] = false;

        return Math.Max(leftHeight, rightHeight) + 1;
    }

    private static bool IsBalanced2(Node head)
    {
        return Process(head).IsBalanced;
    }

    private static Info Process(Node? x)
    {
        if (x == null) return new Info(true, 0);

        var leftInfo = Process(x.Left);
        var rightInfo = Process(x.Right);
        var height = Math.Max(leftInfo.Height, rightInfo.Height) + 1;
        var isBalanced = leftInfo.IsBalanced;

        if (!rightInfo.IsBalanced) isBalanced = false;

        if (Math.Abs(leftInfo.Height - rightInfo.Height) > 1) isBalanced = false;

        return new Info(isBalanced, height);
    }


    //���ڲ���
    private static Node? GenerateRandomBst(int maxLevel, int maxValue)
    {
        return Generate(1, maxLevel, maxValue);
    }

    //���ڲ���
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
        var maxLevel = 5;
        var maxValue = 100;
        var testTimes = 1000000;
        for (var i = 0; i < testTimes; i++)
        {
            var head = GenerateRandomBst(maxLevel, maxValue);
            if (head != null && IsBalanced1(head) != IsBalanced2(head)) Console.WriteLine("��������");
        }

        Console.WriteLine("finish!");
    }

    public class Node
    {
        public Node? Left;
        public Node? Right;
        public int Value;

        public Node(int data)
        {
            Value = data;
        }
    }

    private class Info
    {
        public readonly int Height;
        public readonly bool IsBalanced;

        public Info(bool i, int h)
        {
            IsBalanced = i;
            Height = h;
        }
    }
}