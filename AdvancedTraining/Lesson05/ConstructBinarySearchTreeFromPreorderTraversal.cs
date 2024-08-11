namespace AdvancedTraining.Lesson05;

// 本题测试链接 : https://leetcode.cn/problems/construct-binary-search-tree-from-preorder-traversal/
public class ConstructBinarySearchTreeFromPreorderTraversal
{
    // 提交下面的方法
    private static TreeNode? BstFromPreorder1(int[]? pre)
    {
        if (pre == null || pre.Length == 0) return null;
        return Process1(pre, 0, pre.Length - 1);
    }

    private static TreeNode? Process1(int[] pre, int l, int r)
    {
        if (l > r) return null;
        var firstBig = l + 1;
        for (; firstBig <= r; firstBig++)
            if (pre[firstBig] > pre[l])
                break;

        var head = new TreeNode(pre[l])
        {
            Left = Process1(pre, l + 1, firstBig - 1),
            Right = Process1(pre, firstBig, r)
        };
        return head;
    }

    // 已经是时间复杂度最优的方法了，但是常数项还能优化
    private static TreeNode? BstFromPreorder2(int[]? pre)
    {
        if (pre == null || pre.Length == 0) return null;
        var n = pre.Length;
        var nearBig = new int[n];
        for (var i = 0; i < n; i++) nearBig[i] = -1;
        var stack = new Stack<int>();
        for (var i = 0; i < n; i++)
        {
            while (stack.Count > 0 && pre[stack.Peek()] < pre[i]) nearBig[stack.Pop()] = i;
            stack.Push(i);
        }

        return Process2(pre, 0, n - 1, nearBig);
    }

    private static TreeNode? Process2(int[] pre, int l, int r, int[] nearBig)
    {
        if (l > r) return null;
        var firstBig = nearBig[l] == -1 || nearBig[l] > r ? r + 1 : nearBig[l];
        var head = new TreeNode(pre[l])
        {
            Left = Process2(pre, l + 1, firstBig - 1, nearBig),
            Right = Process2(pre, firstBig, r, nearBig)
        };
        return head;
    }

    // 最优解
    private static TreeNode? BstFromPreorder3(int[]? pre)
    {
        if (pre == null || pre.Length == 0) return null;
        var n = pre.Length;
        var nearBig = new int[n];
        for (var i = 0; i < n; i++) nearBig[i] = -1;
        var stack = new int[n];
        var size = 0;
        for (var i = 0; i < n; i++)
        {
            while (size != 0 && pre[stack[size - 1]] < pre[i]) nearBig[stack[--size]] = i;
            stack[size++] = i;
        }

        return Process3(pre, 0, n - 1, nearBig);
    }

    private static TreeNode? Process3(int[] pre, int l, int r, int[] nearBig)
    {
        if (l > r) return null;
        var firstBig = nearBig[l] == -1 || nearBig[l] > r ? r + 1 : nearBig[l];
        var head = new TreeNode(pre[l])
        {
            Left = Process3(pre, l + 1, firstBig - 1, nearBig),
            Right = Process3(pre, firstBig, r, nearBig)
        };
        return head;
    }

    public static void Run()
    {
        int[] pre = [8, 5, 1, 7, 10, 12];
        var head = BstFromPreorder1(pre);
        PrintTree(head);
        Console.WriteLine();

        head = BstFromPreorder2(pre);
        PrintTree(head);
        Console.WriteLine();

        head = BstFromPreorder3(pre);
        PrintTree(head);
    }

    private static void PrintTree(TreeNode? head)
    {
        if (head == null) return;
        Console.Write(head.Value != null ? head.Value + " " : "null" + " ");
        PrintTree(head.Left);
        PrintTree(head.Right);
    }

    // 不用提交这个类
    public class TreeNode
    {
        public TreeNode? Left;
        public TreeNode? Right;
        public int? Value;

        public TreeNode()
        {
        }

        public TreeNode(int value)
        {
            Value = value;
        }

        public TreeNode(int value, TreeNode left, TreeNode right)
        {
            Value = value;
            Left = left;
            Right = right;
        }
    }
}