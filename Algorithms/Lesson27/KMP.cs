//测试通过

namespace Algorithms.Lesson27;

public class Kmp
{
    private static int KmpGetIndexOf(string? s1, string? s2)
    {
        if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2)) return -1;

        var str1 = s1.ToCharArray();
        var str2 = s2.ToCharArray();
        var x = 0;
        var y = 0;
        // O(M) m <= n
        var next = GetNextArray(str2);
        // O(N)
        while (x < str1.Length && y < str2.Length)
            if (str1[x] == str2[y])
            {
                x++;
                y++;
            }
            else if (next[y] == -1)
            {
                // y == 0
                x++;
            }
            else
            {
                y = next[y];
            }

        return y == str2.Length ? x - y : -1;
    }

    private static int[] GetNextArray(char[] str2)
    {
        if (str2.Length == 1) return [-1];

        var next = new int[str2.Length];
        next[0] = -1;
        next[1] = 0;
        var i = 2; // 目前在哪个位置上求next数组的值
        var cn = 0; // 当前是哪个位置的值再和i-1位置的字符比较
        while (i < next.Length)
            if (str2[i - 1] == str2[cn])
                // 配成功的时候
                next[i++] = ++cn;
            else if (cn > 0)
                cn = next[cn];
            else
                next[i++] = 0;

        return next;
    }

    //用于测试
    private static string? GetRandomString(int possibilities, int size)
    {
        var ans = new char[(int)(new Random().NextDouble() * size) + 1];
        for (var i = 0; i < ans.Length; i++) ans[i] = (char)((int)(new Random().NextDouble() * possibilities) + 'a');

        return ans.ToString();
    }

    public static void Run()
    {
        const int possibilities = 5;
        const int strSize = 20;
        const int matchSize = 5;
        const int testTimes = 50000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTimes; i++)
        {
            var str = GetRandomString(possibilities, strSize);
            var match = GetRandomString(possibilities, matchSize);
            if (match != null && KmpGetIndexOf(str, match) != str?.IndexOf(match, StringComparison.Ordinal))
                Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");
    }
}