#region

using System.Text;

#endregion

namespace AdvancedTraining.Lesson28;

public class GroupAnagrams //Problem_0049
{
    private static IList<IList<string>> GroupAnagrams1(string[] strings)
    {
        var map = new Dictionary<string, IList<string>>();
        foreach (var str in strings)
        {
            var record = new int[26];
            foreach (var cha in str) record[cha - 'a']++;
            var builder = new StringBuilder();
            foreach (var value in record) builder.Append(value.ToString()).Append("_");
            var key = builder.ToString();
            if (!map.ContainsKey(key)) map[key] = new List<string>();
            map[key].Add(str);
        }

        IList<IList<string>> res = new List<IList<string>>();
        foreach (var list in map.Values) res.Add(list);
        return res;
    }

    private static IList<IList<string>> GroupAnagrams2(string[] strings)
    {
        var map = new Dictionary<string, IList<string>>();
        foreach (var str in strings)
        {
            var chs = str.ToCharArray();
            Array.Sort(chs);
            var key = new string(chs);
            if (!map.ContainsKey(key)) map[key] = new List<string>();
            map[key].Add(str);
        }

        IList<IList<string>> res = new List<IList<string>>();
        foreach (var list in map.Values) res.Add(list);
        return res;
    }

    public static void Run()
    {
        foreach (var row in GroupAnagrams1(["eat", "tea", "tan", "ate", "nat", "bat"]))
        {
            foreach (var item in row) Console.Write(item + ","); // 输出: [["bat"],["nat","tan"],["ate","eat","tea"]]

            Console.WriteLine();
        }

        Console.WriteLine("==============================");

        foreach (var row in GroupAnagrams2(["eat", "tea", "tan", "ate", "nat", "bat"]))
        {
            foreach (var item in row) Console.Write(item + ","); // 输出: [["bat"],["nat","tan"],["ate","eat","tea"]]

            Console.WriteLine();
        }
    }
}