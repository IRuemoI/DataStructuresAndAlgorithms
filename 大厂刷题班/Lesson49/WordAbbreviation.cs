#region

using System.Text;

#endregion

namespace AdvancedTraining.Lesson49;

public class WordAbbreviation //leetcode_0527
{
    private static IList<string> wordsAbbreviation(IList<string> words)
    {
        var len = words.Count;
        IList<string> res = new List<string>();
        var map = new Dictionary<string, IList<int>>();
        for (var i = 0; i < len; i++)
        {
            res.Add(MakeAbbr(words[i], 1));
            var list = map.GetValueOrDefault(res[i], new List<int>());
            list.Add(i);
            map[res[i]] = list;
        }

        var prefix = new int[len];
        for (var i = 0; i < len; i++)
            if (map[res[i]].Count > 1)
            {
                var indexes = map[res[i]];
                map.Remove(res[i]);
                foreach (var j in indexes)
                {
                    prefix[j]++;
                    res[j] = MakeAbbr(words[j], prefix[j]);
                    var list = map.GetValueOrDefault(res[j], new List<int>());
                    list.Add(j);
                    map[res[j]] = list;
                }

                i--;
            }

        return res;
    }

    private static string MakeAbbr(string s, int k)
    {
        if (k >= s.Length - 2) return s;
        var builder = new StringBuilder();
        builder.Append(s[..k]);
        builder.Append(s.Length - 1 - k);
        builder.Append(s[^1]);
        return builder.ToString();
    }

    public static void Run()
    {
        var words = new List<string> { "like", "god", "internal", "me", "internet", "interval", "intension", "face", "intrusion" };
        var res = wordsAbbreviation(words);
        foreach (var item in res) Console.WriteLine(item);
        //["l2e","god","internal","me","i6t","interval","inte4n","f2e","intr4n"]
    }
}