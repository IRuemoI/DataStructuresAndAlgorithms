namespace AdvancedTraining.Lesson13;

// 本题测试链接 : https://leetcode.cn/problems/bricks-falling-when-hit/
public class BricksFallingWhenHit
{
    private static int[] HitBricks(int[][] grid, int[][] hits)
    {
        foreach (var item in hits)
            if (grid[item[0]][item[1]] == 1)
                grid[item[0]][item[1]] = 2;

        var unionFind = new UnionFind(grid);
        var ans = new int[hits.Length];
        for (var i = hits.Length - 1; i >= 0; i--)
            if (grid[hits[i][0]][hits[i][1]] == 2)
                ans[i] = unionFind.Finger(hits[i][0], hits[i][1]);

        return ans;
    }

    public static void Run()
    {
        int[][] grid = [[1, 0, 0, 0], [1, 1, 1, 0]], hits = [[1, 0]];
        var result = HitBricks(grid, hits);
        foreach (var item in result) Console.WriteLine(item);
    }

    // 并查集
    private class UnionFind
    {
        // 有多少块砖，连到了天花板上
        private int _cellingAll;

        // cellingSet[i] = true; i 是头节点，所在的集合是天花板集合
        private bool[] _cellingSet;

        private int[] _fatherMap;

        // 原始矩阵，因为炮弹的影响，1 -> 2
        private int[][] _grid;
        private int _m;
        private int _n;
        private int[] _sizeMap;
        private int[] _stack;

        public UnionFind(int[][] matrix)
        {
            InitSpace(matrix);
            InitConnect();
        }

        private void InitSpace(int[][] matrix)
        {
            _grid = matrix;
            _n = _grid.Length;
            _m = _grid[0].Length;
            var all = _n * _m;
            _cellingAll = 0;
            _cellingSet = new bool[all];
            _fatherMap = new int[all];
            _sizeMap = new int[all];
            _stack = new int[all];
            for (var row = 0; row < _n; row++)
            for (var col = 0; col < _m; col++)
                if (_grid[row][col] == 1)
                {
                    var index = row * _m + col;
                    _fatherMap[index] = index;
                    _sizeMap[index] = 1;
                    if (row == 0)
                    {
                        _cellingSet[index] = true;
                        _cellingAll++;
                    }
                }
        }

        private void InitConnect()
        {
            for (var row = 0; row < _n; row++)
            for (var col = 0; col < _m; col++)
            {
                Union(row, col, row - 1, col);
                Union(row, col, row + 1, col);
                Union(row, col, row, col - 1);
                Union(row, col, row, col + 1);
            }
        }

        private int Find(int row, int col)
        {
            var stackSize = 0;
            var index = row * _m + col;
            while (index != _fatherMap[index])
            {
                _stack[stackSize++] = index;
                index = _fatherMap[index];
            }

            while (stackSize != 0) _fatherMap[_stack[--stackSize]] = index;
            return index;
        }

        private void Union(int r1, int c1, int r2, int c2)
        {
            if (Valid(r1, c1) && Valid(r2, c2))
            {
                var father1 = Find(r1, c1);
                var father2 = Find(r2, c2);
                if (father1 != father2)
                {
                    var size1 = _sizeMap[father1];
                    var size2 = _sizeMap[father2];
                    var status1 = _cellingSet[father1];
                    var status2 = _cellingSet[father2];
                    if (size1 <= size2)
                    {
                        _fatherMap[father1] = father2;
                        _sizeMap[father2] = size1 + size2;
                        if (status1 ^ status2)
                        {
                            _cellingSet[father2] = true;
                            _cellingAll += status1 ? size2 : size1;
                        }
                    }
                    else
                    {
                        _fatherMap[father2] = father1;
                        _sizeMap[father1] = size1 + size2;
                        if (status1 ^ status2)
                        {
                            _cellingSet[father1] = true;
                            _cellingAll += status1 ? size2 : size1;
                        }
                    }
                }
            }
        }

        private bool Valid(int row, int col)
        {
            return row >= 0 && row < _n && col >= 0 && col < _m && _grid[row][col] == 1;
        }

        public int CellingNum()
        {
            return _cellingAll;
        }

        public int Finger(int row, int col)
        {
            _grid[row][col] = 1;
            var cur = row * _m + col;
            if (row == 0)
            {
                _cellingSet[cur] = true;
                _cellingAll++;
            }

            _fatherMap[cur] = cur;
            _sizeMap[cur] = 1;
            var pre = _cellingAll;
            Union(row, col, row - 1, col);
            Union(row, col, row + 1, col);
            Union(row, col, row, col - 1);
            Union(row, col, row, col + 1);
            var now = _cellingAll;
            if (row == 0)
                return now - pre;
            return now == pre ? 0 : now - pre - 1;
        }
    }
}