//pass
#region

using System.Text;

#endregion

namespace AdvancedTraining.Lesson32;

public class FractionToRecurringDecimal //leetcode_0166
{
    private static string FractionToDecimal(int numerator, int denominator)
    {
        if (numerator == 0) return "0";
        var res = new StringBuilder();
        // "+" or "-"
        res.Append((numerator > 0) ^ (denominator > 0) ? "-" : "");
        var num = Math.Abs((long)numerator);
        var den = Math.Abs((long)denominator);
        // integral part
        res.Append(num / den);
        num %= den;
        if (num == 0) return res.ToString();
        // fractional part
        res.Append(".");
        var map = new Dictionary<long, int>
        {
            [num] = res.Length
        };
        while (num != 0)
        {
            num *= 10;
            res.Append(num / den);
            num %= den;
            if (map.TryGetValue(num, out var index))
            {
                res.Insert(index, "(");
                res.Append(")");
                break;
            }

            map[num] = res.Length;
        }

        return res.ToString();
    }

    public static void Run()
    {
        Console.WriteLine(FractionToDecimal(4, 333));
    }
}