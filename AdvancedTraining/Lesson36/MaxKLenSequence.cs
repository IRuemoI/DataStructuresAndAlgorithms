#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson36;

// 来自腾讯
// 给定一个字符串str，和一个正数k
// 返回长度为k的所有子序列中，字典序最大的子序列
public class MaxKLenSequence
{
    private static string MaxString(string s, int k)
    {
        if (k <= 0 || s.Length < k) return "";
        var str = s.ToCharArray();
        var n = str.Length;
        var stack = new char[n];
        var size = 0;
        for (var i = 0; i < n; i++)
        {
            while (size > 0 && stack[size - 1] < str[i] && size + n - i > k) size--;
            if (size + n - i == k) return new string(stack, 0, size) + s.Substring(i);
            stack[size++] = str[i];
        }

        return new string(stack, 0, k);
    }

    // 为了测试
    private static string? Test(string str, int k)
    {
        if (k <= 0 || str.Length < k) return "";
        var ans = new SortedSet<string>();
        Process(0, 0, str.ToCharArray(), new char[k], ans);
        return ans.Max;
    }

    // 为了测试
    private static void Process(int si, int pi, char[] str, char[] path, SortedSet<string> ans)
    {
        if (si == str.Length)
        {
            if (pi == path.Length) ans.Add(new string(path));
        }
        else
        {
            Process(si + 1, pi, str, path, ans);
            if (pi < path.Length)
            {
                path[pi] = str[si];
                Process(si + 1, pi + 1, str, path, ans);
            }
        }
    }

    // 为了测试
    private static string RandomString(int len, int range)
    {
        var str = new char[len];
        for (var i = 0; i < len; i++) str[i] = (char)((int)(Utility.getRandomDouble * range) + 'a');
        return new string(str);
    }

    public static void Run()
    {
        const int n = 12;
        const int r = 5;
        const int testTime = 10000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var len = (int)(Utility.getRandomDouble * (n + 1));
            var str = RandomString(len, r);
            var k = (int)(Utility.getRandomDouble * (str.Length + 1));
            var ans1 = MaxString(str, k);
            var ans2 = Test(str, k);
            if (!ans1.Equals(ans2))
            {
                Console.WriteLine("出错了！");
                Console.WriteLine(str);
                Console.WriteLine(k);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}