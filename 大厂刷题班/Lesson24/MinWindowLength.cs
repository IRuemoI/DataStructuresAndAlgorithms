//pass

namespace AdvancedTraining.Lesson24;

public class MinWindowLength
{
    private static int MinLength(string s1, string s2)
    {
        if (ReferenceEquals(s1, null) || ReferenceEquals(s2, null) || s1.Length < s2.Length) return int.MaxValue;
        var str1 = s1.ToCharArray();
        var str2 = s2.ToCharArray();
        var map = new int[256]; // map[37] = 4 37 4次
        for (var i = 0; i != str2.Length; i++) map[str2[i]]++;
        var all = str2.Length;

        // [L,R-1] R
        // [L,R) -> [0,0)
        var l = 0;
        var r = 0;
        var minLen = int.MaxValue;
        while (r != str1.Length)
        {
            map[str1[r]]--;
            if (map[str1[r]] >= 0) all--;
            if (all == 0)
            {
                // 还完了
                while (map[str1[l]] < 0) map[str1[l++]]++;
                // [L..R]
                minLen = Math.Min(minLen, r - l + 1);
                all++;
                map[str1[l++]]++;
            }

            r++;
        }

        return minLen == int.MaxValue ? 0 : minLen;
    }

    // 测试链接 : https://leetcode.cn/problems/minimum-window-substring/
    private static string MinWindow(string s, string t)
    {
        if (s.Length < t.Length) return "";
        var str = s.ToCharArray();
        var target = t.ToCharArray();
        var map = new int[256];
        foreach (var cha in target) map[cha]++;
        var all = target.Length;
        var l = 0;
        var r = 0;
        var minLen = int.MaxValue;
        var leftAnswer = -1;
        var rightAnswer = -1;
        while (r != str.Length)
        {
            map[str[r]]--;
            if (map[str[r]] >= 0) all--;
            if (all == 0)
            {
                while (map[str[l]] < 0) map[str[l++]]++;
                if (minLen > r - l + 1)
                {
                    minLen = r - l + 1;
                    leftAnswer = l;
                    rightAnswer = r;
                }

                all++;
                map[str[l++]]++;
            }

            r++;
        }

        return minLen == int.MaxValue ? "" : s.Substring(leftAnswer, rightAnswer + 1 - leftAnswer);
    }
}