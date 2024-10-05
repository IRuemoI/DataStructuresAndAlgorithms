using Common.Utilities;

namespace AdvancedTraining.Lesson21;

public class TreeChainPartition
{
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
    private static void Swap(int[] arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    // 为了测试
    private static int[] GenerateValueArray(int n, int v)
    {
        var ans = new int[n];
        for (var i = 0; i < n; i++) ans[i] = (int)(Utility.getRandomDouble * v) + 1;

        return ans;
    }

    // 对数器
    public static void Run()
    {
        const int n = 50000;
        const int v = 100000;
        var father = GenerateFatherArray(n);
        var values = GenerateValueArray(n, v);
        var tc = new TreeChain(father, values);
        var right = new Right(father, values);
        var testTime = 1000000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var decision = Utility.getRandomDouble;
            if (decision < 0.25)
            {
                var head = (int)(Utility.getRandomDouble * n);
                var value = (int)(Utility.getRandomDouble * v);
                tc.AddSubtree(head, value);
                right.AddSubtree(head, value);
            }
            else if (decision < 0.5)
            {
                var head = (int)(Utility.getRandomDouble * n);
                if (tc.QuerySubtree(head) != right.QuerySubtree(head)) Console.WriteLine("出错了!");
            }
            else if (decision < 0.75)
            {
                var a = (int)(Utility.getRandomDouble * n);
                var b = (int)(Utility.getRandomDouble * n);
                var value = (int)(Utility.getRandomDouble * v);
                tc.AddChain(a, b, value);
                right.AddChain(a, b, value);
            }
            else
            {
                var a = (int)(Utility.getRandomDouble * n);
                var b = (int)(Utility.getRandomDouble * n);
                if (tc.QueryChain(a, b) != right.QueryChain(a, b)) Console.WriteLine("出错了!");
            }
        }

        Console.WriteLine("测试结束");
    }

    private class TreeChain
    {
        // 线段树，在tnw上，玩连续的区间查询或者更新
        private readonly SegmentTree _seg;

        // 深度数组！
        private int[] _depth;

        // dfn[i] = j i这个节点，在dfs序中是第j个
        private int[] _dfn;

        // father数组一个平移，因为标号要+1
        private int[] _fa;

        // 谁是头
        private int _h;

        // 节点个数是n，节点编号是1~n
        private int _n;

        // siz[i] i这个节点为头的子树，有多少个节点
        private int[] _size;

        // son[i] = 0 i这个节点，没有儿子
        // son[i] != 0 j i这个节点，重儿子是j
        private int[] _son;

        // 时间戳 0 1 2 3 4
        private int _timestamp;

        // 如果原来的节点a，权重是10
        // 如果a节点在dfs序中是第5个节点, tnw[5] = 10
        private int[] _tnw;

        // top[i] = j i这个节点，所在的重链，头是j
        private int[] _top;

        // 朴素树结构
        private int[][] _tree;

        // 权重数组 原始的0节点权重是6 -> val[1] = 6
        private int[] _val;

        public TreeChain(int[] father, int[] values)
        {
            // 原始的树 tree，弄好了，可以从i这个点，找到下级的直接孩子
            // 上面的一大堆结构，准备好了空间，values -> val
            // 找到头部点
            InitTree(father, values);
            // fa;
            // dep;
            // son;
            // siz;
            Dfs1(_h, 0);
            // top;
            // dfn;
            // tnw;
            Dfs2(_h, _h);
            _seg = new SegmentTree(_tnw);
            _seg.Build(1, _n, 1);
        }

        private void InitTree(int[] father, int[] values)
        {
            _timestamp = 0;
            _n = father.Length + 1;
            _tree = new int[_n][];
            _val = new int[_n];
            _fa = new int[_n];
            _depth = new int[_n];
            _son = new int[_n];
            _size = new int[_n];
            _top = new int[_n];
            _dfn = new int[_n];
            _tnw = new int[_n--];
            var cnum = new int[_n];
            for (var i = 0; i < _n; i++) _val[i + 1] = values[i];

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

        // u 当前节点
        // f u的父节点
        private void Dfs1(int u, int f)
        {
            _fa[u] = f;
            _depth[u] = _depth[f] + 1;
            _size[u] = 1;
            var maxSize = -1;
            foreach (var v in _tree[u])
            {
                // 遍历u节点，所有的直接孩子
                Dfs1(v, u);
                _size[u] += _size[v];
                if (_size[v] > maxSize)
                {
                    maxSize = _size[v];
                    _son[u] = v;
                }
            }
        }

        // u当前节点
        // t是u所在重链的头部
        private void Dfs2(int u, int t)
        {
            _dfn[u] = ++_timestamp;
            _top[u] = t;
            _tnw[_timestamp] = _val[u];
            if (_son[u] != 0)
            {
                // 如果u有儿子 siz[u] > 1
                Dfs2(_son[u], t);
                foreach (var v in _tree[u])
                    if (v != _son[u])
                        Dfs2(v, v);
            }
        }

        // head为头的子树上，所有节点值+value
        // 因为节点经过平移，所以head(原始节点) -> head(平移节点)
        public void AddSubtree(int head, int value)
        {
            // 原始点编号 -> 平移编号
            head++;
            // 平移编号 -> dfs编号 dfn[head]
            _seg.Add(_dfn[head], _dfn[head] + _size[head] - 1, value, 1, _n, 1);
        }

        public int QuerySubtree(int head)
        {
            head++;
            return _seg.Query(_dfn[head], _dfn[head] + _size[head] - 1, 1, _n, 1);
        }

        public void AddChain(int a, int b, int v)
        {
            a++;
            b++;
            while (_top[a] != _top[b])
                if (_depth[_top[a]] > _depth[_top[b]])
                {
                    _seg.Add(_dfn[_top[a]], _dfn[a], v, 1, _n, 1);
                    a = _fa[_top[a]];
                }
                else
                {
                    _seg.Add(_dfn[_top[b]], _dfn[b], v, 1, _n, 1);
                    b = _fa[_top[b]];
                }

            if (_depth[a] > _depth[b])
                _seg.Add(_dfn[b], _dfn[a], v, 1, _n, 1);
            else
                _seg.Add(_dfn[a], _dfn[b], v, 1, _n, 1);
        }

        public int QueryChain(int a, int b)
        {
            a++;
            b++;
            var ans = 0;
            while (_top[a] != _top[b])
                if (_depth[_top[a]] > _depth[_top[b]])
                {
                    ans += _seg.Query(_dfn[_top[a]], _dfn[a], 1, _n, 1);
                    a = _fa[_top[a]];
                }
                else
                {
                    ans += _seg.Query(_dfn[_top[b]], _dfn[b], 1, _n, 1);
                    b = _fa[_top[b]];
                }

            if (_depth[a] > _depth[b])
                ans += _seg.Query(_dfn[b], _dfn[a], 1, _n, 1);
            else
                ans += _seg.Query(_dfn[a], _dfn[b], 1, _n, 1);

            return ans;
        }
    }

    private class SegmentTree
    {
        private readonly int[] _arr;
        private readonly int[] _lazy;
        private readonly int[] _sum;

        public SegmentTree(int[] origin)
        {
            var maxN = origin.Length;
            _arr = origin;
            _sum = new int[maxN << 2];
            _lazy = new int[maxN << 2];
        }

        private void PushUp(int rt)
        {
            _sum[rt] = _sum[rt << 1] + _sum[(rt << 1) | 1];
        }

        private void PushDown(int rt, int ln, int rn)
        {
            if (_lazy[rt] != 0)
            {
                _lazy[rt << 1] += _lazy[rt];
                _sum[rt << 1] += _lazy[rt] * ln;
                _lazy[(rt << 1) | 1] += _lazy[rt];
                _sum[(rt << 1) | 1] += _lazy[rt] * rn;
                _lazy[rt] = 0;
            }
        }

        public void Build(int l, int r, int rt)
        {
            if (l == r)
            {
                _sum[rt] = _arr[l];
                return;
            }

            var mid = (l + r) >> 1;
            Build(l, mid, rt << 1);
            Build(mid + 1, r, (rt << 1) | 1);
            PushUp(rt);
        }

        public void Add(int left, int right, int c, int l, int r, int rt)
        {
            if (left <= l && r <= right)
            {
                _sum[rt] += c * (r - l + 1);
                _lazy[rt] += c;
                return;
            }

            var mid = (l + r) >> 1;
            PushDown(rt, mid - l + 1, r - mid);
            if (left <= mid) Add(left, right, c, l, mid, rt << 1);

            if (right > mid) Add(left, right, c, mid + 1, r, (rt << 1) | 1);

            PushUp(rt);
        }

        public int Query(int left, int right, int l, int r, int rt)
        {
            if (left <= l && r <= right) return _sum[rt];

            var mid = (l + r) >> 1;
            PushDown(rt, mid - l + 1, r - mid);
            var ans = 0;
            if (left <= mid) ans += Query(left, right, l, mid, rt << 1);

            if (right > mid) ans += Query(left, right, mid + 1, r, (rt << 1) | 1);

            return ans;
        }
    }

    // 为了测试，这个结构是暴力但正确的方法
    private class Right
    {
        private readonly int[] _fa;
        private readonly Dictionary<int, int?> _path;
        private readonly int[][] _tree;
        private readonly int[] _val;

        public Right(int[] father, int[] value)
        {
            var n = father.Length;
            _tree = new int[n][];
            _fa = new int[n];
            _val = new int[n];
            for (var i = 0; i < n; i++)
            {
                _fa[i] = father[i];
                _val[i] = value[i];
            }

            var help = new int[n];
            var h = 0;
            for (var i = 0; i < n; i++)
                if (father[i] == i)
                    h = i;
                else
                    help[father[i]]++;

            for (var i = 0; i < n; i++) _tree[i] = new int[help[i]];

            for (var i = 0; i < n; i++)
                if (i != h)
                    _tree[father[i]][--help[father[i]]] = i;

            _path = new Dictionary<int, int?>();
        }

        public void AddSubtree(int head, int value)
        {
            _val[head] += value;
            foreach (var next in _tree[head]) AddSubtree(next, value);
        }

        public int QuerySubtree(int head)
        {
            var ans = _val[head];
            foreach (var next in _tree[head]) ans += QuerySubtree(next);

            return ans;
        }

        public void AddChain(int a, int b, int v)
        {
            _path.Clear();
            _path[a] = null;
            while (a != _fa[a])
            {
                _path[_fa[a]] = a;
                a = _fa[a];
            }

            while (!_path.ContainsKey(b))
            {
                _val[b] += v;
                b = _fa[b];
            }

            _val[b] += v;
            while (_path[b] != null)
            {
                b = _path[b] ?? throw new InvalidOperationException();
                _val[b] += v;
            }
        }

        public int QueryChain(int a, int b)
        {
            _path.Clear();
            _path[a] = null;
            while (a != _fa[a])
            {
                _path[_fa[a]] = a;
                a = _fa[a];
            }

            var ans = 0;
            while (!_path.ContainsKey(b))
            {
                ans += _val[b];
                b = _fa[b];
            }

            ans += _val[b];
            while (_path[b] != null)
            {
                b = _path[b] ?? throw new InvalidOperationException();
                ans += _val[b];
            }

            return ans;
        }
    }
}