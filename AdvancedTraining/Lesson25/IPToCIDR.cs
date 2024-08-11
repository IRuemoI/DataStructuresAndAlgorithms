#region

using System.Text;

#endregion

namespace AdvancedTraining.Lesson25;

// 本题测试链接 : https://leetcode.cn/problems/ip-to-cidr/
// 题目描述：https://blog.csdn.net/qq_29051413/article/details/108575329
public class IpToCidr
{
    private static readonly Dictionary<int, int> Map = new();

    private static IList<string> IpToCidrCode(string ip, int n)
    {
        // ip -> 32位整数
        var cur = Status(ip);
        IList<string> ans = new List<string>();
        while (n > 0)
        {
            // cur最右侧的1，能搞定2的几次方个ip
            // cur : 000...000 01001000
            // 3
            var maxPower = MostRightPower(cur); // cur这个状态，最右侧的1，能表示下2的几次方
            // cur : 0000....0000 00001000 -> 2的3次方的问题
            // sol : 0000....0000 00000001 -> 1 2的0次方
            // sol : 0000....0000 00000010 -> 2 2的1次方
            // sol : 0000....0000 00000100 -> 4 2的2次方
            // sol : 0000....0000 00001000 -> 8 2的3次方
            var solved = 1; // 已经解决了多少ip了
            var power = 0;
            // 怕溢出
            // solved
            while (solved << 1 <= n && power + 1 <= maxPower)
            {
                solved <<= 1;
                power++;
            }

            ans.Add(Content(cur, power));
            n -= solved;
            cur += solved;
        }

        return ans;
    }

    // ip -> int(32位状态)
    private static int Status(string ip)
    {
        var ans = 0;
        var move = 24;
        foreach (var str in ip.Split("."))
        {
            // 17.23.16.5 "17" "23" "16" "5"
            // "17" -> 17 << 24
            // "23" -> 23 << 16
            // "16" -> 16 << 8
            // "5" -> 5 << 0
            ans |= Convert.ToInt32(str) << move;
            move -= 8;
        }

        return ans;
    }
    // 1 000000....000000 -> 2的32次方

    private static int MostRightPower(int num)
    {
        // map只会生成1次，以后直接用
        if (Map.Count == 0)
        {
            Map[0] = 32;
            for (var i = 0; i < 32; i++)
                // 00...0000 00000001 2的0次方
                // 00...0000 00000010 2的1次方
                // 00...0000 00000100 2的2次方
                // 00...0000 00001000 2的3次方
                Map[1 << i] = i;
        }

        // num & (-num) -> num & (~num+1) -> 提取出最右侧的1
        return Map[num & -num];
    }

    private static string Content(int status, int power)
    {
        var builder = new StringBuilder();
        for (var move = 24; move >= 0; move -= 8) builder.Append(((status & (255 << move)) >>> move) + ".");
        builder[^1] = '/';
        builder.Append(32 - power);
        return builder.ToString();
    }

    public static void Run()
    {
        foreach (var item in
                 IpToCidrCode("255.0.0.7", 10))
            Console.Write(item + ", "); //输出 255.0.0.7/32, 255.0.0.8/29, 255.0.0.16/32
    }
}