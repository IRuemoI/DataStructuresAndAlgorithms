//测试通过

#region

using Common.Utilities;

#endregion

namespace Common.DataStructures.LinkedList;

public class DNode(int value)
{
    public readonly int Value = value;
    public DNode? Next;
    public DNode? Pre;
}

public class DoubleLinkedList : ICloneable
{
    private DNode? _head;
    private DNode? _tail;

    private bool IsEmpty => _head == null && _tail == null;

    private static DNode? Reverse(DNode? head)
    {
        //将链表分为两个部分：已反转链表和未反转链表
        //next指向未反转链表的头节点
        //pre指向已反转链表的头节点
        var pre = head;
        while (head != null)
        {
            //缩短next指向的未反转链表
            var next = head.Next;
            //让原本的下一个节点指向上一个节点
            head.Next = pre;
            //让原本的上一个节点指向下一个节点
            head.Pre = next;
            //让pre指向已反转链表的头节点
            pre = head;
            //向右扩展已反转区
            head = next;
        }

        return pre; //返回最终已反转链表的头节点
    }

    private void AddAtHead(int value)
    {
        //创建新节点
        var newNode = new DNode(value);
        //如果链表为空
        if (_head == null)
        {
            //将新节点同时作为头节点和尾节点
            _head = _tail = newNode;
        }
        else
        {
            //将新节点的下一个节点指向当前的头节点
            newNode.Next = _head;
            //将当前头节点的前一个节点指向新节点
            _head.Pre = newNode;
            //将头节点指向新节点
            _head = newNode;
        }
    }

    private void AddAtTail(int value)
    {
        //创建新节点
        var newNode = new DNode(value);
        //如果链表为空
        if (_tail == null)
        {
            //将新节点同时作为头节点和尾节点
            _head = _tail = newNode;
        }
        else
        {
            //将当前尾节点的下一个节点指向新节点
            _tail.Next = newNode;
            //将新节点的前一个节点指向当前尾节点
            newNode.Pre = _tail;
            //将尾节点指向新节点
            _tail = newNode;
        }
    }

    private DNode? RemoveAt(int index)
    {
        if (_head == null) return null; //空链表，无节点可移除

        var current = _head;
        var currentIndex = 0;

        while (current != null && currentIndex < index)
        {
            current = current.Next;
            currentIndex++;
        }

        if (current == null) return null; // Index out of bounds

        if (current.Pre != null)
            current.Pre.Next = current.Next;
        else
            _head = current.Next; // Removing the head

        if (current.Next != null)
            current.Next.Pre = current.Pre;
        else
            _tail = current.Pre; // Removing the tail

        current.Next = null;
        current.Pre = null;
        return current;
    }

    private DNode? RemoveValue(int num)
    {
        if (_head == null) return null; // 空链表，无节点可移除
        // head来到第一个不需要删的位置
        while (_head != null)
        {
            if (_head.Value != num) break;
            _head = _head.Next;
        }

        var pre = _head; //pre表示需要删除节点的上一个节点
        var cur = _head; //cur表示当前节点
        while (cur != null) //直到遍历完成链表
        {
            //如果当前节点的值等于目标值num，并且pre不为空
            if (cur.Value == num && pre != null)
            {
                //则将pre的next指向cur的next(cur节点内存之后被回收)
                pre.Next = cur.Next;
            }
            else
            {
                //否则将pre向后移动至cur
                pre = cur;
            }

            cur = cur.Next;
        }

        //将cur向后移动
        return _head;
    }

    private void Print()
    {
        var tempHead = _head;
        while (tempHead != null)
        {
            Console.Write(tempHead.Value + ",");
            tempHead = tempHead.Next;
        }

        Console.WriteLine();
    }

    public object Clone()
    {
        // 创建一个新的DoubleLinkedList实例
        var clonedList = new DoubleLinkedList();

        // 如果原始链表为空，直接返回新的空链表
        if (_head == null)
            return clonedList;

        // 从头节点开始复制
        var currentOriginalNode = _head;
        var newHead = new DNode(currentOriginalNode.Value);
        var currentNewNode = newHead;
        clonedList._head = newHead;

        // 遍历原始链表，并复制每个节点
        while (currentOriginalNode.Next != null)
        {
            currentOriginalNode = currentOriginalNode.Next;
            currentNewNode.Next = new DNode(currentOriginalNode.Value)
            {
                Pre = currentNewNode
            };
            currentNewNode = currentNewNode.Next;
        }

        // 设置尾节点
        clonedList._tail = currentNewNode;

        return clonedList;
    }

    # region 用于测试

    private static DNode? TestReverse(DNode? head) //头节点包含数据
    {
        if (head == null) return null;
        var list = new List<DNode>();
        while (head != null)
        {
            list.Add(head);
            head = head.Next;
        }

        list[0].Next = null;
        var pre = list[0];
        var listLength = list.Count;
        for (var i = 1; i < listLength; i++)
        {
            var cur = list[i];
            cur.Pre = null;
            cur.Next = pre;
            pre.Pre = cur;
            pre = cur;
        }

        return list[listLength - 1];
    }

    private static DNode? GenerateRandomList(int len, int value)
    {
        var size = (int)(Utility.getRandomDouble * (len + 1));
        if (size == 0) return null;
        size--;
        var head = new DNode((int)(Utility.getRandomDouble * (value + 1)));
        var pre = head;
        while (size != 0)
        {
            var cur = new DNode((int)(Utility.getRandomDouble * (value + 1)));
            pre.Next = cur;
            cur.Pre = pre;
            pre = cur;
            size--;
        }

        return head;
    }

    private static List<int> GetDoubleLinkedListOriginOrder(DNode? head)
    {
        var ans = new List<int>();
        while (head != null)
        {
            ans.Add(head.Value);
            head = head.Next;
        }

        return ans;
    }


    private static bool CheckReverse(List<int>? origin, DNode? head)
    {
        DNode? end = null;
        if (origin != null)
        {
            for (var i = origin.Count - 1; i >= 0; i--)
                if (head != null)
                {
                    if (!origin[i].Equals(head.Value)) return false;
                    end = head;
                    head = head.Next;
                }

            for (var i = 0; i < origin.Count(); i++)
                if (end != null)
                {
                    if (!origin[i].Equals(end.Value)) return false;
                    end = end.Pre;
                }
        }

        return true;
    }

    public static void Run()
    {
        //双链表测试
        var doubleLinkedList = new DoubleLinkedList();
        Console.WriteLine(doubleLinkedList.IsEmpty);
        doubleLinkedList.AddAtHead(0);
        doubleLinkedList.AddAtHead(1);
        doubleLinkedList.AddAtHead(2);
        doubleLinkedList.AddAtTail(3);
        doubleLinkedList.AddAtTail(4);
        Console.WriteLine(doubleLinkedList.IsEmpty);
        var copy = (DoubleLinkedList)doubleLinkedList.Clone();
        doubleLinkedList.Print();
        Console.WriteLine(doubleLinkedList.RemoveAt(3)?.Value);
        Console.WriteLine(doubleLinkedList.RemoveValue(0)?.Value);
        doubleLinkedList.Print();
        copy.Print();

        var len = 50;
        var value = 100;
        var testTime = 100000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var node3 = GenerateRandomList(len, value);
            var list3 = GetDoubleLinkedListOriginOrder(node3);
            node3 = Reverse(node3);
            if (!CheckReverse(list3, node3)) Console.WriteLine("Oops3!");

            var node4 = GenerateRandomList(len, value);
            var list4 = GetDoubleLinkedListOriginOrder(node4);
            node4 = TestReverse(node4);
            if (!CheckReverse(list4, node4)) Console.WriteLine("Oops4!");
        }

        Console.WriteLine("测试完成");
    }

    #endregion
}