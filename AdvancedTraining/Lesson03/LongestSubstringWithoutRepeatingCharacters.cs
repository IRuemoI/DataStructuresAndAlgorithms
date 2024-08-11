namespace AdvancedTraining.Lesson03;

// 本题测试链接 : https://leetcode.cn/problems/longest-substring-without-repeating-characters/
public class LongestSubstringWithoutRepeatingCharacters
{
    private static int LengthOfLongestSubstring(string s)
    {
        if (ReferenceEquals(s, null) || s.Equals("")) return 0;
        var str = s.ToCharArray();
        var map = new int[256];
        for (var i = 0; i < 256; i++) map[i] = -1;
        map[str[0]] = 0;
        var n = str.Length;
        var ans = 1;
        var pre = 1;
        for (var i = 1; i < n; i++)
        {
            pre = Math.Min(i - map[str[i]], pre + 1);
            ans = Math.Max(ans, pre);
            map[str[i]] = i;
        }

        return ans;
    }

    public static void Run()
    {
        Console.WriteLine(LengthOfLongestSubstring("abcabcbb")); //输出3
    }
}