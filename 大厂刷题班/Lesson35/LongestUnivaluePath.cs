namespace AdvancedTraining.Lesson35;

//https://leetcode.cn/problems/longest-univalue-path/
//pass
public class LongestUnivaluePath //leetcode_0687
{
    private static int LongestUnivaluePathCode(TreeNode? root)
    {
        if (root == null) return 0;
        return Process(root).Max - 1;
    }

    private static Info Process(TreeNode? x)
    {
        if (x == null) return new Info(0, 0);
        var l = x.Left;
        var r = x.Right;
        // 左树上，不要求从左孩子出发，最大路径
        // 左树上，必须从左孩子出发，往下的最大路径
        var leftInfo = Process(l);
        // 右树上，不要求从右孩子出发，最大路径
        // 右树上，必须从右孩子出发，往下的最大路径
        var rinfo = Process(r);
        // 必须从x出发的情况下，往下的最大路径
        var len = 1;
        if (l != null && l.Val == x.Val) len = leftInfo.Len + 1;
        if (r != null && r.Val == x.Val) len = Math.Max(len, rinfo.Len + 1);
        // 不要求从x出发，最大路径
        var max = Math.Max(Math.Max(leftInfo.Max, rinfo.Max), len);
        if (l != null && r != null && l.Val == x.Val && r.Val == x.Val)
            max = Math.Max(max, leftInfo.Len + rinfo.Len + 1);
        return new Info(len, max);
    }

    public class TreeNode(int v, TreeNode? left, TreeNode? right)
    {
        public readonly TreeNode? Left = left;
        public readonly TreeNode? Right = right;
        public readonly int Val = v;
    }

    // 建设以x节点为头的树，返回两个信息
    private class Info(int l, int m)
    {
        // 在一条路径上：要求每个节点通过且只通过一遍
        public readonly int Len = l; // 路径必须从x出发且只能往下走的情况下，路径的最大距离
        public readonly int Max = m; // 路径不要求必须从x出发的情况下，整棵树的合法路径最大距离
    }
}