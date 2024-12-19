//pass
namespace AdvancedTraining.Lesson30;

// follow up : 如果要求返回整个路径怎么做？
public class BinaryTreeMaximumPathSum //leetcode_0124
{
    private static int MaxPathSum(TreeNode? root)
    {
        if (root == null) return 0;
        return Process(root)!.MaxPathSumValue;
    }

    private static Info? Process(TreeNode? x)
    {
        if (x == null) return null;
        var leftInfo = Process(x.Left);
        var rightInfo = Process(x.Right);
        // x 1)只有x 2）x往左扎 3）x往右扎
        var maxPathSumFromHead = x.Val;
        if (leftInfo != null) maxPathSumFromHead = Math.Max(maxPathSumFromHead, x.Val + leftInfo.MaxPathSumFromHead);
        if (rightInfo != null) maxPathSumFromHead = Math.Max(maxPathSumFromHead, x.Val + rightInfo.MaxPathSumFromHead);
        // x整棵树最大路径和 1) 只有x 2)左树整体的最大路径和 3) 右树整体的最大路径和
        var maxPathSum = x.Val;
        if (leftInfo != null) maxPathSum = Math.Max(maxPathSum, leftInfo.MaxPathSumValue);
        if (rightInfo != null) maxPathSum = Math.Max(maxPathSum, rightInfo.MaxPathSumValue);
        // 4) x只往左扎 5）x只往右扎
        maxPathSum = Math.Max(maxPathSumFromHead, maxPathSum);
        // 6）一起扎
        if (leftInfo != null && rightInfo != null && leftInfo.MaxPathSumFromHead > 0 &&
            rightInfo.MaxPathSumFromHead > 0)
            maxPathSum = Math.Max(maxPathSum, leftInfo.MaxPathSumFromHead + rightInfo.MaxPathSumFromHead + x.Val);
        return new Info(maxPathSum, maxPathSumFromHead);
    }

    // 如果要返回路径的做法
    private static IList<TreeNode> GetMaxSumPath(TreeNode? head)
    {
        IList<TreeNode> ans = new List<TreeNode>();
        if (head != null)
        {
            var data = F(head);
            var fMap = new Dictionary<TreeNode, TreeNode>
            {
                [head] = head
            };
            FatherMap(head, fMap);
            if (data is { From: not null, To: not null })
                FillPath(fMap, data.From, data.To, ans);
        }

        return ans;
    }

    private static Data? F(TreeNode? x)
    {
        if (x == null) return null;
        var l = F(x.Left);
        var r = F(x.Right);
        var maxHeadSum = x.Val;
        var end = x;
        if (l is { MaxHeadSum: > 0 } && (r == null || l.MaxHeadSum > r.MaxHeadSum))
        {
            maxHeadSum += l.MaxHeadSum;
            end = l.End;
        }

        if (r is { MaxHeadSum: > 0 } && (l == null || r.MaxHeadSum > l.MaxHeadSum))
        {
            maxHeadSum += r.MaxHeadSum;
            end = r.End;
        }

        var maxAllSum = int.MinValue;
        TreeNode? from = null;
        TreeNode? to = null;
        if (l != null)
        {
            maxAllSum = l.MaxAllSum;
            from = l.From;
            to = l.To;
        }

        if (r != null && r.MaxAllSum > maxAllSum)
        {
            maxAllSum = r.MaxAllSum;
            from = r.From;
            to = r.To;
        }

        var p3 = x.Val + (l is { MaxHeadSum: > 0 } ? l.MaxHeadSum : 0) + (r is { MaxHeadSum: > 0 } ? r.MaxHeadSum : 0);
        if (p3 > maxAllSum)
        {
            maxAllSum = p3;
            from = l is { MaxHeadSum: > 0 } ? l.End : x;
            to = r is { MaxHeadSum: > 0 } ? r.End : x;
        }

        return new Data(maxAllSum, from, to, maxHeadSum, end);
    }

    private static void FatherMap(TreeNode? h, Dictionary<TreeNode, TreeNode> map)
    {
        if (h?.Left == null && h?.Right == null) return;
        if (h.Left != null)
        {
            map[h.Left] = h;
            FatherMap(h.Left, map);
        }

        if (h.Right != null)
        {
            map[h.Right] = h;
            FatherMap(h.Right, map);
        }
    }

    private static void FillPath(Dictionary<TreeNode, TreeNode> fmap, TreeNode a, TreeNode b, IList<TreeNode> ans)
    {
        if (a == b)
        {
            ans.Add(a);
        }
        else
        {
            var ap = new HashSet<TreeNode>();
            var cur = a;
            while (cur != fmap[cur])
            {
                ap.Add(cur);
                cur = fmap[cur];
            }

            ap.Add(cur);
            cur = b;
            TreeNode? lca = null;
            while (lca == null)
                if (ap.Contains(cur))
                    lca = cur;
                else
                    cur = fmap[cur];
            while (a != lca)
            {
                ans.Add(a);
                a = fmap[a];
            }

            ans.Add(lca);
            var right = new List<TreeNode>();
            while (b != lca)
            {
                right.Add(b);
                b = fmap[b];
            }

            for (var i = right.Count - 1; i >= 0; i--) ans.Add(right[i]);
        }
    }

    public static void Run()
    {
        var head = new TreeNode(4)
        {
            Left = new TreeNode(-7),
            Right = new TreeNode(-5)
        };
        head.Left.Left = new TreeNode(9);
        head.Left.Right = new TreeNode(9);
        head.Right.Left = new TreeNode(4);
        head.Right.Right = new TreeNode(3);

        var maxPath = GetMaxSumPath(head);

        foreach (var n in maxPath) Console.WriteLine(n.Val);
    }

    public class TreeNode(int v)
    {
        internal readonly int Val = v;
        internal TreeNode? Left;
        internal TreeNode? Right;
    }

    // 任何一棵树，必须汇报上来的信息
    private class Info(int path, int head)
    {
        public readonly int MaxPathSumFromHead = head;
        public readonly int MaxPathSumValue = path;
    }

    private class Data(int a, TreeNode? b, TreeNode? c, int d, TreeNode? e)
    {
        public readonly TreeNode? End = e;
        public readonly TreeNode? From = b;
        public readonly int MaxAllSum = a;
        public readonly int MaxHeadSum = d;
        public readonly TreeNode? To = c;
    }
}