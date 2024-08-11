namespace AdvancedTraining.Lesson06;

// 测试链接 : https://leetcode.cn/problems/maximum-xor-with-an-element-from-array/
public class MaximumXorWithAnElementFromArray
{
    private static int[] maximizeXor(int[] arr, int[][] queries)
    {
        var n = arr.Length;
        var trie = new NumTrie();
        for (var i = 0; i < n; i++) trie.Add(arr[i]);
        var m = queries.Length;
        var ans = new int[m];
        for (var i = 0; i < m; i++) ans[i] = trie.MaxXorWithXBehindM(queries[i][0], queries[i][1]);
        return ans;
    }

    public static void Run()
    {
        int[] arr = [0, 1, 2, 3, 4];
        int[][] queries = [[3, 1], [1, 3], [5, 6]];
        var result = maximizeXor(arr, queries);

        foreach (var item in result) Console.Write(item + " ");
    }

    public class Node
    {
        public readonly Node?[] NextList = new Node[2];
        public int Min = int.MaxValue;
    }

    private class NumTrie
    {
        private readonly Node _head = new();

        public void Add(int num)
        {
            var cur = _head;
            _head.Min = Math.Min(_head.Min, num);
            for (var move = 30; move >= 0; move--)
            {
                var path = (num >> move) & 1;
                if (cur != null)
                {
                    cur.NextList[path] = cur.NextList[path] == null ? new Node() : cur.NextList[path];
                    cur = cur.NextList[path];
                    if (cur != null) cur.Min = Math.Min(cur.Min, num);
                }
            }
        }

        // 这个结构中，已经收集了一票数字
        // 请返回哪个数字与X异或的结果最大，返回最大结果
        // 但是，只有<=m的数字，可以被考虑
        public int MaxXorWithXBehindM(int x, int m)
        {
            if (_head.Min > m) return -1;
            // 一定存在某个数可以和x结合
            var cur = _head;
            var ans = 0;
            for (var move = 30; move >= 0; move--)
            {
                var path = (x >> move) & 1;
                // 期待遇到的东西
                var best = path ^ 1;
                best ^= cur?.NextList[best] == null || cur.NextList[best]?.Min > m ? 1 : 0;
                // best变成了实际遇到的
                ans |= (path ^ best) << move;
                cur = cur?.NextList[best];
            }

            return ans;
        }
    }
}