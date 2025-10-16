namespace AdvancedTraining.Lesson30;

public class BinaryTreeZigzagLevelOrderTraversal //leetcode_0103
{
    private static IList<IList<int>> ZigzagLevelOrder(TreeNode? root)
    {
        IList<IList<int>> ans = new List<IList<int>>();
        if (root == null) return ans;
        var deque = new LinkedList<TreeNode>();
        deque.AddLast(root);
        var isHead = true;
        while (deque.Count > 0)
        {
            var size = deque.Count;
            IList<int> curLevel = new List<int>();
            for (var i = 0; i < size; i++)
            {
                var cur = isHead ? deque.First() : deque.Last();
                curLevel.Add(cur.Val);
                if (isHead)
                {
                    deque.RemoveFirst();
                    if (cur.Left != null) deque.AddLast(cur.Left);
                    if (cur.Right != null) deque.AddLast(cur.Right);
                }
                else
                {
                    deque.RemoveLast();
                    if (cur.Right != null) deque.AddFirst(cur.Right);
                    if (cur.Left != null) deque.AddFirst(cur.Left);
                }
            }

            ans.Add(curLevel);
            isHead = !isHead;
        }

        return ans;
    }

    public static void Run()
    {
        var result = ZigzagLevelOrder(
            new TreeNode
            {
                Val = 1, Left = new TreeNode
                {
                    Val = 2, Left = new TreeNode
                    {
                        Val = 4
                    },
                    Right = new TreeNode
                    {
                        Val = 5
                    }
                },
                Right = new TreeNode
                {
                    Val = 3
                }
            });

        // 打印结果
        Console.WriteLine("[");
        foreach (var level in result)
        {
            Console.Write(" [");
            for (int i = 0; i < level.Count; i++)
            {
                Console.Write(level[i]);
                if (i < level.Count - 1) Console.Write(", ");
            }
            Console.WriteLine("]");
        }
        Console.WriteLine("]");
    }

    public class TreeNode
    {
        internal TreeNode? Left;
        internal TreeNode? Right;
        internal int Val;
    }
}