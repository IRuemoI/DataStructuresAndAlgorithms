//二叉树从下向上层次遍历

namespace AdvancedTraining.算法新手班补充;

public class BinaryTreeLevelOrderTraversal
{
    public IList<IList<int>> LevelOrderBottom(TreeNode root)
    {
        IList<IList<int>> result = new List<IList<int>>();
        Stack<IList<int>> stack = new Stack<IList<int>>();
        Queue<TreeNode> queue = new Queue<TreeNode>();
        Queue<TreeNode> queue2 = new Queue<TreeNode>();
        if (root != null)
            queue.Enqueue(root);
        while (queue.Count > 0)
        {
            IList<int> layerResult = new List<int>();
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                layerResult.Add(node.val);
                if (node.left != null)
                    queue2.Enqueue(node.left);
                if (node.right != null)
                    queue2.Enqueue(node.right);
            }

            stack.Push(layerResult);
            (queue, queue2) = (queue2, queue);
        }

        while (stack.Count > 0) result.Add(stack.Pop());

        return result;
    }

    public class TreeNode(int value)
    {
        public TreeNode? left;
        public TreeNode? right;
        public int val = value;
    }
}