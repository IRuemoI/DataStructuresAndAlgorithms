//pass
namespace AdvancedTraining.Lesson31;

// lintcode也有测试，数据量比leetcode大很多 : https://www.lintcode.com/problem/107/
public class WordBreak //leetcode_0139
{
    private static bool WordBreak1(string s, IList<string> wordDict)
    {
        var root = new Node();
        foreach (var str in wordDict)
        {
            var chs = str.ToCharArray();
            var node = root;
            foreach (var item in chs)
            {
                var index = item - 'a';
                if (node != null && node.NextList[index] == null) node.NextList[index] = new Node();
                node = node?.NextList[index];
            }

            node!.End = true;
        }

        var str0 = s.ToCharArray();
        var n = str0.Length;
        var dp = new bool[n + 1];
        dp[n] = true; // dp[i]  word[i.....] 能不能被分解
        // dp[N] word[N...]  -> ""  能不能够被分解 
        // dp[i] ... dp[i+1....]
        for (var i = n - 1; i >= 0; i--)
        {
            // i
            // word[i....] 能不能够被分解
            // i..i    i+1....
            // i..i+1  i+2...
            var cur = root;
            for (var end = i; end < n; end++)
            {
                cur = cur.NextList[str0[end] - 'a'];
                if (cur == null) break;
                // 有路！
                if (cur.End)
                    // i...end 真的是一个有效的前缀串  end+1....  能不能被分解
                    dp[i] |= dp[end + 1];
                if (dp[i]) break;
            }
        }

        return dp[0];
    }

    private static int WordBreak2(string s, IList<string> wordDict)
    {
        var root = new Node();
        foreach (var str1 in wordDict)
        {
            var chs = str1.ToCharArray();
            var node = root;
            foreach (var item in chs)
            {
                var index = item - 'a';
                if (node != null && node.NextList[index] == null) node.NextList[index] = new Node();
                node = node?.NextList[index];
            }

            node!.End = true;
        }

        var str = s.ToCharArray();
        var n = str.Length;
        var dp = new int[n + 1];
        dp[n] = 1;
        for (var i = n - 1; i >= 0; i--)
        {
            var cur = root;
            for (var end = i; end < n; end++)
            {
                cur = cur.NextList[str[end] - 'a'];
                if (cur == null) break;
                if (cur.End) dp[i] += dp[end + 1];
            }
        }

        return dp[0];
    }

    public static void Run()
    {
        var s = "applepenapple";
        string[] wordDict = ["apple", "pen"];
        Console.WriteLine(WordBreak2(s, wordDict.ToList()));
    }

    private class Node
    {
        public readonly Node?[] NextList = new Node[26];
        public bool End;
    }
}