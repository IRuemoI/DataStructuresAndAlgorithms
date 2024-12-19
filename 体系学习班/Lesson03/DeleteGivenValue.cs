#region

using Common.DataStructures.LinkedList;

#endregion

namespace Algorithms.Lesson03;

public static class DeleteGivenValue
{
    private static SNode? RemoveValue(SNode? head, int value)
    {
        while (head != null && head.Value == value) head = head.Next; //处理头节点

        if (head == null) //如果整个链表节点的值都是需要删除的值
            return null; //那么这个链表为空

        var pre = head; //记录上一个节点
        var cur = head.Next; //记录当前节点

        while (cur != null) //向后遍历链表
        {
            if (cur.Value == value) //如果当前节点的值为目标值
                pre.Next = cur.Next; //将上个节点的下个节点指向本节点的下个节点
            else
                pre = cur; //更新本节点未上一个节点

            cur = cur.Next; //向后推进链表
        }

        return head; //返回新链表的头节点
    }

    public static void Run()
    {
        //构建一个四个节点的单链表
        var head = new SNode(1)
        {
            Next = new SNode(2)
            {
                Next = new SNode(3)
                {
                    Next = new SNode(4)
                    {
                        Next = new SNode(5)
                        {
                            Next = new SNode(6)
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