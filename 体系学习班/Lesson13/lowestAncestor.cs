//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson13;

public class LowestAncestor
{
    private static Node? LowestAncestor1(Node? head, Node? o1, Node? o2)
    {
        if (head == null) return null;

        // key的父节点是value
        Dictionary<Node, Node?> parentMap = new() { { head, null } };
        FillParentMap(head, parentMap);
        HashSet<Node?> o1Set = new();
        var cur = o1;
        o1Set.Add(cur);
        while (parentMap[cur ?? throw new InvalidOperationException()] != null)
        {
            cur = parentMap[cur];
            o1Set.Add(cur);
        }

        cur = o2;
        while (!o1Set.Contains(cur)) cur = parentMap[cur ?? throw new InvalidOperationException()];

        return cur;
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

    private static Node? LowestAncestor2(Node? head, Node? a, Node? b)
    {
        return Process(head, a, b).Ans;
    }

    private static Info Process(Node? x, Node? a, Node? b)
    {
        if (x == null) return new Info(false, false, null);

        var leftInfo = Process(x.Left, a, b);
        var rightInfo = Process(x.Right, a, b);
        var findA = x == a || leftInfo.FindA || rightInfo.FindA;
        var findB = x == b || leftInfo.FindB || rightInfo.FindB;
        Node? ans = null;
        if (leftInfo.Ans != null)
        {
            ans = leftInfo.Ans;
        }
        else if (rightInfo.Ans != null)
        {
            ans = rightInfo.Ans;
        }
        else
        {
            if (findA && findB) ans = x;
        }

        return new Info(findA, findB, ans);
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

    //用于测试
    private static Node? pickRandomOne(Node? head)
    {
        if (head == null) return null;

        List<Node> arr = new();
        FillPreList(head, arr);
        var randomIndex = (int)(Utility.getRandomDouble * arr.Count);
        return arr[randomIndex];
    }

    //用于测试
    private static void FillPreList(Node? head, List<Node> arr)
    {
        if (head == null) return;

        arr.Add(head);
        FillPreList(head.Left, arr);
        FillPreList(head.Right, arr);
    }

    public static void Run()
    {
        var maxLevel = 4;
        var maxValue = 100;
        var testTimes = 1000000;
        for (var i = 0; i < testTimes; i++)
        {
            var head = GenerateRandomBst(maxLevel, maxValue);
            var o1 = pickRandomOne(head);
            var o2 = pickRandomOne(head);
            if (LowestAncestor1(head, o1, o2) != LowestAncestor2(head, o1, o2)) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");
    }

    private class Node(int data)
    {
        public Node? Left;
        public Node? Right;
        public int Value = data;
    }

    private class Info(bool fA, bool fB, Node? an)
    {
        public readonly Node? Ans = an;
        public readonly bool FindA = fA;
        public readonly bool FindB = fB;
    }
}