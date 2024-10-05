//通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson03;

public class ReverseList
{
    //  head
    //   a    ->   b    ->  c  ->  null
    //   c    ->   b    ->  a  ->  null
    private static Node? ReverseLinkedList(Node? head)
    {
        //将链表分为两个部分:已反转链表和未反转链表
        //next指向未反转链表的头节点
        //pre指向已反转链表的头节点
        var pre = head;
        while (head != null)
        {
            //向右缩短next指向的未反转链表
            var next = head.Next;
            //让原本的下一个节点指向上一个节点
            head.Next = pre;
            //让pre指向已反转链表的头节点
            pre = head;
            //向右扩展已反转区
            head = next;
        }

        return pre; //返回最终已反转链表的头节点
    }

    private static DoubleNode? ReverseDoubleList(DoubleNode? head)
    {
        DoubleNode? pre = null;
        while (head != null)
        {
            var next = head.Next;
            head.Next = pre;
            head.Last = next;
            pre = head;
            head = next;
        }

        return pre;
    }

    private static Node? TestReverseLinkedList(Node? head)
    {
        if (head == null) return null;

        var list = new List<Node>();
        while (head != null)
        {
            list.Add(head);
            head = head.Next;
        }

        list[0].Next = null;
        var length = list.Count;
        for (var i = 1; i < length; i++) list[i].Next = list[i - 1];

        return list[length - 1];
    }

    private static DoubleNode? TestReverseDoubleList(DoubleNode? head)
    {
        if (head == null) return null;

        var list = new List<DoubleNode>();
        while (head != null)
        {
            list.Add(head);
            head = head.Next;
        }

        list[0].Next = null;
        var pre = list[0];
        var length = list.Count;
        for (var i = 1; i < length; i++)
        {
            var cur = list[i];
            cur.Last = null;
            cur.Next = pre;
            pre.Last = cur;
            pre = cur;
        }

        return list[length - 1];
    }

    //生成随机单链表
    private static Node? GenerateRandomLinkedList(int len, int value)
    {
        var size = (int)(Utility.getRandomDouble * (len + 1));
        if (size == 0) return null;

        size--;
        var head = new Node((int)(Utility.getRandomDouble * (value + 1)));
        var pre = head;
        while (size != 0)
        {
            var cur = new Node((int)(Utility.getRandomDouble * (value + 1)));
            pre.Next = cur;
            pre = cur;
            size--;
        }

        return head;
    }

    //生成随机双链表
    private static DoubleNode? GenerateRandomDoubleList(int len, int value)
    {
        var size = (int)(Utility.getRandomDouble * (len + 1));
        if (size == 0) return null;

        size--;
        var head = new DoubleNode((int)(Utility.getRandomDouble * (value + 1)));
        var pre = head;
        while (size != 0)
        {
            var cur = new DoubleNode((int)(Utility.getRandomDouble * (value + 1)));
            pre.Next = cur;
            cur.Last = pre;
            pre = cur;
            size--;
        }

        return head;
    }

    //获取单链表原始顺序
    private static List<int> GetLinkedListOriginOrder(Node? head)
    {
        List<int> ans = new();
        while (head != null)
        {
            ans.Add(head.Value);
            head = head.Next;
        }

        return ans;
    }

    //检查单链表是否反转成功
    private static bool CheckLinkedListReverse(List<int> origin, Node? head)
    {
        for (var i = origin.Count - 1; i >= 0; i--)
            if (head != null)
            {
                if (!origin[i].Equals(head.Value)) return false;

                head = head.Next;
            }

        return true;
    }

    //获取双链表原始顺序
    private static List<int> GetDoubleListOriginOrder(DoubleNode? head)
    {
        List<int> ans = new();
        while (head != null)
        {
            ans.Add(head.Value);
            head = head.Next;
        }

        return ans;
    }

    //检查双链表是否反转成功
    private static bool CheckDoubleListReverse(List<int> origin, DoubleNode? head)
    {
        DoubleNode? end = null;
        for (var i = origin.Count - 1; i >= 0; i--)
            if (head != null)
            {
                if (!origin[i].Equals(head.Value)) return false;

                end = head;
                head = head.Next;
            }

        foreach (var t in origin)
            if (end != null)
            {
                if (!t.Equals(end.Value)) return false;

                end = end.Last;
            }

        return true;
    }

    public static void Run()
    {
        var len = 50;
        var value = 100;
        var testTime = 100000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var node1 = GenerateRandomLinkedList(len, value);
            var list1 = GetLinkedListOriginOrder(node1);
            node1 = ReverseLinkedList(node1);
            if (!CheckLinkedListReverse(list1, node1)) Console.WriteLine("Oops1!");

            var node2 = GenerateRandomLinkedList(len, value);
            var list2 = GetLinkedListOriginOrder(node2);
            node2 = TestReverseLinkedList(node2);
            if (!CheckLinkedListReverse(list2, node2)) Console.WriteLine("Oops2!");

            var node3 = GenerateRandomDoubleList(len, value);
            var list3 = GetDoubleListOriginOrder(node3);
            node3 = ReverseDoubleList(node3);
            if (!CheckDoubleListReverse(list3, node3)) Console.WriteLine("Oops3!");

            var node4 = GenerateRandomDoubleList(len, value);
            var list4 = GetDoubleListOriginOrder(node4);
            node4 = TestReverseDoubleList(node4);
            if (!CheckDoubleListReverse(list4, node4)) Console.WriteLine("Oops4!");
        }

        Console.WriteLine("测试完成");
    }

    private class Node(int data)
    {
        public readonly int Value = data;
        public Node? Next;
    }

    public class DoubleNode(int data)
    {
        public readonly int Value = data;
        public DoubleNode? Last;
        public DoubleNode? Next;
    }
}