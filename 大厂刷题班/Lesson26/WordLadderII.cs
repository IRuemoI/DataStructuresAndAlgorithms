//pass

namespace AdvancedTraining.Lesson26;

// 本题测试链接 : https://leetcode.cn/problems/word-ladder-ii/
public class WordLadderIi
{
    private static IList<IList<string>> FindLadders(string start, string end, IList<string> list)
    {
        list.Add(start);
        var nextList = GetNextList(list);
        var distances = getDistances(start, nextList);
        var pathList = new LinkedList<string>();
        IList<IList<string>> res = new List<IList<string>>();
        GetShortestPaths(start, end, nextList, distances, pathList, res);
        return res;
    }

    //
    private static Dictionary<string, IList<string>> GetNextList(IList<string> words)
    {
        var dict = new HashSet<string>(words);
        var nextList = new Dictionary<string, IList<string>>();
        foreach (var item in words)
            nextList[item] = getNext(item, dict);

        return nextList;
    }

    // word, 在表中，有哪些邻居，把邻居们，生成list返回
    private static IList<string> getNext(string word, HashSet<string> dict)
    {
        var res = new List<string>();
        var chs = word.ToCharArray();
        for (var cur = 'a'; cur <= 'z'; cur++)
        for (var i = 0; i < chs.Length; i++)
            if (chs[i] != cur)
            {
                var tmp = chs[i];
                chs[i] = cur;
                if (dict.Contains(new string(chs))) res.Add(new string(chs));
                chs[i] = tmp;
            }

        return res;
    }

    // 生成距离表，从start开始，根据邻居表，宽度优先遍历，对于能够遇到的所有字符串，生成(字符串，距离)这条记录，放入距离表中
    private static Dictionary<string, int> getDistances(string start, Dictionary<string, IList<string>> nextList)
    {
        var distances = new Dictionary<string, int>
        {
            [start] = 0
        };
        var queue = new LinkedList<string>();
        queue.AddLast(start);
        var set = new HashSet<string> { start };
        while (queue.Count > 0)
        {
            var cur = queue.First();
            queue.RemoveFirst();
            foreach (var next in nextList[cur])
                if (!set.Contains(next))
                {
                    distances[next] = distances[cur] + 1;
                    queue.AddLast(next);
                    set.Add(next);
                }
        }

        return distances;
    }

    // cur 当前来到的字符串 可变
    // to 目标，固定参数
    // nextList 每一个字符串的邻居表
    // cur 到开头距离5 -> 到开头距离是6的支路 distances距离表
    // path : 来到cur之前，深度优先遍历之前的历史是什么
    // res : 当遇到cur，把历史，放入res，作为一个结果
    private static void GetShortestPaths(string cur, string to, Dictionary<string, IList<string>> nextList,
        Dictionary<string, int> distances, LinkedList<string> path, IList<IList<string>> res)
    {
        path.AddLast(cur);
        if (to.Equals(cur))
            res.Add(new List<string>(path));
        else
            foreach (var next in nextList[cur])
                if (distances[next] == distances[cur] + 1)
                    GetShortestPaths(next, to, nextList, distances, path, res);
        path.RemoveLast();
    }

    public static void Run()
    {
        foreach (var row in FindLadders("hit", "cog", ["hot", "dot", "dog", "lot", "log", "cog"]))
        {
            foreach (var item in row)
                Console.Write(item + ","); // 输出：[["hit","hot","dot","dog","cog"],["hit","hot","lot","log","cog"]]

            Console.WriteLine();
        }
    }
}