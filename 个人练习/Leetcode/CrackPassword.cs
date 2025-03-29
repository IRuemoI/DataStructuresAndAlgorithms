using System.Text;

namespace CustomTraining.Leetcode;

// https://leetcode.cn/problems/ba-shu-zu-pai-cheng-zui-xiao-de-shu-lcof/
public static class CrackPassword
{
    private static string CrackPasswordCode(int[] password)
    {
        var strings = new string[password.Length];
        for (var i = 0; i < password.Length; i++)
            strings[i] = password[i].ToString();
        Array.Sort(strings, (x, y) => string.Compare(x + y, y + x, StringComparison.Ordinal));
        var res = new StringBuilder();
        foreach (var s in strings)
            res.Append(s);
        return res.ToString();
    }

    public static void Run()
    {
        Console.WriteLine(CrackPasswordCode([15, 8, 7]));
        Console.WriteLine(CrackPasswordCode([0, 3, 30, 34, 5, 9]));
    }
}