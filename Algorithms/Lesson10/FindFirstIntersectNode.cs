//测试通过

namespace Algorithms.Lesson10;

public class FindFirstIntersectNode
{
    private static Node? GetIntersectNode(Node? head1, Node? head2)
    {
        if (head1 == null || head2 == null) return null;

        var loop1 = GetLoopNode(head1);
        var loop2 = GetLoopNode(head2);
        if (loop1 == null && loop2 == null) return NoLoop(head1, head2);

        if (loop1 != null && loop2 != null) return BothLoop(head1, loop1, head2, loop2);

        return null;
    }

    // 找到链表第一个入环节点，如果无环，返回null
    private static Node? GetLoopNode(Node? head)
    {
        if (head == null || head.Next == null || head.Next.Next == null) return null;

        // n1 慢  n2 快
        var slow = head.Next; // n1 -> slow
        var fast = head.Next.Next; // n2 -> fast
        while (slow != fast)
        {
            if (fast.Next == null || fast.Next.Next == null) return null;

            fast = fast.Next.Next;
            slow = slow?.Next;
        }

        // slow fast  相遇
        fast = head; // n2 -> walk again from head
        while (slow != fast)
        {
            slow = slow?.Next;
            fast = fast?.Next;
        }

        return slow;
    }

    // 如果两个链表都无环，返回第一个相交节点，如果不想交，返回null
    private static Node? NoLoop(Node? head1, Node? head2)
    {
        if (head1 == null || head2 == null) return null;

        var cur1 = head1;
        var cur2 = head2;
        var n = 0;
        while (cur1.Next != null)
        {
            n++;
            cur1 = cur1.Next;
        }

        while (cur2.Next != null)
        {
            n--;
            cur2 = cur2.Next;
        }

        if (cur1 != cur2) return null;

        // n  :  链表1长度减去链表2长度的值
        cur1 = n > 0 ? head1 : head2; // 谁长，谁的头变成cur1
        cur2 = cur1 == head1 ? head2 : head1; // 谁短，谁的头变成cur2
        n = Math.Abs(n);
        while (n != 0)
        {
            n--;
            cur1 = cur1?.Next;
        }

        while (cur1 != cur2)
        {
            cur1 = cur1?.Next;
            cur2 = cur2?.Next;
        }

        return cur1;
    }

    // 两个有环链表，返回第一个相交节点，如果不想交返回null
    private static Node? BothLoop(Node head1, Node loop1, Node head2, Node loop2)
    {
        Node? cur1;
        if (loop1 == loop2)
        {
            cur1 = head1;
            var cur2 = head2;
            var n = 0;
            while (cur1 != loop1)
            {
                n++;
                cur1 = cur1?.Next;
            }

            while (cur2 != loop2)
            {
                n--;
                cur2 = cur2?.Next;
            }

            cur1 = n > 0 ? head1 : head2;
            cur2 = cur1 == head1 ? head2 : head1;
            n = Math.Abs(n);
            while (n != 0)
            {
                n--;
                cur1 = cur1?.Next;
            }

            while (cur1 != cur2)
            {
                cur1 = cur1?.Next;
                cur2 = cur2?.Next;
            }

            return cur1;
        }

        cur1 = loop1.Next;
        while (cur1 != loop1)
        {
            if (cur1 == loop2) return loop1;

            cur1 = cur1?.Next;
        }

        return null;
    }

    public static void Run()
    {
        // 1->2->3->4->5->6->7->null
        var head1 = new Node(1)
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
                            }
                        }
                    }
                }
            }
        };

        // 0->9->8->6->7->null
        var head2 = new Node(0)
        {
            Next = new Node(9)
            {
                Next = new Node(8)
                {
                    Next = head1.Next.Next.Next.Next.Next // 8->6
                }
            }
        };
        Console.WriteLine(GetIntersectNode(head1, head2)?.Value);

        // 1->2->3->4->5->6->7->4...
        head1 = new Node(1)
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
                            }
                        }
                    }
                }
            }
        };
        head1.Next.Next.Next.Next.Next!.Next = head1.Next.Next.Next; // 7->4

        // 0->9->8->2...
        head2 = new Node(0)
        {
            Next = new Node(9)
            {
                Next = new Node(8)
                {
                    Next = head1.Next // 8->2
                }
            }
        };
        Console.WriteLine(GetIntersectNode(head1, head2)?.Value);

        // 0->9->8->6->4->5->6..
        head2 = new Node(0)
        {
            Next = new Node(9)
            {
                Next = new Node(8)
                {
                    Next = head1.Next.Next.Next.Next.Next // 8->6
                }
            }
        };
        Console.WriteLine(GetIntersectNode(head1, head2)?.Value);
    }

    public class Node
    {
        public readonly int Value;
        public Node? Next;

        public Node(int data)
        {
            Value = data;
        }
    }
}