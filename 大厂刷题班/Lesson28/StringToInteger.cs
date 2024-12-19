namespace AdvancedTraining.Lesson28;

public class StringToInteger //leetcode_0008
{
    private static int MyAsciiToInt(string s)
    {
        if (s is null or "") return 0;
        s = RemoveHeadZero(s.Trim());
        if (s is null or "") return 0;
        var str = s.ToCharArray();
        if (!IsValid(str)) return 0;
        // str 是符合日常书写的，正经整数形式
        var posI = str[0] != '-';
        const int minQ = int.MinValue / 10;
        const int minR = int.MinValue % 10;
        var res = 0;
        for (var i = str[0] == '-' || str[0] == '+' ? 1 : 0; i < str.Length; i++)
        {
            // 3  cur = -3   '5'  cur = -5    '0' cur = 0
            var cur = '0' - str[i];
            if (res < minQ || (res == minQ && cur < minR)) return posI ? int.MaxValue : int.MinValue;
            res = res * 10 + cur;
        }

        // res 负
        if (posI && res == int.MinValue) return int.MaxValue;
        return posI ? -res : res;
    }

    private static string RemoveHeadZero(string str)
    {
        var r = str.StartsWith("+", StringComparison.Ordinal) || str.StartsWith("-", StringComparison.Ordinal);
        var s = r ? 1 : 0;
        for (; s < str.Length; s++)
            if (str[s] != '0')
                break;
        // s 到了第一个不是'0'字符的位置
        var e = -1;
        // 左<-右
        for (var i = str.Length - 1; i >= (r ? 1 : 0); i--)
            if (str[i] < '0' || str[i] > '9')
                e = i;
        // e 到了最左的 不是数字字符的位置
        return (r ? str[0].ToString() : "") + str.Substring(s, (e == -1 ? str.Length : e) - s);
    }

    private static bool IsValid(char[] chas)
    {
        if (chas[0] != '-' && chas[0] != '+' && (chas[0] < '0' || chas[0] > '9')) return false;
        if ((chas[0] == '-' || chas[0] == '+') && chas.Length == 1) return false;
        // 0 +... -... num
        for (var i = 1; i < chas.Length; i++)
            if (chas[i] < '0' || chas[i] > '9')
                return false;
        return true;
    }

    public static void Run()
    {
        Console.WriteLine(MyAsciiToInt(" -042")); //输出-42
    }
}