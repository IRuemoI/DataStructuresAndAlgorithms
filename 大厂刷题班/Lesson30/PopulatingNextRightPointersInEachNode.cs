//pass
namespace AdvancedTraining.Lesson30;

public class PopulatingNextRightPointersInEachNode //leetcode_0116
{
    // 提交下面的代码
    private static Node? Connect(Node? root)
    {
        if (root == null) return root;
        var queue = new MyQueue();
        queue.Offer(root);
        while (!queue.Empty)
        {
            // 第一个弹出的节点
            Node? pre = null;
            var size = queue.Size;
            for (var i = 0; i < size; i++)
            {
                var cur = queue.Poll();
                if (cur.Left != null) queue.Offer(cur.Left);
                if (cur.Right != null) queue.Offer(cur.Right);
                if (pre != null) pre.Next = cur;
                pre = cur;
            }
        }

        return root;
    }

    // 不要提交这个类
    private class Node
    {
        public Node? Left;
        public Node? Next;
        public Node? Right;
        public int Val;
    }

    private class MyQueue
    {
        private Node? _head;
        private Node? _tail;
        public int Size;

        public bool Empty => Size == 0;

        public void Offer(Node cur)
        {
            Size++;
            if (_head == null)
            {
                _head = cur;
                _tail = cur;
            }
            else
            {
                if (_tail != null) _tail.Next = cur;
                _tail = cur;
            }
        }

        public Node Poll()
        {
            Size--;
            var ans = _head;
            _head = _head?.Next;
            ans!.Next = null;
            return ans;
        }
    }
}