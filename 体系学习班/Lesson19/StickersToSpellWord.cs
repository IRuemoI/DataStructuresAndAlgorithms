//测试通过

#region

using System.Text;
using Common.Utilities;

#endregion

namespace Algorithms.Lesson19;

// 本题测试链接：https://leetcode.cn/problems/stickers-to-spell-word
public static class StickersToSpellWord
{
    private static int MinStickers1(string[] stickers, string target)
    {
        var ans = Process1(stickers, target);
        return ans == int.MaxValue ? -1 : ans;
    }

    // 所有贴纸stickers，每一种贴纸都有无穷张
    // target
    // 最少张数
    private static int Process1(string[] stickers, string target)
    {
        if (target.Length == 0) return 0;

        var min = int.MaxValue; //刚开始是最大值，表示无效
        foreach (var first in stickers)
        {
            var rest = Minus(target, first); //减去第一个贴纸
            if (rest.Length != target.Length) min = Math.Min(min, Process1(stickers, rest)); //如果能让剩余的匹配目标减小,说明有效
        }

        return min + (min == int.MaxValue ? 0 : 1); //当可行方案中有第一个可用贴纸时，min=1,当整个方案不可用时min始终为整型最大值
    }

    // 将贴纸中的已有字符从目标中减去
    private static string Minus(string s1, string s2)
    {
        var str1 = s1.ToCharArray();
        var str2 = s2.ToCharArray();
        var count = new int[26];
        foreach (var cha in str1) count[cha - 'a']++;

        foreach (var cha in str2) count[cha - 'a']--;

        var builder = new StringBuilder();
        for (var i = 0; i < 26; i++)
            if (count[i] > 0)
                for (var j = 0; j < count[i]; j++)
                    builder.Append((char)(i + 'a'));

        return builder.ToString();
    }

    private static int MinStickers2(string[] stickers, string target)
    {
        var n = stickers.Length;
        // 关键优化(用词频表替代贴纸数组)
        var counts = new int[n][];
        for (var i = 0; i < n; i++) counts[i] = new int[26];

        for (var i = 0; i < n; i++)
        {
            var str = stickers[i].ToCharArray();
            foreach (var cha in str) counts[i][cha - 'a']++;
        }

        var ans = Process2(counts, target);
        return ans == int.MaxValue ? -1 : ans;
    }

    // stickers[i] 数组，当初i号贴纸的字符统计 int[,] stickers -> 所有的贴纸
    // 每一种贴纸都有无穷张
    // 返回搞定target的最少张数
    // 最少张数
    private static int Process2(int[][] stickers, string t)
    {
        if (t.Length == 0) return 0;

        // target做出词频统计
        // target  aabbc  2 2 1..
        //                0 1 2..
        var target = t.ToCharArray();
        var tCounts = new int[26];
        foreach (var cha in target) tCounts[cha - 'a']++;

        var n = stickers.Length;
        var min = int.MaxValue;
        for (var i = 0; i < n; i++)
        {
            // 尝试第一张贴纸是谁
            var sticker = stickers[i];
            // 最关键的优化(重要的剪枝!这一步也是贪心!)
            if (sticker[target[0] - 'a'] > 0)
            {
                var builder = new StringBuilder();
                for (var j = 0; j < 26; j++)
                    if (tCounts[j] > 0)
                    {
                        var nums = tCounts[j] - sticker[j];
                        for (var k = 0; k < nums; k++) builder.Append((char)(j + 'a'));
                    }

                var rest = builder.ToString();
                min = Math.Min(min, Process2(stickers, rest));
            }
        }

        return min + (min == int.MaxValue ? 0 : 1);
    }

    private static int MinStickers3(string[] stickers, string target)
    {
        var n = stickers.Length;
        var counts = new int[n][];
        for (var i = 0; i < n; i++) counts[i] = new int[26];

        for (var i = 0; i < n; i++)
        {
            var str = stickers[i].ToCharArray();
            foreach (var cha in str) counts[i][cha - 'a']++;
        }

        Dictionary<string, int> dp = new() { { "", 0 } };
        var ans = Process3(counts, target, dp);
        return ans == int.MaxValue ? -1 : ans;
    }

    private static int Process3(int[][] stickers, string t, Dictionary<string, int> dp)
    {
        if (dp.TryGetValue(t, out var process3)) return process3;

        var target = t.ToCharArray();
        var tCounts = new int[26];
        foreach (var cha in target) tCounts[cha - 'a']++;

        var n = stickers.Length;
        var min = int.MaxValue;
        for (var i = 0; i < n; i++)
        {
            var sticker = stickers[i];
            if (sticker[target[0] - 'a'] > 0)
            {
                var builder = new StringBuilder();
                for (var j = 0; j < 26; j++)
                    if (tCounts[j] > 0)
                    {
                        var nums = tCounts[j] - sticker[j];
                        for (var k = 0; k < nums; k++) builder.Append((char)(j + 'a'));
                    }

                var rest = builder.ToString();
                min = Math.Min(min, Process3(stickers, rest, dp));
            }
        }

        var ans = min + (min == int.MaxValue ? 0 : 1);
        dp.Add(t, ans);
        return ans;
    }

    public static void Run()
    {
        string[] stickers1 = ["with", "example", "science"];
        var target1 = "thehat";
        string[] stickers2 = ["notice", "possible"];
        var target2 = "basicbasic";


        Utility.RestartStopwatch();
        Console.WriteLine("方法1结果:" + MinStickers1(stickers1, target1) + ",耗时:" +
                          Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法2结果:" + MinStickers2(stickers1, target1) + ",耗时:" +
                          Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法3结果:" + MinStickers3(stickers1, target1) + ",耗时:" +
                          Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Console.WriteLine("----------");
        Utility.RestartStopwatch();
        Console.WriteLine("方法1结果:" + MinStickers1(stickers2, target2) + ",耗时:" +
                          Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法2结果:" + MinStickers2(stickers2, target2) + ",耗时:" +
                          Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法3结果:" + MinStickers3(stickers2, target2) + ",耗时:" +
                          Utility.GetStopwatchElapsedMilliseconds() + "ms");
    }
}