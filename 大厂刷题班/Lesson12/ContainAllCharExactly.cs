//pass
#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson12;

// 本题测试链接 : https://leetcode.cn/problems/permutation-in-string/
public class ContainAllCharExactly
{
    private static int ContainExactly1(string s, string a)
    {
        if (ReferenceEquals(s, null) || ReferenceEquals(a, null) || s.Length < a.Length) return -1;
        var aim = a.ToCharArray();
        Array.Sort(aim);
        var aimSort = new string(aim);
        for (var l = 0; l < s.Length; l++)
        for (var r = l; r < s.Length; r++)
        {
            var cur = s.Substring(l, r + 1 - l).ToCharArray();
            Array.Sort(cur);
            var curSort = new string(cur);
            if (curSort.Equals(aimSort)) return l;
        }

        return -1;
    }

    private static int ContainExactly2(string s, string a)
    {
        if (ReferenceEquals(s, null) || ReferenceEquals(a, null) || s.Length < a.Length) return -1;
        var str = s.ToCharArray();
        var aim = a.ToCharArray();
        for (var l = 0; l <= str.Length - aim.Length; l++)
            if (IsCountEqual(str, l, aim))
                return l;
        return -1;
    }

    private static bool IsCountEqual(char[] str, int l, char[] aim)
    {
        var count = new int[256];
        foreach (var item in aim)
            count[item]++;

        for (var i = 0; i < aim.Length; i++)
            if (count[str[l + i]]-- == 0)
                return false;
        return true;
    }

    private static int ContainExactly3(string s1, string s2)
    {
        if (ReferenceEquals(s1, null) || ReferenceEquals(s2, null) || s1.Length < s2.Length) return -1;
        var str2 = s2.ToCharArray();
        var m = str2.Length;
        var count = new int[256];
        for (var i = 0; i < m; i++) count[str2[i]]++;
        var all = m;
        var str1 = s1.ToCharArray();
        var r = 0;
        // 0~M-1
        for (; r < m; r++)
            // 最早的M个字符，让其窗口初步形成
            if (count[str1[r]]-- > 0)
                all--;
        // 窗口初步形成了，并没有判断有效无效，决定下一个位置一上来判断
        // 接下来的过程，窗口右进一个，左吐一个
        for (; r < str1.Length; r++)
        {
            if (all == 0)
                // R-1
                return r - m;
            if (count[str1[r]]-- > 0) all--;
            if (count[str1[r - m]]++ >= 0) all++;
        }

        return all == 0 ? r - m : -1;
    }

    //用于测试
    private static string GetRandomString(int possibilities, int maxSize)
    {
        var ans = new char[(int)(Utility.getRandomDouble * maxSize) + 1];
        for (var i = 0; i < ans.Length; i++) ans[i] = (char)((int)(Utility.getRandomDouble * possibilities) + 'a');
        return new string(ans);
    }

    public static void Run()
    {
        var possibilities = 5;
        var strMaxSize = 20;
        var aimMaxSize = 10;
        var testTimes = 5000;
        Console.WriteLine("test begin, test time : " + testTimes);
        for (var i = 0; i < testTimes; i++)
        {
            var str = GetRandomString(possibilities, strMaxSize);
            var aim = GetRandomString(possibilities, aimMaxSize);
            var ans1 = ContainExactly1(str, aim);
            var ans2 = ContainExactly2(str, aim);
            var ans3 = ContainExactly3(str, aim);
            if (ans1 != ans2 || ans2 != ans3)
            {
                Console.WriteLine("出错啦！");
                Console.WriteLine(str);
                Console.WriteLine(aim);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                Console.WriteLine(ans3);
                break;
            }
        }

        Console.WriteLine("测试完成");
    }
}