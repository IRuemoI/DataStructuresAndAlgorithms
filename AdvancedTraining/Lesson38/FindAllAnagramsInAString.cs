namespace AdvancedTraining.Lesson38;

public class FindAllAnagramsInAString //Problem_0438
{
    private static IList<int> FindAnagrams(string s, string p)
    {
        IList<int> ans = new List<int>();
        if (ReferenceEquals(s, null) || ReferenceEquals(p, null) || s.Length < p.Length) return ans;
        var str = s.ToCharArray();
        var n = str.Length;
        var pst = p.ToCharArray();
        var m = pst.Length;
        var map = new Dictionary<char, int>();
        foreach (var cha in pst)
            if (!map.TryAdd(cha, 1))
                map[cha] = map[cha] + 1;
        var all = m;
        for (var end = 0; end < m - 1; end++)
            if (map.ContainsKey(str[end]))
            {
                var count = map[str[end]];
                if (count > 0) all--;
                map[str[end]] = count - 1;
            }

        for (int end = m - 1, start = 0; end < n; end++, start++)
        {
            if (map.ContainsKey(str[end]))
            {
                var count = map[str[end]];
                if (count > 0) all--;
                map[str[end]] = count - 1;
            }

            if (all == 0) ans.Add(start);
            if (map.ContainsKey(str[start]))
            {
                var count = map[str[start]];
                if (count >= 0) all++;
                map[str[start]] = count + 1;
            }
        }

        return ans;
    }

    public static void Run()
    {
        Console.WriteLine(string.Join(",", FindAnagrams("cbaebabacd", "abc"))); //输出: [0,6]
    }
}