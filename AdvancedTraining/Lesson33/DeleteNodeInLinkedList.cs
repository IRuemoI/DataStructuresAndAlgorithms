namespace AdvancedTraining.Lesson33;

//https://leetcode.com/problems/delete-node-in-a-linked-list/
//todo:待整理
public class DeleteNodeInLinkedList //Problem_0237
{
    public virtual void DeleteNode(ListNode node)
    {
        node.Val = node.Next.Val;
        node.Next = node.Next.Next;
    }

    public class ListNode(int x, ListNode next)
    {
        public ListNode Next = next;
        public int Val = x;
    }
}