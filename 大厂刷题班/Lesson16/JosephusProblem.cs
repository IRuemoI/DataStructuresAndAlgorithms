//pass

namespace AdvancedTraining.Lesson16;

// 本题测试链接 : https://leetcode-cn.com/problems/yuan-quan-zhong-zui-hou-sheng-xia-de-shu-zi-lcof/
public class JosephusProblem
{
    // 提交直接通过
    // 给定的编号是0~n-1的情况下，数到m就杀
    // 返回谁会活？
    public virtual int LastRemaining1(int n, int m)
    {
        return GetLive(n, m) - 1;
    }

    // 课上题目的设定是，给定的编号是1~n的情况下，数到m就杀
    // 返回谁会活？
    private static int GetLive(int n, int m)
    {
        if (n == 1) return 1;
        return (GetLive(n - 1, m) + m - 1) % n + 1;
    }

    // 提交直接通过
    // 给定的编号是0~n-1的情况下，数到m就杀
    // 返回谁会活？
    // 这个版本是迭代版
    public virtual int LastRemaining2(int n, int m)
    {
        var ans = 1;
        var r = 1;
        while (r <= n) ans = (ans + m - 1) % r++ + 1;
        return ans - 1;
    }

    private static Node? JosephusKill1(Node? head, int m)
    {
        if (head == null || head.Next == head || m < 1) return head;
        var last = head;
        while (last?.Next != head) last = last?.Next;
        var count = 0;
        while (head != last)
        {
            if (++count == m)
            {
                last!.Next = head?.Next;
                count = 0;
            }
            else
            {
                last = last?.Next;
            }

            head = last?.Next;
        }

        return head;
    }

    private static Node? JosephusKill2(Node? head, int m)
    {
        if (head == null || head.Next == head || m < 1) return head;
        var cur = head.Next;
        var size = 1; // tmp -> list size
        while (cur != head)
        {
            size++;
            cur = cur?.Next;
        }

        var live = GetLive(size, m); // tmp -> service node position
        while (--live != 0) head = head?.Next;
        head!.Next = head;
        return head;
    }

    private static void PrintCircularList(Node? head)
    {
        if (head == null) return;
        Console.Write("Circular List: " + head.Value + " ");
        var cur = head.Next;
        while (cur != head)
        {
            Console.Write(cur?.Value + " ");
            cur = cur?.Next;
        }

        Console.WriteLine("-> " + head.Value);
    }

    public static void Run()
    {
        var head1 = new Node(1);
        head1.Next = new Node(2)
        {
            Next = new Node(3)
            {
                Next = new Node(4)
                {
                    Next = new Node(5)
                    {
                        Next = head1
                    }
                }
            }
        };
        PrintCircularList(head1);
        head1 = JosephusKill1(head1, 3);
        PrintCircularList(head1);

        var head2 = new Node(1);
        head2.Next = new Node(2)
        {
            Next = new Node(3)
            {
                Next = new Node(4)
                {
                    Next = new Node(5)
                    {
                        Next = head2
                    }
                }
            }
        };
        PrintCircularList(head2);
        head2 = JosephusKill2(head2, 3);
        PrintCircularList(head2);
    }

    // 以下的code针对单链表，不要提交
    private class Node(int data)
    {
        public readonly int Value = data;
        public Node? Next;
    }
}