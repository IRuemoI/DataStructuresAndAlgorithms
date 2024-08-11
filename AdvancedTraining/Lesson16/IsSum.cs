#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson16;

// 这道题是一个小小的补充，课上没有讲
// 但是如果你听过体系学习班动态规划专题和本节课的话
// 这道题就是一道水题
public class IsSum
{
    // arr中的值可能为正，可能为负，可能为0
    // 自由选择arr中的数字，能不能累加得到sum
    // 暴力递归方法
    private static bool IsSum1(int[]? arr, int sum)
    {
        if (sum == 0) return true;
        if (arr == null || arr.Length == 0) return false;
        return Process1(arr, arr.Length - 1, sum);
    }

    // 可以自由使用arr[0...i]上的数字，能不能累加得到sum
    private static bool Process1(int[] arr, int i, int sum)
    {
        if (sum == 0) return true;
        if (i == -1) return false;
        return Process1(arr, i - 1, sum) || Process1(arr, i - 1, sum - arr[i]);
    }

    // arr中的值可能为正，可能为负，可能为0
    // 自由选择arr中的数字，能不能累加得到sum
    // 记忆化搜索方法
    // 从暴力递归方法来，加了记忆化缓存，就是动态规划了
    private static bool IsSum2(int[]? arr, int sum)
    {
        if (sum == 0) return true;
        if (arr == null || arr.Length == 0) return false;
        return Process2(arr, arr.Length - 1, sum, new Dictionary<int, Dictionary<int, bool>>());
    }

    private static bool Process2(int[] arr, int i, int sum, Dictionary<int, Dictionary<int, bool>> dp)
    {
        if (dp.ContainsKey(i) && dp[i].ContainsKey(sum)) return dp[i][sum];
        var ans = false;
        if (sum == 0)
            ans = true;
        else if (i != -1) ans = Process2(arr, i - 1, sum, dp) || Process2(arr, i - 1, sum - arr[i], dp);
        if (!dp.ContainsKey(i)) dp[i] = new Dictionary<int, bool>();
        dp[i][sum] = ans;
        return ans;
    }

    // arr中的值可能为正，可能为负，可能为0
    // 自由选择arr中的数字，能不能累加得到sum
    // 经典动态规划
    private static bool IsSum3(int[]? arr, int sum)
    {
        if (sum == 0) return true;
        // sum != 0
        if (arr == null || arr.Length == 0) return false;
        // arr有数，sum不为0
        var min = 0;
        var max = 0;
        foreach (var num in arr)
        {
            min += num < 0 ? num : 0;
            max += num > 0 ? num : 0;
        }

        // min~max
        if (sum < min || sum > max) return false;

        // min <= sum <= max
        var n = arr.Length;
        // dp[i,j]
        // 
        //  0   1   2   3  4    5   6    7 (实际的对应)
        // -7  -6  -5  -4  -3   -2  -1   0（想象中）
        // 
        // dp[0,-min] -> dp[0,7] -> dp[0,0]
        var dp = new bool[n, max - min + 1];
        // dp[0,0] = true
        dp[0, -min] = true;
        // dp[0,arr[0]] = true
        dp[0, arr[0] - min] = true;
        for (var i = 1; i < n; i++)
        for (var j = min; j <= max; j++)
        {
            // dp[i,j] = dp[i-1,j]
            dp[i, j - min] = dp[i - 1, j - min];
            // dp[i,j] |= dp[i-1,j - arr[i]]
            var next = j - min - arr[i];
            dp[i, j - min] |= next >= 0 && next <= max - min && dp[i - 1, next];
        }

        // dp[N-1,sum]
        return dp[n - 1, sum - min];
    }

    // arr中的值可能为正，可能为负，可能为0
    // 自由选择arr中的数字，能不能累加得到sum
    // 分治的方法
    // 如果arr中的数值特别大，动态规划方法依然会很慢
    // 此时如果arr的数字个数不算多(40以内)，哪怕其中的数值很大，分治的方法也将是最优解
    private static bool IsSum4(int[]? arr, int sum)
    {
        if (sum == 0) return true;
        if (arr == null || arr.Length == 0) return false;
        if (arr.Length == 1) return arr[0] == sum;
        var n = arr.Length;
        var mid = n >> 1;
        var leftSum = new HashSet<int>();
        var rightSum = new HashSet<int>();
        // 0...mid-1
        Process4(arr, 0, mid, 0, leftSum);
        // mid..N-1
        Process4(arr, mid, n, 0, rightSum);
        // 单独查看，只使用左部分，能不能搞出sum
        // 单独查看，只使用右部分，能不能搞出sum
        // 左+右，联合能不能搞出sum
        // 左部分搞出所有累加和的时候，包含左部分一个数也没有，这种情况的，leftsum表里，0
        // 17  17
        foreach (var l in leftSum)
            if (rightSum.Contains(sum - l))
                return true;
        return false;
    }

    // arr[0...i-1]决定已经做完了！形成的累加和是pre
    // arr[i...end - 1] end(终止)  所有数字随意选择，
    // arr[0...end-1]所有可能的累加和存到ans里去
    private static void Process4(int[] arr, int i, int end, int pre, HashSet<int> ans)
    {
        if (i == end)
        {
            ans.Add(pre);
        }
        else
        {
            Process4(arr, i + 1, end, pre, ans);
            Process4(arr, i + 1, end, pre + arr[i], ans);
        }
    }

    // 为了测试
    // 生成长度为len的随机数组
    // 值在[-max, max]上随机
    private static int[] randomArray(int len, int max)
    {
        var arr = new int[len];
        for (var i = 0; i < len; i++) arr[i] = (int)(Utility.GetRandomDouble * ((max << 1) + 1)) - max;
        return arr;
    }

    // 对数器验证所有方法
    public static void Run()
    {
        const int n = 20;
        const int m = 100;
        var testTime = 100000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var size = (int)(Utility.GetRandomDouble * (n + 1));
            var arr = randomArray(size, m);
            var sum = (int)(Utility.GetRandomDouble * ((m << 1) + 1)) - m;
            var ans1 = IsSum1(arr, sum);
            var ans2 = IsSum2(arr, sum);
            var ans3 = IsSum3(arr, sum);
            var ans4 = IsSum4(arr, sum);
            if (ans1 ^ ans2 || ans3 ^ ans4 || ans1 ^ ans3)
            {
                Console.WriteLine("出错了！");
                Console.Write("arr : ");
                foreach (var num in arr) Console.Write(num + " ");
                Console.WriteLine();
                Console.WriteLine("sum : " + sum);
                Console.WriteLine("方法一答案 : " + ans1);
                Console.WriteLine("方法二答案 : " + ans2);
                Console.WriteLine("方法三答案 : " + ans3);
                Console.WriteLine("方法四答案 : " + ans4);
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}