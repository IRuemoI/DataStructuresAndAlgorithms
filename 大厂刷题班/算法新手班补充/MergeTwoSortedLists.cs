namespace AdvancedTraining.算法新手班补充;

public class MergeTwoLists
{
    public class ListNode
    {
        public int val;
        public ListNode? next;
    }

    public ListNode? MergeTwoListsCode(ListNode? list1, ListNode? list2)
    {
        if (list1 == null || list2 == null)
        {
            return list1 == null ? list2 : list1;
        }

        ListNode head = list1.val <= list2.val ? list1 : list2;//先拿到开头数字小的，在后面插入大数字的节点
        ListNode? cur1 = head.next;//指向小头的下一个节点
        ListNode? cur2 = head == list1 ? list2 : list1;//指向大头
        ListNode pre = head;//保存上一个位置

        while (cur1 != null && cur2 != null)
        {
            if (cur1.val <= cur2.val)
            {//将下一个较小的节点放到pre的下一个，并向后移动已赋值的指针
                pre.next = cur1;
                cur1 = cur1.next;
            }
            else
            {
                pre.next = cur2;
                cur2 = cur2.next;
            }
            pre = pre.next;
        }
        pre.next = cur1 ?? cur2;//将剩余的链表连接到pre的后续节点
        return head;//返回头节点
    }
}