//通过

namespace Algorithms.Lesson27;

public class IsRotation
{
    private static bool Code(string a, string b)
    {
        if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return false;

        var b2 = b + b;
        return GetIndexOf(b2, a) != -1;
    }

    // KMP Algorithm
    private static int GetIndexOf(string s, string m)
    {
        if (s.Length < m.Length) return -1;

        var ss = s.ToCharArray();
        var ms = m.ToCharArray();
        var si = 0;
        var mi = 0;
        var next = GetNextArray(ms);
        while (si < ss.Length && mi < ms.Length)
            if (ss[si] == ms[mi])
            {
                si++;
                mi++;
            }
            else if (next[mi] == -1)
            {
                si++;
            }
            else
            {
                mi = next[mi];
            }

        return mi == ms.Length ? si - mi : -1;
    }

    private static int[] GetNextArray(char[] ms)
    {
        if (ms.Length == 1) return new[] { -1 };

        var next = new int[ms.Length];
        next[0] = -1;
        next[1] = 0;
        var pos = 2;
        var cn = 0;
        while (pos < next.Length)
            if (ms[pos - 1] == ms[cn])
                next[pos++] = ++cn;
            else if (cn > 0)
                cn = next[cn];
            else
                next[pos++] = 0;

        return next;
    }

    public static void Run()
    {
        const string str1 = "yunzuocheng";
        const string str2 = "zuochengyun";
        Console.WriteLine(Code(str1, str2));
    }
}