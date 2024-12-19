namespace CustomTraining.Leetcode;

public class AddTwoNumbers
{
    public class ListNode(int val = 0, ListNode? next = null)
    {
        public ListNode? Next = next;
        public int Val = val;
    }

    public static ListNode? AddTwoNumbersCode(ListNode? l1, ListNode? l2)
    {
        //思路：
        //1.先将链表L1和L2中数据组成整数
        //2.将组合好的整数相加
        //3.将结果构造成链表
        var number1 = 0;
        while (l1 != null)
        {
            number1 = number1 * 10 + l1.Val;
            l1 = l1.Next;
        }

        var number2 = 0;
        while (l2 != null)
        {
            number2 = number2 * 10 + l2.Val;
            l2 = l2.Next;
        }

        var sum = number1 + number2;
        if (sum == 0) return new ListNode();

        var head = new ListNode();
        var cur = head;
        while (sum > 0)
        {
            cur.Next = new ListNode
            {
                Val = sum % 10
            };
            cur = cur.Next;
            sum /= 10;
        }

        return head.Next;
    }
}