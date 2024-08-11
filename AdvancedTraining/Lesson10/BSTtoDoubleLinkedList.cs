namespace AdvancedTraining.Lesson10;

// 本题测试链接 : https://leetcode.cn/problems/convert-binary-search-tree-to-sorted-doubly-linked-list/
public class BstToDoubleLinkedList
{
    // 提交下面的代码
    private static Node? TreeToDoublyList(Node? head)
    {
        if (head == null) return null;
        var allInfo = Process(head);
        allInfo.End!.Right = allInfo.Start;
        allInfo.Start!.Left = allInfo.End;
        return allInfo.Start;
    }

    private static Info Process(Node? x)
    {
        if (x == null) return new Info(null, null);
        var lInfo = Process(x.Left);
        var rInfo = Process(x.Right);
        if (lInfo.End != null) lInfo.End.Right = x;
        x.Left = lInfo.End;
        x.Right = rInfo.Start;
        if (rInfo.Start != null) rInfo.Start.Left = x;
        // 整体链表的头    lInfo.start != null ? lInfo.start : X
        // 整体链表的尾    rInfo.end != null ? rInfo.end : X
        return new Info(lInfo.Start ?? x, rInfo.End ?? x);
    }

    public static void Run()
    {
        //todo:构建参数
    }

    // 提交时不要提交这个类
    public class Node(int data)
    {
        public Node? Left;
        public Node? Right;
        public int Value = data;
    }

    private class Info(Node? start, Node? end)
    {
        public readonly Node? End = end;
        public readonly Node? Start = start;
    }
}