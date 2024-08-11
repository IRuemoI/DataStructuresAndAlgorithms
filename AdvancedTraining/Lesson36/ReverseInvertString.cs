namespace AdvancedTraining.Lesson36;

// 来自网易
// 规定：L[1]对应a，L[2]对应b，L[3]对应c，...，L[25]对应y
// S1 = a
// S(i) = S(i-1) + L[i] + reverse(Invert(S(i-1)));
// 解释invert操作：
// S1 = a
// S2 = aby
// 假设invert(S(2)) = 甲乙丙
// a + 甲 = 26, 那么 甲 = 26 - 1 = 25 -> y
// b + 乙 = 26, 那么 乙 = 26 - 2 = 24 -> x
// y + 丙 = 26, 那么 丙 = 26 - 25 = 1 -> a
// 如上就是每一位的计算方式，所以invert(S2) = yxa
// 所以S3 = S2 + L[3] + reverse(Invert(S2)) = aby + c + axy = abycaxy
// Invert(abycaxy) = yxawyba, 再reverse = abywaxy
// 所以S4 = abycaxy + d + abywaxy = abycaxydabywaxy
// 直到S25结束
// 给定两个参数n和k，返回Sn的第k位是什么字符，n从1开始，k从1开始
// 比如n=4，k=2，表示S4的第2个字符是什么，返回b字符
public class ReverseInvertString
{
    private static int[]? _lens;

    private static void FillLens()
    {
        _lens = new int[26];
        _lens[1] = 1;
        for (var i = 2; i <= 25; i++) _lens[i] = (_lens[i - 1] << 1) + 1;
    }

    // 求sn中的第k个字符
    // O(n), s <= 25 O(1)
    private static char Kth(int n, int k)
    {
        if (_lens == null) FillLens();
        if (n == 1)
            // 无视k
            return 'a';
        // sn half
        if (_lens != null)
        {
            var half = _lens[n - 1];
            if (k <= half)
                return Kth(n - 1, k);
            if (k == half + 1)
                return (char)('a' + n - 1);
            // sn
            // 我需要右半区，从左往右的第a个
            // 需要找到，s(n-1)从右往左的第a个
            // 当拿到字符之后，invert一下，就可以返回了！
            return Invert(Kth(n - 1, ((half + 1) << 1) - k));
        }

        throw new Exception();
    }

    private static char Invert(char c)
    {
        return (char)(('a' << 1) + 24 - c);
    }

    // 为了测试
    private static string GenerateString(int n)
    {
        var s = "a";
        for (var i = 2; i <= n; i++) s = s + (char)('a' + i - 1) + ReverseInvert(s);
        return s;
    }

    // 为了测试
    private static string ReverseInvert(string s)
    {
        var invert = Invert(s).ToCharArray();
        for (int l = 0, r = invert.Length - 1; l < r; l++, r--) (invert[l], invert[r]) = (invert[r], invert[l]);

        return new string(invert);
    }

    // 为了测试
    private static string Invert(string s)
    {
        var str = s.ToCharArray();
        for (var i = 0; i < str.Length; i++) str[i] = Invert(str[i]);
        return new string(str);
    }

    // 为了测试
    public static void Run()
    {
        var n = 20;
        var str = GenerateString(n);
        var len = str.Length;
        Console.WriteLine("测试开始");
        for (var i = 1; i <= len; i++)
            if (str[i - 1] != Kth(n, i))
            {
                Console.WriteLine(i);
                Console.WriteLine(str[i - 1]);
                Console.WriteLine(Kth(n, i));
                Console.WriteLine("出错了！");
                break;
            }

        Console.WriteLine("测试结束");
    }
}