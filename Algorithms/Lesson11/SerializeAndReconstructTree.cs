//测试通过

#region

using System.Text;
using Common.Utilities;

#endregion

namespace Algorithms.Lesson11;

public class SerializeAndReconstructTree
{
    private static Queue<string?> PreSerial(Node? head)
    {
        Queue<string?> ans = new();
        Pres(head, ans);
        return ans;
    }

    private static void Pres(Node? head, Queue<string?> ans)
    {
        if (head == null)
        {
            ans.Enqueue(null);
        }
        else
        {
            ans.Enqueue(head.Value.ToString());
            Pres(head.Left, ans);
            Pres(head.Right, ans);
        }
    }

    private static Queue<string?> InSerial(Node head)
    {
        Queue<string?> ans = new();
        Ins(head, ans);
        return ans;
    }

    private static void Ins(Node? head, Queue<string?> ans)
    {
        if (head == null)
        {
            ans.Enqueue(null);
        }
        else
        {
            Ins(head.Left, ans);
            ans.Enqueue(head.Value.ToString());
            Ins(head.Right, ans);
        }
    }

    private static Queue<string?> PosSerial(Node? head)
    {
        Queue<string?> ans = new();
        Poss(head, ans);
        return ans;
    }

    private static void Poss(Node? head, Queue<string?> ans)
    {
        if (head == null)
        {
            ans.Enqueue(null);
        }
        else
        {
            Poss(head.Left, ans);
            Poss(head.Right, ans);
            ans.Enqueue(head.Value.ToString());
        }
    }

    private static Node? BuildByPreQueue(Queue<string?>? preList)
    {
        if (preList == null || preList.Count == 0) return null;

        return PreBuild(preList);
    }

    private static Node? PreBuild(Queue<string?> preList)
    {
        var value = preList.Dequeue();
        if (value == null) return null;

        var head = new Node(int.Parse(value))
        {
            Left = PreBuild(preList),
            Right = PreBuild(preList)
        };
        return head;
    }

    private static Node? BuildByPosQueue(Queue<string?>? posList)
    {
        if (posList == null || posList.Count == 0) return null;

        // 左右中  ->  stack(中右左)
        Stack<string?> stack = new();
        while (posList.Count != 0) stack.Push(posList.Dequeue());

        return PosBuild(stack);
    }

    private static Node? PosBuild(Stack<string?> posStack)
    {
        var value = posStack.Pop();
        if (value == null) return null;

        var head = new Node(int.Parse(value))
        {
            Right = PosBuild(posStack),
            Left = PosBuild(posStack)
        };
        return head;
    }

    private static Queue<string?> LevelSerial(Node? head)
    {
        Queue<string?> ans = new();
        if (head == null)
        {
            ans.Enqueue(null);
        }
        else
        {
            ans.Enqueue(head.Value.ToString());
            Queue<Node> queue = new();
            queue.Enqueue(head);
            while (queue.Count != 0)
            {
                head = queue.Dequeue(); // head 父   子
                if (head.Left != null)
                {
                    ans.Enqueue(head.Left.Value.ToString());
                    queue.Enqueue(head.Left);
                }
                else
                {
                    ans.Enqueue(null);
                }

                if (head.Right != null)
                {
                    ans.Enqueue(head.Right.Value.ToString());
                    queue.Enqueue(head.Right);
                }
                else
                {
                    ans.Enqueue(null);
                }
            }
        }

        return ans;
    }

    private static Node? BuildByLevelQueue(Queue<string?>? levelList)
    {
        if (levelList == null || levelList.Count == 0) return null;

        var head = GenerateNode(levelList.Dequeue());
        Queue<Node> queue = new();
        if (head != null) queue.Enqueue(head);

        while (queue.Count != 0)
        {
            var node = queue.Dequeue();
            node.Left = GenerateNode(levelList.Dequeue());
            node.Right = GenerateNode(levelList.Dequeue());
            if (node.Left != null) queue.Enqueue(node.Left);

            if (node.Right != null) queue.Enqueue(node.Right);
        }

        return head;
    }

    private static Node? GenerateNode(string? val)
    {
        if (val == null) return null;

        return new Node(int.Parse(val));
    }

    //用于测试
    private static Node? GenerateRandomBst(int maxLevel, int maxValue)
    {
        return Generate(1, maxLevel, maxValue);
    }

    //用于测试
    private static Node? Generate(int level, int maxLevel, int maxValue)
    {
        if (level > maxLevel || Utility.GetRandomDouble < 0.5) return null;

        var head = new Node((int)(Utility.GetRandomDouble * maxValue))
        {
            Left = Generate(level + 1, maxLevel, maxValue),
            Right = Generate(level + 1, maxLevel, maxValue)
        };
        return head;
    }

    //用于测试
    private static bool IsSameValueStructure(Node? head1, Node? head2)
    {
        if (head1 == null && head2 != null) return false;

        if (head1 != null && head2 == null) return false;

        if (head1 == null && head2 == null) return true;

        if (head1?.Value != head2?.Value) return false;

        return IsSameValueStructure(head1?.Left, head2?.Left) && IsSameValueStructure(head1?.Right, head2?.Right);
    }

    //用于测试
    // private static void PrintTree(Node head)
    // {
    //     Console.WriteLine("Binary Tree:");
    //     PrintInOrder(head, 0, "H", 17);
    //     Console.WriteLine();
    // }

    private static void PrintInOrder(Node? head, int height, string to, int len)
    {
        if (head == null) return;

        PrintInOrder(head.Right, height + 1, "v", len);
        var val = to + head.Value + to;
        var lenM = val.Length;
        var lenL = (len - lenM) / 2;
        var lenR = len - lenM - lenL;
        val = GetSpace(lenL) + val + GetSpace(lenR);
        Console.WriteLine(GetSpace(height * len) + val);
        PrintInOrder(head.Left, height + 1, "^", len);
    }

    private static string GetSpace(int num)
    {
        var space = " ";
        var buf = new StringBuilder("");
        for (var i = 0; i < num; i++) buf.Append(space);

        return buf.ToString();
    }

    public static void Run()
    {
        var maxLevel = 5;
        var maxValue = 100;
        var testTimes = 1000000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTimes; i++)
        {
            var head = GenerateRandomBst(maxLevel, maxValue);
            var pre = PreSerial(head);
            var pos = PosSerial(head);
            var level = LevelSerial(head);
            var preBuild = BuildByPreQueue(pre);
            var posBuild = BuildByPosQueue(pos);
            var levelBuild = BuildByLevelQueue(level);
            if (!IsSameValueStructure(preBuild, posBuild) || !IsSameValueStructure(posBuild, levelBuild))
                Console.WriteLine("出错啦！");
        }

        Console.WriteLine("test finish!");
    }

    /*
     * 二叉树可以通过先序、后序或者按层遍历的方式序列化和反序列化，
     * 以下代码全部实现了。
     * 但是，二叉树无法通过中序遍历的方式实现序列化和反序列化
     * 因为不同的两棵树，可能得到同样的中序序列，即便补了空位置也可能一样。
     * 比如如下两棵树
     *         __2
     *        /
     *       1
     *       和
     *       1__
     *          \
     *           2
     * 补足空位置的中序遍历结果都是{ null, 1, null, 2, null}
     *
     * */
    public class Node(int data)
    {
        public readonly int Value = data;
        public Node? Left;
        public Node? Right;
    }
}