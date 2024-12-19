//pass
namespace AdvancedTraining.Lesson03;

// 本题测试链接 : https://leetcode.cn/problems/longest-substring-without-repeating-characters/
public class LongestSubstringWithoutRepeatingCharacters
{
    private static int LengthOfLongestSubstring(string s)
    {
        if (s is null || s.Equals("")) return 0;
        var str = s.ToCharArray();
        var map = new int[256];//创建一个存储上一次次字符出现的在字符串中下表的数组
        for (var i = 0; i < 256; i++) map[i] = -1;//初始时，这个数组的内容全部为-1
        map[str[0]] = 0;//字符串中的第一个字符的下标在0位置
        var n = str.Length;
        var ans = 1;//记录的答案
        var pre = 1;//上一轮最长不重复子串的长度
        for (var i = 1; i < n; i++)//对于字符串后续的所有字符
        {
            pre = Math.Min(i - map[str[i]], pre + 1);//将当前上次出现的下标与本次下标的距离(不重复距离)和又一个没出现的字符之后增加的距离中较小的保存为上一次的最大长度
            ans = Math.Max(ans, pre);//更新最大长度
            map[str[i]] = i;//更新字符上次出席的下标
        }

        return ans;
    }

    public static void Run()
    {
        Console.WriteLine(LengthOfLongestSubstring("abcabcbb")); //输出3
    }
}