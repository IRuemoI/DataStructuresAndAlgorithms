//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson46;

// 如果一个字符相邻的位置没有相同字符，那么这个位置的字符出现不能被消掉
// 比如:"ab"，其中a和b都不能被消掉
// 如果一个字符相邻的位置有相同字符，就可以一起消掉
// 比如:"abbbc"，中间一串的b是可以被消掉的，消除之后剩下"ac"
// 某些字符如果消掉了，剩下的字符认为重新靠在一起
// 给定一个字符串，你可以决定每一步消除的顺序，目标是请尽可能多的消掉字符，返回最少的剩余字符数量
// 比如："aacca", 如果先消掉最左侧的"aa"，那么将剩下"cca"，然后把"cc"消掉，剩下的"a"将无法再消除，返回1
// 但是如果先消掉中间的"cc"，那么将剩下"aaa"，最后都消掉就一个字符也不剩了，返回0，这才是最优解。
// 再比如："baaccabb"，
// 如果先消除最左侧的两个a，剩下"bccabb"，
// 如果再消除最左侧的两个c，剩下"babb"，
// 最后消除最右侧的两个b，剩下"ba"无法再消除，返回2
// 而最优策略是：
// 如果先消除中间的两个c，剩下"baaabb"，
// 如果再消除中间的三个a，剩下"bbb"，
// 最后消除三个b，不留下任何字符，返回0，这才是最优解
public class DeleteAdjacentSameCharacter
{
    // 暴力解
    private static int RestMin1(string s)
    {
        if (ReferenceEquals(s, null)) return 0;
        if (s.Length < 2) return s.Length;
        var minLen = s.Length;
        for (var l = 0; l < s.Length; l++)
        for (var r = l + 1; r < s.Length; r++)
            if (CanDelete(s.Substring(l, r + 1 - l)))
                minLen = Math.Min(minLen, RestMin1(s.Substring(0, l) + s.Substring(r + 1, s.Length - (r + 1))));
        return minLen;
    }

    private static bool CanDelete(string s)
    {
        var str = s.ToCharArray();
        for (var i = 1; i < str.Length; i++)
            if (str[i - 1] != str[i])
                return false;
        return true;
    }

    // 优良尝试的暴力递归版本
    private static int RestMin2(string s)
    {
        if (ReferenceEquals(s, null)) return 0;
        if (s.Length < 2) return s.Length;
        var str = s.ToCharArray();
        return Process(str, 0, str.Length - 1, false);
    }

    // str[L...R] 前面有没有跟着[L]字符，has T 有 F 无
    // L,R,has
    // 最少能剩多少字符，消不了
    private static int Process(char[] str, int l, int r, bool has)
    {
        if (l > r) return 0;
        if (l == r) return has ? 0 : 1;
        var index = l;
        var k = has ? 1 : 0;
        while (index <= r && str[index] == str[l])
        {
            k++;
            index++;
        }

        // index表示，第一个不是[L]字符的位置
        var way1 = (k > 1 ? 0 : 1) + Process(str, index, r, false);
        var way2 = int.MaxValue;
        for (var split = index; split <= r; split++)
            if (str[split] == str[l] && str[split] != str[split - 1])
                if (Process(str, index, split - 1, false) == 0)
                    way2 = Math.Min(way2, Process(str, split, r, k != 0));
        return Math.Min(way1, way2);
    }

    // 优良尝试的动态规划版本
    private static int RestMin3(string s)
    {
        if (ReferenceEquals(s, null)) return 0;
        if (s.Length < 2) return s.Length;
        var str = s.ToCharArray();
        var n = str.Length;
        var dp = new int [n, n, 2];
        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
        for (var k = 0; k < 2; k++)
            dp[i, j, k] = -1;
        return DpProcess(str, 0, n - 1, false, dp);
    }

    private static int DpProcess(char[] str, int l, int r, bool has, int[,,] dp)
    {
        if (l > r) return 0;
        var k = has ? 1 : 0;
        if (dp[l, r, k] != -1) return dp[l, r, k];
        int ans;
        if (l == r)
        {
            ans = k == 0 ? 1 : 0;
        }
        else
        {
            var index = l;
            var all = k;
            while (index <= r && str[index] == str[l])
            {
                all++;
                index++;
            }

            var way1 = (all > 1 ? 0 : 1) + DpProcess(str, index, r, false, dp);
            var way2 = int.MaxValue;
            for (var split = index; split <= r; split++)
                if (str[split] == str[l] && str[split] != str[split - 1])
                    if (DpProcess(str, index, split - 1, false, dp) == 0)
                        way2 = Math.Min(way2, DpProcess(str, split, r, all > 0, dp));
            ans = Math.Min(way1, way2);
        }

        dp[l, r, k] = ans;
        return ans;
    }

    private static string RandomString(int len, int variety)
    {
        var str = new char[len];
        for (var i = 0; i < len; i++) str[i] = (char)((int)(Utility.getRandomDouble * variety) + 'a');
        return new string(str);
    }

    public static void Run()
    {
        var maxLen = 16;
        var variety = 3;
        var testTime = 100000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var len = (int)(Utility.getRandomDouble * maxLen);
            var str = RandomString(len, variety);
            var ans1 = RestMin1(str);
            var ans2 = RestMin2(str);
            var ans3 = RestMin3(str);
            if (ans1 != ans2 || ans1 != ans3)
            {
                Console.WriteLine(str);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                Console.WriteLine(ans3);
                Console.WriteLine("出错了！");
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}