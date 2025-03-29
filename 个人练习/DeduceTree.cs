namespace CustomTraining;

public class DeduceTree
{
    private readonly Dictionary<int, int> _dict = new();

    private int[] preOrder;

    public TreeNode DeduceTreeCode(int[] preOrder, int[] inOrder)
    {
        this.preOrder = preOrder;
        for (var i = 0; i < inOrder.Length; i++) _dict.Add(inOrder[i], i);
        return Recur(0, 0, inOrder.Length - 1);
    }

    private TreeNode Recur(int root, int left, int right)
    {
        if (left > right) return null;

        TreeNode node = new(preOrder[root]);
        var i = _dict[preOrder[root]];
        node.left = Recur(root + 1, left, i - 1);
        node.right = Recur(root + 1 - left, i + 1, right);
        return node;
    }

    public class TreeNode(int value = 0)
    {
        public TreeNode? left;
        public TreeNode? right;
        public int val = value;
    }
}