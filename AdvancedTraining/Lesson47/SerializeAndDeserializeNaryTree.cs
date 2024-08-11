#region

using System.Text;

#endregion

namespace AdvancedTraining.Lesson47;

//todo:待整理
public class SerializeAndDeserializeNaryTree //Problem_0428
{
    public static void Run()
    {
        // 如果想跑以下的code，请把Codec类描述和内部所有方法改成static的
        var a = new Node(1);
        var b = new Node(2);
        var c = new Node(3);
        var d = new Node(4);
        var e = new Node(5);
        var f = new Node(6);
        var g = new Node(7);
        a.children.Add(b);
        a.children.Add(c);
        a.children.Add(d);
        b.children.Add(e);
        b.children.Add(f);
        e.children.Add(g);
        var content = Codec.Serialize(a);
        Console.WriteLine(content);
        var head = Codec.Deserialize(content);
        Console.WriteLine(content.Equals(Codec.Serialize(head)));
    }

    // 不要提交这个类
    public class Node
    {
        public List<Node> children;
        public int val;

        public Node()
        {
            children = new List<Node>();
        }

        public Node(int _val)
        {
            val = _val;
            children = new List<Node>();
        }

        public Node(int _val, List<Node> _children)
        {
            val = _val;
            children = _children;
        }
    }

    // 提交下面这个类
    public class Codec
    {
        public static string Serialize(Node root)
        {
            if (root == null)
                // 空树！直接返回#
                return "#";
            var builder = new StringBuilder();
            Serial(builder, root);
            return builder.ToString();
        }

        // 当前来到head
        internal static void Serial(StringBuilder builder, Node head)
        {
            builder.Append(head.val + ",");
            if (head.children.Count > 0)
            {
                builder.Append("[,");
                foreach (var next in head.children) Serial(builder, next);
                builder.Append("],");
            }
        }

        public static Node Deserialize(string data)
        {
            if (data.Equals("#")) return null;
            var splits = data.Split(",");
            var queue = new LinkedList<string>();
            foreach (var str in splits) queue.AddLast(str);
            return Deserial(queue);
        }

        private static Node Deserial(LinkedList<string> queue)
        {
            var temp = queue.First();
            queue.RemoveFirst();
            var cur = new Node(Convert.ToInt32(temp));
            cur.children = new List<Node>();
            if (queue.Count > 0 && queue.First.Value.Equals("["))
            {
                queue.RemoveFirst();
                while (!queue.First.Value.Equals("]"))
                {
                    var child = Deserial(queue);
                    cur.children.Add(child);
                }

                queue.RemoveFirst();
            }

            return cur;
        }
    }
}