#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson04;

public class QueryHobby
{
    private static int[] GetRandomStringArray(int len, int value)
    {
        var ans = new int[(int)(Utility.GetRandomDouble * len) + 1];
        for (var i = 0; i < ans.Length; i++) ans[i] = (int)(Utility.GetRandomDouble * value) + 1;
        return ans;
    }

    public static void Run()
    {
        const int len = 300;
        const int value = 20;
        const int testTimes = 1000;
        const int queryTimes = 1000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTimes; i++)
        {
            var arr = GetRandomStringArray(len, value);
            var n = arr.Length;
            var box1 = new QueryBox1(arr);
            var box2 = new QueryBox2(arr);
            for (var j = 0; j < queryTimes; j++)
            {
                var a = (int)(Utility.GetRandomDouble * n);
                var b = (int)(Utility.GetRandomDouble * n);
                var l = Math.Min(a, b);
                var r = Math.Max(a, b);
                var v = (int)(Utility.GetRandomDouble * value) + 1;
                if (box1.Query(l, r, v) != box2.Query(l, r, v)) Console.WriteLine("出错啦！");
            }
        }

        Console.WriteLine("测试结束");
    }

    /*
     * 今日头条原题
     *
     * 数组为{3, 2, 2, 3, 1}，查询为(0, 3, 2)。意思是在数组里下标0~3这个范围上，有几个2？返回2。
     * 假设给你一个数组arr，对这个数组的查询非常频繁，请返回所有查询的结果
     *
     */

    private class QueryBox1
    {
        private readonly int[] _arr;

        public QueryBox1(int[] array)
        {
            _arr = new int[array.Length];
            for (var i = 0; i < _arr.Length; i++) _arr[i] = array[i];
        }

        public int Query(int l, int r, int v)
        {
            var ans = 0;
            for (; l <= r; l++)
                if (_arr[l] == v)
                    ans++;

            return ans;
        }
    }

    private class QueryBox2
    {
        private readonly Dictionary<int, List<int>> _map;

        public QueryBox2(int[] arr)
        {
            _map = new Dictionary<int, List<int>>();
            for (var i = 0; i < arr.Length; i++)
            {
                if (!_map.ContainsKey(arr[i])) _map[arr[i]] = new List<int>();
                _map[arr[i]].Add(i);
            }
        }

        public int Query(int l, int r, int value)
        {
            if (!_map.TryGetValue(value, out var indexArr)) return 0;
            // 查询 < L 的下标有几个
            var a = CountLess(indexArr, l);
            // 查询 < R+1 的下标有几个
            var b = CountLess(indexArr, r + 1);
            return b - a;
        }

        // 在有序数组arr中，用二分的方法数出<limit的数有几个
        // 也就是用二分法，找到<limit的数中最右的位置
        private int CountLess(List<int> arr, int limit)
        {
            var l = 0;
            var r = arr.Count - 1;
            var mostRight = -1;
            while (l <= r)
            {
                var mid = l + ((r - l) >> 1);
                if (arr[mid] < limit)
                {
                    mostRight = mid;
                    l = mid + 1;
                }
                else
                {
                    r = mid - 1;
                }
            }

            return mostRight + 1;
        }
    }
}