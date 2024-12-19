//测试通过

namespace Algorithms.Lesson09;

public class SmallerEqualBigger
{
    private static Node? ListPartition1(Node? head, int target)
    {
        if (head == null) return head; //空链表直接返回

        //获得链表的长度
        var current = head;
        var length = 0;
        while (current != null)
        {
            length++;
            current = current.Next;
        }

        //将链表中的节点放到数组中
        var nodeArray = new Node?[length];
        current = head;
        for (length = 0; length != nodeArray.Length; length++)
        {
            nodeArray[length] = current;
            current = current?.Next;
        }

        //对数组进行划分
        ArrayPartition(nodeArray, target);
        //把数组中的节点从左向右重新连接并将最后一个节点的Next设置为null
        for (length = 1; length != nodeArray.Length; length++) nodeArray[length - 1]!.Next = nodeArray[length];
        nodeArray[length - 1]!.Next = null;
        //返回头指针
        return nodeArray[0];
    }

    private static void ArrayPartition(Node?[] nodeArr, int target)
    {
        var small = -1;
        var big = nodeArr.Length;
        var index = 0;
        while (index != big)
        {
            if (nodeArr[index]?.Value < target)
            {
                small++;
                (nodeArr[small], nodeArr[index]) = (nodeArr[index], nodeArr[small]);
                index++;
            }
            else if (nodeArr[index]?.Value == target)
            {
                index++;
            }
            else
            {
                big--;
                (nodeArr[big], nodeArr[index]) = (nodeArr[index], nodeArr[big]);
            }
        }
    }

    private static Node? ListPartition2(Node? head, int target)
    {
        Node? lessRegionHead = null; //小于区域头指针
        Node? lessRegionTail = null; //小于区域尾指针
        Node? equalRegionHead = null; //等于区域头指针
        Node? equalRegionTail = null; //等于区域尾指针
        Node? greaterRegionHead = null; //大于区域头指针
        Node? greaterRegionTail = null; //大于区域尾指针
        // 将每个节点分配到三个区域中
        var current = head;
        while (current != null)
        {
            var next = current.Next; //保存当前节点的下一个节点
            current.Next = null; //将当前节点的于之后的节点断开
            if (current.Value < target) //小于区域
            {
                if (lessRegionHead == null) //小于区域为空
                {
                    //小于区域的头尾指针都指向这个节点
                    lessRegionHead = current;
                    lessRegionTail = current;
                }
                else
                {
                    //在小于区域的尾指针指向的节点后追加这个新节点并更新尾指针
                    if (lessRegionTail != null) lessRegionTail.Next = current;
                    lessRegionTail = current;
                }
            }
            else if (current.Value == target) //等于区域
            {
                if (equalRegionHead == null) //等于区域为空
                {
                    //等于区域的头尾指针都指向这个节点
                    equalRegionHead = current;
                    equalRegionTail = current;
                }
                else
                {
                    //在等于区域的尾指针指向的节点后追加这个新节点并更新尾指针
                    if (equalRegionTail != null) equalRegionTail.Next = current;
                    equalRegionTail = current;
                }
            }
            else //大于区域
            {
                if (greaterRegionHead == null) //大于区域为空
                {
                    //大于区域的头尾指针都指向这个节点
                    greaterRegionHead = current;
                    greaterRegionTail = current;
                }
                else
                {
                    //在大于区域的尾指针指向的节点后追加这个新节点并更新尾指针
                    if (greaterRegionTail != null) greaterRegionTail.Next = current;
                    greaterRegionTail = current;
                }
            }

            current = next;
        }

        // 小于区域的尾巴，连等于区域的头，等于区域的尾巴连大于区域的头
        if (lessRegionTail != null) // 如果有小于区域
        {
            lessRegionTail.Next = equalRegionHead;
            equalRegionTail ??= lessRegionTail; // 如果没有等于区域，等于区域的尾指针指向小于区域的尾指针
        }
        
        // 等于区域的尾巴，连大于区域的头
        if (equalRegionTail != null) // 如果小于区域和等于区域，不是都没有
            equalRegionTail.Next = greaterRegionHead;

        //返回小于区域的头，如果没有小于区域，返回等于区域的头，如果没有等于区域，返回大于区域的头
        return lessRegionHead ?? (equalRegionHead ?? greaterRegionHead);
    }

    private static void PrintLinkedList(Node? node)
    {
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
                                {
                                    Next = new Node(6)
                                    {
                                        Next = new Node(6)
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        var head2 = new Node(7)
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
                                {
                                    Next = new Node(6)
                                    {
                                        Next = new Node(6)
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        Console.Write("原本的链表：");
        PrintLinkedList(head1);
        head1 = ListPartition1(head1, 6);
        Console.Write("方法一结果：");
        PrintLinkedList(head1);
        head2 = ListPartition2(head2, 6);
        Console.Write("方法二结果：");
        PrintLinkedList(head2);
    }

    private class Node(int data)
    {
        public readonly int Value = data;
        public Node? Next;
    }
}