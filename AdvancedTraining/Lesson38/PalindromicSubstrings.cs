namespace AdvancedTraining.Lesson38;

public class PalindromicSubstrings //Problem_0647
{
    private static int CountSubstrings(string s)
    {
        if (ReferenceEquals(s, null) || s.Length == 0) return 0;
        var dp = GetManacherDp(s);
        var ans = 0;
        foreach (var item in dp)
            ans += item >> 1;

        return ans;
    }

    private static int[] GetManacherDp(string s)
    {
        var str = ManacherString(s);
        var pArr = new int[str.Length];
        var c = -1;
        var r = -1;
        for (var i = 0; i < str.Length; i++)
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
        }

        return pArr;
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
        Console.WriteLine(CountSubstrings("aaa")); //输出6
    }
}