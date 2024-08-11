//测试通过

namespace Algorithms.Lesson09;

public class LinkedListMid
{
    // head 头
    private static Node? MidOrUpMidNode(Node? head)
    {
        if (head?.Next?.Next == null) return head;

        // 链表有3个点或以上
        var slow = head.Next;
        var fast = head.Next.Next;
        while (fast.Next is { Next: not null })
        {
            slow = slow?.Next;
            fast = fast.Next.Next;
        }

        return slow;
    }

    private static Node? MidOrDownMidNode(Node? head)
    {
        if (head?.Next == null) return head;

        var slow = head.Next;
        var fast = head.Next;
        while (fast.Next is { Next: not null })
        {
            slow = slow?.Next;
            fast = fast.Next.Next;
        }

        return slow;
    }

    private static Node? MidOrUpMidPreNode(Node? head)
    {
        if (head?.Next?.Next == null) return null;

        var slow = head;
        var fast = head.Next.Next;
        while (fast.Next is { Next: not null })
        {
            slow = slow?.Next;
            fast = fast.Next.Next;
        }

        return slow;
    }

    private static Node? MidOrDownMidPreNode(Node? head)
    {
        if (head?.Next == null) return null;

        if (head.Next.Next == null) return head;

        var slow = head;
        var fast = head.Next;
        while (fast.Next is { Next: not null })
        {
            slow = slow?.Next;
            fast = fast.Next.Next;
        }

        return slow;
    }

    private static Node? Right1(Node? head)
    {
        if (head == null) return null;

        var cur = head;
        List<Node> arr = new();
        while (cur != null)
        {
            arr.Add(cur);
            cur = cur.Next;
        }

        return arr[(arr.Count - 1) / 2];
    }

    private static Node? Right2(Node? head)
    {
        if (head == null) return null;

        var cur = head;
        List<Node> arr = new();
        while (cur != null)
        {
            arr.Add(cur);
            cur = cur.Next;
        }

        return arr[arr.Count / 2];
    }

    private static Node? Right3(Node? head)
    {
        if (head?.Next?.Next == null) return null;

        var cur = head;
        List<Node> arr = new();
        while (cur != null)
        {
            arr.Add(cur);
            cur = cur.Next;
        }

        return arr[(arr.Count - 3) / 2];
    }

    private static Node? Right4(Node? head)
    {
        if (head?.Next == null) return null;

        var cur = head;
        List<Node> arr = new();
        while (cur != null)
        {
            arr.Add(cur);
            cur = cur.Next;
        }

        return arr[(arr.Count - 2) / 2];
    }

    public static void Run()
    {
        var test = new Node(0)
        {
            Next = new Node(1)
            {
                Next = new Node(2)
                {
                    Next = new Node(3)
                    {
                        Next = new Node(4)
                        {
                            Next = new Node(5)
                            {
                                Next = new Node(6)
                                {
                                    Next = new Node(7)
                                    {
                                        Next = new Node(8)
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        var ans1 = MidOrUpMidNode(test);
        var ans2 = Right1(test);
        Console.WriteLine(ans1 != null ? ans1.Value : "无");
        Console.WriteLine(ans2 != null ? ans2.Value : "无");

        ans1 = MidOrDownMidNode(test);
        ans2 = Right2(test);
        Console.WriteLine(ans1 != null ? ans1.Value : "无");
        Console.WriteLine(ans2 != null ? ans2.Value : "无");

        ans1 = MidOrUpMidPreNode(test);
        ans2 = Right3(test);
        Console.WriteLine(ans1 != null ? ans1.Value : "无");
        Console.WriteLine(ans2 != null ? ans2.Value : "无");

        ans1 = MidOrDownMidPreNode(test);
        ans2 = Right4(test);
        Console.WriteLine(ans1 != null ? ans1.Value : "无");
        Console.WriteLine(ans2 != null ? ans2.Value : "无");
    }

    public class Node
    {
        public readonly int Value;
        public Node? Next;

        public Node(int v)
        {
            Value = v;
        }
    }
}