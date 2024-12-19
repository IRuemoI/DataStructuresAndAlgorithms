namespace CustomTraining.Leetcode;

//动态规划题:https://leetcode.cn/problems/jian-sheng-zi-lcof/
public static class CuttingBamboo
{
    private static int CuttingBambooCode(int bambooLength)
    {
        var dp = new int [bambooLength + 1];
        for (var i = 2; i < bambooLength + 1; i++)
        {
            var curMax = 0;
            for (var j = 1; j < i; j++)
                //找当前值，不继续拆，继续拆的三种乘积的最大值
                curMax = Math.Max(curMax, Math.Max(j * (i - j), j * dp[i - j]));

            dp[i] = curMax;
        }

        return dp[bambooLength];
    }

    public static void Run()
    {
        Console.WriteLine(CuttingBambooCode(12));
    }
}