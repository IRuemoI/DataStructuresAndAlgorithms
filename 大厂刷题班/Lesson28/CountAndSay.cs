//pass

#region

using System.Text;

#endregion

namespace AdvancedTraining.Lesson28;

//https://leetcode.cn/problems/count-and-say/description/
public class CountAndSay //leetcode_0038
{
    private static string Code(int n)
    {
        if (n < 1) return "";
        if (n == 1) return "1";
        var last = Code(n - 1).ToCharArray();
        var ans = new StringBuilder();
        var times = 1;
        for (var i = 1; i < last.Length; i++)
            if (last[i - 1] == last[i])
            {
                times++;
            }
            else
            {
                ans.Append(times);
                ans.Append(last[i - 1]);
                times = 1;
            }

        ans.Append(times);
        ans.Append(last[^1]);
        return ans.ToString();
    }

    public static void Run()
    {
        Console.WriteLine(Code(4)); //输出1211
    }
}