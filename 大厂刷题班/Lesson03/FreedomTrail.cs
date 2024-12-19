//pass
namespace AdvancedTraining.Lesson03;

// 本题测试链接 : https://leetcode.cn/problems/freedom-trail/
public class FreedomTrail
{
    private static int FindRotateSteps(string r, string k)
    {
        var ring = r.ToCharArray();
        var n = ring.Length;
        var map = new Dictionary<char, List<int>>();
        for (var i = 0; i < n; i++)
        {
            if (!map.ContainsKey(ring[i])) map[ring[i]] = new List<int>();
            map[ring[i]].Add(i);
        }

        var str = k.ToCharArray();
        var m = str.Length;
        var dp = new int[n, m + 1];
        // hashmap
        // dp[,] == -1 : 表示没算过！
        for (var i = 0; i < n; i++)
        for (var j = 0; j <= m; j++)
            dp[i, j] = -1;
        return Process(0, 0, str, map, n, dp);
    }

    // 电话里：指针指着的上一个按键preButton
    // 目标里：此时要搞定哪个字符？keyIndex
    // map : key 一种字符 value: 哪些位置拥有这个字符
    // N: 电话大小
    // f(0, 0, aim, map, N)
    private static int Process(int preButton, int index, char[] str, Dictionary<char, List<int>> map, int n, int[,] dp)
    {
        if (dp[preButton, index] != -1) return dp[preButton, index];
        var ans = int.MaxValue;
        if (index == str.Length)
        {
            ans = 0;
        }
        else
        {
            // 还有字符需要搞定呢！
            var cur = str[index];
            var nextPositions = map[cur];
            foreach (var next in nextPositions)
            {
                var cost = Dial(preButton, next, n) + 1 + Process(next, index + 1, str, map, n, dp);
                ans = Math.Min(ans, cost);
            }
        }

        dp[preButton, index] = ans;
        return ans;
    }

    private static int Dial(int i1, int i2, int size)
    {
        return Math.Min(Math.Abs(i1 - i2), Math.Min(i1, i2) + size - Math.Max(i1, i2));
    }

    public static void Run()
    {
        Console.WriteLine(FindRotateSteps("godding", "godding")); //输出13
    }
}