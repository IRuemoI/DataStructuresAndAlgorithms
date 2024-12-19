namespace AdvancedTraining.Lesson39;

// 真实笔试，忘了哪个公司，但是绝对大厂
// 一个子序列的消除规则如下:
// 1) 在某一个子序列中，如果'1'的左边有'0'，那么这两个字符->"01"可以消除
// 2) 在某一个子序列中，如果'3'的左边有'2'，那么这两个字符->"23"可以消除
// 3) 当这个子序列的某个部分消除之后，认为其他字符会自动贴在一起，可以继续寻找消除的机会
// 比如，某个子序列"0231"，先消除掉"23"，那么剩下的字符贴在一起变成"01"，继续消除就没有字符了
// 如果某个子序列通过最优良的方式，可以都消掉，那么这样的子序列叫做"全消子序列"
// 一个只由'0'、'1'、'2'、'3'四种字符组成的字符串str，可以生成很多子序列，返回"全消子序列"的最大长度
// 字符串str长度 <= 200
// 体系学习班，代码46节，第2题+第3题
public class Char0123Disappear
{
    // str[L...R]上，都能消掉的子序列，最长是多少？
    private static int F(char[] str, int l, int r)
    {
        if (l >= r) return 0;
        if (l == r - 1) return (str[l] == '0' && str[r] == '1') || (str[l] == '2' && str[r] == '3') ? 2 : 0;
        // L...R 有若干个字符 > 2
        // str[L...R]上，都能消掉的子序列，最长是多少？
        // 可能性1，能消掉的子序列完全不考虑str[L]，最长是多少？
        var p1 = F(str, l + 1, r);
        if (str[l] == '1' || str[l] == '3') return p1;
        // str[L] =='0' 或者 '2'
        // '0' 去找 '1'
        // '2' 去找 '3'
        var find = str[l] == '0' ? '1' : '3';
        var p2 = 0;
        // L() ......
        for (var i = l + 1; i <= r; i++)
            // L(0) ..... i(1) i+1....R
            if (str[i] == find)
                p2 = Math.Max(p2, F(str, l + 1, i - 1) + 2 + F(str, i + 1, r));
        return Math.Max(p1, p2);
    }

    private static int MaxDisappear(string str)
    {
        if (ReferenceEquals(str, null) || str.Length == 0) return 0;
        return Disappear(str.ToCharArray(), 0, str.Length - 1);
    }

    // s[l..r]范围上，如题目所说的方式，最长的都能消掉的子序列长度
    private static int Disappear(char[] s, int l, int r)
    {
        if (l >= r) return 0;
        if (l == r - 1) return (s[l] == '0' && s[r] == '1') || (s[l] == '2' && s[r] == '3') ? 2 : 0;
        var p1 = Disappear(s, l + 1, r);
        if (s[l] == '1' || s[l] == '3') return p1;
        var p2 = 0;
        var find = s[l] == '0' ? '1' : '3';
        for (var i = l + 1; i <= r; i++)
            if (s[i] == find)
                p2 = Math.Max(p2, Disappear(s, l + 1, i - 1) + 2 + Disappear(s, i + 1, r));
        return Math.Max(p1, p2);
    }

    public static void Run()
    {
        var str1 = "010101";
        Console.WriteLine(MaxDisappear(str1));

        var str2 = "021331";
        Console.WriteLine(MaxDisappear(str2));
    }
}