//测试通过

namespace Algorithms.Lesson09;

public class SmallerEqualBigger
{
    private static Node? ListPartition1(Node? head, int pivot)
    {
        if (head == null) return head;

        var cur = head;
        var i = 0;
        while (cur != null)
        {
            i++;
            cur = cur.Next;
        }

        var nodeArr = new Node?[i];
        //i = 0;
        cur = head;
        for (i = 0; i != nodeArr.Length; i++)
        {
            nodeArr[i] = cur;
            cur = cur?.Next;
        }

        ArrPartition(nodeArr, pivot);
        for (i = 1; i != nodeArr.Length; i++) nodeArr[i - 1]!.Next = nodeArr[i];

        nodeArr[i - 1]!.Next = null;
        return nodeArr[0];
    }

    private static void ArrPartition(Node?[] nodeArr, int pivot)
    {
        var small = -1;
        var big = nodeArr.Length;
        var index = 0;
        while (index != big)
            if (nodeArr[index]?.Value < pivot)
                Swap(nodeArr, ++small, index++);
            else if (nodeArr[index]?.Value == pivot)
                index++;
            else
                Swap(nodeArr, --big, index);
    }

    private static void Swap(Node?[] nodeArr, int a, int b)
    {
        (nodeArr[a], nodeArr[b]) = (nodeArr[b], nodeArr[a]);
    }

    private static Node? ListPartition2(Node? head, int pivot)
    {
        Node? sH = null; // small head
        Node? sT = null; // small tail
        Node? eH = null; // equal head
        Node? eT = null; // equal tail
        Node? mH = null; // big head
        Node? mT = null; // big tail
        // every node distributed to three lists
        while (head != null)
        {
            var next = head.Next; // save next node
            head.Next = null;
            if (head.Value < pivot)
            {
                if (sH == null)
                {
                    sH = head;
                    sT = head;
                }
                else
                {
                    if (sT != null) sT.Next = head;
                    sT = head;
                }
            }
            else if (head.Value == pivot)
            {
                if (eH == null)
                {
                    eH = head;
                    eT = head;
                }
                else
                {
                    if (eT != null) eT.Next = head;
                    eT = head;
                }
            }
            else
            {
                if (mH == null)
                {
                    mH = head;
                    mT = head;
                }
                else
                {
                    if (mT != null) mT.Next = head;
                    mT = head;
                }
            }

            head = next;
        }

        // 小于区域的尾巴，连等于区域的头，等于区域的尾巴连大于区域的头
        if (sT != null)
        {
            // 如果有小于区域
            sT.Next = eH;
            eT ??= sT; // 下一步，谁去连大于区域的头，谁就变成eT
        }

        // 下一步，一定是需要用eT 去接 大于区域的头
        // 有等于区域，eT -> 等于区域的尾结点
        // 无等于区域，eT -> 小于区域的尾结点
        // eT 尽量不为空的尾巴节点
        if (eT != null)
            // 如果小于区域和等于区域，不是都没有
            eT.Next = mH;

        return sH ?? (eH ?? mH);
    }

    private static void PrintLinkedList(Node? node)
    {
        Console.Write("Linked List: ");
        while (node != null)
        {
            Console.Write(node.Value + " ");
            node = node.Next;
        }

        Console.WriteLine();
    }

    public static void Run()
    {
        var head1 = new Node(7)
        {
            Next = new Node(9)
            {
                Next = new Node(1)
                {
                    Next = new Node(8)
                    {
                        Next = new Node(5)
                        {
                            Next = new Node(2)
                            {
                                Next = new Node(5)
                            }
                        }
                    }
                }
            }
        };
        PrintLinkedList(head1);
        // head1 = listPartition1(head1, 4);
        head1 = ListPartition2(head1, 5);
        PrintLinkedList(head1);
    }

    public class Node(int data)
    {
        public readonly int Value = data;
        public Node? Next;
    }
}