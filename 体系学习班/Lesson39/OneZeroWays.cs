//测试通过

namespace Algorithms.Lesson39;

public class OneZeroWays
{
    private static long Ways1(int n)
    {
        var zero = n;
        var one = n;
        var path = new LinkedList<int>();
        var ans = new LinkedList<LinkedList<int>>();
        Process(zero, one, path, ans);
        long count = 0;
        foreach (var cur in ans)
        {
            var status = 0;
            foreach (int? num in cur)
            {
                if (num == 0)
                    status++;
                else
                    status--;

                if (status < 0) break;
            }

            if (status == 0) count++;
        }

        return count;
    }

    private static void Process(int zero, int one, LinkedList<int> path, LinkedList<LinkedList<int>> ans)
    {
        if (zero == 0 && one == 0)
        {
            var cur = new LinkedList<int>();
            foreach (var num in path) cur.AddLast(num);

            ans.AddLast(cur);
        }
        else
        {
            if (zero == 0)
            {
                path.AddLast(1);
                Process(zero, one - 1, path, ans);
                path.RemoveLast();
            }
            else if (one == 0)
            {
                path.AddLast(0);
                Process(zero - 1, one, path, ans);
                path.RemoveLast();
            }
            else
            {
                path.AddLast(1);
                Process(zero, one - 1, path, ans);
                path.RemoveLast();
                path.AddLast(0);
                Process(zero - 1, one, path, ans);
                path.RemoveLast();
            }
        }
    }

    private static long Ways2(int n)
    {
        if (n < 0) return 0;

        if (n < 2) return 1;

        long a = 1;
        long b = 1;
        long limit = n << 1;
        for (long i = 1; i <= limit; i++)
            if (i <= n)
                a *= i;
            else
                b *= i;

        return b / a / (n + 1);
    }

    public static void Run()
    {
        Console.WriteLine("测试开始");
        for (var i = 0; i < 10; i++)
        {
            var ans1 = Ways1(i);
            var ans2 = Ways2(i);
            if (ans1 != ans2) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");
    }
}