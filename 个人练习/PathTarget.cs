namespace CustomTraining;

public class PathTarget
{
    private readonly List<int> path = new();

    private readonly List<List<int>> res = new();

    public List<List<int>> PathTargetCode(TreeNode root, int target)
    {
        Recur(root, target);
        return res;
    }

    public void Recur(TreeNode root, int target)
    {
        if (root == null) return;

        path.Add(root.val);
        target -= root.val;
        if (target == 0 && root.left == null && root.right == null) res.Add(new List<int>(path));

        Recur(root.left, target);
        Recur(root.right, target);
        path.RemoveAt(path.Count - 1);
    }

    public class TreeNode(int value = 0)
    {
        public TreeNode? left;
        public TreeNode? right;
        public int val = value;
    }
}