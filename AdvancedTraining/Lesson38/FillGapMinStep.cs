namespace AdvancedTraining.Lesson38;

// 来自字节
// 给定两个数a和b
// 第1轮，把1选择给a或者b
// 第2轮，把2选择给a或者b
// ...
// 第i轮，把i选择给a或者b
// 想让a和b的值一样大，请问至少需要多少轮？
public class FillGapMinStep
{
    // 暴力方法
    // 不要让a、b过大！
    private static int MinStep0(int a, int b)
    {
        if (a == b) return 0;
        var limit = 15;
        return Process(a, b, 1, limit);
    }

    private static int Process(int a, int b, int i, int n)
    {
        if (i > n) return int.MaxValue;
        if (a + i == b || a == b + i) return i;
        return Math.Min(Process(a + i, b, i + 1, n), Process(a, b + i, i + 1, n));
    }

    private static int MinStep1(int a, int b)
    {
        if (a == b) return 0;
        var s = Math.Abs(a - b);
        var num = 1;
        var sum = 0;
        for (; !(sum >= s && (sum - s) % 2 == 0); num++) sum += num;
        return num - 1;
    }

    private static int MinStep2(int a, int b)
    {
        if (a == b) return 0;
        var s = Math.Abs(a - b);
        // 找到sum >= s, 最小的i
        var begin = Best(s << 1);
        for (; (begin * (begin + 1) / 2 - s) % 2 != 0;) begin++;
        return begin;
    }

    private static int Best(int s2)
    {
        var l = 0;
        var r = 1;
        for (; r * (r + 1) < s2;)
        {
            l = r;
            r *= 2;
        }

        var ans = 0;
        while (l <= r)
        {
            var m = (l + r) / 2;
            if (m * (m + 1) >= s2)
            {
                ans = m;
                r = m - 1;
            }
            else
            {
                l = m + 1;
            }
        }

        return ans;
    }

    public static void Run()
    {
        Console.WriteLine("功能测试开始");
        for (var a = 1; a < 100; a++)
        for (var b = 1; b < 100; b++)
        {
            var ans1 = MinStep0(a, b);
            var ans21 = MinStep1(a, b);
            var ans31 = MinStep2(a, b);
            if (ans1 != ans21 || ans1 != ans31)
            {
                Console.WriteLine("出错了！");
                Console.WriteLine(a + " , " + b);
                break;
            }
        }

        Console.WriteLine("功能测试结束");

        var a1 = 19019;
        var b1 = 8439284;
        var ans2 = MinStep1(a1, b1);
        var ans3 = MinStep2(a1, b1);
        Console.WriteLine(ans2);
        Console.WriteLine(ans3);
    }
}