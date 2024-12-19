// 本题测试链接:
// https://lightoj.com/problem/internet-bandwidth
// 这是一道DinicAlgorithm算法的题过
// 请看网页上的题目描述并结合main函数的写法去了解这个模板的用法

namespace Algorithms.Lesson47;

public class DinicAlgorithm
{
    public static void Run()
    {
        var cases = Console.Read();
        for (var i = 1; i <= cases; i++)
        {
            var n = Console.Read();
            var s = Console.Read();
            var t = Console.Read();
            var m = Console.Read();
            var dinic = new Dinic(n);
            for (var j = 0; j < m; j++)
            {
                var from = Console.Read();
                var to = Console.Read();
                var weight = Console.Read();
                dinic.AddEdge(from, to, weight);
                dinic.AddEdge(to, from, weight);
            }

            var ans = dinic.MaxFlow(s, t);
            Console.WriteLine("Case " + i + ": " + ans);
        }
    }

    private class Edge
    {
        public readonly int To;
        public int Available;
        public int From;

        public Edge(int a, int b, int c)
        {
            From = a;
            To = b;
            Available = c;
        }
    }

    private class Dinic
    {
        private readonly int[] _cur;
        private readonly int[] _depth;
        private readonly List<Edge> _edges;
        private readonly int _n;
        private readonly List<List<int>> _nextList;

        public Dinic(int nums)
        {
            _n = nums + 1;
            _nextList = new List<List<int>>();
            for (var i = 0; i <= _n; i++) _nextList.Add(new List<int>());

            _edges = new List<Edge>();
            _depth = new int[_n];
            _cur = new int[_n];
        }

        public virtual void AddEdge(int u, int v, int r)
        {
            var m = _edges.Count;
            _edges.Add(new Edge(u, v, r));
            _nextList[u].Add(m);
            _edges.Add(new Edge(v, u, 0));
            _nextList[v].Add(m + 1);
        }

        public virtual int MaxFlow(int s, int t)
        {
            var flow = 0;
            while (Bfs(s, t))
            {
                Array.Fill(_cur, 0);
                flow += Dfs(s, t, int.MaxValue);
                Array.Fill(_depth, 0);
            }

            return flow;
        }

        protected virtual bool Bfs(int s, int t)
        {
            var queue = new LinkedList<int>();
            queue.AddFirst(s);
            var visited = new bool[_n];
            visited[s] = true;
            while (queue is { Count: > 0, Last: not null })
            {
                var u = queue.Last.Value;
                queue.RemoveLast();
                for (var i = 0; i < _nextList[u].Count; i++)
                {
                    var e = _edges[_nextList[u][i]];
                    var v = e.To;
                    if (!visited[v] && e.Available > 0)
                    {
                        visited[v] = true;
                        _depth[v] = _depth[u] + 1;
                        if (v == t) break;

                        queue.AddFirst(v);
                    }
                }
            }

            return visited[t];
        }

        // 当前来到了s点，s可变
        // 最终目标是t，t固定参数
        // r，收到的任务
        // 收集到的流，作为结果返回，ans <= r
        protected virtual int Dfs(int s, int t, int r)
        {
            if (s == t || r == 0) return r;

            var flow = 0;
            // s点从哪条边开始试 -> cur[s]
            for (; _cur[s] < _nextList[s].Count; _cur[s]++)
            {
                var ei = _nextList[s][_cur[s]];
                var e = _edges[ei];
                var o = _edges[ei ^ 1];
                int f;
                if (_depth[e.To] == _depth[s] + 1 && (f = Dfs(e.To, t, Math.Min(e.Available, r))) != 0)
                {
                    e.Available -= f;
                    o.Available += f;
                    flow += f;
                    r -= f;
                    if (r <= 0) break;
                }
            }

            return flow;
        }
    }
}