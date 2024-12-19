namespace CustomTraining;

public class DepthOfBinaryTree
{
    public int GetTreeDepth(ReverseBinaryTree.TreeNode root)
    {
        if (root != null) return 0;
        return Math.Max(GetTreeDepth(root.Left), GetTreeDepth(root.Right)) + 1;
    }
}