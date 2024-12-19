//测试通过

namespace Algorithms.Lesson28;

public class Manacher
{
    private static int Code(string? s)
    {
        if (string.IsNullOrEmpty(s)) return 0;

        // "12132" -> "#1#2#1#3#2#"
        var str = ManacherString(s);
        // 回文半径的大小
        var pArr = new int[str.Length];
        var c = -1;
        // 讲述中：R代表最右的扩成功的位置
        // coding：最右的扩成功位置的，再下一个位置
        var r = -1;
        var max = int.MinValue;
        for (var i = 0; i < str.Length; i++)
        {
            // 0 1 2
            // R第一个违规的位置，i>= R
            // i位置扩出来的答案，i位置扩的区域，至少是多大。
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

            max = Math.Max(max, pArr[i]);
        }

        return max - 1;
    }

    private static char[] ManacherString(string str)
    {
        var charArr = str.ToCharArray();
        var res = new char[str.Length * 2 + 1];
        var index = 0;
        for (var i = 0; i != res.Length; i++) res[i] = (i & 1) == 0 ? '#' : charArr[index++];

        return res;
    }

    //用于测试
    private static int Right(string? s)
    {
        if (string.IsNullOrEmpty(s)) return 0;

        var str = ManacherString(s);
        var max = 0;
        for (var i = 0; i < str.Length; i++)
        {
            var l = i - 1;
            var r = i + 1;
            while (l >= 0 && r < str.Length && str[l] == str[r])
            {
                l--;
                r++;
            }

            max = Math.Max(max, r - l - 1);
        }

        return max / 2;
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
        const int testTimes = 50000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTimes; i++)
        {
            var str = GetRandomString(possibilities, strSize);
            if (Code(str) != Right(str)) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");
    }
}