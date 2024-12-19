namespace Algorithms.Lesson15;

// 本题为leetcode原题
// 测试链接：https://leetcode.cn/problems/number-of-islands-ii/
// 所有方法都可以直接通过
public static class NumberOfIslandsIi
{
    public static List<int> NumIslandsIi1(int m, int n, int[,] positions)
    {
        var uf = new UnionFind1(m, n);
        List<int> ans = new();

        for (var i = 0; i < positions.GetLength(0); i++) ans.Add(uf.Connect(positions[i, 0], positions[i, 1]));

        return ans;
    }

    // 课上讲的如果m*n比较大，会经历很重的初始化，而k比较小，怎么优化的方法
    public static List<int> NumIslandsIi2(int m, int n, int[,] positions)
    {
        var uf = new UnionFind2();
        List<int> ans = new();
        for (var i = 0; i < positions.GetLength(0); i++) ans.Add(uf.Connect(positions[i, 0], positions[i, 1]));

        return ans;
    }

    private class UnionFind1
    {
        private readonly int _col;
        private readonly int[] _help;
        private readonly int[] _parent;
        private readonly int _row;
        private readonly int[] _size;
        private int _sets;

        public UnionFind1(int m, int n)
        {
            _row = m;
            _col = n;
            _sets = 0;
            var len = _row * _col;
            _parent = new int[len];
            _size = new int[len];
            _help = new int[len];
        }

        private int Index(int r, int c)
        {
            return r * _col + c;
        }

        private int Find(int i)
        {
            var hi = 0;
            while (i != _parent[i])
            {
                _help[hi++] = i;
                i = _parent[i];
            }

            for (hi--; hi >= 0; hi--) _parent[_help[hi]] = i;

            return i;
        }

        private void Union(int r1, int c1, int r2, int c2)
        {
            if (r1 < 0 || r1 == _row || r2 < 0 || r2 == _row || c1 < 0 || c1 == _col || c2 < 0 || c2 == _col) return;

            var i1 = Index(r1, c1);
            var i2 = Index(r2, c2);
            if (_size[i1] == 0 || _size[i2] == 0) return;

            var f1 = Find(i1);
            var f2 = Find(i2);
            if (f1 != f2)
            {
                if (_size[f1] >= _size[f2])
                {
                    _size[f1] += _size[f2];
                    _parent[f2] = f1;
                }
                else
                {
                    _size[f2] += _size[f1];
                    _parent[f1] = f2;
                }

                _sets--;
            }
        }

        public int Connect(int row, int col)
        {
            var index = Index(row, col);
            if (_size[index] == 0)
            {
                _parent[index] = index;
                _size[index] = 1;
                _sets++;
                Union(row - 1, col, row, col);
                Union(row + 1, col, row, col);
                Union(row, col - 1, row, col);
                Union(row, col + 1, row, col);
            }

            return _sets;
        }
    }

    private class UnionFind2
    {
        private readonly List<string> _help = [];
        private readonly Dictionary<string, string> _parent = new();
        private readonly Dictionary<string, int> _size = new();
        private int _sets;

        private string Find(string cur)
        {
            while (!cur.Equals(_parent[cur]))
            {
                _help.Add(cur);
                cur = _parent[cur];
            }

            foreach (var str in _help) _parent[str] = cur;

            _help.Clear();
            return cur;
        }

        private void Union(string s1, string s2)
        {
            if (_parent.ContainsKey(s1) && _parent.ContainsKey(s2))
            {
                var f1 = Find(s1);
                var f2 = Find(s2);
                if (!f1.Equals(f2))
                {
                    var size1 = _size[f1];
                    var size2 = _size[f2];
                    var big = size1 >= size2 ? f1 : f2;
                    var small = big == f1 ? f2 : f1;
                    _parent[small] = big;
                    _size[big] = size1 + size2;
                    _sets--;
                }
            }
        }

        public int Connect(int row, int col)
        {
            var key = row + "_" + col;
            if (_parent.TryAdd(key, key))
            {
                _size[key] = 1;
                _sets++;
                var up = row - 1 + "_" + col;
                var down = row + 1 + "_" + col;
                var left = row + "_" + (col - 1);
                var right = row + "_" + (col + 1.ToString());
                Union(up, key);
                Union(down, key);
                Union(left, key);
                Union(right, key);
            }

            return _sets;
        }
    }
}

public static class NumberOfIslandsIiTest
{
    public static void Run()
    {
        const int m = 3, n = 3;
        int[,] positions = { { 0, 0 }, { 0, 1 }, { 1, 2 }, { 2, 1 } };
        var list1 = NumberOfIslandsIi.NumIslandsIi1(m, n, positions);
        Console.WriteLine(string.Join(",", list1)); //输出:1,1,2,3
        Console.WriteLine("--------------------");
        var list2 = NumberOfIslandsIi.NumIslandsIi2(m, n, positions);
        Console.WriteLine(string.Join(",", list2)); //输出:1,1,2,3
    }
}