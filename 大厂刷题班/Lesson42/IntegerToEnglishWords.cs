namespace AdvancedTraining.Lesson42;

//https://leetcode.cn/problems/integer-to-english-words/description/
public class IntegerToEnglishWords //leetcode_0273
{
    private static string Num1To19(int num)
    {
        if (num is < 1 or > 19) return "";
        string[] names =
        [
            "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine ", "Ten ", "Eleven ",
            "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen", "Eighteen ", "Nineteen "
        ];
        return names[num - 1];
    }

    private static string Num1To99(int num)
    {
        if (num is < 1 or > 99) return "";
        if (num < 20) return Num1To19(num);
        var high = num / 10;
        string[] tyNames = ["Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety "];
        return tyNames[high - 2] + Num1To19(num % 10);
    }

    private static string Num1To999(int num)
    {
        if (num < 1 || num > 999) return "";
        if (num < 100) return Num1To99(num);
        var high = num / 100;
        return Num1To19(high) + "Hundred " + Num1To99(num % 100);
    }

    private static string NumberToWords(int num)
    {
        if (num == 0) return "Zero";
        var res = "";
        if (num < 0) res = "Negative ";
        if (num == int.MinValue)
        {
            res += "Two Billion ";
            num %= -2000000000;
        }

        num = Math.Abs(num);
        var high = 1000000000;
        var highIndex = 0;
        string[] names = ["Billion ", "Million ", "Thousand ", ""];
        while (num != 0)
        {
            var cur = num / high;
            num %= high;
            if (cur != 0)
            {
                res += Num1To999(cur);
                res += names[highIndex];
            }

            high /= 1000;
            highIndex++;
        }

        return res.Trim();
    }

    public static void Run()
    {
        var test = int.MinValue;
        Console.WriteLine(test);

        test = -test;
        Console.WriteLine(test);

        var num = -10001;
        Console.WriteLine(NumberToWords(num));
    }
}