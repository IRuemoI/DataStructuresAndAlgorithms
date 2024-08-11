//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson18;

public class RobotWalk
{
    private static int Ways1(int n, int start, int target, int k)
    {
        if (n < 2 || start < 1 || start > n || target < 1 || target > n || k < 1) return -1;

        return Process1(start, k, target, n);
    }

    // 机器人当前来到的位置是cur，
    // 机器人还有rest步需要去走，
    // 最终的目标是aim，
    // 有哪些位置？1~N
    // 返回：机器人从cur出发，走过rest步之后，最终停在aim的方法数，是多少？
    private static int Process1(int cur, int rest, int target, int n)
    {
        if (rest == 0)
            // 如果已经不需要走了，走完了！
            return cur == target ? 1 : 0;

        // (cur, rest)
        if (cur == 1)
            // 1 -> 2
            return Process1(2, rest - 1, target, n);

        // (cur, rest)
        if (cur == n)
            // N-1 <- N
            return Process1(n - 1, rest - 1, target, n);

        // (cur, rest)
        return Process1(cur - 1, rest - 1, target, n) + Process1(cur + 1, rest - 1, target, n);
    }

    private static int Ways2(int n, int start, int target, int k)
    {
        if (n < 2 || start < 1 || start > n || target < 1 || target > n || k < 1) return -1;

        var dp = new int[n + 1, k + 1];
        for (var i = 0; i <= n; i++)
        for (var j = 0; j <= k; j++)
            dp[i, j] = -1;

        // dp就是缓存表
        // dp[cur,rest] == -1 -> process1(cur, rest)之前没算过！
        // dp[cur,rest] != -1 -> process1(cur, rest)之前算过！返回值，dp[cur,rest]
        // N+1 * K+1
        return Process2(start, k, target, n, dp);
    }

    // cur 范围: 1 ~ N
    // rest 范围：0 ~ K
    private static int Process2(int cur, int rest, int target, int n, int[,] dp)
    {
        if (dp[cur, rest] != -1) return dp[cur, rest];

        // 之前没算过！
        int ans;
        if (rest == 0)
            ans = cur == target ? 1 : 0;
        else if (cur == 1)
            ans = Process2(2, rest - 1, target, n, dp);
        else if (cur == n)
            ans = Process2(n - 1, rest - 1, target, n, dp);
        else
            ans = Process2(cur - 1, rest - 1, target, n, dp) + Process2(cur + 1, rest - 1, target, n, dp);

        dp[cur, rest] = ans;
        return ans;
    }

    private static int Ways3(int n, int start, int target, int k)
    {
        if (n < 2 || start < 1 || start > n || target < 1 || target > n || k < 1) return -1;

        var dp = new int[n + 1, k + 1]; //行为各个位置，列为剩余步数
        //在目标位置时，向目标位置移动的距离是1，其他位置不是目标位置，所以设置为0，第一列设置完成
        dp[target, 0] = 1;

        //数组遍历方式为先列后行
        for (var rest = 1; rest <= k; rest++)
        {
            // 第"0"列的代码无需处理
            dp[1, rest] = dp[2, rest - 1]; //第"1"列的元素依赖它的左下元素
            for (var cur = 2; cur < n; cur++) //普遍情况下
                dp[cur, rest] = dp[cur - 1, rest - 1] + dp[cur + 1, rest - 1]; //作为普遍情况下的中间部分的结果依赖左上和右上元素
            dp[n, rest] = dp[n - 1, rest - 1]; //第"n"列的元素依赖它的左上元素
        }

        return dp[start, k];
    }

    public static void Run()
    {
        Utility.RestartStopwatch();
        Console.WriteLine("方法一:" + Ways1(5, 2, 4, 6));
        Console.WriteLine("耗时:" + Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法二:" + Ways2(5, 2, 4, 6));
        Console.WriteLine("耗时:" + Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法三:" + Ways3(5, 2, 4, 6));
        Console.WriteLine("耗时:" + Utility.GetStopwatchElapsedMilliseconds() + "ms");
    }
}