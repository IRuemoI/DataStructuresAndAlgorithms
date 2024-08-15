#region

using Common.Utilities;

#endregion

namespace Common.DataStructures.LinkedList;

public class SNode(int value)
{
    public readonly int Value = value;
    public SNode? Next;
}

public class SingleLinkedList : ICloneable
{
    private SNode? _head;

    private bool IsEmpty => _head == null;

    private static SNode? Reverse(SNode? head)
    {
        //将链表分为两个部分:已反转链表和未反转链表
        //next指向未反转链表的头节点
        //pre指向已反转链表的头节点
        var pre = head;
        while (head != null)
        {
            //缩短next指向的未反转链表
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

    private void AddAtHead(int value)
    {
        //创建一个新的节点，将它的下一个节点设置为原来头节点
        var newNode = new SNode(value) { Next = _head };
        //将新的节点设置为头节点
        _head = newNode;
    }

    private void AddAtTail(int value)
    {
        //创建一个新的节点
        var newNode = new SNode(value);
        //如果链表为空，将新节点设置为头节点
        if (_head == null)
        {
            _head = newNode;
            return;
        }

        //否则，找到链表的最后一个节点，并将新节点设置为最后一个节点的下一个节点
        var current = _head;
        while (current.Next != null) current = current.Next;

        current.Next = newNode;
    }

    private SNode? RemoveAt(int index)
    {
        if (_head == null) return null; // 空链表，无节点可移除
        //如果要删除头节点
        if (index == 0)
        {
            //将头节点记录为被删除的节点
            var deletedNode = _head;
            //将头节点指向下一个节点
            _head = _head.Next;
            //将删除节点的Next断开
            deletedNode.Next = null;
            //返回被删除的节点
            return deletedNode;
        }


        var current = _head; //获取当前节点
        SNode? previous = null; //定义上一个节点
        var currentIndex = 0; //定义当前索引

        //如果当前节点不为空且当前索引还未到达目标索引
        while (current != null && currentIndex < index)
        {
            previous = current; //将当前节点设置为上一个节点
            current = current.Next; //当前节点向后移动
            currentIndex++; //当前索引加1
        }

        if (current == null) return null; // 索引超出链表长度，无节点可移除

        previous!.Next = current.Next; //则将pre的next指向cur的next(cur节点内存之后被回收)
        current.Next = null; //将删除节点的Next断开
        return current; //返回被删除的节点
    }

    private SNode? RemoveValue(int num)
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

            //将cur向后移动
            cur = cur.Next;
        }

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
        // 创建一个新的SingleLinkedList实例
        var clonedList = new SingleLinkedList();

        // 如果原始链表为空，则直接返回新链表
        if (_head == null)
        {
            return clonedList;
        }

        // 复制头节点
        clonedList._head = new SNode(_head.Value);

        // 用于遍历原始链表的当前节点和克隆链表的当前节点
        var currentOriginal = _head.Next;
        var currentCloned = clonedList._head;

        // 遍历原始链表并复制每个节点
        while (currentOriginal != null)
        {
            var newNode = new SNode(currentOriginal.Value);
            currentCloned.Next = newNode;
            currentCloned = newNode;
            currentOriginal = currentOriginal.Next;
        }

        return clonedList;
    }

    #region 用于测试

    private static SNode? TestReverse(SNode? head) //头节点包含数据
    {
        if (head == null) return null;
        var list = new List<SNode>();
        while (head != null)
        {
            list.Add(head);
            head = head.Next;
        }

        list[0].Next = null;
        var listLength = list.Count;
        for (var i = 1; i < listLength; i++) list[i].Next = list[i - 1];
        return list[listLength - 1];
    }

    private static SNode? GenerateRandomList(int len, int value)
    {
        var size = (int)(Utility.GetRandomDouble * (len + 1));
        if (size == 0) return null;
        size--;
        var head = new SNode((int)(Utility.GetRandomDouble * (value + 1)));
        var pre = head;
        while (size != 0)
        {
            var cur = new SNode((int)(Utility.GetRandomDouble * (value + 1)));
            pre.Next = cur;
            pre = cur;
            size--;
        }

        return head;
    }

    private static List<int> GetSingleLinkedListOriginOrder(SNode? head)
    {
        var ans = new List<int>();
        while (head != null)
        {
            ans.Add(head.Value);
            head = head.Next;
        }

        return ans;
    }

    private static bool CheckReverse(List<int>? origin, SNode? head)
    {
        if (origin != null)
            for (var i = origin.Count - 1; i >= 0; i--)
                if (head != null)
                {
                    if (!origin[i].Equals(head.Value)) return false;
                    head = head.Next;
                }

        return true;
    }

    public static void Run()
    {
        //单链表测试
        var singleLinkedList = new SingleLinkedList();
        Console.WriteLine(singleLinkedList.IsEmpty);
        singleLinkedList.AddAtHead(0);
        singleLinkedList.AddAtHead(1);
        singleLinkedList.AddAtHead(2);
        singleLinkedList.AddAtTail(3);
        singleLinkedList.AddAtTail(4);
        Console.WriteLine(singleLinkedList.IsEmpty);
        var copy = (SingleLinkedList)singleLinkedList.Clone();
        singleLinkedList.Print();
        Console.WriteLine(singleLinkedList.RemoveAt(3)?.Value);
        Console.WriteLine(singleLinkedList.RemoveValue(0)?.Value);
        singleLinkedList.Print();
        copy.Print();

        var len = 50;
        var value = 100;
        var testTime = 100000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var node1 = GenerateRandomList(len, value);
            var list1 = GetSingleLinkedListOriginOrder(node1);
            node1 = Reverse(node1);
            if (!CheckReverse(list1, node1)) Console.WriteLine("Oops1!");

            var node2 = GenerateRandomList(len, value);
            var list2 = GetSingleLinkedListOriginOrder(node2);
            node2 = TestReverse(node2);
            if (!CheckReverse(list2, node2)) Console.WriteLine("Oops2!");
        }

        Console.WriteLine("测试结束");
    }

    #endregion
}