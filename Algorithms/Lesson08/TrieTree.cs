#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson08;

// 该程序完全正确
public class TrieTree
{
    //用于测试
    private static string GenerateRandomString(int strLen)
    {
        var ans = new char[(int)(Utility.GetRandomDouble * strLen) + 1];
        for (var i = 0; i < ans.Length; i++)
        {
            var value = (int)(Utility.GetRandomDouble * 26);
            ans[i] = (char)('a' + value);
        }

        return new string(ans);
    }

    //用于测试
    private static string[] GenerateRandomStringArray(int arrLen, int strLen)
    {
        var ans = new string[(int)(Utility.GetRandomDouble * arrLen) + 1];
        for (var i = 0; i < ans.Length; i++) ans[i] = GenerateRandomString(strLen);

        return ans;
    }

    public static void Run()
    {
        var arrLen = 100;
        var strLen = 20;
        var testTimes = 100000;

        for (var i = 0; i < testTimes; i++)
        {
            var arr = GenerateRandomStringArray(arrLen, strLen);
            var trie1 = new Trie1();
            var trie2 = new Trie2();
            var right = new Right();
            foreach (var item in arr)
            {
                var decide = Utility.GetRandomDouble;
                if (decide < 0.25)
                {
                    trie1.Insert(item);
                    trie2.Insert(item);
                    right.Insert(item);
                }
                else if (decide < 0.5)
                {
                    trie1.Delete(item);
                    trie2.Delete(item);
                    right.Delete(item);
                }
                else if (decide < 0.75)
                {
                    var ans1 = trie1.Search(item);
                    var ans2 = trie2.Search(item);
                    var ans3 = right.Search(item);
                    if (ans1 != ans2 || ans2 != ans3) Console.WriteLine("出错啦！");
                }
                else
                {
                    var ans1 = trie1.PrefixNumber(item);
                    var ans2 = trie2.PrefixNumber(item);
                    var ans3 = right.PrefixNumber(item);
                    if (ans1 != ans2 || ans2 != ans3) Console.WriteLine("出错啦！");
                }
            }
        }

        Console.WriteLine("finish!");
    }

    public class Node1
    {
        public readonly Node1?[] NextArray;
        public int End;
        public int Pass;

        // char tmp = 'b'  (tmp - 'a')
        public Node1()
        {
            Pass = 0;
            End = 0;
            // 0    a
            // 1    b
            // 2    c
            // ..   ..
            // 25   z
            // nexts[i] == null   i方向的路不存在
            // nexts[i] != null   i方向的路存在
            NextArray = new Node1[26];
        }
    }

    private class Trie1
    {
        private readonly Node1 _root;

        public Trie1()
        {
            _root = new Node1();
        }

        public void Insert(string? word)
        {
            if (word == null) return;

            var str = word.ToCharArray();
            var node = _root;
            node.Pass++;
            foreach (var character in str)
            {
                // 从左往右遍历字符
                var path = character - 'a';
                node.NextArray[path] ??= new Node1();

                node = node.NextArray[path];
                node!.Pass++;
            }

            node.End++;
        }

        public void Delete(string word)
        {
            if (Search(word) != 0)
            {
                var chs = word.ToCharArray();
                var node = _root;
                node.Pass--;
                foreach (var character in chs)
                {
                    var path = character - 'a';
                    if (--node!.NextArray[path]!.Pass == 0)
                    {
                        node.NextArray[path] = null;
                        return;
                    }

                    node = node.NextArray[path];
                }

                node!.End--;
            }
        }

        // word这个单词之前加入过几次
        public int Search(string? word)
        {
            if (word == null) return 0;

            var chs = word.ToCharArray();
            var node = _root;
            foreach (var character in chs)
            {
                var index = character - 'a';
                if (node?.NextArray[index] == null) return 0;
                node = node.NextArray[index];
            }

            return node!.End;
        }

        // 所有加入的字符串中，有几个是以pre这个字符串作为前缀的
        public int PrefixNumber(string? pre)
        {
            if (pre == null) return 0;

            var chs = pre.ToCharArray();
            var node = _root;
            foreach (var character in chs)
            {
                var index = character - 'a';
                if (node?.NextArray[index] == null) return 0;

                node = node.NextArray[index];
            }

            return node!.Pass;
        }
    }

    public class Node2
    {
        public readonly Dictionary<int, Node2> NextDict;
        public int End;
        public int Pass;

        public Node2()
        {
            Pass = 0;
            End = 0;
            NextDict = new Dictionary<int, Node2>();
        }
    }

    private class Trie2
    {
        private readonly Node2 _root;

        public Trie2()
        {
            _root = new Node2();
        }

        public void Insert(string? word)
        {
            if (word == null) return;

            var chs = word.ToCharArray();
            var node = _root;
            node.Pass++;
            foreach (int index in chs)
            {
                if (!node.NextDict.ContainsKey(index)) node.NextDict.Add(index, new Node2());

                node = node.NextDict[index];
                node.Pass++;
            }

            node.End++;
        }

        public void Delete(string word)
        {
            if (Search(word) != 0)
            {
                var chs = word.ToCharArray();
                var node = _root;
                node.Pass--;
                foreach (int index in chs)
                {
                    if (--node.NextDict[index].Pass == 0)
                    {
                        node.NextDict.Remove(index);
                        return;
                    }

                    node = node.NextDict[index];
                }

                node.End--;
            }
        }

        // word这个单词之前加入过几次
        public int Search(string? word)
        {
            if (word == null) return 0;

            var chs = word.ToCharArray();
            var node = _root;
            foreach (int index in chs)
            {
                if (!node.NextDict.ContainsKey(index)) return 0;

                node = node.NextDict[index];
            }

            return node.End;
        }

        // 所有加入的字符串中，有几个是以pre这个字符串作为前缀的
        public int PrefixNumber(string? pre)
        {
            if (pre == null) return 0;

            var chs = pre.ToCharArray();
            var node = _root;
            foreach (int index in chs)
            {
                if (!node.NextDict.ContainsKey(index)) return 0;

                node = node.NextDict[index];
            }

            return node.Pass;
        }
    }

    private class Right
    {
        private readonly Dictionary<string, int> _box;

        public Right()
        {
            _box = new Dictionary<string, int>();
        }

        public void Insert(string word)
        {
            if (!_box.ContainsKey(word))
                _box.Add(word, 1);
            else
                _box[word] += 1;
        }

        public void Delete(string word)
        {
            if (_box.ContainsKey(word))
            {
                if (_box[word] == 1)
                    _box.Remove(word);
                else
                    _box[word] -= 1;
            }
        }

        public int Search(string word)
        {
            if (!_box.ContainsKey(word))
                return 0;
            return _box[word];
        }

        public int PrefixNumber(string pre)
        {
            var count = 0;
            foreach (var cur in _box.Keys)
                if (cur.StartsWith(pre))
                    count += _box[cur];

            return count;
        }
    }
}