//通过

namespace Algorithms.Lesson28;

public class AddShortestEnd
{
    private static string? ShortestEnd(string? s)
    {
        if (string.IsNullOrEmpty(s)) return null;
        var str = ManacherString(s);
        var pArr = new int[str.Length];
        var c = -1;
        var r = -1;
        var maxContainsEnd = -1;
        for (var i = 0; i != str.Length; i++)
        {
            pArr[i] = r > i ? Math.Min(pArr[2 * c - i], r - i) : 1;
            while (i + pArr[i] < str.Length && i - pArr[i] > -1)
                if (str[i + pArr[i]] == str[i - pArr[i]])
                    pArr[i]++;
                else
                    break;
            if (i + pArr[i] > r)
            {
                r = i + pArr[i];
                c = i;
            }

            if (r == str.Length)
            {
                maxContainsEnd = pArr[i];
                break;
            }
        }

        var res = new char[s.Length - maxContainsEnd + 1];
        for (var i = 0; i < res.Length; i++) res[res.Length - 1 - i] = str[i * 2 + 1];
        return new string(res);
    }

    private static char[] ManacherString(string str)
    {
        var charArr = str.ToCharArray();
        var res = new char[str.Length * 2 + 1];
        var index = 0;
        for (var i = 0; i != res.Length; i++) res[i] = (i & 1) == 0 ? '#' : charArr[index++];
        return res;
    }

    public static void Run()
    {
        const string str1 = "abcd123321";
        Console.WriteLine(ShortestEnd(str1));
    }
}