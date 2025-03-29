namespace AdvancedTraining.Lesson35;

//pass
public class LongestSubstringWithAtLeastKRepeatingCharacters //leetcode_0395
{
    private static int LongestSubstring1(string s, int k)
    {
        var str = s.ToCharArray();
        var n = str.Length;
        var max = 0;
        for (var i = 0; i < n; i++)
        {
            var count = new int[256];
            var collect = 0;
            var satisfy = 0;
            for (var j = i; j < n; j++)
            {
                if (count[str[j]] == 0) collect++;
                if (count[str[j]] == k - 1) satisfy++;
                count[str[j]]++;
                if (collect == satisfy) max = Math.Max(max, j - i + 1);
            }
        }

        return max;
    }

    private static int LongestSubstring2(string s, int k)
    {
        var str = s.ToCharArray();
        var n = str.Length;
        var max = 0;
        for (var require = 1; require <= 26; require++)
        {
            // 3种
            // a~z 出现次数
            var count = new int[26];
            // 目前窗口内收集了几种字符了
            var collect = 0;
            // 目前窗口内出现次数>=k次的字符，满足了几种
            var satisfy = 0;
            // 窗口右边界
            var r = -1;
            for (var l = 0; l < n; l++)
            {
                // L要尝试每一个窗口的最左位置
                // [L..R] R+1
                while (r + 1 < n && !(collect == require && count[str[r + 1] - 'a'] == 0))
                {
                    r++;
                    if (count[str[r] - 'a'] == 0) collect++;
                    if (count[str[r] - 'a'] == k - 1) satisfy++;
                    count[str[r] - 'a']++;
                }

                // [L...R]
                if (satisfy == require) max = Math.Max(max, r - l + 1);
                // L++
                if (count[str[l] - 'a'] == 1) collect--;
                if (count[str[l] - 'a'] == k) satisfy--;
                count[str[l] - 'a']--;
            }
        }

        return max;
    }

    // 会超时，但是思路的确是正确的
    private static int LongestSubstring3(string s, int k)
    {
        return Process(s.ToCharArray(), 0, s.Length - 1, k);
    }

    private static int Process(char[] str, int l, int r, int k)
    {
        if (l > r) return 0;
        var counts = new int[26];
        for (var i = l; i <= r; i++) counts[str[i] - 'a']++;
        var few = (char)0;
        var min = int.MaxValue;
        for (var i = 0; i < 26; i++)
            if (counts[i] != 0 && min > counts[i])
            {
                few = (char)(i + 'a');
                min = counts[i];
            }

        if (min >= k) return r - l + 1;
        var pre = 0;
        var max = int.MinValue;
        for (var i = l; i <= r; i++)
            if (str[i] == few)
            {
                max = Math.Max(max, Process(str, pre, i - 1, k));
                pre = i + 1;
            }

        if (pre != r + 1) max = Math.Max(max, Process(str, pre, r, k));
        return max;
    }

    public static void Run()
    {
        Console.WriteLine(LongestSubstring1("aaabb", 3)); //输出3
        Console.WriteLine(LongestSubstring2("aaabb", 3)); //输出3
        Console.WriteLine(LongestSubstring3("aaabb", 3)); //输出3
    }
}