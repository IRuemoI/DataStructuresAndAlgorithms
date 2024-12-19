#region

using System.Text;

#endregion

namespace AdvancedTraining.Lesson46;

public class MinimumUniqueWordAbbreviation //leetcode_0411
{
    // 区分出来之后，缩写的长度，最短是多少？
    private static int _min = int.MaxValue;

    // 取得缩写的长度最短的时候，决定是什么(fix)
    private static int _best;

    private static int AbbrLen(int fix, int len)
    {
        var ans = 0;
        var cnt = 0;
        for (var i = 0; i < len; i++)
            if ((fix & (1 << i)) != 0)
            {
                ans++;
                if (cnt != 0) ans += (cnt > 9 ? 2 : 1) - cnt;
                cnt = 0;
            }
            else
            {
                cnt++;
            }

        if (cnt != 0) ans += (cnt > 9 ? 2 : 1) - cnt;
        return ans;
    }

    // 原始的字典，被改了
    // target : abc  字典中的词 : bbb   ->  101 -> int -> 
    // fix -> int -> 根本不用值，用状态 -> 每一位保留还是不保留的决定
    private static bool CanFix(int[] words, int fix)
    {
        foreach (var word in words)
            if ((fix & word) == 0)
                return false;
        return true;
    }

    // 利用位运算加速
    private static string MinAbbreviation1(string target, string[] dictionary)
    {
        _min = int.MaxValue;
        _best = 0;
        var t = target.ToCharArray();
        var len = t.Length;
        var siz = 0;
        foreach (var word in dictionary)
            if (word.Length == len)
                siz++;
        var words = new int[siz];
        var index = 0;
        foreach (var word in dictionary)
            if (word.Length == len)
            {
                var w = word.ToCharArray();
                var status = 0;
                for (var j = 0; j < len; j++)
                    if (t[j] != w[j])
                        status |= 1 << j;
                words[index++] = status;
            }

        Dfs1(words, len, 0, 0);
        var builder = new StringBuilder();
        var count = 0;
        for (var i = 0; i < len; i++)
            if ((_best & (1 << i)) != 0)
            {
                if (count > 0) builder.Append(count);
                builder.Append(t[i]);
                count = 0;
            }
            else
            {
                count++;
            }

        if (count > 0) builder.Append(count);
        return builder.ToString();
    }

    // 所有字典中的单词现在都变成了int，放在words里
    // 0....len-1 位去决定保留还是不保留！当前来到index位
    // 之前做出的决定!
    private static void Dfs1(int[] words, int len, int fix, int index)
    {
        if (!CanFix(words, fix))
        {
            if (index < len)
            {
                Dfs1(words, len, fix, index + 1);
                Dfs1(words, len, fix | (1 << index), index + 1);
            }
        }
        else
        {
            // 决定是fix，一共的长度是len，求出缩写是多长？
            var ans = AbbrLen(fix, len);
            if (ans < _min)
            {
                _min = ans;
                _best = fix;
            }
        }
    }

    // 进一步设计剪枝，注意diff的用法
    private static string MinAbbreviation2(string target, string[] dictionary)
    {
        _min = int.MaxValue;
        _best = 0;
        var t = target.ToCharArray();
        var len = t.Length;
        var siz = 0;
        foreach (var word in dictionary)
            if (word.Length == len)
                siz++;
        var words = new int[siz];
        var index = 0;
        // 用来剪枝
        var diff = 0;
        foreach (var word in dictionary)
            if (word.Length == len)
            {
                var w = word.ToCharArray();
                var status = 0;
                for (var j = 0; j < len; j++)
                    if (t[j] != w[j])
                        status |= 1 << j;
                words[index++] = status;
                diff |= status;
            }

        Dfs2(words, len, diff, 0, 0);
        var builder = new StringBuilder();
        var count = 0;
        for (var i = 0; i < len; i++)
            if ((_best & (1 << i)) != 0)
            {
                if (count > 0) builder.Append(count);
                builder.Append(t[i]);
                count = 0;
            }
            else
            {
                count++;
            }

        if (count > 0) builder.Append(count);
        return builder.ToString();
    }

    private static void Dfs2(int[] words, int len, int diff, int fix, int index)
    {
        if (!CanFix(words, fix))
        {
            if (index < len)
            {
                Dfs2(words, len, diff, fix, index + 1);
                if ((diff & (1 << index)) != 0) Dfs2(words, len, diff, fix | (1 << index), index + 1);
            }
        }
        else
        {
            var ans = AbbrLen(fix, len);
            if (ans < _min)
            {
                _min = ans;
                _best = fix;
            }
        }
    }

    public static void Run()
    {
        Console.WriteLine(MinAbbreviation1("apple", ["plain", "amber", "blade"])); //输出"1p3"
        Console.WriteLine(MinAbbreviation2("apple", ["plain", "amber", "blade"])); //输出"1p3"
    }
}