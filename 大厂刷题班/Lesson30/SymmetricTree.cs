//pass
namespace AdvancedTraining.Lesson30;

public class SymmetricTree //leetcode_0101
{
    public virtual bool IsSymmetric(TreeNode root)
    {
        return IsMirror(root, root);
    }

    // 一棵树是原始树  head1
    // 另一棵是翻面树  head2
    private static bool IsMirror(TreeNode? head1, TreeNode? head2)
    {
        if (head1 == null && head2 == null) return true;
        if (head1 != null && head2 != null)
            return head1.Val == head2.Val && IsMirror(head1.Left, head2.Right) && IsMirror(head1.Right, head2.Left);
        // 一个为空，一个不为空  false
        return false;
    }

    public class TreeNode
    {
        public TreeNode? Left;
        public TreeNode? Right;
        public int Val;
    }
}