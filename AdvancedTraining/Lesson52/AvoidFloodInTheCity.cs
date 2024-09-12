#region

using Common.DataStructures.Heap;

#endregion

namespace AdvancedTraining.Lesson52;

public class AvoidFloodInTheCity //Problem_1488
{
    // rains[i] = j 第i天轮到j号湖泊下雨
    // 规定，下雨日，干啥 : -1
    // 不下雨日，如果没有湖泊可抽 : 1
    private static int[] AvoidFlood(int[] rains)
    {
        var n = rains.Length;
        var ans = new int[n];
        var invalid = Array.Empty<int>();
        // key : 某个湖
        // value : 这个湖在哪些位置降雨
        // 4 : {3,7,19,21}
        // 1 : { 13 }
        // 2 : {4, 56}
        var map = new Dictionary<int, LinkedList<int>>();
        for (var i = 0; i < n; i++)
            if (rains[i] != 0)
            {
                // 第i天要下雨，rains[i]
                // 3天 9号
                // 9号 { 3 }
                // 9号 {1, 3}
                if (!map.ContainsKey(rains[i])) map[rains[i]] = new LinkedList<int>();
                map[rains[i]].AddLast(i);
            }

        // 没抽干的湖泊表
        // 某个湖如果满了，加入到set里
        // 某个湖被抽干了，从set中移除
        var set = new HashSet<int>();
        // 这个堆的堆顶表示最先处理的湖是哪个
        Heap<Work> minHeap = new((x, y) => x.NextRain - y.NextRain);
        for (var i = 0; i < n; i++)
            // 0 1 2 3 ...
            if (rains[i] != 0)
            {
                if (!set.Add(rains[i])) return invalid;
                // 放入到没抽干的表里
                map[rains[i]].RemoveFirst();
                if (map[rains[i]].Count > 0) minHeap.Push(new Work(rains[i], map[rains[i]].First()));
                // 题目规定
                ans[i] = -1;
            }
            else
            {
                // 今天干活！
                if (minHeap.IsEmpty)
                {
                    ans[i] = 1;
                }
                else
                {
                    var cur = minHeap.Pop();
                    set.Remove(cur.Lake);
                    ans[i] = cur.Lake;
                }
            }

        return ans;
    }

    public class Work(int l, int p) : IComparable<Work>
    {
        public readonly int Lake = l;
        public readonly int NextRain = p;

        public int CompareTo(Work? o)
        {
            return NextRain - o!.NextRain;
        }
    }
}