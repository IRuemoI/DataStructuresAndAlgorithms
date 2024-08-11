namespace AdvancedTraining.Lesson36;

// 来自京东
// 把一个01字符串切成多个部分，要求每一部分的0和1比例一样，同时要求尽可能多的划分
// 比如 : 01010101
// 01 01 01 01 这是一种切法，0和1比例为 1 : 1
// 0101 0101 也是一种切法，0和1比例为 1 : 1
// 两种切法都符合要求，但是那么尽可能多的划分为第一种切法，部分数为4
// 比如 : 00001111
// 只有一种切法就是00001111整体作为一块，那么尽可能多的划分，部分数为1
// 给定一个01字符串str，假设长度为N，要求返回一个长度为N的数组ans
// 其中ans[i] = str[0...i]这个前缀串，要求每一部分的0和1比例一样，同时要求尽可能多的划分下，部分数是多少
// 输入: str = "010100001"
// 输出: ans = [1, 1, 1, 2, 1, 2, 1, 1, 3]
public class Ratio01Split
{
    // 001010010100...
    private static int[] Split(int[] arr)
    {
        // key : 分子
        // value : 属于key的分母表, 每一个分母，及其 分子/分母 这个比例，多少个前缀拥有
        var pre = new Dictionary<int, Dictionary<int, int>>();
        var n = arr.Length;
        var ans = new int[n];
        var zero = 0; // 0出现的次数
        var one = 0; // 1出现的次数
        for (var i = 0; i < n; i++)
        {
            if (arr[i] == 0)
                zero++;
            else
                one++;
            if (zero == 0 || one == 0)
            {
                ans[i] = i + 1;
            }
            else
            {
                // 0和1，都有数量 -> 最简分数
                var gcd = Gcd(zero, one);
                var a = zero / gcd;
                var b = one / gcd;
                // a / b 比例，之前有多少前缀拥有？ 3+1 4 5+1 6
                if (!pre.ContainsKey(a)) pre[a] = new Dictionary<int, int>();
                if (!pre[a].ContainsKey(b))
                    pre[a][b] = 1;
                else
                    pre[a][b] = pre[a][b] + 1;
                ans[i] = pre[a][b];
            }
        }

        return ans;
    }

    private static int Gcd(int m, int n)
    {
        return n == 0 ? m : Gcd(n, m % n);
    }

    public static void Run()
    {
        int[] arr = [0, 1, 0, 1, 0, 1, 1, 0];
        var ans = Split(arr);
        foreach (var t in ans) Console.Write(t + " ");
    }
}