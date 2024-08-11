#region

using Common.Utilities;

#endregion

namespace Common.DataStructures.LinkedList;

public abstract class SingleLinkedList
{
    private SNode? _innerHead;

    private static SNode? Reverse(SNode? head) //头节点包含数据
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

    public void AddAtHead(int num)
    {
        throw new NotImplementedException("未实现从头部添加节点");
    }

    public void AddAtTail(int num)
    {
        throw new NotImplementedException("未实现从尾部添加节点");
    }

    public SNode? RemoveAt(int index)
    {
        throw new NotImplementedException("未实现删除指定位置的节点");
    }

    public SNode? RemoveValue(int num)
    {
        if (_innerHead == null) return null;
        // head来到第一个不需要删的位置
        while (_innerHead != null)
        {
            if (_innerHead.Value != num) break;
            _innerHead = _innerHead.Next;
        }

        // 1 ) head == null
        // 2 ) head != null
        var pre = _innerHead;
        var cur = _innerHead;
        while (cur != null)
        {
            if (cur.Value == num && pre != null)
                pre.Next = cur.Next;
            else
                pre = cur;
            cur = cur.Next;
        }

        return _innerHead;
    }

    public class SNode(int value)
    {
        public readonly int Value = value;
        public SNode? Next;
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