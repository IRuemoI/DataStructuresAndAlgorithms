namespace AdvancedTraining.Lesson42;

//https://www.jianshu.com/p/a80f5f7b88f9
public class PaintHouseIi //leetcode_0265
{
    // costs[i][k] i号房子用k颜色刷的花费
    // 要让0...N-1的房子相邻不同色
    // 返回最小花费
    private static int MinCostIi(int[][] costs)
    {
        var n = costs.Length;
        if (n == 0) return 0;
        var k = costs[0].Length;
        // 之前取得的最小代价、取得最小代价时的颜色
        var preMin1 = 0;
        var preEnd1 = -1;
        // 之前取得的次小代价、取得次小代价时的颜色
        var preMin2 = 0;
        var preEnd2 = -1;
        for (var i = 0; i < n; i++)
        {
            // i房子
            var curMin1 = int.MaxValue;
            var curEnd1 = -1;
            var curMin2 = int.MaxValue;
            var curEnd2 = -1;
            for (var j = 0; j < k; j++)
                // j颜色！
                if (j != preEnd1)
                {
                    if (preMin1 + costs[i][j] < curMin1)
                    {
                        curMin2 = curMin1;
                        curEnd2 = curEnd1;
                        curMin1 = preMin1 + costs[i][j];
                        curEnd1 = j;
                    }
                    else if (preMin1 + costs[i][j] < curMin2)
                    {
                        curMin2 = preMin1 + costs[i][j];
                        curEnd2 = j;
                    }
                }
                else if (j != preEnd2)
                {
                    if (preMin2 + costs[i][j] < curMin1)
                    {
                        curMin2 = curMin1;
                        curEnd2 = curEnd1;
                        curMin1 = preMin2 + costs[i][j];
                        curEnd1 = j;
                    }
                    else if (preMin2 + costs[i][j] < curMin2)
                    {
                        curMin2 = preMin2 + costs[i][j];
                        curEnd2 = j;
                    }
                }

            preMin1 = curMin1;
            preEnd1 = curEnd1;
            preMin2 = curMin2;
            preEnd2 = curEnd2;
        }

        return preMin1;
    }

    public static void Run()
    {
        Console.WriteLine(MinCostIi([[1, 5, 3], [2, 9, 4]])); //输出5
    }
}