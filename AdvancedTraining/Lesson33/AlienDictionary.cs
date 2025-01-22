#region

using System.Text;

#endregion
//pass
namespace AdvancedTraining.Lesson33;

//https://www.cnblogs.com/lishiblog/p/5681041.html
public class AlienDictionary //leetcode_0269
{
    private static string AlienOrder(string[]? words)
    {
        if (words == null || words.Length == 0) return "";
        var n = words.Length;
        var indegree = new Dictionary<char, int>();
        for (var i = 0; i < n; i++)
            foreach (var c in words[i])
                indegree[c] = 0;
        var graph = new Dictionary<char, HashSet<char>>();
        for (var i = 0; i < n - 1; i++)
        {
            var cur = words[i].ToCharArray();
            var nex = words[i + 1].ToCharArray();
            var len = Math.Min(cur.Length, nex.Length);
            var j = 0;
            for (; j < len; j++)
                if (cur[j] != nex[j])
                {
                    if (!graph.ContainsKey(cur[j])) graph[cur[j]] = new HashSet<char>();
                    if (!graph[cur[j]].Contains(nex[j]))
                    {
                        graph[cur[j]].Add(nex[j]);
                        indegree[nex[j]] = indegree[nex[j]] + 1;
                    }

                    break;
                }

            if (j < cur.Length && j == nex.Length) return "";
        }

        var ans = new StringBuilder();
        var q = new LinkedList<char>();
        foreach (var key in indegree.Keys)
            if (indegree[key] == 0)
                q.AddLast(key);
        while (q.Count > 0)
        {
            var cur = q.First();
            q.RemoveFirst();
            ans.Append(cur);
            if (graph.TryGetValue(cur, out var value))
                foreach (var next in value)
                {
                    indegree[next] = indegree[next] - 1;
                    if (indegree[next] == 0) q.AddLast(next);
                }
        }

        return ans.Length == indegree.Count ? ans.ToString() : "";
    }

    public static void Run()
    {
        string[] arr =
        [
            "wrt",
            "wrf",
            "er",
            "ett",
            "rftt"
        ];
        Console.WriteLine(AlienOrder(arr)); //输出wertf
    }
}