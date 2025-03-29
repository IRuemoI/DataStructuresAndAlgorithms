//pass

namespace AdvancedTraining.Lesson32;

public class ExcelSheetColumnNumber //leetcode_0171
{
    // 这道题反过来也要会写
    private static int TitleToNumber(string s)
    {
        var str = s.ToCharArray();
        var ans = 0;
        foreach (var item in str) ans = ans * 26 + (item - 'A') + 1;

        return ans;
    }

    public static void Run()
    {
        Console.WriteLine(TitleToNumber("AB")); //输出28
    }
}