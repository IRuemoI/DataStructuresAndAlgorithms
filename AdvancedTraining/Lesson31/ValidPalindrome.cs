namespace AdvancedTraining.Lesson31;

public class ValidPalindrome //Problem_0125
{
    // 忽略空格、忽略大小写 -> 是不是回文
    // 数字不在忽略大小写的范围内
    private static bool IsPalindrome(string s)
    {
        if (ReferenceEquals(s, null) || s.Length == 0) return true;
        var str = s.ToCharArray();
        var l = 0;
        var r = str.Length - 1;
        while (l < r)
            // 英文（大小写） + 数字
            if (ValidChar(str[l]) && ValidChar(str[r]))
            {
                if (!Equal(str[l], str[r])) return false;
                l++;
                r--;
            }
            else
            {
                l += ValidChar(str[l]) ? 0 : 1;
                r -= ValidChar(str[r]) ? 0 : 1;
            }

        return true;
    }

    private static bool ValidChar(char c)
    {
        return IsLetter(c) || IsNumber(c);
    }

    private static bool Equal(char c1, char c2)
    {
        if (IsNumber(c1) || IsNumber(c2)) return c1 == c2;
        // a  A   32
        // b  B   32
        // c  C   32
        return c1 == c2 || Math.Max(c1, c2) - Math.Min(c1, c2) == 32;
    }

    private static bool IsLetter(char c)
    {
        return c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';
    }

    private static bool IsNumber(char c)
    {
        return c is >= '0' and <= '9';
    }
}