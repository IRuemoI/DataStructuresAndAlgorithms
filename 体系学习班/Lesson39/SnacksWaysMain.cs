namespace Algorithms.Lesson39;
//本文件是Code02_SnacksWays问题的牛客题目解答
//但是用的分治的方法
//这是牛客的测试链接：
//https://www.nowcoder.com/questionTerminal/d94bb2fa461d42bcb4c0f2b94f5d4281
//把如下的全部代码拷贝进编辑器（java）
//可以直接通过

public class SnacksWaysMain
{
    public static void Run()
    {
        const int bag = 10;
        int[] arr = [1, 2, 4];
        var ways = Ways(arr, bag);
        Console.WriteLine(ways);
    }

    private static long Ways(int[]? arr, int bag)
    {
        if (arr == null || arr.Length == 0) return 0;

        if (arr.Length == 1) return arr[0] <= bag ? 2 : 1;

        var mid = (arr.Length - 1) >> 1;
        var lMap = new SortedDictionary<long, long>();
        var ways = Process(arr, 0, 0, mid, bag, lMap);
        var rMap = new SortedDictionary<long, long>();
        ways += Process(arr, mid + 1, 0, arr.Length - 1, bag, rMap);
        var rPre = new SortedDictionary<long, long>();
        long pre = 0;
        foreach (var entry in rMap)
        {
            pre += entry.Value;
            rPre[entry.Key] = pre;
        }

        foreach (var entry in lMap)
        {
            var lWeight = entry.Key;
            var lWays = entry.Value;
            long? floor = rPre.LastOrDefault(x => x.Key <= bag - lWeight).Key;
            if (floor != null)
            {
                var rWays = rPre[(int)floor];
                ways += lWays * rWays;
            }
        }

        return ways + 1;
    }


    // arr 30
    // func(arr, 0, 14, 0, bag, map)

    // func(arr, 15, 29, 0, bag, map)

    // 从index出发，到end结束
    // 之前的选择，已经形成的累加和sum
    // 零食[index....end]自由选择，出来的所有累加和，不能超过bag，每一种累加和对应的方法数，填在map里
    // 最后不能什么货都没选
    // [3,3,3,3] bag = 6
    // 0 1 2 3
    // - - - - 0 -> （0 : 1）
    // - - - $ 3 -> （0 : 1）(3, 1)
    // - - $ - 3 -> （0 : 1）(3, 2)
    private static long Func(int[] arr, int index, int end, long sum, long bag, SortedDictionary<long, long> map)
    {
        if (sum > bag) return 0;

        // sum <= bag
        if (index > end)
        {
            // 所有商品自由选择完了！
            // sum
            if (sum != 0)
            {
                if (!map.TryAdd(sum, 1L))
                    map[sum] = map[sum] + 1;

                return 1;
            }

            return 0;
        }

        // sum <= bag 并且 index <= end(还有货)
        // 1) 不要当前index位置的货
        var ways = Func(arr, index + 1, end, sum, bag, map);

        // 2) 要当前index位置的货
        ways += Func(arr, index + 1, end, sum + arr[index], bag, map);
        return ways;
    }

    private static long Process(int[] arr, int index, long w, int end, int bag, SortedDictionary<long, long> map)
    {
        if (w > bag) return 0;

        if (index > end)
        {
            if (w != 0)
            {
                if (!map.TryGetValue(w, out var value))
                    map[w] = 1L;
                else
                    map[w] = value + 1;

                return 1;
            }

            return 0;
        }

        var ways = Process(arr, index + 1, w, end, bag, map);
        ways += Process(arr, index + 1, w + arr[index], end, bag, map);
        return ways;
    }
}