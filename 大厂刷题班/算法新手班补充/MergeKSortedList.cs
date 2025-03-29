//合并K个升序链表

using Common.DataStructures.Heap;

namespace AdvancedTraining.算法新手班补充;

public class MergeKSortedLists
{
    //需要一个节点的比较器
    //return node1.val - node2.val;
    public ListNode? MergeKList(ListNode[] lists)
    {
        if (lists == null) return null;
        Heap<ListNode> minHeap = new((o1, o2) => o1.val - o2.val); //leetcode需要10000的容量才能全过
        for (var i = 0; i < lists.Length; i++)
            if (lists[i] != null)
                minHeap.Push(lists[i]);

        if (minHeap.isEmpty) return null;
        var head = minHeap.Pop();
        var pre = head;
        if (pre.next != null) minHeap.Push(pre.next);
        while (!minHeap.isEmpty)
        {
            var cur = minHeap.Pop();
            pre.next = cur;
            pre = cur;
            if (cur.next != null) minHeap.Push(cur.next);
        }

        return head;
    }

    public class ListNode
    {
        public ListNode? next;
        public int val;
    }
}