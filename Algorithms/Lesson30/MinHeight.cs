//测试通过

namespace Algorithms.Lesson30;

public class MinHeight
{
    private static int MinHeight1(Node? head)
    {
        if (head == null) return 0;

        return P(head);
    }

    // 返回x为头的树，最小深度是多少
    private static int P(Node? x)
    {
        if (x?.Left == null && x?.Right == null) return 1;

        // 左右子树起码有一个不为空
        var leftH = int.MaxValue;
        if (x.Left != null) leftH = P(x.Left);

        var rightH = int.MaxValue;
        if (x.Right != null) rightH = P(x.Right);

        return 1 + Math.Min(leftH, rightH);
    }

    // 根据morris遍历改写
    private static int MinHeight2(Node? head)
    {
        if (head == null) return 0;

        var cur = head;
        var curLevel = 0;
        var minHeight = int.MaxValue;
        while (cur != null)
        {
            var mostRight = cur.Left;
            if (mostRight != null)
            {
                var rightBoardSize = 1;
                while (mostRight.Right != null && mostRight.Right != cur)
                {
                    rightBoardSize++;
                    mostRight = mostRight.Right;
                }

                if (mostRight.Right == null)
                {
                    // 第一次到达
                    curLevel++;
                    mostRight.Right = cur;
                    cur = cur.Left;
                    continue;
                }

                // 第二次到达
                if (mostRight.Left == null) minHeight = Math.Min(minHeight, curLevel);

                curLevel -= rightBoardSize;
                mostRight.Right = null;
            }
            else
            {
                // 只有一次到达
                curLevel++;
            }

            cur = cur.Right;
        }

        var finalRight = 1;
        cur = head;
        while (cur.Right != null)
        {
            finalRight++;
            cur = cur.Right;
        }

        if (cur.Left == null && cur.Right == null) minHeight = Math.Min(minHeight, finalRight);

        return minHeight;
    }

    //用于测试
    private static Node? GenerateRandomBst(int maxLevel, int maxValue)
    {
        return Generate(1, maxLevel, maxValue);
    }

    //用于测试
    private static Node? Generate(int level, int maxLevel, int maxValue)
    {
        if (level > maxLevel || new Random().NextDouble() < 0.5) return null;

        var head = new Node((int)(new Random().NextDouble() * maxValue))
        {
            Left = Generate(level + 1, maxLevel, maxValue),
            Right = Generate(level + 1, maxLevel, maxValue)
        };
        return head;
    }

    public static void Run()
    {
        var treeLevel = 7;
        var nodeMaxValue = 5;
        var testTimes = 100000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTimes; i++)
        {
            var head = GenerateRandomBst(treeLevel, nodeMaxValue);
            var ans1 = MinHeight1(head);
            var ans2 = MinHeight2(head);
            if (ans1 != ans2) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("test finish!");
    }

    public class Node
    {
        public Node? Left;
        public Node? Right;
        public int Val;

        public Node(int x)
        {
            Val = x;
        }
    }
}