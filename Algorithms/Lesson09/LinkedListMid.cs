//测试通过

namespace Algorithms.Lesson09;

public class LinkedListMid
{
    //返回链表中点或者上中点
    private static Node? MidOrUpMidNode(Node? head)
    {
        if (head?.Next?.Next == null) return head; //如果这个链表的长度小于3那么返回头节点

        var slow = head.Next; //定义慢指针并向后移动一次
        var fast = head.Next.Next; //定义快指针并向后移动一次
        while (fast.Next is { Next: not null })
        {
            //下一个节点和下两个节点不为空
            slow = slow?.Next; //慢指针向后移动
            fast = fast.Next.Next; //快指针向后移动
        }

        return slow;
    }

    //返回链表中点或者下中点
    private static Node? MidOrDownMidNode(Node? head)
    {
        if (head?.Next == null) return head; //如果这个链表的长度小于2那么返回头节点

        //要找中点的下一个节点需要定义时向后移一个节点
        var slow = head.Next; //定义慢指针
        var fast = head.Next; //定义快指针
        while (fast.Next is { Next: not null })
        {
            //下一个节点和下两个节点不为空
            slow = slow?.Next; //慢指针向后移动
            fast = fast.Next.Next; //快指针向后移动
        }

        return slow;
    }

    //返回链表中点或者上中点的上一个节点
    private static Node? MidOrUpMidPreNode(Node? head)
    {
        if (head?.Next?.Next == null) return null; //如果这个链表的长度小于3那么返回头节点

        //要找中点的上一个节点需要快定义时块指针先走一步
        var slow = head; //定义慢指针
        var fast = head.Next.Next; //定义快指针
        while (fast.Next is { Next: not null })
        {
            //下一个节点和下两个节点不为空
            slow = slow?.Next; //慢指针向后移动
            fast = fast.Next.Next; //块指针向后移动
        }

        return slow;
    }

    //返回链表中点或者下中点的上一个节点
    private static Node? MidOrDownMidPreNode(Node? head)
    {
        if (head?.Next == null) return null; //如果这个链表的长度小于2那么返回空

        if (head.Next.Next == null) return head; //如果这个链表的长度小于3那么返回头节点

        //要找中点的下一个节点需要快定义时快指针少走一个节点
        var slow = head; //定义慢指针
        var fast = head.Next; //定义快指针
        while (fast.Next is { Next: not null })
        {
            //下一个节点和下两个节点不为空
            slow = slow?.Next; //慢指针向后移动
            fast = fast.Next.Next; //块指针向后移动
        }

        return slow;
    }

    #region 用于测试

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

    #endregion
    
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

    private class Node(int v)
    {
        public readonly int Value = v;
        public Node? Next;
    }
}