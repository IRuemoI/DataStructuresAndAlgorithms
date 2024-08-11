#region

using System.Text;

#endregion

namespace AdvancedTraining.Lesson28;

public class IntegerToRoman //Problem_0012
{
    private static string IntToRoman(int num)
    {
        string[][] c =
        [
            ["", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"],
            ["", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC"],
            ["", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM"],
            ["", "M", "MM", "MMM"]
        ];
        var roman = new StringBuilder();
        roman.Append(c[3][num / 1000 % 10]).Append(c[2][num / 100 % 10]).Append(c[1][num / 10 % 10])
            .Append(c[0][num % 10]);
        return roman.ToString();
    }


    public static void Run()
    {
        Console.WriteLine(IntToRoman(3749)); //输出： "MMMDCCXLIX"
    }
}