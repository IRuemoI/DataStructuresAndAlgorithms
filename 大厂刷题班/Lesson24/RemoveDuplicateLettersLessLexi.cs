//pass
namespace AdvancedTraining.Lesson24;

// 本题测试链接 : https://leetcode.cn/problems/remove-duplicate-letters/
public class RemoveDuplicateLettersLessLexi
{
    // 在str中，每种字符都要保留一个，让最后的结果，字典序最小 ，并返回
    private static string? RemoveDuplicateLetters1(string str)
    {
        if (ReferenceEquals(str, null) || str.Length < 2) return str;
        var map = new int[256];
        foreach (var item in str) map[item]++;

        var minAcsIndex = 0;
        for (var i = 0; i < str.Length; i++)
        {
            minAcsIndex = str[minAcsIndex] > str[i] ? i : minAcsIndex;
            if (--map[str[i]] == 0) break;
        }

        // 0...break(之前) minACSIndex
        // str[minACSIndex] 剩下的字符串str[minACSIndex+1...] -> 去掉str[minACSIndex]字符 -> s'
        // s'...
        return str[minAcsIndex] +
               RemoveDuplicateLetters1(str.Substring(minAcsIndex + 1).Replace(str[minAcsIndex].ToString(), ""));
    }

    private static string RemoveDuplicateLetters2(string s)
    {
        var str = s.ToCharArray();
        // 小写字母ascii码值范围[97~122]，所以用长度为26的数组做次数统计
        // 如果map[i] > -1，则代表ascii码值为i的字符的出现次数
        // 如果map[i] == -1，则代表ascii码值为i的字符不再考虑
        var map = new int[26];
        foreach (var item in str)
            map[item - 'a']++;

        var res = new char[26];
        var index = 0;
        var l = 0;
        var r = 0;
        while (r != str.Length)
            // 如果当前字符是不再考虑的，直接跳过
            // 如果当前字符的出现次数减1之后，后面还能出现，直接跳过
            if (map[str[r] - 'a'] == -1 || --map[str[r] - 'a'] > 0)
            {
                r++;
            }
            else
            {
                // 当前字符需要考虑并且之后不会再出现了
                // 在str[L..R]上所有需要考虑的字符中，找到ascii码最小字符的位置
                var pick = -1;
                for (var i = l; i <= r; i++)
                    if (map[str[i] - 'a'] != -1 && (pick == -1 || str[i] < str[pick]))
                        pick = i;
                // 把ascii码最小的字符放到挑选结果中
                res[index++] = str[pick];
                // 在上一个的for循环中，str[L..R]范围上每种字符的出现次数都减少了
                // 需要把str[pick + 1..R]上每种字符的出现次数加回来
                for (var i = pick + 1; i <= r; i++)
                    if (map[str[i] - 'a'] != -1)
                        // 只增加以后需要考虑字符的次数
                        map[str[i] - 'a']++;
                // 选出的ascii码最小的字符，以后不再考虑了
                map[str[pick] - 'a'] = -1;
                // 继续在str[pick + 1......]上重复这个过程
                l = pick + 1;
                r = l;
            }

        return new string(res, 0, index);
    }

    public static void Run()
    {
        Console.WriteLine(RemoveDuplicateLetters1("cbacdcbc")); //输出acdb
        Console.WriteLine(RemoveDuplicateLetters2("cbacdcbc")); //输出acdb
    }
}