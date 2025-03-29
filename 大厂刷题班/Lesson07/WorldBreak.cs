//pass

#region

using System.Text;
using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson07;

public class WorldBreak
{
    /*
     *
     * 假设所有字符都是小写字母. 大字符串是str. arr是去重的单词表, 每个单词都不是空字符串且可以使用任意次.
     * 使用arr中的单词有多少种拼接str的方式. 返回方法数.
     *
     */

    private static int Ways(string str, string[] arr)
    {
        var set = new HashSet<string>();
        foreach (var candidate in arr) set.Add(candidate);
        return Process(str, 0, set);
    }

    // 所有的可分解字符串，都已经放在了set中
    // str[i....] 能够被set中的贴纸分解的话，返回分解的方法数
    private static int Process(string str, int i, HashSet<string> set)
    {
        if (i == str.Length)
            // 没字符串需要分解了！
            return 1;
        //  i....还有字符串需要分解
        var ways = 0;
        // [i ... end] 前缀串 每一个前缀串
        for (var end = i; end < str.Length; end++)
        {
            var pre = str.Substring(i, end + 1 - i); // [)
            if (set.Contains(pre)) ways += Process(str, end + 1, set);
        }

        return ways;
    }

    private static int Ways1(string str, string[]? arr)
    {
        if (ReferenceEquals(str, null) || str.Length == 0 || arr == null || arr.Length == 0) return 0;
        var map = new HashSet<string>();
        foreach (var s in arr) map.Add(s);
        return F(str, map, 0);
    }

    private static int F(string str, HashSet<string> map, int index)
    {
        if (index == str.Length) return 1;
        var ways = 0;
        for (var end = index; end < str.Length; end++)
            if (map.Contains(str.Substring(index, end + 1 - index)))
                ways += F(str, map, end + 1);
        return ways;
    }

    private static int Ways2(string str, string[]? arr)
    {
        if (ReferenceEquals(str, null) || str.Length == 0 || arr == null || arr.Length == 0) return 0;
        var map = new HashSet<string>();
        foreach (var s in arr) map.Add(s);
        var n = str.Length;
        var dp = new int[n + 1];
        dp[n] = 1;
        for (var i = n - 1; i >= 0; i--)
        for (var end = i; end < n; end++)
            if (map.Contains(str.Substring(i, end + 1 - i)))
                dp[i] += dp[end + 1];
        return dp[0];
    }

    private static int Ways3(string str, string[]? arr)
    {
        if (ReferenceEquals(str, null) || str.Length == 0 || arr == null || arr.Length == 0) return 0;
        var root = new Node();
        foreach (var s in arr)
        {
            var chs = s.ToCharArray();
            var node = root;
            foreach (var item in chs)
            {
                var index = item - 'a';
                if (node != null && node.NextList[index] == null) node.NextList[index] = new Node();
                node = node?.NextList[index];
            }

            if (node != null) node.End = true;
        }

        return G(str.ToCharArray(), root, 0);
    }

    // str[i...] 被分解的方法数，返回

    private static int G(char[] str, Node root, int i)
    {
        if (i == str.Length) return 1;
        var ways = 0;
        var cur = root;
        // i...end
        for (var end = i; end < str.Length; end++)
        {
            var path = str[end] - 'a';
            if (cur?.NextList[path] == null) break;
            cur = cur.NextList[path];
            if (cur is { End: true })
                // i...end
                ways += G(str, root, end + 1);
        }

        return ways;
    }

    private static int Ways4(string s, string[]? arr)
    {
        if (ReferenceEquals(s, null) || s.Length == 0 || arr == null || arr.Length == 0) return 0;
        var root = new Node();
        foreach (var str in arr)
        {
            var chs = str.ToCharArray();
            var node = root;
            foreach (var item in chs)
            {
                var index = item - 'a';
                if (node != null && node.NextList[index] == null) node.NextList[index] = new Node();
                node = node?.NextList[index];
            }

            if (node != null) node.End = true;
        }

        var str1 = s.ToCharArray();
        var n = str1.Length;
        var dp = new int[n + 1];
        dp[n] = 1;
        for (var i = n - 1; i >= 0; i--)
        {
            var cur = root;
            for (var end = i; end < n; end++)
            {
                var path = str1[end] - 'a';
                if (cur?.NextList[path] == null) break;
                cur = cur.NextList[path];
                if (cur is { End: true }) dp[i] += dp[end + 1];
            }
        }

        return dp[0];
    }

    // 随机样本产生器
    private static RandomSample GenerateRandomSample(char[] candidates, int num, int len, int joint)
    {
        var seeds = randomSeeds(candidates, num, len);
        var set = new HashSet<string>();
        foreach (var str in seeds) set.Add(str);
        var arr = new string[set.Count];
        var index = 0;
        foreach (var str in set) arr[index++] = str;
        var all = new StringBuilder();
        for (var i = 0; i < joint; i++) all.Append(arr[(int)(Utility.getRandomDouble * arr.Length)]);
        return new RandomSample(all.ToString(), arr);
    }

    private static string[] randomSeeds(char[] candidates, int num, int len)
    {
        var arr = new string[(int)(Utility.getRandomDouble * num) + 1];
        for (var i = 0; i < arr.Length; i++)
        {
            var str = new char[(int)(Utility.getRandomDouble * len) + 1];
            for (var j = 0; j < str.Length; j++)
                str[j] = candidates[(int)(Utility.getRandomDouble * candidates.Length)];
            arr[i] = new string(str);
        }

        return arr;
    }

    public static void Run()
    {
        char[] candidates = ['a', 'b'];
        var num = 20;
        var len = 4;
        var joint = 5;
        var testTimes = 30000;
        var testResult = true;
        for (var i = 0; i < testTimes; i++)
        {
            var sample = GenerateRandomSample(candidates, num, len, joint);
            var ans1 = Ways1(sample.Str, sample.Arr);
            var ans2 = Ways2(sample.Str, sample.Arr);
            var ans3 = Ways3(sample.Str, sample.Arr);
            var ans4 = Ways4(sample.Str, sample.Arr);
            if (ans1 != ans2 || ans3 != ans4 || ans2 != ans4) testResult = false;
        }

        Console.WriteLine(testTimes + "次随机测试是否通过：" + testResult);
    }

    private class Node
    {
        public readonly Node?[] NextList = new Node[26];
        public bool End;
    }

    // 以下的逻辑都是为了测试
    private class RandomSample(string s, string[] a)
    {
        public readonly string[] Arr = a;
        public readonly string Str = s;
    }
}