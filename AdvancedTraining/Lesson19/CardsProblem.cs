using Common.Utilities;

namespace AdvancedTraining.Lesson19;

/*
 * 一张扑克有3个属性，每种属性有3种值（A、B、C）
 * 比如"AAA"，第一个属性值A，第二个属性值A，第三个属性值A
 * 比如"BCA"，第一个属性值B，第二个属性值C，第三个属性值A
 * 给定一个字符串类型的数组cards[]，每一个字符串代表一张扑克
 * 从中挑选三张扑克，一个属性达标的条件是：这个属性在三张扑克中全一样，或全不一样
 * 挑选的三张扑克达标的要求是：每种属性都满足上面的条件
 * 比如："ABC"、"CBC"、"BBC"
 * 第一张第一个属性为"A"、第二张第一个属性为"C"、第三张第一个属性为"B"，全不一样
 * 第一张第二个属性为"B"、第二张第二个属性为"B"、第三张第二个属性为"B"，全一样
 * 第一张第三个属性为"C"、第二张第三个属性为"C"、第三张第三个属性为"C"，全一样
 * 每种属性都满足在三张扑克中全一样，或全不一样，所以这三张扑克达标
 * 返回在cards[]中任意挑选三张扑克，达标的方法数
 *
 * */
public class CardsProblem
{
    private static int Ways1(string[] cards)
    {
        var picks = new List<string>();
        return Process1(cards, 0, picks);
    }

    private static int Process1(string[] cards, int index, List<string> picks)
    {
        if (picks.Count == 3) return GetWays1(picks);

        if (index == cards.Length) return 0;

        var ways = Process1(cards, index + 1, picks);
        picks.Add(cards[index]);
        ways += Process1(cards, index + 1, picks);
        picks.RemoveAt(picks.Count - 1);
        return ways;
    }

    private static int GetWays1(List<string> picks)
    {
        var s1 = picks[0].ToCharArray();
        var s2 = picks[1].ToCharArray();
        var s3 = picks[2].ToCharArray();
        for (var i = 0; i < 3; i++)
        {
            if ((s1[i] != s2[i] && s1[i] != s3[i] && s2[i] != s3[i]) || (s1[i] == s2[i] && s1[i] == s3[i])) continue;

            return 0;
        }

        return 1;
    }

    private static int Ways2(string[] cards)
    {
        var counts = new int[27];
        foreach (var s in cards)
        {
            var str = s.ToCharArray();
            counts[(str[0] - 'A') * 9 + (str[1] - 'A') * 3 + (str[2] - 'A') * 1]++;
        }

        var ways = 0;
        for (var status = 0; status < 27; status++)
        {
            var n = counts[status];
            if (n > 2) ways += n == 3 ? 1 : n * (n - 1) * (n - 2) / 6;
        }

        var path = new List<int>();
        for (var i = 0; i < 27; i++)
            if (counts[i] != 0)
            {
                path.Add(i);
                ways += Process2(counts, i, path);
                path.RemoveAt(path.Count - 1);
            }

        return ways;
    }

    // 之前的牌面，拿了一些    ABC  BBB  ... 
    // pre = BBB
    // ABC  ...
    // pre  = ABC
    // ABC BBB CAB
    // pre = CAB
    // 牌面一定要依次变大，所有形成的有效牌面，把方法数返回
    private static int Process2(int[] counts, int pre, List<int> path)
    {
        if (path.Count == 3) return GetWays2(counts, path);

        var ways = 0;
        for (var next = pre + 1; next < 27; next++)
            if (counts[next] != 0)
            {
                path.Add(next);
                ways += Process2(counts, next, path);
                path.RemoveAt(path.Count - 1);
            }

        return ways;
    }

    private static int GetWays2(int[] counts, List<int> path)
    {
        var v1 = path[0];
        var v2 = path[1];
        var v3 = path[2];
        for (var i = 9; i > 0; i /= 3)
        {
            var cur1 = v1 / i;
            var cur2 = v2 / i;
            var cur3 = v3 / i;
            v1 %= i;
            v2 %= i;
            v3 %= i;
            if ((cur1 != cur2 && cur1 != cur3 && cur2 != cur3) || (cur1 == cur2 && cur1 == cur3)) continue;

            return 0;
        }

        v1 = path[0];
        v2 = path[1];
        v3 = path[2];
        return counts[v1] * counts[v2] * counts[v3];
    }

    //用于测试
    private static string[] GenerateCards(int size)
    {
        var n = (int)(Utility.GetRandomDouble * size) + 3;
        var ans = new string[n];
        for (var i = 0; i < n; i++)
        {
            var cha0 = (char)((int)(Utility.GetRandomDouble * 3) + 'A');
            var cha1 = (char)((int)(Utility.GetRandomDouble * 3) + 'A');
            var cha2 = (char)((int)(Utility.GetRandomDouble * 3) + 'A');
            ans[i] = cha0 + cha1.ToString() + cha2;
        }

        return ans;
    }

    //用于测试
    public static void Run()
    {
        var size = 20;
        var testTime = 100000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var arr = GenerateCards(size);
            var ans1 = Ways1(arr);
            var ans2 = Ways2(arr);
            if (ans1 != ans2)
            {
                foreach (var str in arr) Console.WriteLine(str);

                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                break;
            }
        }

        Console.WriteLine("测试完成");

        var arr2 = GenerateCards(10000000);
        Console.WriteLine("arr size : " + arr2.Length + " runtime test begin");
        Utility.RestartStopwatch();
        Ways2(arr2);

        Console.WriteLine("run time : " + Utility.GetStopwatchElapsedMilliseconds() + " ms");
        Console.WriteLine("runtime test end");
    }
}