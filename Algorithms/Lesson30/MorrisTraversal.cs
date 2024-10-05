//通过

#region

using System.Text;

#endregion

namespace Algorithms.Lesson30;

public class MorrisTraversal
{
    private static void Process(Node? root)
    {
        if (root == null) return;

        // 1
        Process(root.Left);
        // 2
        Process(root.Right);
        // 3
    }

    private static void Morris(Node? head)
    {
        if (head == null) return;

        var cur = head;
        while (cur != null)
        {
            var mostRight = cur.Left;
            if (mostRight != null)
            {
                while (mostRight.Right != null && mostRight.Right != cur) mostRight = mostRight.Right;

                if (mostRight.Right == null)
                {
                    mostRight.Right = cur;
                    cur = cur.Left;
                    continue;
                }

                mostRight.Right = null;
            }

            cur = cur.Right;
        }
    }

    private static void MorrisPre(Node? head)
    {
        if (head == null) return;

        var cur = head;
        while (cur != null)
        {
            var mostRight = cur.Left;
            if (mostRight != null)
            {
                while (mostRight.Right != null && mostRight.Right != cur) mostRight = mostRight.Right;

                if (mostRight.Right == null)
                {
                    Console.Write(cur.Value + " ");
                    mostRight.Right = cur;
                    cur = cur.Left;
                    continue;
                }

                mostRight.Right = null;
            }
            else
            {
                Console.Write(cur.Value + " ");
            }

            cur = cur.Right;
        }

        Console.WriteLine();
    }

    private static void MorrisIn(Node? head)
    {
        if (head == null) return;

        var cur = head;
        while (cur != null)
        {
            var mostRight = cur.Left;
            if (mostRight != null)
            {
                while (mostRight.Right != null && mostRight.Right != cur) mostRight = mostRight.Right;

                if (mostRight.Right == null)
                {
                    mostRight.Right = cur;
                    cur = cur.Left;
                    continue;
                }

                mostRight.Right = null;
            }

            Console.Write(cur.Value + " ");
            cur = cur.Right;
        }

        Console.WriteLine();
    }

    private static void MorrisPos(Node? head)
    {
        if (head == null) return;

        var cur = head;
        while (cur != null)
        {
            var mostRight = cur.Left;
            if (mostRight != null)
            {
                while (mostRight.Right != null && mostRight.Right != cur) mostRight = mostRight.Right;

                if (mostRight.Right == null)
                {
                    mostRight.Right = cur;
                    cur = cur.Left;
                    continue;
                }

                mostRight.Right = null;
                PrintEdge(cur.Left);
            }

            cur = cur.Right;
        }

        PrintEdge(head);
        Console.WriteLine();
    }

    private static void PrintEdge(Node? head)
    {
        var tail = ReverseEdge(head);
        var cur = tail;
        while (cur != null)
        {
            Console.Write(cur.Value + " ");
            cur = cur.Right;
        }

        ReverseEdge(tail);
    }

    private static Node? ReverseEdge(Node? from)
    {
        Node? pre = null;
        while (from != null)
        {
            var next = from.Right;
            from.Right = pre;
            pre = from;
            from = next;
        }

        return pre;
    }

    //���ڲ��� -- print tree
    private static void PrintTree(Node head)
    {
        Console.WriteLine("Binary Tree:");
        PrintInOrder(head, 0, "H", 17);
        Console.WriteLine();
    }

    private static void PrintInOrder(Node? head, int height, string to, int len)
    {
        if (head == null) return;

        PrintInOrder(head.Right, height + 1, "v", len);
        var val = to + head.Value + to;
        var lenM = val.Length;
        var lenL = (len - lenM) / 2;
        var lenR = len - lenM - lenL;
        val = GetSpace(lenL) + val + GetSpace(lenR);
        Console.WriteLine(GetSpace(height * len) + val);
        PrintInOrder(head.Left, height + 1, "^", len);
    }

    private static string GetSpace(int num)
    {
        var space = " ";
        var buf = new StringBuilder("");
        for (var i = 0; i < num; i++) buf.Append(space);

        return buf.ToString();
    }

    private static bool IsBst(Node? head)
    {
        if (head == null) return true;

        var cur = head;
        int? pre = null;
        var ans = true;
        while (cur != null)
        {
            var mostRight = cur.Left;
            if (mostRight != null)
            {
                while (mostRight.Right != null && mostRight.Right != cur) mostRight = mostRight.Right;

                if (mostRight.Right == null)
                {
                    mostRight.Right = cur;
                    cur = cur.Left;
                    continue;
                }

                mostRight.Right = null;
            }

            if (pre != null && pre >= cur.Value) ans = false;

            pre = cur.Value;
            cur = cur.Right;
        }

        return ans;
    }

    public static void Run()
    {
        var head = new Node(4)
        {
            Left = new Node(2),
            Right = new Node(6)
        };
        head.Left.Left = new Node(1);
        head.Left.Right = new Node(3);
        head.Right.Left = new Node(5);
        head.Right.Right = new Node(7);
        PrintTree(head);
        MorrisIn(head);
        MorrisPre(head);
        MorrisPos(head);
        PrintTree(head);
    }

    private class Node(int data)
    {
        public readonly int Value = data;
        public Node? Left;
        public Node? Right;
    }
}