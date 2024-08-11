//测试通过

namespace Algorithms.Lesson32;

public class Ac2
{
    public static void Run()
    {
        var ac = new AcAutomation();
        ac.Insert("dhe");
        ac.Insert("he");
        ac.Insert("abcdheks");
        // 设置fail指针
        ac.Build();

        var contains = ac.ContainWords("abcdhekskdjfafhasldkflskdjhwqaeruv");
        foreach (var word in contains) Console.WriteLine(word);
    }

    // 前缀树的节点
    public class Node
    {
        public readonly Node?[] NextArray;

        // 如果一个node，end为空，不是结尾
        // 如果end不为空，表示这个点是某个字符串的结尾，end的值就是这个字符串
        public string? End;

        // 只有在上面的end变量不为空的时候，endUse才有意义
        // 表示，这个字符串之前有没有加入过答案
        public bool EndUse;
        public Node? Fail;

        public Node()
        {
            EndUse = false;
            End = null;
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

            if (cur != null) cur.End = s;
        }

        public void Build()
        {
            Queue<Node> queue = new();
            queue.Enqueue(_root);
            while (queue.Count != 0)
            {
                // 当前节点弹出，
                // 当前节点的所有后代加入到队列里去，
                // 当前节点给它的子去设置fail指针
                // cur -> 父亲
                var cur = queue.Dequeue();
                for (var i = 0; i < 26; i++)
                    // 所有的路
                    if (cur.NextArray[i] != null)
                    {
                        // 找到所有有效的路
                        cur.NextArray[i]!.Fail = _root; //
                        var cFail = cur.Fail;
                        while (cFail != null)
                        {
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

        public List<string> ContainWords(string content)
        {
            var str = content.ToCharArray();
            var cur = _root;
            List<string> ans = new();
            foreach (var element in str)
            {
                var index = element - 'a';
                // 如果当前字符在这条路上没配出来，就随着fail方向走向下条路径
                while (cur?.NextArray[index] == null && cur != _root) cur = cur?.Fail;

                // 1) 现在来到的路径，是可以继续匹配的
                // 2) 现在来到的节点，就是前缀树的根节点
                cur = cur.NextArray[index] != null ? cur.NextArray[index] : _root;
                var follow = cur;
                while (follow != _root)
                {
                    if (follow!.EndUse) break;

                    // 不同的需求，在这一段之间修改
                    if (follow.End != null)
                    {
                        ans.Add(follow.End);
                        follow.EndUse = true;
                    }

                    // 不同的需求，在这一段之间修改
                    follow = follow.Fail;
                }
            }

            return ans;
        }
    }
}