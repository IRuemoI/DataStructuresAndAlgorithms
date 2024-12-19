//测试通过

namespace Algorithms.Lesson10;

public static class FindFirstIntersectNode
{
    private static Node? GetIntersectNode1(Node? head1, Node? head2)
    {
        if (head1 == null || head2 == null) return null;

        //获取两个链表的入环节点
        var loop1 = GetLoopNode(head1);
        var loop2 = GetLoopNode(head2);
        //如果两个链表的入伙节点一致，按照两链表相交并无环的请情况处理
        if (loop1 == null && loop2 == null) return NoLoop(head1, head2);

        //如果两个链表都有入环节点，按照两链表相交有环的请情况处理
        if (loop1 != null && loop2 != null) return BothLoop(head1, loop1, head2, loop2);

        //如果两个链表一个有入环节点，一个没有入环节点，则两链表不相交
        return null;
    }

    // 找到链表的入环节点，如果无环，返回null。关于这个函数的原理，笔记EP09中有说明
    private static Node? GetLoopNode(Node? head)
    {
        if (head?.Next?.Next == null) return null;
        
        var slow = head.Next;
        var fast = head.Next.Next;
        while (slow != fast)
        {
            if (fast.Next?.Next == null) return null;

            fast = fast.Next.Next;
            slow = slow?.Next;
        }
        
        fast = head;
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
        var lengthGap = 0;
        while (cur1.Next != null)
        {
            lengthGap++;
            cur1 = cur1.Next;
        }

        while (cur2.Next != null)
        {
            lengthGap--;
            cur2 = cur2.Next;
        }

        if (cur1 != cur2) return null;
        
        cur1 = lengthGap > 0 ? head1 : head2; // 谁长，谁的头变成cur1
        cur2 = cur1 == head1 ? head2 : head1; // 谁短，谁的头变成cur2
        lengthGap = Math.Abs(lengthGap);
        //让长链表的current指针先走lengthGap步
        while (lengthGap != 0)
        {
            lengthGap--;
            cur1 = cur1?.Next;
        }

        //两个链表current指针同时走，如果相交，则返回第一个相交节点
        while (cur1 != cur2)
        {
            cur1 = cur1?.Next;
            cur2 = cur2?.Next;
        }

        //如果不想交，cur1为null
        return cur1;
    }

    // 两个有环链表，返回第一个相交节点，如果不想交返回null
    private static Node? BothLoop(Node head1, Node loop1, Node head2, Node loop2)
    {
        Node? cur1;
        //如果两个入环节点一致，说明此时两链表呈>-O，按照>-处理，只不过他的末尾节点变成了loop1或者loop2
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

        //如果两个入环节点能向后移动遇到另个入环节点，说明此时两链表呈😈(圆上两个触角)
        cur1 = loop1.Next;
        while (cur1 != loop1)
        {
            if (cur1 == loop2) return loop1;

            cur1 = cur1?.Next;
        }

        //两个链表有环但不相交
        return null;
    }
    
    private static Node? GetIntersectNode2(Node? head1, Node? head2)
    {
        ISet<Node> visited = new HashSet<Node>();//记录链表A中访问过的节点
        //将访问过的链表放入集合中
        var temp = head1;
        while (temp != null)
        {
            //如果添加了重复的节点说明已经将环上的节点添加完毕，退出循环
            if(!visited.Add(temp)) break;
            temp = temp.Next;
        }

        //遍历链表B，如果B中存在A中访问过的节点，这个节点就是两链表相交时的节点
        temp = head2;
        while (temp != null)
        {
            if (visited.Contains(temp))
            {
                return temp;
            }

            temp = temp.Next;
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
        Console.WriteLine("算法1:" + GetIntersectNode1(head1, head2)?.Value);
        Console.WriteLine("算法2:" + GetIntersectNode2(head1, head2)?.Value);//输出6

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
        Console.WriteLine("算法1:" + GetIntersectNode1(head1, head2)?.Value);
        Console.WriteLine("算法2:" + GetIntersectNode2(head1, head2)?.Value);//输出2

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
        Console.WriteLine("算法1:" + GetIntersectNode1(head1, head2)?.Value);
        Console.WriteLine("算法2:" + GetIntersectNode2(head1, head2)?.Value);//输出4或者6
    }

    private class Node(int data)
    {
        public readonly int Value = data;
        public Node? Next;
    }
}