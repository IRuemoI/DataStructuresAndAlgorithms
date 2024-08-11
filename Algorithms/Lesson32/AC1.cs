//测试通过

namespace Algorithms.Lesson32;

public class Ac1
{
    public static void Run()
    {
        var ac = new AcAutomation();
        ac.Insert("dhe");
        ac.Insert("he");
        ac.Insert("c");
        ac.Build();
        Console.WriteLine(ac.ContainNum("cdhe"));
    }

    private class Node
    {
        public readonly Node?[] NextArray;
        public int End; // 有多少个字符串以该节点结尾
        public Node? Fail;

        public Node()
        {
            End = 0;
            Fail = null;
            NextArray = new Node[26];
        }
    }

    private class AcAutomation
    {
        private readonly Node _root;

        public AcAutomation()
        {
            _root = new Node();
        }

        // 你有多少个匹配串，就调用多少次insert
        public void Insert(string s)
        {
            var str = s.ToCharArray();
            var cur = _root;
            foreach (var element in str)
            {
                var index = element - 'a';
                if (cur?.NextArray[index] == null)
                {
                    var next = new Node();
                    if (cur != null) cur.NextArray[index] = next;
                }

                cur = cur?.NextArray[index];
            }

            if (cur != null) cur.End++;
        }

        public void Build()
        {
            Queue<Node> queue = new();
            queue.Enqueue(_root);
            while (queue.Count != 0)
            {
                var cur = queue.Dequeue();
                for (var i = 0; i < 26; i++)
                    // 下级所有的路
                    if (cur.NextArray[i] != null)
                    {
                        // 该路下有子节点
                        cur.NextArray[i]!.Fail = _root; // 初始时先设置一个值
                        var cFail = cur.Fail;
                        while (cFail != null)
                        {
                            // cur不是头节点
                            if (cFail.NextArray[i] != null)
                            {
                                cur.NextArray[i]!.Fail = cFail.NextArray[i];
                                break;
                            }

                            cFail = cFail.Fail;
                        }

                        queue.Enqueue(cur.NextArray[i]!);
                    }
            }
        }

        public int ContainNum(string content)
        {
            var str = content.ToCharArray();
            var cur = _root;
            var ans = 0;
            foreach (var element in str)
            {
                var index = element - 'a';
                while (cur?.NextArray[index] == null && cur != _root) cur = cur?.Fail;

                cur = cur.NextArray[index] != null ? cur.NextArray[index] : _root;
                var follow = cur;
                while (follow != _root)
                {
                    if (follow?.End == -1) break;

                    {
                        // 不同的需求，在这一段{ }之间修改
                        if (follow != null)
                        {
                            ans += follow.End;
                            follow.End = -1;
                        }
                    } // 不同的需求，在这一段{ }之间修改
                    follow = follow?.Fail;
                }
            }

            return ans;
        }
    }
}