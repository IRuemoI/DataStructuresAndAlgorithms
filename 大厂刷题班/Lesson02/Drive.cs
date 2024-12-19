#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson02;

public class Drive
{
    // 课上的现场版本
    // income -> N * 2 的矩阵 N是偶数！
    // 0 [9, 13]
    // 1 [45,60]
    private static int MaxMoney1(int[,]? income)
    {
        if (income == null || income.Length < 2 || (income.Length & 1) != 0) return 0;
        var n = income.Length; // 司机数量一定是偶数，所以才能平分，A N /2 B N/2
        var m = n >> 1; // M = N / 2 要去A区域的人
        return Process1(income, 0, m);
    }

    // index.....所有的司机，往A和B区域分配！
    // A区域还有rest个名额!
    // 返回把index...司机，分配完，并且最终A和B区域同样多的情况下，index...这些司机，整体收入最大是多少！
    private static int Process1(int[,] income, int index, int rest)
    {
        if (index == income.Length) return 0;
        // 还剩下司机！
        if (income.Length - index == rest) return income[index, 0] + Process1(income, index + 1, rest - 1);
        if (rest == 0) return income[index, 1] + Process1(income, index + 1, rest);
        // 当前司机，可以去A，或者去B
        var p1 = income[index, 0] + Process1(income, index + 1, rest - 1);
        var p2 = income[index, 1] + Process1(income, index + 1, rest);
        return Math.Max(p1, p2);
    }

    // 严格位置依赖的动态规划版本
    private static int MaxMoney2(int[,] income)
    {
        var n = income.Length;
        var m = n >> 1;
        var dp = new int[n + 1, m + 1];
        for (var i = n - 1; i >= 0; i--)
        for (var j = 0; j <= m; j++)
            if (n - i == j)
            {
                dp[i, j] = income[i, 0] + dp[i + 1, j - 1];
            }
            else if (j == 0)
            {
                dp[i, j] = income[i, 1] + dp[i + 1, j];
            }
            else
            {
                var p1 = income[i, 0] + dp[i + 1, j - 1];
                var p2 = income[i, 1] + dp[i + 1, j];
                dp[i, j] = Math.Max(p1, p2);
            }

        return dp[0, m];
    }

    // 这题有贪心策略 :
    // 假设一共有10个司机，思路是先让所有司机去A，得到一个总收益
    // 然后看看哪5个司机改换门庭(去B)，可以获得最大的额外收益
    // 这道题有贪心策略，打了我的脸
    // 但是我课上提到的技巧请大家重视
    // 根据数据量猜解法可以省去大量的多余分析，节省时间
    // 这里感谢卢圣文同学
    private static int MaxMoney3(int[,] income)
    {
        var n = income.Length;
        var arr = new int[n];
        var sum = 0;
        for (var i = 0; i < n; i++)
        {
            arr[i] = income[i, 1] - income[i, 0];
            sum += income[i, 0];
        }

        Array.Sort(arr);
        var m = n >> 1;
        for (var i = n - 1; i >= m; i--) sum += arr[i];
        return sum;
    }

    // 返回随机len*2大小的正数矩阵
    // 值在0~value-1之间
    private static int[,] randomMatrix(int len, int value)
    {
        var ans = new int[len << 1, 2];
        for (var i = 0; i < ans.Length; i++)
        {
            ans[i, 0] = (int)(Utility.getRandomDouble * value);
            ans[i, 1] = (int)(Utility.getRandomDouble * value);
        }

        return ans;
    }

    public static void Run()
    {
        const int n = 10;
        const int value = 100;
        const int testTime = 500;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var len = (int)(Utility.getRandomDouble * n) + 1;
            var matrix = randomMatrix(len, value);
            var ans1 = MaxMoney1(matrix);
            var ans2 = MaxMoney2(matrix);
            var ans3 = MaxMoney3(matrix);
            if (ans1 != ans2 || ans1 != ans3)
            {
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                Console.WriteLine(ans3);
                Console.WriteLine("出错啦！");
            }
        }

        Console.WriteLine("测试结束");
    }
}