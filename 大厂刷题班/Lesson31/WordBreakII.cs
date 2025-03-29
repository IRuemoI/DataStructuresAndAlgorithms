//pass

#region

using System.Text;

#endregion

namespace AdvancedTraining.Lesson31;

public class WordBreakIi //leetcode_0140
{
    private static IList<string> WordBreak(string s, IList<string> wordDict)
    {
        var str = s.ToCharArray();
        var root = Gettrie(wordDict);
        var dp = GetDp(s, root);
        var path = new List<string?>();
        IList<string> ans = new List<string>();
        Process(str, 0, root, dp, path, ans);
        return ans;
    }

    // str[index.....] 是要搞定的字符串
    // dp[0...N-1] 0... 1.... 2... N-1... 在dp里
    // root 单词表所有单词生成的前缀树头节点
    // path str[0..index-1]做过决定了，做的决定放在path里
    private static void Process(char[] str, int index, Node root, bool[] dp, List<string?> path, IList<string> ans)
    {
        if (index == str.Length)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < path.Count - 1; i++) builder.Append(path[i] + " ");
            builder.Append(path[^1]);
            ans.Add(builder.ToString());
        }
        else
        {
            var cur = root;
            for (var end = index; end < str.Length; end++)
            {
                // str[i..end] （能不能拆出来）
                var road = str[end] - 'a';
                if (cur != null && cur.NextList[road] == null) break;
                cur = cur?.NextList[road];
                if (cur is { End: true } && dp[end + 1])
                {
                    // [i...end] 前缀串
                    // str.subString(i,end+1)  [i..end]
                    path.Add(cur.Path);
                    Process(str, end + 1, root, dp, path, ans);
                    path.RemoveAt(path.Count - 1);
                }
            }
        }
    }

    private static Node Gettrie(IList<string> wordDict)
    {
        var root = new Node();
        foreach (var str in wordDict)
        {
            var chs = str.ToCharArray();
            var node = root;
            foreach (var item in chs)
            {
                var index = item - 'a';
                if (node != null && node.NextList[index] == null)
                    node.NextList[index] = new Node();
                node = node?.NextList[index];
            }

            node!.Path = str;
            node.End = true;
        }

        return root;
    }

    private static bool[] GetDp(string s, Node root)
    {
        var str = s.ToCharArray();
        var n = str.Length;
        var dp = new bool[n + 1];
        dp[n] = true;
        for (var i = n - 1; i >= 0; i--)
        {
            var cur = root;
            for (var end = i; end < n; end++)
            {
                var path = str[end] - 'a';
                if (cur != null && cur.NextList[path] == null) break;
                cur = cur?.NextList[path];
                if (cur is { End: true } && dp[end + 1])
                {
                    dp[i] = true;
                    break;
                }
            }
        }

        return dp;
    }

    public static void Run()
    {
        Console.WriteLine(string.Join(",", WordBreak("catsanddog", ["cat", "cats", "and", "sand", "dog"])));
    }

    private class Node
    {
        public readonly Node?[] NextList = new Node[26];
        public bool End;
        public string? Path;
    }
}