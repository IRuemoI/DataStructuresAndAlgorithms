namespace AdvancedTraining.Lesson14;

public class CompleteTreeNodeNumber
{
    // 请保证head为头的树，是完全二叉树
    private static int NodeNum(Node? head)
    {
        if (head == null) return 0;
        return Bs(head, 1, MostLeftLevel(head, 1));
    }

    // 当前来到node节点，node节点在level层，总层数是h
    // 返回node为头的子树(必是完全二叉树)，有多少个节点
    private static int Bs(Node? node, int level, int h)
    {
        if (level == h) return 1;
        if (node != null && MostLeftLevel(node.Right, level + 1) == h)
            return (1 << (h - level)) + Bs(node.Right, level + 1, h);
        return (1 << (h - level - 1)) + Bs(node?.Left, level + 1, h);
    }

    // 如果node在第level层，
    // 求以node为头的子树，最大深度是多少
    // node为头的子树，一定是完全二叉树
    private static int MostLeftLevel(Node? node, int level)
    {
        while (node != null)
        {
            level++;
            node = node.Left;
        }

        return level - 1;
    }

    public static void Run()
    {
        var head = new Node(1)
        {
            Left = new Node(2),
            Right = new Node(3)
        };
        head.Left.Left = new Node(4);
        head.Left.Right = new Node(5);
        head.Right.Left = new Node(6);
        Console.WriteLine(NodeNum(head));
    }

    public class Node(int data)
    {
        public readonly int Value = data;
        public Node? Left;
        public Node? Right;
    }
}