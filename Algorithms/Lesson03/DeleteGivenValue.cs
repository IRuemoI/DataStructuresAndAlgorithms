#region

using Common.DataStructures.LinkedList;

#endregion

namespace Algorithms.Lesson03;

public static class DeleteGivenValue
{
    private static SingleLinkedList.SNode? RemoveValue(SingleLinkedList.SNode? head, int num)
    {
        // 处理头节点  
        while (head != null && head.Value == num) head = head.Next;

        if (head == null) return null; // 如果链表为空或全部节点都被删除，返回null  

        var pre = head;
        var cur = head.Next;
        while (cur != null)
        {
            if (cur.Value == num)
                pre.Next = cur.Next;
            else
                pre = cur;

            cur = cur.Next;
        }

        return head;
    }

    public static void Run()
    {
        //构建一个四个节点的单链表
        var head = new SingleLinkedList.SNode(1)
        {
            Next = new SingleLinkedList.SNode(2)
            {
                Next = new SingleLinkedList.SNode(3)
                {
                    Next = new SingleLinkedList.SNode(4)
                    {
                        Next = new SingleLinkedList.SNode(5)
                        {
                            Next = new SingleLinkedList.SNode(6)
                        }
                    }
                }
            }
        };
        head = RemoveValue(head, 1);
        Console.WriteLine("删除1后的链表：");
        var temp = head;
        while (temp != null)
        {
            Console.WriteLine(temp.Value);
            temp = temp.Next;
        }

        Console.WriteLine("===============");
        head = RemoveValue(head, 3);
        Console.WriteLine("删除3后的链表：");
        temp = head;
        while (temp != null)
        {
            Console.WriteLine(temp.Value);
            temp = temp.Next;
        }
    }
}