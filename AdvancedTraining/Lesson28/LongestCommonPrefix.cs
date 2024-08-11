namespace AdvancedTraining.Lesson28;

//Question@https://leetcode.cn/problems/longest-common-prefix/
public class LongestCommonPrefix
{
    private static string Code(string[]? strings)
    {
        if (strings == null || strings.Length == 0) return "";
        //先取出第一个字符串作为比较的基础
        var chs = strings[0].ToCharArray();
        var min = int.MaxValue;
        foreach (var str in strings)
        {
            var tmp = str.ToCharArray();
            var index = 0;
            while (index < tmp.Length && index < chs.Length)
            {
                if (chs[index] != tmp[index]) break;
                index++;
            }

            min = Math.Min(index, min);
            if (min == 0) return "";
        }

        return strings[0].Substring(0, min);
    }

    public static void Run()
    {
        Console.WriteLine(Code(["flower", "flow", "flight"])); //输出"fl"
    }
}