namespace AdvancedTraining.Lesson49;

//pass
public class FindTheClosestPalindrome //leetcode_0564
{
    private static string? NearestPalindromic(string n)
    {
        long? num = Convert.ToInt64(n);
        var raw = GetRawPalindrome(n);
        var big = raw > num ? raw : GetBigPalindrome(raw);
        var small = raw < num ? raw : GetSmallPalindrome(raw);
        return (big - num >= num - small ? small : big).ToString();
    }

    private static long? GetRawPalindrome(string n)
    {
        var chs = n.ToCharArray();
        var len = chs.Length;
        for (var i = 0; i < len / 2; i++) chs[len - 1 - i] = chs[i];
        return Convert.ToInt64(new string(chs));
    }

    private static long? GetBigPalindrome(long? raw)
    {
        var chs = raw.ToString()!.ToCharArray();
        var res = new char[chs.Length + 1];
        res[0] = '0';
        for (var i = 0; i < chs.Length; i++) res[i + 1] = chs[i];
        var size = chs.Length;
        for (var j = (size - 1) / 2 + 1; j >= 0; j--)
            if (++res[j] > '9')
                res[j] = '0';
            else
                break;
        var offset = res[0] == '1' ? 1 : 0;
        size = res.Length;
        for (var i = size - 1; i >= (size + offset) / 2; i--) res[i] = res[size - i - offset];
        return Convert.ToInt64(new string(res));
    }

    private static long? GetSmallPalindrome(long? raw)
    {
        var chs = raw.ToString()!.ToCharArray();
        var res = new char[chs.Length];
        var size = res.Length;
        for (var i = 0; i < size; i++) res[i] = chs[i];
        for (var j = (size - 1) / 2; j >= 0; j--)
            if (--res[j] < '0')
                res[j] = '9';
            else
                break;
        if (res[0] == '0')
        {
            res = new char[size - 1];
            for (var i = 0; i < res.Length; i++) res[i] = '9';
            return size == 1 ? 0 : long.Parse(new string(res));
        }

        for (var k = 0; k < size / 2; k++) res[size - 1 - k] = res[k];
        return Convert.ToInt64(new string(res));
    }
}