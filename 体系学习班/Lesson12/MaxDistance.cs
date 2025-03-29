//通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson12;

public class MaxDistance
{
    private static int MaxDistance1(Node? head)
    {
        if (head == null) return 0;

        var arr = GetPreList(head);
        var parentMap = GetParentMap(head);
        var max = 0;
        for (var i = 0; i < arr.Count; i++)
        for (var j = i; j < arr.Count; j++)
            max = Math.Max(max, Distance(parentMap, arr[i], arr[j]));

        return max;
    }

    private static List<Node?> GetPreList(Node head)
    {
        List<Node?> arr = new();
        FillPreList(head, arr);
        return arr;
    }

    private static void FillPreList(Node? head, List<Node?> arr)
    {
        if (head == null) return;

        arr.Add(head);
        FillPreList(head.Left, arr);
        FillPreList(head.Right, arr);
    }

    private static Dictionary<Node, Node?> GetParentMap(Node head)
    {
        Dictionary<Node, Node?> map = new() { { head, null } };
        FillParentMap(head, map);
        return map;
    }

    private static void FillParentMap(Node head, Dictionary<Node, Node?> parentMap)
    {
        if (head.Left != null)
        {
            parentMap.Add(head.Left, head);
            FillParentMap(head.Left, parentMap);
        }

        if (head.Right != null)
        {
            parentMap.Add(head.Right, head);
            FillParentMap(head.Right, parentMap);
        }
    }

    private static int Distance(Dictionary<Node, Node?> parentMap, Node? o1, Node? o2)
    {
        HashSet<Node?> o1Set = [];
        var cur = o1;
        o1Set.Add(cur);
        while (parentMap[cur ?? throw new InvalidOperationException()] != null)
        {
            cur = parentMap[cur];
            o1Set.Add(cur);
        }

        cur = o2;
        while (!o1Set.Contains(cur)) cur = parentMap[cur ?? throw new InvalidOperationException()];

        var lowestAncestor = cur;
        cur = o1;
        var distance1 = 1;
        while (cur != lowestAncestor)
        {
            cur = parentMap[cur ?? throw new InvalidOperationException()];
            distance1++;
        }

        cur = o2;
        var distance2 = 1;
        while (cur != lowestAncestor)
        {
            cur = parentMap[cur ?? throw new InvalidOperationException()];
            distance2++;
        }

        return distance1 + distance2 - 1;
    }

    private static int MaxDistance2(Node? head)
    {
        return Process(head).MaxDistance;
    }

    private static Info Process(Node? x)
    {
        if (x == null) return new Info(0, 0);

        var leftInfo = Process(x.Left);
        var rightInfo = Process(x.Right);
        var height = Math.Max(leftInfo.Height, rightInfo.Height) + 1;
        var p1 = leftInfo.MaxDistance;
        var p2 = rightInfo.MaxDistance;
        var p3 = leftInfo.Height + rightInfo.Height + 1;
        var maxDistance = Math.Max(Math.Max(p1, p2), p3);
        return new Info(maxDistance, height);
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
            if (MaxDistance1(head) != MaxDistance2(head)) Console.WriteLine("出错了！");
        }

        Console.WriteLine("测试完成");
    }

    private class Node(int data)
    {
        public Node? Left;
        public Node? Right;
        public int Value = data;
    }

    private class Info(int m, int h)
    {
        public readonly int Height = h;
        public readonly int MaxDistance = m;
    }
}