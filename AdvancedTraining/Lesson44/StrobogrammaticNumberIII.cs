namespace AdvancedTraining.Lesson44;

//https://www.cnblogs.com/EdwardLiu/p/5063082.html
public class StrobogrammaticNumberIii //leetcode_0248
{
    private static int StrobogrammaticInRange(string l, string h)
    {
        var low = l.ToCharArray();
        var high = h.ToCharArray();
        if (!EqualMore(low, high)) return 0;
        var lowLen = low.Length;
        var highLen = high.Length;
        if (lowLen == highLen)
        {
            var up1 = Up(low, 0, false, 1);
            var up2 = Up(high, 0, false, 1);
            return up1 - up2 + (Valid(high) ? 1 : 0);
        }

        var ans = 0;
        // lowLen = 3 hightLen = 7
        // 4 5 6
        for (var i = lowLen + 1; i < highLen; i++) ans += All(i);
        ans += Up(low, 0, false, 1);
        ans += Down(high, 0, false, 1);
        return ans;
    }

    private static bool EqualMore(char[] low, char[] cur)
    {
        if (low.Length != cur.Length) return low.Length < cur.Length;
        for (var i = 0; i < low.Length; i++)
            if (low[i] != cur[i])
                return low[i] < cur[i];
        return true;
    }

    private static bool Valid(char[] str)
    {
        var l = 0;
        var r = str.Length - 1;
        while (l <= r)
        {
            var t = l != r;
            if ((char)Convert(str[l++], t) != str[r--]) return false;
        }

        return true;
    }

    // left想得到cha字符，right配合应该做什么决定，
    // 如果left怎么也得不到cha字符，返回-1；如果能得到，返回right配合应做什么决定
    // 比如，left!=right，即不是同一个位置
    // left想得到0，那么就right就需要是0
    // left想得到1，那么就right就需要是1
    // left想得到6，那么就right就需要是9
    // left想得到8，那么就right就需要是8
    // left想得到9，那么就right就需要是6
    // 除此了这些之外，left不能得到别的了。
    // 比如，left==right，即是同一个位置
    // left想得到0，那么就right就需要是0
    // left想得到1，那么就right就需要是1
    // left想得到8，那么就right就需要是8
    // 除此了这些之外，left不能得到别的了，比如：
    // left想得到6，那么就right就需要是9，而left和right是一个位置啊，怎么可能即6又9，返回-1
    // left想得到9，那么就right就需要是6，而left和right是一个位置啊，怎么可能即9又6，返回-1
    private static int Convert(char cha, bool diff)
    {
        switch (cha)
        {
            case '0':
                return '0';
            case '1':
                return '1';
            case '6':
                return diff ? '9' : -1;
            case '8':
                return '8';
            case '9':
                return diff ? '6' : -1;
            default:
                return -1;
        }
    }

    // low [左边已经做完决定了 left.....right 右边已经做完决定了]
    // 左边已经做完决定的部分，如果大于low的原始，leftMore = true;
    // 左边已经做完决定的部分，如果不大于low的原始，那一定是相等，不可能小于，leftMore = false;
    // 右边已经做完决定的部分，如果小于low的原始，rightLessEqualMore = 0;
    // 右边已经做完决定的部分，如果等于low的原始，rightLessEqualMore = 1;
    // 右边已经做完决定的部分，如果大于low的原始，rightLessEqualMore = 2;
    // rightLessEqualMore < = >
    //                    0 1 2
    // 返回 ：没做决定的部分，随意变，几个有效的情况？返回！
    private static int Up(char[] low, int left, bool leftMore, int rightLessEqualMore)
    {
        var n = low.Length;
        var right = n - 1 - left;
        if (left > right)
            // 都做完决定了！
            // 如果左边做完决定的部分大于原始 或者 如果左边做完决定的部分等于原始&左边做完决定的部分不小于原始
            // 有效！
            // 否则，无效！
            return leftMore || (!leftMore && rightLessEqualMore != 0) ? 1 : 0;
        // 如果上面没有return，说明决定没做完，就继续做
        if (leftMore)
            // 如果左边做完决定的部分大于原始
            return Num(n - (left << 1)); // 如果左边做完决定的部分等于原始

        var ways = 0;
        // 当前left做的决定，大于原始的left
        for (var cha = (char)(low[left] + 1); cha <= '9'; cha++)
            if (Convert(cha, left != right) != -1)
                ways += Up(low, left + 1, true, rightLessEqualMore);
        // 当前left做的决定，等于原始的left
        var convert = Convert(low[left], left != right);
        if (convert != -1)
        {
            if ((char)convert < low[right])
                ways += Up(low, left + 1, false, 0);
            else if ((char)convert == low[right])
                ways += Up(low, left + 1, false, rightLessEqualMore);
            else
                ways += Up(low, left + 1, false, 2);
        }

        return ways;
    }

    // ll < =
    // rs < = >
    private static int Down(char[] high, int left, bool ll, int rs)
    {
        var n = high.Length;
        var right = n - 1 - left;
        if (left > right) return ll || (!ll && rs != 2) ? 1 : 0;
        if (ll) return Num(n - (left << 1));

        var ways = 0;
        for (var cha = n != 1 && left == 0 ? '1' : '0'; cha < high[left]; cha++)
            if (Convert(cha, left != right) != -1)
                ways += Down(high, left + 1, true, rs);
        var convert = Convert(high[left], left != right);
        if (convert != -1)
        {
            if ((char)convert < high[right])
                ways += Down(high, left + 1, false, 0);
            else if ((char)convert == high[right])
                ways += Down(high, left + 1, false, rs);
            else
                ways += Down(high, left + 1, false, 2);
        }

        return ways;
    }

    private static int Num(int bits)
    {
        if (bits == 1) return 3;
        if (bits == 2) return 5;
        var p2 = 3;
        var p1 = 5;
        var ans = 0;
        for (var i = 3; i <= bits; i++)
        {
            ans = 5 * p2;
            p2 = p1;
            p1 = ans;
        }

        return ans;
    }

    // 如果是最开始 :
    // Y X X X Y
    // -> 1 X X X 1
    // -> 8 X X X 8
    // -> 9 X X X 6
    // -> 6 X X X 9
    // 如果不是最开始 :
    // Y X X X Y
    // -> 0 X X X 0
    // -> 1 X X X 1
    // -> 8 X X X 8
    // -> 9 X X X 6
    // -> 6 X X X 9
    // 所有的len位数，有几个有效的？
    private static int All(int len)
    {
        var ans = (len & 1) == 0 ? 1 : 3;
        for (var i = (len & 1) == 0 ? 2 : 3; i < len; i += 2) ans *= 5;
        return ans << 2;
    }

    // 我们课上讲的
    private static int All(int len, bool init)
    {
        if (len == 0)
            // init == true，不可能调用all(0)
            return 1;
        if (len == 1) return 3;
        if (init)
            return All(len - 2, false) << 2;
        return All(len - 2, false) * 5;
    }

    public static void Run()
    {
        Console.WriteLine(StrobogrammaticInRange("50", "100")); //输出3
    }
}