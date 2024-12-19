//合并K个升序链表
using System.Security.Cryptography.X509Certificates;
using Common.DataStructures.Heap;

namespace AdvancedTraining.算法新手班补充;

public class MergeKSortedLists
{
    public class ListNode
    {
        public int val;
        public ListNode? next;
    }

    //需要一个节点的比较器
    //return node1.val - node2.val;
    public ListNode? MergeKList(ListNode[] lists)
    {
        if (lists == null)
        {
            return null;
        }
        Heap<ListNode> minHeap = new((o1, o2) => o1.val - o2.val);//leetcode需要10000的容量才能全过
        for (int i = 0; i < lists.Length; i++)
        {
            if (lists[i] != null)
            {
                minHeap.Push(lists[i]);
            }
        }
        if (minHeap.isEmpty)
        {
            return null;
        }
        ListNode head = minHeap.Pop();
        ListNode pre = head;
        if (pre.next != null)
        {
            minHeap.Push(pre.next);
        }
        while (!minHeap.isEmpty)
        {
            ListNode cur = minHeap.Pop();
            pre.next = cur;
            pre = cur;
            if (cur.next != null)
            {
                minHeap.Push(cur.next);
            }
        }
        return head;
    }
}