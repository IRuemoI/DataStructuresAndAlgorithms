//通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson03;

public class ReverseList
{
    private static Node? ReverseLinkedList(Node? head)
    {
        //将链表分为两个部分:已反转链表和未反转链表
        //next指向未反转链表的头节点
        //current指向正在移动的节点
        //pre指向已反转链表的头节点
        Node? pre = null;
        var current = head;
        while (current != null)
        {
            //向右缩短next指向的未反转链表
            var next = current.Next;
            //让原本的下一个节点指向上一个节点
            current.Next = pre;
            //让pre指向已反转链表的头节点
            pre = current;
            //向右扩展已反转区
            current = next;
        }

        return pre; //返回最终已反转链表的头节点
    }

    private static DoubleNode? ReverseDoubleList(DoubleNode? head)
    {
        //与反转单链表相同，只需要处理prev指针即可
        DoubleNode? pre = null;
        var current = head;
        while (current != null)
        {
            // 保存下一个节点
            var next = current.Next;

            // 反转当前节点的Next指针
            current.Next = pre;

            // 反转当前节点的Prev指针
            current.Prev = next;

            // pre移动到当前节点
            pre = current;

            // current移动到下一个节点
            current = next;
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
            cur.Prev = null;
            cur.Next = pre;
            pre.Prev = cur;
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
            cur.Prev = pre;
            pre = cur;
            size--;
        }

        return head;
    }

    //获取单链表原始顺序
    private static List<int> GetLinkedListOriginOrder(Node? head)
    {
        List<int> ans = [];
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
        // 检测链表是否有环
        var validLength = origin.Count;
        var temp = head;
        while (temp != null)
        {
            temp = temp.Next;
            validLength--;
            if (validLength < 0)
            {
                Console.WriteLine("链表理论上应该遍历完成，但是某个节点上出现了环");
                return false;
            }
        }

        // 检查链表是否是列表的逆序
        for (var i = origin.Count - 1; i >= 0; i--)
        {
            if (head == null)
            {
                Console.WriteLine("链表比列表短");
                return false;
            }

            if (!origin[i].Equals(head.Value))
            {
                return false;
            }

            head = head.Next;
        }

        // 确保链表没有比列表长
        if (head != null)
        {
            Console.WriteLine("链表比列表长");
            return false;
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
        // 如果原始列表为空，双链表头也应该是空的
        if (origin.Count == 0)
        {
            return head == null;
        }

        // 从双链表头开始遍历
        DoubleNode? current = head;
        int index = origin.Count - 1; // 从原始列表的最后一个元素开始比较

        while (current != null)
        {
            // 检查当前节点的值是否与原始列表中对应位置的值相等
            if (current.Value != origin[index])
            {
                return false;
            }

            // 检查前驱指针是否正确
            if (current.Prev != null && current.Prev.Next != current)
            {
                return false;
            }

            // 检查后继指针是否正确
            if (current.Next != null && current.Next.Prev != current)
            {
                return false;
            }

            // 移动到下一个节点，并更新索引
            current = current.Next;
            index--;
        }

        // 如果所有节点都检查完毕，且索引回到-1，则表示双链表反转成功
        return index == -1;
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
        public DoubleNode? Prev;
        public DoubleNode? Next;
    }
}