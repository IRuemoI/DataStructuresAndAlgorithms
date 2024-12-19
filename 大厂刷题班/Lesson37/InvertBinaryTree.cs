namespace AdvancedTraining.Lesson37;

// https://leetcode.cn/problems/invert-binary-tree/description/
public class InvertBinaryTree //leetcode_0226
{
    private static TreeNode? InvertTree(TreeNode? root)
    {
        if (root == null) return null;
        var left = root.Left;
        root.Left = InvertTree(root.Right);
        root.Right = InvertTree(left);
        return root;
    }


    public static void Run()
    {
        var root = new TreeNode
        {
            Val = 4, Left = new TreeNode { Val = 2, Left = new TreeNode { Val = 1 }, Right = new TreeNode { Val = 3 } },
            Right = new TreeNode { Val = 7, Left = new TreeNode { Val = 6 }, Right = new TreeNode { Val = 9 } }
        };
        InvertTree(root);
    }

    public class TreeNode
    {
        public TreeNode? Left;
        public TreeNode? Right;

        public int Val;
    }
}