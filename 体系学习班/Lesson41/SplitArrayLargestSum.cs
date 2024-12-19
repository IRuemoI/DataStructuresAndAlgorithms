//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson41;

// leetcode原题
// 测试链接：https://leetcode.cn/problems/split-array-largest-sum/
public class SplitArrayLargestSum
{
    // 求原数组arr[L...R]的累加和
    private static int Sum(int[] sum, int l, int r)
    {
        return sum[r + 1] - sum[l];
    }

    // 不优化枚举的动态规划方法，O(N^2 * K)
    private static int SplitArray1(int[] nums, int k)
    {
        var n = nums.Length;
        var sum = new int[n + 1];
        for (var i = 0; i < n; i++) sum[i + 1] = sum[i] + nums[i];

        var dp = new int [n, k + 1];
        for (var j = 1; j <= k; j++) dp[0, j] = nums[0];

        for (var i = 1; i < n; i++) dp[i, 1] = Sum(sum, 0, i);

        // 每一行从上往下
        // 每一列从左往右
        // 根本不去凑优化位置对儿！
        for (var i = 1; i < n; i++)
        for (var j = 2; j <= k; j++)
        {
            var ans = int.MaxValue;
            // 枚举是完全不优化的！
            for (var leftEnd = 0; leftEnd <= i; leftEnd++)
            {
                var leftCost = leftEnd == -1 ? 0 : dp[leftEnd, j - 1];
                var rightCost = leftEnd == i ? 0 : Sum(sum, leftEnd + 1, i);
                var cur = Math.Max(leftCost, rightCost);
                if (cur < ans) ans = cur;
            }

            dp[i, j] = ans;
        }

        return dp[n - 1, k];
    }

    // 课上现场写的方法，用了枚举优化，O(N * K)
    private static int SplitArray2(int[] nums, int k)
    {
        var n = nums.Length;
        var sum = new int[n + 1];
        for (var i = 0; i < n; i++) sum[i + 1] = sum[i] + nums[i];
        var dp = new int[n, k + 1];
        var best = new int[n, k + 1];
        for (var j = 1; j <= k; j++)
        {
            dp[0, j] = nums[0];
            best[0, j] = -1;
        }

        for (var i = 1; i < n; i++)
        {
            dp[i, 1] = Sum(sum, 0, i);
            best[i, 1] = -1;
        }

        // 从第2列开始，从左往右
        // 每一列，从下往上
        // 为什么这样的顺序？因为要去凑（左，下）优化位置对儿！
        for (var j = 2; j <= k; j++)
        for (var i = n - 1; i >= 1; i--)
        {
            var down = best[i, j - 1];
            // 如果i==N-1，则不优化上限
            var up = i == n - 1 ? n - 1 : best[i + 1, j];
            var ans = int.MaxValue;
            var bestChoose = -1;
            for (var leftEnd = down; leftEnd <= up; leftEnd++)
            {
                var leftCost = leftEnd == -1 ? 0 : dp[leftEnd, j - 1];
                var rightCost = leftEnd == i ? 0 : Sum(sum, leftEnd + 1, i);
                var cur = Math.Max(leftCost, rightCost);
                // 注意下面的if一定是 < 课上的错误就是此处！当时写的 <= ！
                // 也就是说，只有取得明显的好处才移动！
                // 举个例子来说明，比如[2,6,4,4]，3个画匠时候，如下两种方案都是最优:
                // (2,6) (4) 两个画匠负责 | (4) 最后一个画匠负责
                // (2,6) (4,4)两个画匠负责 | 最后一个画匠什么也不负责
                // 第一种方案划分为，[0~2] [3~3]
                // 第二种方案划分为，[0~3] [无]
                // 两种方案的答案都是8，但是划分点位置一定不要移动!
                // 只有明显取得好处时(<)，划分点位置才移动!
                // 也就是说后面的方案如果==前面的最优，不要移动！只有优于前面的最优，才移动
                // 比如上面的两个方案，如果你移动到了方案二，你会得到:
                // [2,6,4,4] 三个画匠时，最优为[0~3](前两个画家) [无](最后一个画家)，
                // 最优划分点为3位置(best[3,3])
                // 那么当4个画匠时，也就是求解dp[3,4]时
                // 因为best[3,3] = 3，这个值提供了dp[3,4]的下限
                // 而事实上dp[3,4]的最优划分为:
                // [0~2]（三个画家处理） [3~3] (一个画家处理)，此时最优解为6
                // 所以，你就得不到dp[3,4]的最优解了，因为划分点已经越过2了
                // 提供了对数器验证，你可以改成<=，对数器和leetcode都过不了
                // 这里是<，对数器和leetcode都能通过
                // 这里面会让同学们感到困惑的点：
                // 为啥==的时候，不移动，只有<的时候，才移动呢？例子懂了，但是道理何在？
                // 哈哈哈哈哈，看了邮局选址问题，你更懵，请看42节！
                if (cur < ans)
                {
                    ans = cur;
                    bestChoose = leftEnd;
                }
            }

            dp[i, j] = ans;
            best[i, j] = bestChoose;
        }

        return dp[n - 1, k];
    }

    private static int SplitArray3(int[] nums, int m)
    {
        long sum = 0;
        foreach (var element in nums) sum += element;

        long l = 0;
        var r = sum;
        long ans = 0;
        while (l <= r)
        {
            var mid = (l + r) / 2;
            long cur = GetNeedParts(nums, mid);
            if (cur <= m)
            {
                ans = mid;
                r = mid - 1;
            }
            else
            {
                l = mid + 1;
            }
        }

        return (int)ans;
    }

    private static int GetNeedParts(int[] arr, long aim)
    {
        foreach (var element in arr)
            if (element > aim)
                return int.MaxValue;

        var parts = 1;
        var all = arr[0];
        for (var i = 1; i < arr.Length; i++)
            if (all + arr[i] > aim)
            {
                parts++;
                all = arr[i];
            }
            else
            {
                all += arr[i];
            }

        return parts;
    }

    private static int[] RandomArray(int len, int maxValue)
    {
        var arr = new int[len];
        for (var i = 0; i < len; i++) arr[i] = (int)(Utility.getRandomDouble * maxValue);

        return arr;
    }

    private static void PrintArray(int[] arr)
    {
        foreach (var element in arr)
            Console.Write(element + " ");

        Console.WriteLine();
    }

    public static void Run()
    {
        var n = 100;
        var maxValue = 100;
        var testTime = 10000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var len = (int)(Utility.getRandomDouble * n) + 1;
            var m = (int)(Utility.getRandomDouble * n) + 1;
            var arr = RandomArray(len, maxValue);
            var ans1 = SplitArray1(arr, m);
            var ans2 = SplitArray2(arr, m);
            var ans3 = SplitArray3(arr, m);
            if (ans1 != ans2 || ans1 != ans3)
            {
                Console.Write("arr : ");
                PrintArray(arr);
                Console.WriteLine("M : " + m);
                Console.WriteLine("ans1 : " + ans1);
                Console.WriteLine("ans2 : " + ans2);
                Console.WriteLine("ans3 : " + ans3);
                Console.WriteLine("出错啦！");
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}