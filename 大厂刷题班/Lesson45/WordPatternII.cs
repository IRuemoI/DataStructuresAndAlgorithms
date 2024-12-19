namespace AdvancedTraining.Lesson45;

public class WordPatternIi //leetcode_0291
{
    private static bool WordPatternMatch(string pattern, string str)
    {
        return Match(str, pattern, 0, 0, new string[26], []);
    }

    // 题目有限制，str和pattern其中的字符，一定是a~z小写
    // p[a] -> "abc"
    // p[b] -> "fbf"
    // 需要指代的表最多26长度
    // String[] map -> new String[26]
    // p[a] -> "abc"   map[0] -> "abc"
    // p[b] -> "fbf"   map[1] -> "fbf";
    // p[z] -> "kfk"   map[25] -> "kfk"
    // HashSet<String> set -> map中指代了哪些字符串
    // str[si.......]  是不是符合  p[pi......]？符合返回true，不符合返回false
    // 之前的决定！由map和set，告诉我！不能冲突！
    private static bool Match(string s, string p, int si, int pi, string?[] map, HashSet<string> set)
    {
        if (pi == p.Length && si == s.Length) return true;
        // str和pattern，并没有都结束！
        if (pi == p.Length || si == s.Length) return false;
        //  str和pattern，都没结束！

        var ch = p[pi];
        var cur = map[ch - 'a'];
        if (!ReferenceEquals(cur, null))
            // 当前p[pi]已经指定过了！
            return si + cur.Length <= s.Length && cur.Equals(s.Substring(si, cur.Length)) &&
                   Match(s, p, si + cur.Length, pi + 1, map, set);
        // p[pi]没指定！
        var end = s.Length;
        // 剪枝！重要的剪枝！
        for (var i = p.Length - 1; i > pi; i--)
            end -= ReferenceEquals(map[p[i] - 'a'], null) ? 1 : map[p[i] - 'a']!.Length;
        for (var i = si; i < end; i++)
        {
            //  从si出发的所有前缀串，全试
            cur = s.Substring(si, i + 1 - si);
            // 但是，只有这个前缀串，之前没占过别的坑！才能去尝试
            if (set.Add(cur))
            {
                map[ch - 'a'] = cur;
                if (Match(s, p, i + 1, pi + 1, map, set)) return true;
                map[ch - 'a'] = null;
                set.Remove(cur);
            }
        }

        return false;
    }


    public static void Run()
    {
        Console.WriteLine(WordPatternMatch("abab", "redblueredblue")); //输出true
    }
}