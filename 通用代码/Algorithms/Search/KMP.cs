//测试通过

namespace Common.Algorithms.Search;

public static class Kmp
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
        //x >= str1.Length表示str1已经比较完了；y >= str2.Length表示已经完全匹配
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
}