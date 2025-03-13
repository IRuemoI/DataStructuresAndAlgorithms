namespace CustomTraining;

public class ReverseBinaryTree
{
    public class TreeNode(int x)
    {
        public readonly int Val = x;
        public TreeNode? Left;
        public TreeNode? Right;
    }

    public void Print(TreeNode node)
    {
        if (node == null) return;

        Queue<TreeNode> queue = new();
        queue.Enqueue(node);
        while (queue.Count > 0)
        {
            node = queue.Dequeue();
            Console.WriteLine(node.Val);
            if (node.Left != null) queue.Enqueue(node.Left);

            if (node.Right != null) queue.Enqueue(node.Right);
        }
    }

    public TreeNode Reverse(TreeNode root)
    {
        if (root == null) return null;

        Reverse(root.Left);
        Reverse(root.Right);
        (root.Right, root.Left) = (root.Left, root.Right);
        return root;
    }
}