namespace AdvancedTraining.Lesson37;

//https://leetcode.cn/problems/house-robber-iii/description/
public class HouseRobberIii //leetcode_0337
{
    private static int Rob(TreeNode root)
    {
        var info = Process(root);
        return Math.Max(info.No, info.Yes);
    }

    private static Info Process(TreeNode? x)
    {
        if (x == null) return new Info(0, 0);
        var leftInfo = Process(x.Left);
        var rightInfo = Process(x.Right);
        var no = Math.Max(leftInfo.No, leftInfo.Yes) + Math.Max(rightInfo.No, rightInfo.Yes);
        var yes = x.Val + leftInfo.No + rightInfo.No;
        return new Info(no, yes);
    }

    public class TreeNode
    {
        public TreeNode? Left;
        public TreeNode? Right;
        public int Val;
    }

    private class Info(int n, int y)
    {
        public readonly int No = n;
        public readonly int Yes = y;
    }
}