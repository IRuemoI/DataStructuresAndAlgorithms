using System.Security.Cryptography;
using System.Text;
using Common.Utilities;

namespace AdvancedTraining.Lesson05;

// 如果一个节点X，它左树结构和右树结构完全一样，那么我们说以X为头的子树是相等子树
// 给定一棵二叉树的头节点head，返回head整棵树上有多少棵相等子树
public class LeftRightSameTreeNumber
{
    // 时间复杂度O(N * logN)
    private static int SameNumber1(Node? head)
    {
        if (head == null) return 0;

        return SameNumber1(head.Left) + SameNumber2(head.Right) + (Same(head.Left, head.Right) ? 1 : 0);
    }

    private static bool Same(Node? h1, Node? h2)
    {
        if ((h1 == null) ^ (h2 == null)) return false;

        if (h1 == null && h2 == null) return true;

        // 两个都不为空
        return h1?.Value == h2?.Value && Same(h1?.Left, h2?.Left) && Same(h1?.Right, h2?.Right);
    }

    // 时间复杂度O(N)
    private static int SameNumber2(Node? head)
    {
        var algorithm = "SHA-256";
        var hash = new Hash(algorithm);
        return Process(head, hash).Ans;
    }

    private static Info Process(Node? head, Hash hash)
    {
        if (head == null) return new Info(0, hash.HashCode("#,"));

        var l = Process(head.Left, hash);
        var r = Process(head.Right, hash);
        var ans = (l.Str.Equals(r.Str) ? 1 : 0) + l.Ans + r.Ans;
        var str = hash.HashCode(head.Value + "," + l.Str + r.Str);
        return new Info(ans, str);
    }

    private static Node? RandomBinaryTree(int restLevel, int maxValue)
    {
        if (restLevel == 0) return null;

        var head = new Node((int)(Utility.GetRandomDouble * maxValue));
        if (head != null)
        {
            head.Left = RandomBinaryTree(restLevel - 1, maxValue);
            head.Right = RandomBinaryTree(restLevel - 1, maxValue);
        }

        return head;
    }

    public static void Run()
    {
        const int maxLevel = 8;
        const int maxValue = 4;
        const int testTime = 100;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var head = RandomBinaryTree(maxLevel, maxValue);
            var ans1 = SameNumber1(head);
            var ans2 = SameNumber2(head);
            if (ans1 != ans2)
            {
                Console.WriteLine("出错了！");
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
            }
        }

        Console.WriteLine("测试结束");
    }

    private class Node(int v)
    {
        public readonly int Value = v;
        public Node? Left;
        public Node? Right;
    }

    private class Info(int a, string s)
    {
        public readonly int Ans = a;
        public readonly string Str = s;
    }
}

public class Hash(string algorithm)
{
    public string HashCode(string input)
    {
        using var hash = HashAlgorithm.Create(algorithm);
        var hashBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}