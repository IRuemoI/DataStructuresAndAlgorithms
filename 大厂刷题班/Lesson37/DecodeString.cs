#region

using System.Text;

#endregion
//pass
namespace AdvancedTraining.Lesson37;

//https://leetcode.cn/problems/decode-string/
public class DecodeString //leetcode_0394
{
    private static string DecodeStringCode(string s)
    {
        var str = s.ToCharArray();
        return Process(str, 0).Ans;
    }

    // s[i....]  何时停？遇到   ']'  或者遇到 s的终止位置，停止
    // 返回Info
    // 0) 串
    // 1) 算到了哪
    private static Info Process(char[] s, int i)
    {
        var ans = new StringBuilder();
        var count = 0;
        while (i < s.Length && s[i] != ']')
            if ((s[i] >= 'a' && s[i] <= 'z') || (s[i] >= 'A' && s[i] <= 'Z'))
            {
                ans.Append(s[i++]);
            }
            else if (s[i] >= '0' && s[i] <= '9')
            {
                count = count * 10 + s[i++] - '0';
            }
            else
            {
                // str[index] = '['
                var next = Process(s, i + 1);
                ans.Append(TimesString(count, next.Ans));
                count = 0;
                i = next.Stop + 1;
            }

        return new Info(ans.ToString(), i);
    }

    private static string TimesString(int times, string str)
    {
        var ans = new StringBuilder();
        for (var i = 0; i < times; i++) ans.Append(str);
        return ans.ToString();
    }

    public static void Run()
    {
        Console.WriteLine(DecodeStringCode("abc3[cd]xyz")); //输出："abccdcdcdxyz"
    }

    private class Info(string a, int e)
    {
        public readonly string Ans = a;
        public readonly int Stop = e;
    }
}