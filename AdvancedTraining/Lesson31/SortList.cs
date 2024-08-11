#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson31;

//todo:待整理
public class SortList //Problem_0148
{
    // 链表的归并排序
    // 时间复杂度O(N*logN), 因为是链表所以空间复杂度O(1)
    private static ListNode SortList1(ListNode? head)
    {
        var n = 0;
        var cur = head;
        while (cur != null)
        {
            n++;
            cur = cur.Next;
        }

        var h = head;
        var teamFirst = head;
        ListNode? pre = null;
        for (var len = 1; len < n; len <<= 1)
        {
            while (teamFirst != null)
            {
                // 左组从哪到哪 ls le
                // 右组从哪到哪 rs re
                // 左 右 next
                var hthtn = Hthtn(teamFirst, len);
                // ls...le rs...re -> merge去
                // 整体的头、整体的尾
                var mhmt = merge(hthtn[0], hthtn[1], hthtn[2], hthtn[3]);
                if (h == teamFirst)
                {
                    h = mhmt[0];
                    pre = mhmt[1];
                }
                else
                {
                    pre.Next = mhmt[0];
                    pre = mhmt[1];
                }

                teamFirst = hthtn[4];
            }

            teamFirst = h;
            pre = null;
        }

        return h;
    }

    private static ListNode?[] Hthtn(ListNode? teamFirst, int len)
    {
        var ls = teamFirst;
        var le = teamFirst;
        ListNode? rs = null;
        ListNode? re = null;
        ListNode? next = null;
        var pass = 0;
        while (teamFirst != null)
        {
            pass++;
            if (pass <= len) le = teamFirst;
            if (pass == len + 1) rs = teamFirst;
            if (pass > len) re = teamFirst;
            if (pass == len << 1) break;
            teamFirst = teamFirst.Next;
        }

        le.Next = null;
        if (re != null)
        {
            next = re.Next;
            re.Next = null;
        }

        return [ls, le, rs, re, next];
    }

    private static ListNode[] merge(ListNode ls, ListNode le, ListNode? rs, ListNode re)
    {
        if (rs == null) return new[] { ls, le };
        ListNode? head = null;
        ListNode? pre = null;
        ListNode? tail = null;
        while (ls != le.Next && rs != re.Next)
        {
            ListNode? cur;
            if (ls.Val <= rs.Val)
            {
                cur = ls;
                ls = ls.Next;
            }
            else
            {
                cur = rs;
                rs = rs.Next;
            }

            if (pre == null)
            {
                head = cur;
                pre = cur;
            }
            else
            {
                pre.Next = cur;
                pre = cur;
            }
        }

        if (ls != le.Next)
            while (ls != le.Next)
            {
                pre.Next = ls;
                pre = ls;
                tail = ls;
                ls = ls.Next;
            }
        else
            while (rs != re.Next)
            {
                pre.Next = rs;
                pre = rs;
                tail = rs;
                rs = rs.Next;
            }

        return new[] { head, tail };
    }

    // 链表的快速排序
    // 时间复杂度O(N*logN), 空间复杂度O(logN)
    private static ListNode? SortList2(ListNode head)
    {
        var n = 0;
        var cur = head;
        while (cur != null)
        {
            cur = cur.Next;
            n++;
        }

        return Process(head, n).Head;
    }

    private static HeadAndTail Process(ListNode head, int n)
    {
        if (n == 0) return new HeadAndTail(head, head);
        var index = (int)(Utility.GetRandomDouble * n);
        var cur = head;
        while (index-- != 0) cur = cur.Next;
        var r = Partition(head, cur);
        var lht = Process(r.LHead, r.LSize);
        var rht = Process(r.RHead, r.RSize);
        if (lht.Tail != null) lht.Tail.Next = r.MHead;
        r.MTail.Next = rht.Head;
        return new HeadAndTail(lht.Head ?? r.MHead, rht.Tail ?? r.MTail);
    }

    private static Record Partition(ListNode head, ListNode mid)
    {
        ListNode? lh = null;
        ListNode? lt = null;
        var ls = 0;
        ListNode? mh = null;
        ListNode? mt = null;
        ListNode? rh = null;
        ListNode? rt = null;
        var rs = 0;
        while (head != null)
        {
            var tmp = head.Next;
            head.Next = null;
            if (head.Val < mid.Val)
            {
                if (lh == null)
                {
                    lh = head;
                    lt = head;
                }
                else
                {
                    lt.Next = head;
                    lt = head;
                }

                ls++;
            }
            else if (head.Val > mid.Val)
            {
                if (rh == null)
                {
                    rh = head;
                    rt = head;
                }
                else
                {
                    rt.Next = head;
                    rt = head;
                }

                rs++;
            }
            else
            {
                if (mh == null)
                {
                    mh = head;
                    mt = head;
                }
                else
                {
                    mt.Next = head;
                    mt = head;
                }
            }

            head = tmp;
        }

        return new Record(lh, ls, rh, rs, mh, mt);
    }

    public class ListNode(int v)
    {
        internal readonly int Val = v;
        internal ListNode? Next;
    }

    private class HeadAndTail(ListNode? h, ListNode? t)
    {
        public readonly ListNode? Head = h;
        public readonly ListNode? Tail = t;
    }

    private class Record(ListNode? lh, int ls, ListNode? rh, int rs, ListNode? mh, ListNode? mt)
    {
        public readonly ListNode? LHead = lh;
        public readonly int LSize = ls;
        public readonly ListNode? MHead = mh;
        public readonly ListNode? MTail = mt;
        public readonly ListNode? RHead = rh;
        public readonly int RSize = rs;
    }
}