namespace AdvancedTraining.Lesson39;

// 来自百度
// 给定一个字符串str，和一个正数k
// str子序列的字符种数必须是k种，返回有多少子序列满足这个条件
// 已知str中都是小写字母
// 原始是取mod
// 本节在尝试上，最难的
// 搞出桶来，组合公式
public class SequenceKDifferentKinds
{
    // bu -> {6,7,0,0,6,3}
    // 0 1 2 3 4 5
    // a b c d e F
    // 在桶数组bu[index....] 一定要凑出rest种来！请问几种方法！
    private static int F(int[] bu, int index, int rest)
    {
        if (index == bu.Length) return rest == 0 ? 1 : 0;
        // 最后形成的子序列，一个index代表的字符也没有!
        var p1 = F(bu, index + 1, rest);
        // 最后形成的子序列，一定要包含index代表的字符，几个呢？(所有可能性都要算上！)
        var p2 = 0;
        if (rest > 0)
            // 剩余的种数，没耗尽，可以包含当前桶的字符
            p2 = (1 << (bu[index] - 1)) * F(bu, index + 1, rest - 1);
        return p1 + p2;
    }

    private static int Numbers(string s, int k)
    {
        var str = s.ToCharArray();
        var counts = new int[26];
        foreach (var c in str) counts[c - 97]++;
        return Ways(counts, 0, k);
    }

    private static int Ways(int[] c, int i, int r)
    {
        if (r == 0) return 1;
        if (i == c.Length) return 0;
        // Math(n) -> 2 ^ n -1
        return Math(c[i]) * Ways(c, i + 1, r - 1) + Ways(c, i + 1, r);
    }

    // n个不同的球
    // 挑出1个的方法数 + 挑出2个的方法数 + ... + 挑出n个的方法数为:
    // C(n,1) + C(n,2) + ... + C(n,n) == (2 ^ n) -1
    private static int Math(int n)
    {
        return (1 << n) - 1;
    }

    public static void Run()
    {
        Console.WriteLine(Numbers("acbbca", 3));
    }
}