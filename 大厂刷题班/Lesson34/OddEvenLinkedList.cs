namespace AdvancedTraining.Lesson34;
//pass
//https://leetcode.cn/problems/odd-even-linked-list/description/
public class OddEvenLinkedList //leetcode_0328
{
    private static ListNode? OddEvenList(ListNode? head)
    {
        ListNode? firstOdd = null;
        ListNode? firstEven = null;
        ListNode? odd = null;
        ListNode? even = null;
        var count = 1;
        while (head != null)
        {
            var next = head.Next;
            head.Next = null;
            if ((count & 1) == 1)
            {
                firstOdd ??= head;
                if (odd != null) odd.Next = head;
                odd = head;
            }
            else
            {
                firstEven ??= head;
                if (even != null) even.Next = head;
                even = head;
            }

            count++;
            head = next;
        }

        if (odd != null) odd.Next = firstEven;
        return firstOdd ?? firstEven;
    }

    public static void Run()
    {
        var head = new ListNode
        {
            Val = 1,
            Next = new ListNode
            {
                Val = 2,
                Next = new ListNode
                {
                    Val = 3,
                    Next = new ListNode
                    {
                        Val = 4, Next = new ListNode
                        {
                            Val = 5
                        }
                    }
                }
            }
        };
        var result = OddEvenList(head);
        while (result != null)
        {
            Console.Write(result.Val + ", "); //输出: [1,3,5,2,4]
            result = head.Next;
        }
    }

    // 提交时不要提交这个类
    public class ListNode
    {
        internal ListNode? Next;
        internal int Val;
    }
}