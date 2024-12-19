//pass
namespace AdvancedTraining.Lesson38;

//https://leetcode.cn/problems/merge-two-binary-trees/description/
public class MergeTwoBinaryTrees //leetcode_0617
{
    // 当前，一棵树的头是t1，另一颗树的头是t2
    // 请返回，整体merge之后的头
    private static TreeNode? MergeTrees(TreeNode? t1, TreeNode? t2)
    {
        if (t1 == null) return t2;
        if (t2 == null) return t1;
        // t1和t2都不是空
        var merge = new TreeNode(t1.Val + t2.Val)
        {
            Left = MergeTrees(t1.Left, t2.Left),
            Right = MergeTrees(t1.Right, t2.Right)
        };
        return merge;
    }

    public class TreeNode(int val)
    {
        public readonly int Val = val;
        public TreeNode? Left;
        public TreeNode? Right;
    }
}