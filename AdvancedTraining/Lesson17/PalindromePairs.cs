namespace AdvancedTraining.Lesson17;

// 测试链接 : https://leetcode.cn/problems/palindrome-pairs/
public class PalindromePairs
{
    private static List<List<int>> palindromePairs(string[] words)
    {
        var wordSet = new Dictionary<string, int?>();
        for (var i = 0; i < words.Length; i++) wordSet.Add(words[i], i);

        var res = new List<List<int>>();
        //{ [6,23] 、 [7,13] }
        for (var i = 0; i < words.Length; i++)
            // i words[i]
            // findAll(字符串，在i位置，wordset) 返回所有生成的结果返回
            res.AddRange(findAll(words[i], i, wordSet));

        return res;
    }

    private static List<List<int>> findAll(string word, int index, Dictionary<string, int?> words)
    {
        List<List<int>> res = new();
        var reverse = Reverse(word);
        var rest = words[""];
        if (rest != null && rest.Value != index && word.Equals(reverse))
        {
            AddRecord(res, rest.Value, index);
            AddRecord(res, index, rest.Value);
        }

        var rs = manacherss(word);
        var mid = rs.Length >> 1;
        for (var i = 1; i < mid; i++)
            if (i - rs[i] == -1)
            {
                rest = words[reverse.Substring(0, mid - i)];
                if (rest != null && rest.Value != index) AddRecord(res, rest.Value, index);
            }

        for (var i = mid + 1; i < rs.Length; i++)
            if (i + rs[i] == rs.Length)
            {
                rest = words[reverse.Substring((mid << 1) - i)];
                if (rest != null && rest.Value != index) AddRecord(res, index, rest.Value);
            }

        return res;
    }

    private static void AddRecord(List<List<int>> res, int left, int right)
    {
        List<int> newRight =
        [
            left,
            right
        ];
        res.Add(newRight);
    }

    private static int[] manacherss(string word)
    {
        var mchs = manachercs(word);
        var rs = new int[mchs.Length];
        var center = -1;
        var pr = -1;
        for (var i = 0; i != mchs.Length; i++)
        {
            rs[i] = pr > i ? Math.Min(rs[(center << 1) - i], pr - i) : 1;
            while (i + rs[i] < mchs.Length && i - rs[i] > -1)
            {
                if (mchs[i + rs[i]] != mchs[i - rs[i]]) break;

                rs[i]++;
            }

            if (i + rs[i] > pr)
            {
                pr = i + rs[i];
                center = i;
            }
        }

        return rs;
    }

    private static char[] manachercs(string word)
    {
        var chs = word.ToCharArray();
        var mchs = new char[chs.Length * 2 + 1];
        var index = 0;
        for (var i = 0; i != mchs.Length; i++) mchs[i] = (i & 1) == 0 ? '#' : chs[index++];

        return mchs;
    }

    private static string Reverse(string str)
    {
        var chs = str.ToCharArray();
        var l = 0;
        var r = chs.Length - 1;
        while (l < r)
        {
            var tmp = chs[l];
            chs[l++] = chs[r];
            chs[r--] = tmp;
        }

        return new string(chs);
    }

    //todo:可能有问题
    public static void Run()
    {
        string[] words = ["abcd", "dcba", "lls", "s", "sssll"];
        var result = palindromePairs(words);
        foreach (var row in result)
        {
            foreach (var item in row) Console.Write(item + " ");

            Console.WriteLine();
        }
    }
}