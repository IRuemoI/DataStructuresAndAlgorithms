namespace AdvancedTraining.Lesson03;

public class DistanceKNodes
{
    private static IList<Node?> Code(Node root, Node target, int k)
    {
        var parents = new Dictionary<Node, Node?>
        {
            [root] = null
        };
        CreateParentMap(root, parents);
        var queue = new LinkedList<Node?>();
        var visited = new HashSet<Node?>();
        queue.AddLast(target);
        visited.Add(target);
        var curLevel = 0;
        var ans = new List<Node?>();
        while (queue.Count > 0)
        {
            var size = queue.Count;
            while (size-- > 0)
            {
                var cur = queue.First();
                queue.RemoveFirst();
                if (curLevel == k) ans.Add(cur);
                if (cur != null)
                {
                    if (cur.Left != null && visited.Add(cur.Left)) queue.AddLast(cur.Left);

                    if (cur.Right != null && visited.Add(cur.Right)) queue.AddLast(cur.Right);

                    if (parents[cur] != null && visited.Add(parents[cur])) queue.AddLast(parents[cur]);
                }
            }

            curLevel++;
            if (curLevel > k) break;
        }

        return ans;
    }

    private static void CreateParentMap(Node? cur, Dictionary<Node, Node?> parents)
    {
        if (cur == null) return;
        if (cur.Left != null)
        {
            parents[cur.Left] = cur;
            CreateParentMap(cur.Left, parents);
        }

        if (cur.Right != null)
        {
            parents[cur.Right] = cur;
            CreateParentMap(cur.Right, parents);
        }
    }

    public static void Run()
    {
        var n0 = new Node(0);
        var n1 = new Node(1);
        var n2 = new Node(2);
        var n3 = new Node(3);
        var n4 = new Node(4);
        var n5 = new Node(5);
        var n6 = new Node(6);
        var n7 = new Node(7);
        var n8 = new Node(8);

        n3.Left = n5;
        n3.Right = n1;
        n5.Left = n6;
        n5.Right = n2;
        n1.Left = n0;
        n1.Right = n8;
        n2.Left = n7;
        n2.Right = n4;

        var root = n3;
        var target = n5;
        const int k = 2;

        var ans = Code(root, target, k);
        foreach (var o1 in ans) Console.WriteLine(o1?.Value);
    }

    public class Node(int v)
    {
        public readonly int Value = v;
        public Node? Left;
        public Node? Right;
    }
}