//pass
namespace AdvancedTraining.Lesson31;
public class WordLadder //leetcode_0127
{
    // start，出发的单词
    // to, 目标单位
    // list, 列表
    // to 一定属于list
    // start未必
    // 返回变幻的最短路径长度
    private static int LadderLength1(string start, string to, IList<string> list)
    {
        list.Add(start);

        // key : 列表中的单词，每一个单词都会有记录！
        // value : key这个单词，有哪些邻居！
        var nexts = getNexts(list);
        // abc  出发     abc  -> abc  0
        // 
        // bbc  1
        var distanceMap = new Dictionary<string, int?>
        {
            [start] = 1
        };
        var set = new HashSet<string> { start };
        var queue = new LinkedList<string>();
        queue.AddLast(start);
        while (queue.Count > 0)
        {
            var cur = queue.First();
            queue.RemoveFirst();
            var distance = distanceMap[cur];
            foreach (var next in nexts[cur])
            {
                if (next.Equals(to)) return 1 + distance ?? throw new InvalidOperationException();

                if (set.Add(next))
                {
                    queue.AddLast(next);
                    distanceMap[next] = distance + 1;
                }
            }
        }

        return 0;
    }

    private static Dictionary<string, List<string>> getNexts(IList<string> words)
    {
        var dict = new HashSet<string>(words);
        var nextList = new Dictionary<string, List<string>>();
        foreach (var item in words)
            nextList[item] = getNext(item, dict);

        return nextList;
    }

    // 应该根据具体数据状况决定用什么来找邻居
    // 1)如果字符串长度比较短，字符串数量比较多，以下方法适合
    // 2)如果字符串长度比较长，字符串数量比较少，以下方法不适合
    private static List<string> getNext(string word, HashSet<string> dict)
    {
        var res = new List<string>();
        var chs = word.ToCharArray();
        for (var i = 0; i < chs.Length; i++)
        for (var cur = 'a'; cur <= 'z'; cur++)
            if (chs[i] != cur)
            {
                var tmp = chs[i];
                chs[i] = cur;
                if (dict.Contains(new string(chs))) res.Add(new string(chs));

                chs[i] = tmp;
            }

        return res;
    }

    private static int LadderLength2(string beginWord, string endWord, IList<string> wordList)
    {
        var dict = new HashSet<string>(wordList);
        if (!dict.Contains(endWord)) return 0;

        var startSet = new HashSet<string>();
        var endSet = new HashSet<string>();
        var visit = new HashSet<string>();
        startSet.Add(beginWord);
        endSet.Add(endWord);
        for (var len = 2; startSet.Count > 0; len++)
        {
            // startSet是较小的，endSet是较大的
            var nextSet = new HashSet<string>();
            foreach (var w in startSet)
                // w -> a(nextSet)
                // a b c
                // 0 
                //   1
                //     2
                for (var j = 0; j < w.Length; j++)
                {
                    var ch = w.ToCharArray();
                    for (var c = 'a'; c <= 'z'; c++)
                        if (c != w[j])
                        {
                            ch[j] = c;
                            var next = new string(ch);
                            if (endSet.Contains(next)) return len;

                            if (dict.Contains(next) && !visit.Contains(next))
                            {
                                nextSet.Add(next);
                                visit.Add(next);
                            }
                        }
                }

            // startSet(小) -> nextSet(某个大小)   和 endSet大小来比
            startSet = nextSet.Count < endSet.Count ? nextSet : endSet;
            endSet = startSet == nextSet ? endSet : nextSet;
        }

        return 0;
    }

    public static void Run()
    {
        Console.WriteLine(LadderLength1("hit", "cog",
            new List<string> { "hot", "dot", "dog", "lot", "log", "cog" })); //输出5
        Console.WriteLine(LadderLength2("hit", "cog",
            new List<string> { "hot", "dot", "dog", "lot", "log", "cog" })); //输出5
    }
}