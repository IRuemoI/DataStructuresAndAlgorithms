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
        return Process2(head).IsBalanced;
    }

    private static Info Process2(Node? x)
    {
        if (x == null) return new Info(true, 0);

        var leftInfo = Process2(x.Left);
        var rightInfo = Process2(x.Right);
        var height = Math.Max(leftInfo.Height, rightInfo.Height) + 1;
        var isBalanced = leftInfo.IsBalanced;

        if (!rightInfo.IsBalanced) isBalanced = false;

        if (Math.Abs(leftInfo.Height - rightInfo.Height) > 1) isBalanced = false;

        return new Info(isBalanced, height);
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
        var maxLevel = 5;
        var maxValue = 100;
        var testTimes = 1000000;
        for (var i = 0; i < testTimes; i++)
        {
            var head = GenerateRandomBst(maxLevel, maxValue);
            if (head != null && IsBalanced1(head) != IsBalanced2(head)) Console.WriteLine("出错了！");
        }

        Console.WriteLine("测试完成");
    }

    private class Node(int data)
    {
        public Node? Left;
        public Node? Right;
        public int Value = data;
    }

    private class Info(bool i, int h)
    {
        public readonly int Height = h;
        public readonly bool IsBalanced = i;
    }
}