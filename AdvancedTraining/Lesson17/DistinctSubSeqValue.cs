namespace AdvancedTraining.Lesson17;

// 本题测试链接 : https://leetcode.cn/problems/distinct-subsequences-ii/
public class DistinctSubSeqValue
{
    private static int DistinctSubSeqIi(string s)
    {
        if (ReferenceEquals(s, null) || s.Length == 0) return 0;
        var m = 1000000007;
        var str = s.ToCharArray();
        var count = new int[26];
        var all = 1; // 算空集
        foreach (var x in str)
        {
            var add = (all - count[x - 'a'] + m) % m;
            all = (all + add) % m;
            count[x - 'a'] = (count[x - 'a'] + add) % m;
        }

        return all - 1;
    }

    private static int Zuo(string s)
    {
        if (ReferenceEquals(s, null) || s.Length == 0) return 0;
        var m = 1000000007;
        var str = s.ToCharArray();
        var map = new Dictionary<char, int>();
        var all = 1; // 一个字符也没遍历的时候，有空集
        foreach (var x in str)
        {
            var newAdd = all;
            //			int curAll = all + newAdd - (map.containsKey(x) ? map.get(x) : 0);
            var curAll = all;
            curAll = (curAll + newAdd) % m;
            curAll = (curAll - map.GetValueOrDefault(x) + m) % m;
            all = curAll;
            map[x] = newAdd;
        }

        return all;
    }

    public static void Run()
    {
        var s = "bccaccbaabbc";
        Console.WriteLine(DistinctSubSeqIi(s) + 1);
        Console.WriteLine(Zuo(s));
    }
}