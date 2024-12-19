namespace AdvancedTraining.算法新手班补充;

public class AddTwoNumber
{
    public class ListNode(int val = 0, ListNode? next = null)
    {
        public int Val = val;
        public ListNode? Next = next;
    }

    public ListNode? AddTwoNumbers(ListNode? l1, ListNode? l2)
    {
        if (l1 == null) return l2;
        if (l2 == null) return l1;

        var length1 = GetListLength(l1);
        var length2 = GetListLength(l2);
        var longer = length1 >= length2 ? l1 : l2;
        var shorter = longer == l1 ? l2 : l1;
        var curLonger = longer;
        var curShorter = shorter;
        var last = curLonger;
        var carry = 0; //进位
        int curNum;

        while (curShorter != null)
        {
            if (curLonger != null)
            {
                curNum = curLonger.Val + curShorter.Val + carry;
                curLonger.Val = curNum % 10;
                carry = curNum / 10;
                last = curLonger;
                curLonger = curLonger.Next;
            }

            curShorter = curShorter.Next;
        }

        while (curLonger != null)
        {
            curNum = curLonger.Val + carry;
            curLonger.Val = curNum % 10;
            carry = curNum / 10;
            last = curLonger;
            curLonger = curLonger.Next;
        }

        if (carry != 0) last.Next = new ListNode(1);

        return longer;
    }

    private static int GetListLength(ListNode? head)
    {
        var length = 0;
        var temp = head;

        while (temp != null)
        {
            length++;
            temp = temp.Next;
        }

        return length;
    }
}