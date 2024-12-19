#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson08;

public class PrefixTree
{
    # region 用于测试

    private static string GenerateRandomString(int strLen)
    {
        var ans = new char[(int)(Utility.getRandomDouble * strLen) + 1];
        for (var i = 0; i < ans.Length; i++)
        {
            var value = (int)(Utility.getRandomDouble * 26);
            ans[i] = (char)('a' + value);
        }

        return new string(ans);
    }

    private static string[] GenerateRandomStringArray(int arrLen, int strLen)
    {
        var ans = new string[(int)(Utility.getRandomDouble * arrLen) + 1];
        for (var i = 0; i < ans.Length; i++) ans[i] = GenerateRandomString(strLen);

        return ans;
    }

    private class Right
    {
        private readonly Dictionary<string, int> _box = new();

        public void Insert(string word)
        {
            if (!_box.TryAdd(word, 1))
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
            return _box.GetValueOrDefault(word, 0);
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

    #endregion

    public static void Run()
    {
        var arrLen = 100;
        var strLen = 20;
        var testTimes = 1000;

        for (var i = 0; i < testTimes; i++)
        {
            var arr = GenerateRandomStringArray(arrLen, strLen);
            var trie1 = new ArrayPrefixTree();
            var trie2 = new DictionaryPrefixTree();
            var right = new Right();
            foreach (var item in arr)
            {
                var decide = Utility.getRandomDouble;
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

        Console.WriteLine("测试完成");
    }

    //适用于只有大写、小写、或者数字字符的情况
    public class ArrayNode
    {
        public readonly ArrayNode?[] NextCharArray = new ArrayNode[26]; //存储下一个字符节点的地址
        public int End; //存储以当前字符结尾的单词个数
        public int Pass; //存储经过当前节点的单词个数
    }

    private class ArrayPrefixTree
    {
        private readonly ArrayNode _root = new();

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
                node.NextCharArray[path] ??= new ArrayNode();

                node = node.NextCharArray[path];
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
                    if (--node!.NextCharArray[path]!.Pass == 0)
                    {
                        node.NextCharArray[path] = null;
                        return;
                        //注意：对于没有GC的语言比如C++可以将需要销毁的节点放入栈中然后依次销毁
                    }

                    node = node.NextCharArray[path];
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
                if (node?.NextCharArray[index] == null) return 0;
                node = node.NextCharArray[index];
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
                if (node?.NextCharArray[index] == null) return 0;

                node = node.NextCharArray[index];
            }

            return node!.Pass;
        }
    }

    //更泛用 适用于各种类型的字符的情况
    public class DictionaryNode
    {
        //存储下一个字符节点的地址的字典(可以将字符转换为unicode或者utf-8编码作为字典的key)
        public readonly Dictionary<int, DictionaryNode> NextCharDict = new();
        public int End; //存储以当前字符结尾的单词个数
        public int Pass; //存储经过当前节点的单词个数
    }
    
    private class DictionaryPrefixTree
    {
        private readonly DictionaryNode _root = new(); //根节点
    
        //将新的单词加入前缀树中
        public void Insert(string? word)
        {
            if (word == null) return;
    
            var chs = word.ToCharArray();
            var node = _root;
            node.Pass++;
            foreach (int index in chs)
            {
                if (!node.NextCharDict.ContainsKey(index)) node.NextCharDict.Add(index, new DictionaryNode());
    
                node = node.NextCharDict[index];
                node.Pass++;
            }
    
            node.End++;
        }
    
        public void Delete(string word)
        {
            if (Search(word) != 0)
            {
                var chars = word.ToCharArray();
                var node = _root;
                node.Pass--;
                foreach (int index in chars)
                {
                    if (--node.NextCharDict[index].Pass == 0)
                    {
                        node.NextCharDict.Remove(index);
                        return;
                        //注意：对于没有GC的语言比如C++可以将需要销毁的节点放入栈中然后依次销毁
                    }
    
                    node = node.NextCharDict[index];
                }
    
                node.End--;
            }
        }
    
        // 单词word在前缀树中出现的次数 
        public int Search(string? word)
        {
            if (word == null) return 0;
    
            var chars = word.ToCharArray();
            var node = _root;
            foreach (int index in chars)
            {
                if (!node.NextCharDict.TryGetValue(index, out var value)) return 0;
                node = value;
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
                if (!node.NextCharDict.TryGetValue(index, out var value)) return 0;
    
                node = value;
            }
    
            return node.Pass;
        }
    }
}