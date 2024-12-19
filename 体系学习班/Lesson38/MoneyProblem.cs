//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson38;

public class MoneyProblem
{
    // int[] d d[i]：i号怪兽的武力
    // int[] p p[i]：i号怪兽要求的钱
    // ability 当前你所具有的能力
    // index 来到了第index个怪兽的面前

    // 目前，你的能力是ability，你来到了index号怪兽的面前，如果要通过后续所有的怪兽，
    // 请返回需要花的最少钱数
    private static long Process1(int[] d, int[] p, int ability, int index)
    {
        if (index == d.Length) return 0;
        if (ability < d[index])
            return p[index] + Process1(d, p, ability + d[index], index + 1);
        // ability >= d[index] 可以贿赂，也可以不贿赂
        return Math.Min(p[index] + Process1(d, p, ability + d[index], index + 1),
            0 + Process1(d, p, ability, index + 1));
    }

    private static long Func1(int[] d, int[] p)
    {
        return Process1(d, p, 0, 0);
    }

    // 从0....index号怪兽，花的钱，必须严格==money
    // 如果通过不了，返回-1
    // 如果可以通过，返回能通过情况下的最大能力值
    private static long Process2(int[] d, int[] p, int index, int money)
    {
        if (index == -1)
            // 一个怪兽也没遇到呢
            return money == 0 ? 0 : -1;
        // index >= 0
        // 1) 不贿赂当前index号怪兽
        var preMaxAbility = Process2(d, p, index - 1, money);
        long p1 = -1;
        if (preMaxAbility != -1 && preMaxAbility >= d[index]) p1 = preMaxAbility;
        // 2) 贿赂当前的怪兽 当前的钱 p[index]
        var preMaxAbility2 = Process2(d, p, index - 1, money - p[index]);
        long p2 = -1;
        if (preMaxAbility2 != -1) p2 = d[index] + preMaxAbility2;
        return Math.Max(p1, p2);
    }

    private static int MinMoney2(int[] d, int[] p)
    {
        var allMoney = 0;
        foreach (var element in p)
            allMoney += element;

        var n = d.Length;
        for (var money = 0; money < allMoney; money++)
            if (Process2(d, p, n - 1, money) != -1)
                return money;
        return allMoney;
    }

    private static long Func2(int[] d, int[] p)
    {
        var sum = 0;
        foreach (var num in d) sum += num;
        var dp = new int[d.Length + 1, sum + 1];
        for (var i = 0; i <= sum; i++) dp[0, i] = 0;
        for (var cur = d.Length - 1; cur >= 0; cur--)
        for (var hp = 0; hp <= sum; hp++)
        {
            // 如果这种情况发生，那么这个hp必然是递归过程中不会出现的状态
            // 既然动态规划是尝试过程的优化，尝试过程碰不到的状态，不必计算
            if (hp + d[cur] > sum) continue;
            if (hp < d[cur])
                dp[cur, hp] = p[cur] + dp[cur + 1, hp + d[cur]];
            else
                dp[cur, hp] = Math.Min(p[cur] + dp[cur + 1, hp + d[cur]], dp[cur + 1, hp]);
        }

        return dp[0, 0];
    }

    private static long Func3(int[] d, int[] p)
    {
        var sum = 0;
        foreach (var num in p) sum += num;
        // dp[i][j]含义：
        // 能经过0～i的怪兽，且花钱为j（花钱的严格等于j）时的武力值最大是多少？
        // 如果dp[i][j]==-1，表示经过0～i的怪兽，花钱为j是无法通过的，或者之前的钱怎么组合也得不到正好为j的钱数
        var dp = new int[d.Length, sum + 1];
        for (var i = 0; i < dp.Length; i++)
        for (var j = 0; j <= sum; j++)
            dp[i, j] = -1;
        // 经过0～i的怪兽，花钱数一定为p[0]，达到武力值d[0]的地步。其他第0行的状态一律是无效的
        dp[0, p[0]] = d[0];
        for (var i = 1; i < d.Length; i++)
        for (var j = 0; j <= sum; j++)
        {
            // 可能性一，为当前怪兽花钱
            // 存在条件：
            // j - p[i]要不越界，并且在钱数为j - p[i]时，要能通过0～i-1的怪兽，并且钱数组合是有效的。
            if (j >= p[i] && dp[i - 1, j - p[i]] != -1) dp[i, j] = dp[i - 1, j - p[i]] + d[i];
            // 可能性二，不为当前怪兽花钱
            // 存在条件：
            // 0~i-1怪兽在花钱为j的情况下，能保证通过当前i位置的怪兽
            if (dp[i - 1, j] >= d[i])
                // 两种可能性中，选武力值最大的
                dp[i, j] = Math.Max(dp[i, j], dp[i - 1, j]);
        }

        var ans = 0;
        // dp表最后一行上，dp[N-1][j]代表：
        // 能经过0～N-1的怪兽，且花钱为j（花钱的严格等于j）时的武力值最大是多少？
        // 那么最后一行上，最左侧的不为-1的列数(j)，就是答案
        for (var j = 0; j <= sum; j++)
            if (dp[d.Length - 1, j] != -1)
            {
                ans = j;
                break;
            }

        return ans;
    }

    private static int[][] GenerateTwoRandomArray(int len, int value)
    {
        var size = (int)(Utility.getRandomDouble * len) + 1;
        int[][] arrays = [new int[size], new int[size]];
        for (var i = 0; i < size; i++)
        {
            arrays[0][i] = (int)(Utility.getRandomDouble * value) + 1;
            arrays[1][i] = (int)(Utility.getRandomDouble * value) + 1;
        }

        return arrays;
    }

    public static void Run()
    {
        var len = 10;
        var value = 20;
        var testTimes = 10000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTimes; i++)
        {
            var arrays = GenerateTwoRandomArray(len, value);
            var d = arrays[0];
            var p = arrays[1];
            var ans1 = Func1(d, p);
            var ans2 = Func2(d, p);
            var ans3 = Func3(d, p);
            long ans4 = MinMoney2(d, p);
            if (ans1 != ans2 || ans2 != ans3 || ans1 != ans4) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试结束");
    }
}