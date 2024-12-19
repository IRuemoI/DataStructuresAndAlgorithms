//todo:进度 https://www.bilibili.com/video/BV1sovaemEhi/?t=2146

namespace CustomTraining;

public static class Program
{
    public class TreeNode(int x)
    {
        public readonly int Val = x;
        public TreeNode? Left;
        public TreeNode? Right;
    }

    public class TreeBuilder
    {
        private int preIndex;

        public TreeNode? BuildTree(int[] preorder, int[] inorder)
        {
            return ArrayToTree(preorder, inorder, 0, inorder.Length - 1);
        }

        private TreeNode? ArrayToTree(int[] preorder, int[] inorder, int inStart, int inEnd)
        {
            // 递归终止条件
            if (inStart > inEnd)
                return null;

            // 先序遍历的第一个值是根节点
            var root = new TreeNode(preorder[preIndex++]);

            // 在中序遍历中找到根节点的位置
            var inIndex = Search(inorder, inStart, inEnd, root.Val);

            // 构建左子树
            root.Left = ArrayToTree(preorder, inorder, inStart, inIndex - 1);

            // 构建右子树
            root.Right = ArrayToTree(preorder, inorder, inIndex + 1, inEnd);

            return root;
        }

        private int Search(int[] arr, int start, int end, int value)
        {
            int i;
            for (i = start; i <= end; i++)
                if (arr[i] == value)
                    break;
            return i;
        }
    }

    public static void Main()
    {
        int[] preorder = [3, 9, 20, 15, 7];
        int[] inorder = [9, 3, 15, 20, 7];
        var solution = new TreeBuilder();
        var root = solution.BuildTree(preorder, inorder);
    }
}