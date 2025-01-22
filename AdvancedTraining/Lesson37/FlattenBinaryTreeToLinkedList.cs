namespace AdvancedTraining.Lesson37;
//pass
// 注意，我们课上讲了一个别的题，并不是leetcode 114
// 我们课上讲的是，把一棵搜索二叉树变成有序链表，怎么做
// 而leetcode 114是，把一棵树先序遍历的结果串成链表
// 所以我更新了代码，这个代码是leetcode 114的实现
// 利用morris遍历
// https://leetcode.cn/problems/flatten-binary-tree-to-linked-list/description/
public class FlattenBinaryTreeToLinkedList //leetcode_0114
{
    // 普通解
    private static void Flatten1(TreeNode root)
    {
        Process(root);
    }

    private static Info? Process(TreeNode? head)
    {
        if (head == null) return null;
        var leftInfo = Process(head.Left);
        var rightInfo = Process(head.Right);
        head.Left = null;
        head.Right = leftInfo?.Head;
        var tail = leftInfo == null ? head : leftInfo.Tail;
        tail!.Right = rightInfo?.Head;
        tail = rightInfo == null ? tail : rightInfo.Tail;
        return new Info(head, tail);
    }

    // Morris遍历的解
    private static void Flatten2(TreeNode? root)
    {
        if (root == null) return;
        TreeNode? pre = null;
        var cur = root;
        while (cur != null)
        {
            var mostRight = cur.Left;
            if (mostRight != null)
            {
                while (mostRight.Right != null && mostRight.Right != cur) mostRight = mostRight.Right;
                if (mostRight.Right == null)
                {
                    mostRight.Right = cur;
                    if (pre != null) pre.Left = cur;
                    pre = cur;
                    cur = cur.Left;
                    continue;
                }

                mostRight.Right = null;
            }
            else
            {
                if (pre != null) pre.Left = cur;
                pre = cur;
            }

            cur = cur.Right;
        }

        cur = root;
        while (cur != null)
        {
            var next = cur.Left;
            cur.Left = null;
            cur.Right = next;
            cur = next;
        }
    }

    // 这个类不用提交
    public class TreeNode(int value)
    {
        public TreeNode? Left;
        public TreeNode? Right;
        public int Val = value;
    }

    private class Info(TreeNode? h, TreeNode? t)
    {
        public readonly TreeNode? Head = h;
        public readonly TreeNode? Tail = t;
    }
}