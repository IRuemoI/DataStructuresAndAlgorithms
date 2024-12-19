namespace AdvancedTraining.Lesson49;

public class CombinationSumIv //leetcode_0377
{
    private static readonly int[] Dp = new int[1001];

    // 当前剩下的值是rest，
    // nums中所有的值，都可能作为分解rest的，第一块！全试一试
    // nums, 无重复，都是正
    // rest,
    private static int Ways(int rest, int[] numbers)
    {
        if (rest < 0) return 0;
        if (rest == 0) return 1;
        var ways = 0;
        foreach (var num in numbers) ways += Ways(rest - num, numbers);
        return ways;
    }

    //这个有问题
    private static int CombinationSum41(int[] numbers, int target)
    {
        Array.Fill(Dp, 0, target + 1, -1);
        return Process1(numbers, target);
    }

    private static int Process1(int[] numbers, int rest)
    {
        if (rest < 0) return 0;
        if (Dp[rest] != -1) return Dp[rest];
        var ans = 0;
        if (rest == 0)
            ans = 1;
        else
            foreach (var num in numbers)
                ans += Process1(numbers, rest - num);
        Dp[rest] = ans;
        return ans;
    }

    // 剪枝 + 严格位置依赖的动态规划
    private static int CombinationSum42(int[] numbers, int target)
    {
        Array.Sort(numbers);
        var dp = new int[target + 1];
        dp[0] = 1;
        for (var rest = 1; rest <= target; rest++)
        for (var i = 0; i < numbers.Length && numbers[i] <= rest; i++)
            dp[rest] += dp[rest - numbers[i]];
        return dp[target];
    }

    public static void Run()
    {
        var numbers = new[] { 1, 2, 3 };
        var target = 4;
        Console.WriteLine(Ways(target, numbers));
        //Console.WriteLine(CombinationSum41(numbers, target)); //输出7
        Console.WriteLine(CombinationSum42(numbers, target)); //输出7
    }
}