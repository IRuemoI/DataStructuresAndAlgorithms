//测试通过

#region

using Common.Utilities;

#endregion

namespace Common.DataStructures.LinkedList;

public class DoubleLinkedList
{
    private DNode? _innerHead;

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

    public void AddAtHead(int num)
    {
        throw new NotImplementedException("未实现从头部添加节点");
    }

    public void AddAtTail(int num)
    {
        throw new NotImplementedException("未实现从尾部添加节点");
    }

    public DNode? RemoveAt(int index)
    {
        throw new NotImplementedException("未实现删除指定位置的节点");
    }

    public DNode? RemoveValue(int num) //头节点包含数据
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
            if (cur.Value == num)
            {
                if (pre != null) pre.Next = cur.Next;
            }
            else
            {
                pre = cur;
            }

            cur = cur.Next;
        }

        return _innerHead;
    }

    public class DNode(int value)
    {
        public readonly int Value = value;
        public DNode? Next;
        public DNode? Pre;
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
        var size = (int)(Utility.GetRandomDouble * (len + 1));
        if (size == 0) return null;
        size--;
        var head = new DNode((int)(Utility.GetRandomDouble * (value + 1)));
        var pre = head;
        while (size != 0)
        {
            var cur = new DNode((int)(Utility.GetRandomDouble * (value + 1)));
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