#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson23;

public class LcaTarjanAndTreeChainPartition
{
    // 给定数组tree大小为N，表示一共有N个节点
    // tree[i] = j 表示点i的父亲是点j，tree一定是一棵树而不是森林
    // queries是二维数组，大小为M*2，每一个长度为2的数组都表示一条查询
    // [4,9], 表示想查询4和9之间的最低公共祖先
    // [3,7], 表示想查询3和7之间的最低公共祖先
    // tree和queries里面的所有值，都一定在0~N-1之间
    // 返回一个数组ans，大小为M，ans[i]表示第i条查询的答案

    // 暴力方法
    private static int[] query1(int[] father, int[,] queries)
    {
        var m = queries.Length;
        var ans = new int[m];
        var path = new HashSet<int>();
        for (var i = 0; i < m; i++)
        {
            var jump = queries[i, 0];
            while (father[jump] != jump)
            {
                path.Add(jump);
                jump = father[jump];
            }

            path.Add(jump);
            jump = queries[i, 1];
            while (!path.Contains(jump)) jump = father[jump];
            ans[i] = jump;
            path.Clear();
        }

        return ans;
    }

    // 离线批量查询最优解 -> Tarjan + 并查集
    // 如果有M条查询，时间复杂度O(N + M)
    // 但是必须把M条查询一次给全，不支持在线查询
    private static int[] query2(int[] father, int[,] queries)
    {
        var n = father.Length;
        var m = queries.Length;
        var help = new int[n];
        var h = 0;
        for (var i = 0; i < n; i++)
            if (father[i] == i)
                h = i;
            else
                help[father[i]]++;
        var mt = new int[n][];
        for (var i = 0; i < n; i++) mt[i] = new int[help[i]];
        for (var i = 0; i < n; i++)
            if (i != h)
                mt[father[i]][--help[father[i]]] = i;
        for (var i = 0; i < m; i++)
            if (queries[i, 0] != queries[i, 1])
            {
                help[queries[i, 0]]++;
                help[queries[i, 1]]++;
            }

        var mq = new int[n][];
        var mi = new int[n][];
        for (var i = 0; i < n; i++)
        {
            mq[i] = new int[help[i]];
            mi[i] = new int[help[i]];
        }

        for (var i = 0; i < m; i++)
            if (queries[i, 0] != queries[i, 1])
            {
                mq[queries[i, 0]][--help[queries[i, 0]]] = queries[i, 1];
                mi[queries[i, 0]][help[queries[i, 0]]] = i;
                mq[queries[i, 1]][--help[queries[i, 1]]] = queries[i, 0];
                mi[queries[i, 1]][help[queries[i, 1]]] = i;
            }

        var ans = new int[m];
        var uf = new UnionFind(n);
        Process(h, mt, mq, mi, uf, ans);
        for (var i = 0; i < m; i++)
            if (queries[i, 0] == queries[i, 1])
                ans[i] = queries[i, 0];
        return ans;
    }

    // 当前来到head点
    // mt是整棵树 head下方有哪些点 mt[head] = {a,b,c,d} head的孩子是abcd
    // mq问题列表 head有哪些问题 mq[head] = {x,y,z} (head，x) (head，y) (head z)
    // mi得到问题的答案，填在ans的什么地方 {6,12,34}
    // uf 并查集
    private static void Process(int head, int[][] mt, int[][] mq, int[][] mi, UnionFind uf, int[] ans)
    {
        foreach (var next in mt[head])
        {
            // head有哪些孩子，都遍历去吧！
            Process(next, mt, mq, mi, uf, ans);
            uf.Union(head, next);
            uf.SetTag(head, head);
        }

        // 解决head的问题！
        var q = mq[head];
        var i = mi[head];
        for (var k = 0; k < q.Length; k++)
        {
            // head和谁有问题 q[k] 答案填哪 i[k]
            var tag = uf.GetTag(q[k]);
            if (tag != -1) ans[i[k]] = tag;
        }
    }

    // 在线查询最优解 -> 树链剖分
    // 空间复杂度O(N), 支持在线查询，单次查询时间复杂度O(logN)
    // 如果有M次查询，时间复杂度O(N + M * logN)
    private static int[] query3(int[] father, int[,] queries)
    {
        var tc = new TreeChain(father);
        var m = queries.Length;
        var ans = new int[m];
        for (var i = 0; i < m; i++)
            // x y ?
            // x x x
            if (queries[i, 0] == queries[i, 1])
                ans[i] = queries[i, 0];
            else
                ans[i] = tc.Lca(queries[i, 0], queries[i, 1]);
        return ans;
    }

    // 为了测试
    // 随机生成N个节点树，可能是多叉树，并且一定不是森林
    // 输入参数N要大于0
    private static int[] GenerateFatherArray(int n)
    {
        var order = new int[n];
        for (var i = 0; i < n; i++) order[i] = i;
        for (var i = n - 1; i >= 0; i--) Swap(order, i, (int)(Utility.getRandomDouble * (i + 1)));
        var ans = new int[n];
        ans[order[0]] = order[0];
        for (var i = 1; i < n; i++) ans[order[i]] = order[(int)(Utility.getRandomDouble * i)];
        return ans;
    }

    // 为了测试
    // 随机生成M条查询，点有N个，点的编号在0~N-1之间
    // 输入参数M和N都要大于0
    private static int[,] GenerateQueries(int m, int n)
    {
        var ans = new int[m, 2];
        for (var i = 0; i < m; i++)
        {
            ans[i, 0] = (int)(Utility.getRandomDouble * n);
            ans[i, 1] = (int)(Utility.getRandomDouble * n);
        }

        return ans;
    }

    // 为了测试
    private static void Swap(int[] arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    // 为了测试
    private static bool Equal(int[] a, int[] b)
    {
        if (a.Length != b.Length) return false;
        for (var i = 0; i < a.Length; i++)
            if (a[i] != b[i])
                return false;
        return true;
    }

    // 为了测试
    public static void Run()
    {
        const int n = 1000;
        const int m = 200;
        const int testTime = 50000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var size = (int)(Utility.getRandomDouble * n) + 1;
            var ques = (int)(Utility.getRandomDouble * m) + 1;
            var father = GenerateFatherArray(size);
            var queries = GenerateQueries(ques, size);
            var ans1 = query1(father, queries);
            var ans2 = query2(father, queries);
            var ans3 = query3(father, queries);
            if (!Equal(ans1, ans2) || !Equal(ans1, ans3))
            {
                Console.WriteLine("出错了！");
                break;
            }
        }

        Console.WriteLine("测试结束");
    }

    private class UnionFind
    {
        private readonly int[] _f; // father -> 并查集里面father信息，i -> i的father
        private readonly int[] _h; // 栈？并查集搞扁平化
        private readonly int[] _s; // size[] -> 集合 --> i size[i]
        private readonly int[] _t; // tag[] -> 集合 ---> tag[i] = ?

        public UnionFind(int n)
        {
            _f = new int[n];
            _s = new int[n];
            _t = new int[n];
            _h = new int[n];
            for (var i = 0; i < n; i++)
            {
                _f[i] = i;
                _s[i] = 1;
                _t[i] = -1;
            }
        }

        private int Find(int i)
        {
            var j = 0;
            // i -> j -> k -> s -> a -> a
            while (i != _f[i])
            {
                _h[j++] = i;
                i = _f[i];
            }

            // i -> a
            // j -> a
            // k -> a
            // s -> a
            while (j > 0) _h[--j] = i;
            // a
            return i;
        }

        public void Union(int i, int j)
        {
            var fi = Find(i);
            var fj = Find(j);
            if (fi != fj)
            {
                var si = _s[fi];
                var sj = _s[fj];
                var m = si >= sj ? fi : fj;
                var l = m == fi ? fj : fi;
                _f[l] = m;
                _s[m] += _s[l];
            }
        }

        // 集合的某个元素是i，请把整个集合打上统一的标签，tag
        public void SetTag(int i, int tag)
        {
            _t[Find(i)] = tag;
        }

        // 集合的某个元素是i，请把整个集合的tag信息返回
        public int GetTag(int i)
        {
            return _t[Find(i)];
        }
    }

    private class TreeChain
    {
        private int[] _dep;
        private int[] _fa;
        private int _h;
        private int _n;
        private int[] _siz;
        private int[] _son;
        private int[] _top;
        private int[][] _tree;

        public TreeChain(int[] father)
        {
            InitTree(father);
            Dfs1(_h, 0);
            Dfs2(_h, _h);
        }

        private void InitTree(int[] father)
        {
            _n = father.Length + 1;
            _tree = new int[_n][];
            _fa = new int[_n];
            _dep = new int[_n];
            _son = new int[_n];
            _siz = new int[_n];
            _top = new int[_n--];
            var cnum = new int[_n];
            for (var i = 0; i < _n; i++)
                if (father[i] == i)
                    _h = i + 1;
                else
                    cnum[father[i]]++;
            _tree[0] = [];
            for (var i = 0; i < _n; i++) _tree[i + 1] = new int[cnum[i]];
            for (var i = 0; i < _n; i++)
                if (i + 1 != _h)
                    _tree[father[i] + 1][--cnum[father[i]]] = i + 1;
        }

        private void Dfs1(int u, int f)
        {
            _fa[u] = f;
            _dep[u] = _dep[f] + 1;
            _siz[u] = 1;
            var maxSize = -1;
            foreach (var v in _tree[u])
            {
                Dfs1(v, u);
                _siz[u] += _siz[v];
                if (_siz[v] > maxSize)
                {
                    maxSize = _siz[v];
                    _son[u] = v;
                }
            }
        }

        private void Dfs2(int u, int t)
        {
            _top[u] = t;
            if (_son[u] != 0)
            {
                Dfs2(_son[u], t);
                foreach (var v in _tree[u])
                    if (v != _son[u])
                        Dfs2(v, v);
            }
        }

        public int Lca(int a, int b)
        {
            a++;
            b++;
            while (_top[a] != _top[b])
                if (_dep[_top[a]] > _dep[_top[b]])
                    a = _fa[_top[a]];
                else
                    b = _fa[_top[b]];
            return (_dep[a] < _dep[b] ? a : b) - 1;
        }
    }
}