namespace AdvancedTraining.Lesson36;

// 来自美团
// 给定两个字符串s1和s2
// 返回在s1中有多少个子串等于s2
//https://blog.nowcoder.net/n/2fecf2b3050c4408a8943f70bc996f42
public class MatchCount
{
    private static int Sa(string s1, string s2)
    {
        if (ReferenceEquals(s1, null) || ReferenceEquals(s2, null) || s1.Length < s2.Length) return 0;
        var str1 = s1.ToCharArray();
        var str2 = s2.ToCharArray();
        return Count(str1, str2);
    }

    // 改写kmp为这道题需要的功能
    private static int Count(char[] str1, char[] str2)
    {
        var x = 0;
        var y = 0;
        var count = 0;
        var next = GetNextArray(str2);
        while (x < str1.Length)
            if (str1[x] == str2[y])
            {
                x++;
                y++;
                if (y == str2.Length)
                {
                    count++;
                    y = next[y];
                }
            }
            else if (next[y] == -1)
            {
                x++;
            }
            else
            {
                y = next[y];
            }

        return count;
    }

    // next数组多求一位
    // 比如：str2 = aaaa
    // 那么，next = -1,0,1,2,3
    // 最后一个3表示，终止位置之前的字符串最长前缀和最长后缀的匹配长度
    // 也就是next数组补一位
    private static int[] GetNextArray(char[] str2)
    {
        if (str2.Length == 1) return new[] { -1, 0 };
        var next = new int[str2.Length + 1];
        next[0] = -1;
        next[1] = 0;
        var i = 2;
        var cn = 0;
        while (i < next.Length)
            if (str2[i - 1] == str2[cn])
                next[i++] = ++cn;
            else if (cn > 0)
                cn = next[cn];
            else
                next[i++] = 0;
        return next;
    }

    public static void Run()
    {
        Console.WriteLine(Sa("aaaab", "aaa")); //输出2
    }
}