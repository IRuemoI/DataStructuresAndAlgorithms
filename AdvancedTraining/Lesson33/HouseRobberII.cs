namespace AdvancedTraining.Lesson33;
//pass
//https://leetcode.cn/problems/house-robber-ii/description/
public class HouseRobberIi //leetcode_0213
{
    // arr 长度大于等于1
    private static int PickMaxSum(int[] arr)
    {
        var n = arr.Length;
        // dp[i] : arr[0..i]范围上，随意选择，但是，任何两数不能相邻。得到的最大累加和是多少？
        var dp = new int[n];
        dp[0] = arr[0];
        dp[1] = Math.Max(arr[0], arr[1]);
        for (var i = 2; i < n; i++)
        {
            var p1 = arr[i];
            var p2 = dp[i - 1];
            var p3 = arr[i] + dp[i - 2];
            dp[i] = Math.Max(p1, Math.Max(p2, p3));
        }

        return dp[n - 1];
    }

    private static int Rob(int[]? numbers)
    {
        if (numbers == null || numbers.Length == 0) return 0;
        if (numbers.Length == 1) return numbers[0];
        if (numbers.Length == 2) return Math.Max(numbers[0], numbers[1]);
        var pre2 = numbers[0];
        var pre1 = Math.Max(numbers[0], numbers[1]);
        for (var i = 2; i < numbers.Length - 1; i++)
        {
            var tmp = Math.Max(pre1, numbers[i] + pre2);
            pre2 = pre1;
            pre1 = tmp;
        }

        var ans1 = pre1;
        pre2 = numbers[1];
        pre1 = Math.Max(numbers[1], numbers[2]);
        for (var i = 3; i < numbers.Length; i++)
        {
            var tmp = Math.Max(pre1, numbers[i] + pre2);
            pre2 = pre1;
            pre1 = tmp;
        }

        var ans2 = pre1;
        return Math.Max(ans1, ans2);
    }

    public static void Run()
    {
        Console.WriteLine(Rob([2, 3, 2])); //输出3
    }
}