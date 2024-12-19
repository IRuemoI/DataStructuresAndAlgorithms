namespace AdvancedTraining.Lesson42;

//https://www.cnblogs.com/fuxuemingzhu/p/15435876.html
public class ClosestBinarySearchTreeValueIi //leetcode_0272
{
    // 这个解法来自讨论区的回答，最优解实现的很易懂且漂亮
    private static List<int> ClosestKValues(TreeNode root, double target, int k)
    {
        var ret = new List<int>();
        // >=8，最近的节点，而且需要快速找后继的这么一种结构
        var moreTops = new Stack<TreeNode>();
        // <=8，最近的节点，而且需要快速找前驱的这么一种结构
        var lessTops = new Stack<TreeNode>();
        GetMoreTops(root, target, moreTops);
        GetLessTops(root, target, lessTops);
        if (moreTops.Count > 0 && lessTops.Count > 0 && moreTops.Peek().Val == lessTops.Peek().Val)
            GetPredecessor(lessTops);
        while (k-- > 0)
            if (moreTops.Count == 0)
            {
                ret.Add(GetPredecessor(lessTops));
            }
            else if (lessTops.Count == 0)
            {
                ret.Add(GetSuccessor(moreTops));
            }
            else
            {
                var diffs = Math.Abs(moreTops.Peek().Val - target);
                var diffP = Math.Abs(lessTops.Peek().Val - target);
                if (diffs < diffP)
                    ret.Add(GetSuccessor(moreTops));
                else
                    ret.Add(GetPredecessor(lessTops));
            }

        return ret;
    }

    // 在root为头的树上
    // 找到>=target，且最接近target的节点
    // 并且找的过程中，只要某个节点x往左走了，就把x放入moreTops里
    private static void GetMoreTops(TreeNode? root, double target, Stack<TreeNode> moreTops)
    {
        while (root != null)
            if (root.Val == target)
            {
                moreTops.Push(root);
                break;
            }
            else if (root.Val > target)
            {
                moreTops.Push(root);
                root = root.Left;
            }
            else
            {
                root = root.Right;
            }
    }

    // 在root为头的树上
    // 找到<=target，且最接近target的节点
    // 并且找的过程中，只要某个节点x往右走了，就把x放入lessTops里
    private static void GetLessTops(TreeNode root, double target, Stack<TreeNode> lessTops)
    {
        while (root != null)
            if (root.Val == target)
            {
                lessTops.Push(root);
                break;
            }
            else if (root.Val < target)
            {
                lessTops.Push(root);
                root = root.Right;
            }
            else
            {
                root = root.Left;
            }
    }

    // 返回moreTops的头部的值
    // 并且调整moreTops : 为了以后能很快的找到返回节点的后继节点
    private static int GetSuccessor(Stack<TreeNode> moreTops)
    {
        var cur = moreTops.Pop();
        var ret = cur.Val;
        cur = cur.Right;
        while (cur != null)
        {
            moreTops.Push(cur);
            cur = cur.Left;
        }

        return ret;
    }

    // 返回lessTops的头部的值
    // 并且调整lessTops : 为了以后能很快的找到返回节点的前驱节点
    private static int GetPredecessor(Stack<TreeNode> lessTops)
    {
        var cur = lessTops.Pop();
        var ret = cur.Val;
        cur = cur.Left;
        while (cur != null)
        {
            lessTops.Push(cur);
            cur = cur.Right;
        }

        return ret;
    }

    //todo:待整理
    public static void Run()
    {
    }

    public class TreeNode(int val)
    {
        public TreeNode? Left;
        public TreeNode? Right;
        public int Val = val;
    }
}