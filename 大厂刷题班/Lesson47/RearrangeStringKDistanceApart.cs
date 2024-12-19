#region

using System.Text;

#endregion

namespace AdvancedTraining.Lesson47;

// 本题的解法思路与leetcode 621题 TaskScheduler 问题是一样的
public class RearrangeStringKDistanceApart //leetcode_0358
{
    public virtual string RearrangeString(string s, int k)
    {
        if (ReferenceEquals(s, null) || s.Length < k) return s;
        var str = s.ToCharArray();
        var cnts = new int[256][];
        for (var i = 0; i < 256; i++) cnts[i] = new[] { i, 0 };
        var maxCount = 0;
        foreach (var task in str)
        {
            cnts[task][1]++;
            maxCount = Math.Max(maxCount, cnts[task][1]);
        }

        var maxKinds = 0;
        for (var task = 0; task < 256; task++)
            if (cnts[task][1] == maxCount)
                maxKinds++;
        var n = str.Length;
        if (!IsValid(n, k, maxCount, maxKinds)) return "";
        var ans = new List<StringBuilder>();
        for (var i1 = 0; i1 < maxCount; i1++) ans.Add(new StringBuilder());
        Array.Sort(cnts, (a, b) => b[1] - a[1]);
        var i2 = 0;
        for (; i2 < 256 && cnts[i2][1] == maxCount; i2++)
        for (var j = 0; j < maxCount; j++)
            ans[j].Append((char)cnts[i2][0]);
        var @out = 0;
        for (; i2 < 256; i2++)
        for (var j = 0; j < cnts[i2][1]; j++)
        {
            ans[@out].Append((char)cnts[i2][0]);
            @out = @out == ans.Count - 2 ? 0 : @out + 1;
        }

        var builder = new StringBuilder();
        foreach (var b in ans) builder.Append(b.ToString());
        return builder.ToString();
    }

    private static bool IsValid(int n, int k, int maxCount, int maxKinds)
    {
        var restTasks = n - maxKinds;
        var spaces = k * (maxCount - 1);
        return spaces - restTasks <= 0;
    }

    public static void Run()
    {
        var str = "aaadbbcc";
        var k = 2;
        var result = new RearrangeStringKDistanceApart().RearrangeString(str, k); // "abacabcd"
        Console.WriteLine(result);
    }
}