//pass
namespace AdvancedTraining.Lesson07;

public class SplitStringMaxValue
{
    // 暴力解
    private static int MaxRecord1(string str, int k, string[] parts, int[] record)
    {
        if (ReferenceEquals(str, null) || str.Length == 0) return 0;
        var records = new Dictionary<string, int>();
        for (var i = 0; i < parts.Length; i++) records[parts[i]] = record[i];
        return Process(str, 0, k, records);
    }

    private static int Process(string str, int index, int rest, Dictionary<string, int> records)
    {
        if (rest < 0) return -1;
        if (index == str.Length) return rest == 0 ? 0 : -1;
        var ans = -1;
        for (var end = index; end < str.Length; end++)
        {
            var first = str.Substring(index, end + 1 - index);
            var next = records.ContainsKey(first) ? Process(str, end + 1, rest - 1, records) : -1;
            if (next != -1) ans = Math.Max(ans, records[first] + next);
        }

        return ans;
    }

    // 动态规划解
    private static int MaxRecord2(string str, int k, string[] parts, int[] record)
    {
        if (ReferenceEquals(str, null) || str.Length == 0) return 0;
        var records = new Dictionary<string, int>();
        for (var i = 0; i < parts.Length; i++) records[parts[i]] = record[i];
        var n = str.Length;
        var dp = new int [n + 1, k + 1];
        for (var rest = 1; rest <= k; rest++) dp[n, rest] = -1;
        for (var index = n - 1; index >= 0; index--)
        for (var rest = 0; rest <= k; rest++)
        {
            var ans = -1;
            for (var end = index; end < n; end++)
            {
                var first = str.Substring(index, end + 1 - index);
                var next = rest > 0 && records.ContainsKey(first) ? dp[end + 1, rest - 1] : -1;
                if (next != -1) ans = Math.Max(ans, records[first] + next);
            }

            dp[index, rest] = ans;
        }

        return dp[0, k];
    }

    // 动态规划解 + 前缀树优化
    private static int MaxRecord3(string s, int k, string[] parts, int[] record)
    {
        if (ReferenceEquals(s, null) || s.Length == 0) return 0;
        var root = RootNode(parts, record);
        var str = s.ToCharArray();
        var n = str.Length;
        var dp = new int[n + 1, k + 1];
        for (var rest = 1; rest <= k; rest++) dp[n, rest] = -1;
        for (var index = n - 1; index >= 0; index--)
        for (var rest = 0; rest <= k; rest++)
        {
            var ans = -1;
            var cur = root;
            for (var end = index; end < n; end++)
            {
                var path = str[end] - 'a';
                if (cur?.NextList[path] == null) break;
                cur = cur.NextList[path];
                var next = rest > 0 && cur?.Value != -1 ? dp[end + 1, rest - 1] : -1;
                if (next != -1 && cur != null) ans = Math.Max(ans, cur.Value + next);
            }

            dp[index, rest] = ans;
        }

        return dp[0, k];
    }

    private static TrieNode RootNode(string[] parts, int[] record)
    {
        var root = new TrieNode();
        for (var i = 0; i < parts.Length; i++)
        {
            var str = parts[i].ToCharArray();
            var cur = root;
            foreach (var item in str)
            {
                var path = item - 'a';
                if (cur != null && cur.NextList[path] == null) cur.NextList[path] = new TrieNode();
                cur = cur?.NextList[path];
            }

            if (cur != null) cur.Value = record[i];
        }

        return root;
    }

    public static void Run()
    {
        const string str = "abcdefg";
        const int k = 3;
        string[] parts = ["abc", "def", "g", "ab", "cd", "efg", "defg"];
        int[] record = [1, 1, 1, 3, 3, 3, 2];
        Console.WriteLine(MaxRecord1(str, k, parts, record));
        Console.WriteLine(MaxRecord2(str, k, parts, record));
        Console.WriteLine(MaxRecord3(str, k, parts, record));
    }

    public class TrieNode
    {
        public readonly TrieNode?[] NextList = new TrieNode[26];
        public int Value = -1;
    }
}