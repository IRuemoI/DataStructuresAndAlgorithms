//测试通过

#region

using System.Text;
using Common.DataStructures.Heap;
using Common.Utilities;

#endregion

namespace Algorithms.Lesson46;

// 本文件不牵扯任何byte类型的转化
// 怎么转byte自己来，我只负责huffman算法本身的正确实现
// 字符串为空的时候，自己处理边界吧
// 实现的代码通过了大样本随机测试的对数器
// 可以从main函数的内容开始看起
public class HuffmanTree
{
    // 根据文章str, 生成词频统计表
    private static Dictionary<char, int> CountMap(string str)
    {
        var ans = new Dictionary<char, int>();
        var s = str.ToCharArray();
        foreach (var cha in s)
            if (!ans.ContainsKey(cha))
                ans[cha] = 1;
            else
                ans[cha] = ans[cha] + 1;

        return ans;
    }

    // 根据由文章生成词频表countMap，生成哈夫曼编码表
    // key : 字符
    // value: 该字符编码后的二进制形式
    // 比如，频率表 A：60, B:45, C:13 D:69 E:14 F:5 G:3
    // A 10
    // B 01
    // C 0011
    // D 11
    // E 000
    // F 00101
    // G 00100
    private static Dictionary<char, string> HuffmanForm(Dictionary<char, int> countMap)
    {
        var ans = new Dictionary<char, string>();
        if (countMap.Count == 1)
        {
            foreach (var key in countMap.Keys) ans[key] = "0";

            return ans;
        }

        var nodes = new Dictionary<Node, char>();
        var minHeap = new Heap<Node>((x, y) => x.Count - y.Count);
        foreach (var entry in countMap)
        {
            var cur = new Node(entry.Value);
            var cha = entry.Key;
            nodes[cur] = cha;
            minHeap.Push(cur);
        }

        while (minHeap.Count != 1)
        {
            var a = minHeap.Pop();
            var b = minHeap.Pop();
            var h = new Node(a.Count + b.Count)
            {
                Left = a,
                Right = b
            };
            minHeap.Push(h);
        }

        var head = minHeap.Pop();
        FillForm(head, "", nodes, ans);
        return ans;
    }

    private static void FillForm(Node head, string pre, Dictionary<Node, char> nodes, Dictionary<char, string> ans)
    {
        if (nodes.TryGetValue(head, out var node))
        {
            ans[node] = pre;
        }
        else
        {
            FillForm(head.Left ?? throw new InvalidOperationException(), pre + "0", nodes, ans);
            FillForm(head.Right ?? throw new InvalidOperationException(), pre + "1", nodes, ans);
        }
    }

    // 原始字符串str，根据哈夫曼编码表，转译成哈夫曼编码返回
    private static string HuffmanEncode(string str, Dictionary<char, string> huffmanForm)
    {
        var s = str.ToCharArray();
        var builder = new StringBuilder();
        foreach (var cha in s) builder.Append(huffmanForm[cha]);

        return builder.ToString();
    }

    // 原始字符串的哈夫曼编码huffmanEncode，根据哈夫曼编码表，还原成原始字符串
    private static string HuffmanDecode(string huffmanEncode, Dictionary<char, string> huffmanForm)
    {
        var root = CreateTrie(huffmanForm);
        var cur = root;
        var encode = huffmanEncode.ToCharArray();
        var builder = new StringBuilder();
        foreach (var element in encode)
        {
            var index = element == '0' ? 0 : 1;
            if (cur.NextArray != null)
            {
                cur = cur.NextArray[index];
                if (cur?.NextArray[0] == null && cur?.NextArray[1] == null)
                {
                    builder.Append(cur?.Value);
                    cur = root;
                }
            }
        }

        return builder.ToString();
    }

    private static TrieNode CreateTrie(Dictionary<char, string> huffmanForm)
    {
        var root = new TrieNode();
        foreach (var key in huffmanForm.Keys)
        {
            var path = huffmanForm[key].ToCharArray();
            var cur = root;
            foreach (var element in path)
            {
                var index = element == '0' ? 0 : 1;
                if (cur?.NextArray[index] == null)
                    if (cur != null)
                        cur.NextArray[index] = new TrieNode();

                cur = cur?.NextArray[index];
            }

            if (cur != null) cur.Value = key;
        }

        return root;
    }

    // 为了测试
    private static string RandomNumberString(int len, int range)
    {
        var str = new char[len];
        for (var i = 0; i < len; i++) str[i] = (char)((int)(Utility.GetRandomDouble * range) + 'a');

        return new string(str);
    }

    // 为了测试
    public static void Run()
    {
        // 根据词频表生成哈夫曼编码表
        var map = new Dictionary<char, int>
        {
            ['A'] = 60,
            ['B'] = 45,
            ['C'] = 13,
            ['D'] = 69,
            ['E'] = 14,
            ['F'] = 5,
            ['G'] = 3
        };
        var huffmanForm = HuffmanForm(map);
        foreach (var entry in huffmanForm) Console.WriteLine(entry.Key + " : " + entry.Value);

        Console.WriteLine("====================");
        // str是原始字符串
        var str = "CBBBAABBACAABDDEFBA";
        Console.WriteLine(str);
        // countMap是根据str建立的词频表
        var countMap = CountMap(str);
        // hf是根据countMap生成的哈夫曼编码表
        var hf = HuffmanForm(countMap);
        // huffmanEncode是原始字符串转译后的哈夫曼编码
        var huffmanEncode = HuffmanEncode(str, hf);
        Console.WriteLine(huffmanEncode);
        // huffmanDecode是哈夫曼编码还原成的原始字符串
        var huffmanDecode = HuffmanDecode(huffmanEncode, hf);
        Console.WriteLine(huffmanDecode);
        Console.WriteLine("====================");
        Console.WriteLine("大样本随机测试开始");
        // 字符串最大长度
        var len = 500;
        // 所含字符种类
        var range = 26;
        // 随机测试进行的次数
        var testTime = 100000;
        for (var i = 0; i < testTime; i++)
        {
            var n = (int)(Utility.GetRandomDouble * len) + 1;
            var test = RandomNumberString(n, range);
            var counts = CountMap(test);
            var form = HuffmanForm(counts);
            var encode = HuffmanEncode(test, form);
            var decode = HuffmanDecode(encode, form);
            if (!test.Equals(decode))
            {
                Console.WriteLine(test);
                Console.WriteLine(encode);
                Console.WriteLine(decode);
                Console.WriteLine("出错了!");
            }
        }

        Console.WriteLine("大样本随机测试结束");
    }

    public class Node
    {
        public readonly int Count;
        public Node? Left;
        public Node? Right;

        public Node(int c)
        {
            Count = c;
        }
    }

    public class TrieNode
    {
        public readonly TrieNode?[] NextArray;
        public char Value;

        public TrieNode()
        {
            Value = (char)0;
            NextArray = new TrieNode[2];
        }
    }
}