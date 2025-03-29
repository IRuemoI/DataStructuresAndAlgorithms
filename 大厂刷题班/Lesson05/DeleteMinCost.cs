//pass

#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson05;

public class DeleteMinCost
{
    // 题目：
    // 给定两个字符串s1和s2，问s2最少删除多少字符可以成为s1的子串？
    // 比如 s1 = "abcde"，s2 = "axbc"
    // 返回 1

    // 解法一
    // 求出str2所有的子序列，然后按照长度排序，长度大的排在前面。
    // 然后考察哪个子序列字符串和s1的某个子串相等(KMP)，答案就出来了。
    // 分析：
    // 因为题目原本的样本数据中，有特别说明s2的长度很小。所以这么做也没有太大问题，也几乎不会超时。
    // 但是如果某一次考试给定的s2长度远大于s1，这么做就不合适了。
    private static int MinCost1(string s1, string s2)
    {
        var s2Subs = new List<string>();
        Process(s2.ToCharArray(), 0, "", s2Subs);
        s2Subs.Sort((x, y) => y.Length - x.Length);
        foreach (var str in s2Subs)
            if (s1.IndexOf(str, StringComparison.Ordinal) != -1)
                // indexOf底层和KMP算法代价几乎一样，也可以用KMP代替
                return s2.Length - str.Length;

        return s2.Length;
    }

    private static void Process(char[] str2, int index, string path, IList<string> list)
    {
        if (index == str2.Length)
        {
            list.Add(path);
            return;
        }

        Process(str2, index + 1, path, list);
        Process(str2, index + 1, path + str2[index], list);
    }

    // x字符串只通过删除的方式，变到y字符串
    // 返回至少要删几个字符
    // 如果变不成，返回Integer.Max
    private static int OnlyDelete(char[] x, char[] y)
    {
        if (x.Length < y.Length) return int.MaxValue;

        var n = x.Length;
        var m = y.Length;
        var dp = new int [n + 1, m + 1];
        for (var i = 0; i <= n; i++)
        for (var j = 0; j <= m; j++)
            dp[i, j] = int.MaxValue;

        dp[0, 0] = 0;
        // dp[i,j]表示前缀长度
        for (var i = 1; i <= n; i++) dp[i, 0] = i;

        for (var xlen = 1; xlen <= n; xlen++)
        for (var ylen = 1; ylen <= Math.Min(m, xlen); ylen++)
        {
            if (dp[xlen - 1, ylen] != int.MaxValue) dp[xlen, ylen] = dp[xlen - 1, ylen] + 1;

            if (x[xlen - 1] == y[ylen - 1] && dp[xlen - 1, ylen - 1] != int.MaxValue)
                dp[xlen, ylen] = Math.Min(dp[xlen, ylen], dp[xlen - 1, ylen - 1]);
        }

        return dp[n, m];
    }

    // 解法二
    // 生成所有s1的子串
    // 然后考察每个子串和s2的编辑距离(假设编辑距离只有删除动作且删除一个字符的代价为1)
    // 如果s1的长度较小，s2长度较大，这个方法比较合适
    private static int MinCost2(string s1, string s2)
    {
        if (s1.Length == 0 || s2.Length == 0) return s2.Length;

        var ans = int.MaxValue;
        var str2 = s2.ToCharArray();
        for (var start = 0; start < s1.Length; start++)
        for (var end = start + 1; end <= s1.Length; end++)
            // str1[start....end]
            // substring -> [ 0,1 )
            ans = Math.Min(ans, Distance(str2, s1.Substring(start, end - start).ToCharArray()));

        return ans == int.MaxValue ? s2.Length : ans;
    }

    // 求str2到s1sub的编辑距离
    // 假设编辑距离只有删除动作且删除一个字符的代价为1
    private static int Distance(char[] str2, char[] str1Sub)
    {
        var row = str2.Length;
        var col = str1Sub.Length;
        var dp = new int[row, col];
        // dp[i,j]的含义：
        // str2[0..i]仅通过删除行为变成s1sub[0..j]的最小代价
        // 可能性一：
        // str2[0..i]变的过程中，不保留最后一个字符(str2[i])，
        // 那么就是通过str2[0..i-1]变成s1sub[0..j]之后，再最后删掉str2[i]即可 -> dp[i,j] = dp[i-1,j] + 1
        // 可能性二：
        // str2[0..i]变的过程中，想保留最后一个字符(str2[i])，然后变成s1sub[0..j]，
        // 这要求str2[i] == s1sub[j]才有这种可能, 然后str2[0..i-1]变成s1sub[0..j-1]即可
        // 也就是str2[i] == s1sub[j] 的条件下，dp[i,j] = dp[i-1,j-1]
        dp[0, 0] = str2[0] == str1Sub[0] ? 0 : int.MaxValue;
        for (var j = 1; j < col; j++) dp[0, j] = int.MaxValue;

        for (var i = 1; i < row; i++)
            dp[i, 0] = dp[i - 1, 0] != int.MaxValue || str2[i] == str1Sub[0] ? i : int.MaxValue;

        for (var i = 1; i < row; i++)
        for (var j = 1; j < col; j++)
        {
            dp[i, j] = int.MaxValue;
            if (dp[i - 1, j] != int.MaxValue) dp[i, j] = dp[i - 1, j] + 1;

            if (str2[i] == str1Sub[j] && dp[i - 1, j - 1] != int.MaxValue)
                dp[i, j] = Math.Min(dp[i, j], dp[i - 1, j - 1]);
        }

        return dp[row - 1, col - 1];
    }

    // 解法二的优化
    private static int MinCost3(string s1, string s2)
    {
        if (s1.Length == 0 || s2.Length == 0) return s2.Length;

        var str2 = s2.ToCharArray();
        var str1 = s1.ToCharArray();
        var m = str2.Length;
        var n = str1.Length;
        var dp = new int[m, n];
        var ans = m;
        for (var start = 0; start < n; start++)
        {
            // 开始的列数
            dp[0, start] = str2[0] == str1[start] ? 0 : m;
            for (var row = 1; row < m; row++)
                dp[row, start] = str2[row] == str1[start] || dp[row - 1, start] != m ? row : m;

            ans = Math.Min(ans, dp[m - 1, start]);
            // 以上已经把start列，填好
            // 以下要把dp[...,start+1....N-1]的信息填好
            // start...end end - start +2
            for (var end = start + 1; end < n && end - start < m; end++)
            {
                // 0... first-1 行 不用管
                var first = end - start;
                dp[first, end] = str2[first] == str1[end] && dp[first - 1, end - 1] == 0 ? 0 : m;
                for (var row = first + 1; row < m; row++)
                {
                    dp[row, end] = m;
                    if (dp[row - 1, end] != m) dp[row, end] = dp[row - 1, end] + 1;

                    if (dp[row - 1, end - 1] != m && str2[row] == str1[end])
                        dp[row, end] = Math.Min(dp[row, end], dp[row - 1, end - 1]);
                }

                ans = Math.Min(ans, dp[m - 1, end]);
            }
        }

        return ans;
    }

    // 来自学生的做法，时间复杂度O(N * M平方)
    // 复杂度和方法三一样，但是思路截然不同
    private static int MinCost4(string s1, string s2)
    {
        var str1 = s1.ToCharArray();
        var str2 = s2.ToCharArray();
        var map1 = new Dictionary<char, List<int>?>();
        for (var i = 0; i < str1.Length; i++)
        {
            var list = map1.ContainsKey(str1[i]) ? map1[str1[i]] : new List<int>();
            list?.Add(i);
            map1[str1[i]] = list;
        }

        var ans = 0;
        // 假设删除后的str2必以i位置开头
        // 那么查找i位置在str1上一共有几个，并对str1上的每个位置开始遍历
        // 再次遍历str2一次，看存在对应str1中i后续连续子串可容纳的最长长度
        for (var i = 0; i < str2.Length; i++)
            if (map1.ContainsKey(str2[i]))
            {
                var keyList = map1[str2[i]];
                if (keyList != null)
                    foreach (var item in keyList)
                    {
                        var cur1 = item + 1;
                        var cur2 = i + 1;
                        var count = 1;
                        for (var k = cur2; k < str2.Length && cur1 < str1.Length; k++)
                            if (str2[k] == str1[cur1])
                            {
                                cur1++;
                                count++;
                            }

                        ans = Math.Max(ans, count);
                    }
            }

        return s2.Length - ans;
    }

    private static string GenerateRandomString(int l, int v)
    {
        var len = (int)(Utility.getRandomDouble * l);
        var str = new char[len];
        for (var i = 0; i < len; i++) str[i] = (char)('a' + (int)(Utility.getRandomDouble * v));

        return new string(str);
    }

    public static void Run()
    {
        char[] x = ['a', 'b', 'c', 'd'];
        char[] y = ['a', 'd'];

        Console.WriteLine(OnlyDelete(x, y));

        const int str1Len = 20;
        const int str2Len = 10;
        const int v = 5;
        const int testTime = 10000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var str1 = GenerateRandomString(str1Len, v);
            var str2 = GenerateRandomString(str2Len, v);
            var ans1 = MinCost1(str1, str2);
            var ans2 = MinCost2(str1, str2);
            var ans3 = MinCost3(str1, str2);
            var ans4 = MinCost4(str1, str2);
            if (ans1 != ans2 || ans3 != ans4 || ans1 != ans3)
            {
                Console.WriteLine("出错了");
                Console.WriteLine(str1);
                Console.WriteLine(str2);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                Console.WriteLine(ans3);
                Console.WriteLine(ans4);
                break;
            }
        }

        Console.WriteLine("测试完成");
    }
}