//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson11;

public class TreeMaxWidth
{
    private static int MaxWidthUseMap(Node? head)
    {
        if (head == null) return 0;

        Queue<Node> queue = new();
        queue.Enqueue(head);
        // key 在 哪一层，value
        Dictionary<Node, int> levelMap = new() { { head, 1 } };
        var curLevel = 1; // 当前你正在统计哪一层的宽度
        var curLevelNodes = 0; // 当前层curLevel层，宽度目前是多少
        var max = 0;
        while (queue.Count != 0)
        {
            var cur = queue.Dequeue();
            var curNodeLevel = levelMap[cur];
            if (cur.Left != null)
            {
                levelMap.Add(cur.Left, curNodeLevel + 1);
                queue.Enqueue(cur.Left);
            }

            if (cur.Right != null)
            {
                levelMap.Add(cur.Right, curNodeLevel + 1);
                queue.Enqueue(cur.Right);
            }

            if (curNodeLevel == curLevel)
            {
                curLevelNodes++;
            }
            else
            {
                max = Math.Max(max, curLevelNodes);
                curLevel++;
                curLevelNodes = 1;
            }
        }

        max = Math.Max(max, curLevelNodes);
        return max;
    }

    private static int MaxWidthNoMap(Node? head)
    {
        if (head == null) return 0;

        Queue<Node> queue = new();
        queue.Enqueue(head);
        var curEnd = head; // 当前层，最右节点是谁
        Node? nextEnd = null; // 下一层，最右节点是谁
        var max = 0;
        var curLevelNodes = 0; // 当前层的节点数
        while (queue.Count != 0)
        {
            var cur = queue.Dequeue();
            if (cur.Left != null)
            {
                queue.Enqueue(cur.Left);
                nextEnd = cur.Left;
            }

            if (cur.Right != null)
            {
                queue.Enqueue(cur.Right);
                nextEnd = cur.Right;
            }

            curLevelNodes++;
            if (cur != curEnd) continue;
            max = Math.Max(max, curLevelNodes);
            curLevelNodes = 0;
            curEnd = nextEnd;
        }

        return max;
    }

    //用于测试
    private static Node? GenerateRandomBst(int maxLevel, int maxValue)
    {
        return GenerateTree(1, maxLevel, maxValue);
    }

    //用于测试
    private static Node? GenerateTree(int level, int maxLevel, int maxValue)
    {
        if (level > maxLevel || Utility.GetRandomDouble < 0.5) return null;

        var head = new Node((int)(Utility.GetRandomDouble * maxValue))
        {
            Left = GenerateTree(level + 1, maxLevel, maxValue),
            Right = GenerateTree(level + 1, maxLevel, maxValue)
        };
        return head;
    }

    public static void Run()

    {
        var maxLevel = 10;
        var maxValue = 100;
        var testTimes = 1000000;
        for (var i = 0; i < testTimes; i++)
        {
            var head = GenerateRandomBst(maxLevel, maxValue);
            if (MaxWidthUseMap(head) != MaxWidthNoMap(head)) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");
    }

    public class Node(int data)
    {
        public Node? Left;
        public Node? Right;
        public int Value = data;
    }
}