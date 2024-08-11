namespace AdvancedTraining.Lesson28;

public class RemoveNthNodeFromEndOfList //Problem_0019
{
    private static ListNode? RemoveNthFromEnd(ListNode? head, int n)
    {
        var cur = head;
        ListNode? pre = null;
        while (cur != null)
        {
            n--;
            if (n == -1) pre = head;
            if (n < -1) pre = pre?.Next;
            cur = cur.Next;
        }

        if (n > 0) return head;
        if (pre == null) return head?.Next;
        pre.Next = pre.Next?.Next;
        return head;
    }

    public static void Run()
    {
        var head = RemoveNthFromEnd(
            new ListNode
            {
                Val = 1,
                Next = new ListNode
                {
                    Val = 2,
                    Next = new ListNode
                    {
                        Val = 3, Next = new ListNode
                        {
                            Val = 4, Next = new ListNode
                            {
                                Val = 5
                            }
                        }
                    }
                }
            }, 2);
        while (head != null)
        {
            Console.WriteLine(head.Val); //输出：[1,2,3,5]
            head = head.Next;
        }
    }

    public class ListNode
    {
        public ListNode? Next;
        public int Val;
    }
}