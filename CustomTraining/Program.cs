//todo:进度 https://www.bilibili.com/video/BV1sovaemEhi/?t=2146

namespace CustomTraining;

public static class Program
{
    private class Node(int data)
    {
        public readonly int Value = data;
        public Node? Next;
    }

    private static Node? ReverseLinkedList(Node? head)
    {
        //将链表分为两个部分:已反转链表和未反转链表
        //next指向未反转链表的头节点
        //pre指向已反转链表的头节点
        Node? pre = null;
        var current = head;
        while (current != null)
        {
            //向右缩短next指向的未反转链表
            var next = current.Next;
            //让原本的下一个节点指向上一个节点
            current.Next = pre;
            //让pre指向已反转链表的头节点
            pre = current;
            //向右扩展已反转区
            current = next;
        }

        return pre; //返回最终已反转链表的头节点
    }

    public static void Main()
    {
        var node1 = new Node(1);
        var node2 = new Node(2);
        var node3 = new Node(3);
        node1.Next = node2;
        node2.Next = node3;

        var res = ReverseLinkedList(node1);
        int i = 0;
        while (res != null)
        {
            Console.WriteLine(res.Value);
            res = res.Next;
            i++;
            if (i > 5) break;
        }
    }
}