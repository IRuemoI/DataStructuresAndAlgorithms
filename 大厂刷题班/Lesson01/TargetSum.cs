//pass

namespace AdvancedTraining.Lesson01;

// leetcode 494题
// https://leetcode.cn/problems/target-sum/description/
public class TargetSum
{
    private static int FindTargetSumWays1(int[] arr, int s)
    {
        return Process1(arr, 0, s);
    }

    // 可以自由使用arr[index....]所有的数字！
    // 搞出rest这个数，方法数是多少？返回
    // index == 7 rest = 13
    // map "7_13" 256
    private static int Process1(int[] arr, int index, int rest)
    {
        if (index == arr.Length)
            // 没数了！
            return rest == 0 ? 1 : 0;
        // 还有数！arr[index] arr[index+1 ... ]
        return Process1(arr, index + 1, rest - arr[index]) + Process1(arr, index + 1, rest + arr[index]);
    }

    private static int FindTargetSumWays2(int[] arr, int s)
    {
        return Process2(arr, 0, s, new Dictionary<int, Dictionary<int, int>>());
    }

    private static int Process2(int[] arr, int index, int rest, Dictionary<int, Dictionary<int, int>> dp)
    {
        if (dp.ContainsKey(index) && dp[index].ContainsKey(rest)) return dp[index][rest];
        // 否则，没命中！
        int ans;
        if (index == arr.Length)
            ans = rest == 0 ? 1 : 0;
        else
            ans = Process2(arr, index + 1, rest - arr[index], dp) + Process2(arr, index + 1, rest + arr[index], dp);

        if (!dp.ContainsKey(index)) dp[index] = new Dictionary<int, int>();
        dp[index][rest] = ans;
        return ans;
    }

    // 优化点一 :
    // 你可以认为arr中都是非负数
    // 因为即便是arr中有负数，比如[3,-4,2]
    // 因为你能在每个数前面用+或者-号
    // 所以[3,-4,2]其实和[3,4,2]达成一样的效果
    // 那么我们就全把arr变成非负数，不会影响结果的
    // 优化点二 :
    // 如果arr都是非负数，并且所有数的累加和是sum
    // 那么如果target<sum，很明显没有任何方法可以达到target，可以直接返回0
    // 优化点三 :
    // arr内部的数组，不管怎么+和-，最终的结果都一定不会改变奇偶性
    // 所以，如果所有数的累加和是sum，
    // 并且与target的奇偶性不一样，没有任何方法可以达到target，可以直接返回0
    // 优化点四 :
    // 比如说给定一个数组, arr = [1, 2, 3, 4, 5] 并且 target = 3
    // 其中一个方案是 : +1 -2 +3 -4 +5 = 3
    // 该方案中取了正的集合为P = {1，3，5}
    // 该方案中取了负的集合为N = {2，4}
    // 所以任何一种方案，都一定有 sum(P) - sum(N) = target
    // 现在我们来处理一下这个等式，把左右两边都加上sum(P) + sum(N)，那么就会变成如下：
    // sum(P) - sum(N) + sum(P) + sum(N) = target + sum(P) + sum(N)
    // 2 * sum(P) = target + 数组所有数的累加和
    // sum(P) = (target + 数组所有数的累加和) / 2
    // 也就是说，任何一个集合，只要累加和是(target + 数组所有数的累加和) / 2
    // 那么就一定对应一种target的方式
    // 也就是说，比如非负数组arr，target = 7, 而所有数累加和是11
    // 求有多少方法组成7，其实就是求有多少种达到累加和(7+11)/2=9的方法
    // 优化点五 :
    // 二维动态规划的空间压缩技巧
    private static int FindTargetSumWays(int[] arr, int target)
    {
        var sum = 0;
        foreach (var n in arr) sum += n;

        return sum < target || ((target & 1) ^ (sum & 1)) != 0 ? 0 : Subset2(arr, (target + sum) >> 1);
    }

    // 求非负数组numbers有多少个子集，累加和是s
    // 二维动态规划
    // 不用空间压缩
    private static int Subset1(int[] numbers, int s)
    {
        if (s < 0) return 0;
        var n = numbers.Length;
        // dp[i,j] : numbers前缀长度为i的所有子集，有多少累加和是j？
        var dp = new int[n + 1, s + 1];
        // numbers前缀长度为0的所有子集，有多少累加和是0？一个：空集
        dp[0, 0] = 1;
        for (var i = 1; i <= n; i++)
        for (var j = 0; j <= s; j++)
        {
            dp[i, j] = dp[i - 1, j];
            if (j - numbers[i - 1] >= 0) dp[i, j] += dp[i - 1, j - numbers[i - 1]];
        }

        return dp[n, s];
    }

    // 求非负数组numbers有多少个子集，累加和是s
    // 二维动态规划
    // 用空间压缩:
    // 核心就是for循环里面的：for (int i = s; i >= n; i--) {
    // 为啥不枚举所有可能的累加和？只枚举 n...s 这些累加和？
    // 因为如果 i - n < 0，dp[i]怎么更新？和上一步的dp[i]一样！所以不用更新
    // 如果 i - n >= 0，dp[i]怎么更新？上一步的dp[i] + 上一步dp[i - n]的值，这才需要更新
    private static int Subset2(int[] numbers, int s)
    {
        if (s < 0) return 0;
        var dp = new int[s + 1];
        dp[0] = 1;
        foreach (var n in numbers)
            for (var i = s; i >= n; i--)
                dp[i] += dp[i - n];

        return dp[s];
    }

    public static void Run()
    {
        int[] arr = [1, 1, 1, 1, 1];
        const int target = 3;

        Console.WriteLine(FindTargetSumWays1(arr, target)); //输出5
        Console.WriteLine(FindTargetSumWays2(arr, target)); //输出5
        Console.WriteLine(FindTargetSumWays(arr, target)); //输出5
    }
}