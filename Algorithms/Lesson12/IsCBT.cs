//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson12;

// 判断一棵二叉树是否是完全二叉树
public class IsCbt
{
    private static bool IsCbt1(Node? head)
    { 
        if (head == null) return true;

        Queue<Node> queue = new();
        // 是否遇到过左右两个孩子不双全的节点
        var leaf = false;
        queue.Enqueue(head);
        while (queue.Count != 0)
        {
            head = queue.Dequeue();
            var l = head.Left;
            var r = head.Right;
            if (
                // 如果遇到了不双全的节点之后，又发现当前节点不是叶节点
                (leaf && (l != null || r != null))
                ||
                (l == null && r != null)
            )
                return false;

            if (l != null) queue.Enqueue(l);

            if (r != null) queue.Enqueue(r);

            if (l == null || r == null) leaf = true;
        }

        return true;
    }

    private static bool IsCbt2(Node? head)
    {
        if (head == null) return true;

        return Process(head).IsCbt;
    }

    private static Info Process(Node? x)
    {
        if (x == null) return new Info(true, true, 0);

        var leftInfo = Process(x.Left);
        var rightInfo = Process(x.Right);


        var height = Math.Max(leftInfo.Height, rightInfo.Height) + 1;


        var isFull = leftInfo.IsFull
                     &&
                     rightInfo.IsFull
                     && leftInfo.Height == rightInfo.Height;


        var isCbt = false;
        if (isFull)
        {
            isCbt = true;
        }
        else
        {
            // 以x为头整棵树，不满
            if (leftInfo.IsCbt && rightInfo.IsCbt)
            {
                if (leftInfo.IsCbt
                    && rightInfo.IsFull
                    && leftInfo.Height == rightInfo.Height + 1)
                    isCbt = true;

                if (leftInfo.IsFull
                    &&
                    rightInfo.IsFull
                    && leftInfo.Height == rightInfo.Height + 1)
                    isCbt = true;

                if (leftInfo.IsFull
                    && rightInfo.IsCbt && leftInfo.Height == rightInfo.Height)
                    isCbt = true;
            }
        }

        return new Info(isFull, isCbt, height);
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
        var maxLevel = 5;
        var maxValue = 100;
        var testTimes = 1000000;
        for (var i = 0; i < testTimes; i++)
        {
            var head = GenerateRandomBst(maxLevel, maxValue);
            if (IsCbt1(head) != IsCbt2(head)) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");
    }

    private class Node(int data)
    {
        public Node? Left;
        public Node? Right;
        public int Value = data;
    }

    // 对每一棵子树，是否是满二叉树、是否是完全二叉树、高度
    private class Info(bool full, bool cbt, int h)
    {
        public readonly int Height = h;
        public readonly bool IsCbt = cbt;
        public readonly bool IsFull = full;
    }
}