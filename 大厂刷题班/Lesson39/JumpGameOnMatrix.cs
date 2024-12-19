#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson39;

// 来自京东
// 给定一个二维数组matrix，matrix[i,j] = k代表:
// 从(i,j)位置可以随意往右跳<=k步，或者从(i,j)位置可以随意往下跳<=k步
// 如果matrix[i,j] = 0，代表来到(i,j)位置必须停止
// 返回从matrix左上角到右下角，至少要跳几次
// 已知matrix中行数n <= 5000, 列数m <= 5000
// matrix中的值，<= 5000
// 最弟弟的技巧也过了。最优解 -> dp+枚举优化(线段树，体系学习班)
public class JumpGameOnMatrix
{
    // 暴力方法，仅仅是做对数器
    // 如果无法到达会返回系统最大值
    private static int Jump1(int[,] map)
    {
        return Process(map, 0, 0);
    }

    // 当前来到的位置是(row,col)
    // 目标：右下角
    // 当前最大能跳多远，map[row,col]值决定，只能向右、或者向下
    // 返回，到达右下角，最小跳几次？
    // 5000 * 5000 = 25000000 -> 2 * (10 ^ 7)
    private static int Process(int[,] map, int row, int col)
    {
        if (row == map.Length - 1 && col == map.GetLength(1) - 1) return 0;
        // 如果没到右下角
        if (map[row, col] == 0) return int.MaxValue;
        // 当前位置，可以去很多的位置，next含义：
        // 在所有能去的位置里，哪个位置最后到达右下角，跳的次数最少，就是next
        var next = int.MaxValue;
        // 往下能到达的位置，全试一遍
        for (var down = row + 1; down < map.Length && down - row <= map[row, col]; down++)
            next = Math.Min(next, Process(map, down, col));
        // 往右能到达的位置，全试一遍
        for (var right = col + 1; right < map.GetLength(1) && right - col <= map[row, col]; right++)
            next = Math.Min(next, Process(map, row, right));
        // 如果所有下一步的位置，没有一个能到右下角，next = 系统最大！
        // 返回系统最大！
        // next != 系统最大 7 + 1
        return next != int.MaxValue ? next + 1 : next;
    }

    // 优化方法, 利用线段树做枚举优化
    // 因为线段树，下标从1开始
    // 所以，该方法中所有的下标，请都从1开始，防止乱！
    private static int Jump2(int[,] arr)
    {
        var n = arr.GetLength(0);
        var m = arr.GetLength(1);
        var map = new int[n + 1, m + 1];
        for (int a = 0, b = 1; a < n; a++, b++)
        for (int c = 0, d = 1; c < m; c++, d++)
            map[b, d] = arr[a, c];
        var rowTrees = new SegmentTree[n + 1];
        for (var i = 1; i <= n; i++) rowTrees[i] = new SegmentTree(m);
        var colTrees = new SegmentTree[m + 1];
        for (var i = 1; i <= m; i++) colTrees[i] = new SegmentTree(n);
        rowTrees[n].Update(m, m, 0, 1, m, 1);
        colTrees[m].Update(n, n, 0, 1, n, 1);
        for (var col = m - 1; col >= 1; col--)
            if (map[n, col] != 0)
            {
                var left = col + 1;
                var right = Math.Min(col + map[n, col], m);
                var next = rowTrees[n].Query(left, right, 1, m, 1);
                if (next != int.MaxValue)
                {
                    rowTrees[n].Update(col, col, next + 1, 1, m, 1);
                    colTrees[col].Update(n, n, next + 1, 1, n, 1);
                }
            }

        for (var row = n - 1; row >= 1; row--)
            if (map[row, m] != 0)
            {
                var up = row + 1;
                var down = Math.Min(row + map[row, m], n);
                var next = colTrees[m].Query(up, down, 1, n, 1);
                if (next != int.MaxValue)
                {
                    rowTrees[row].Update(m, m, next + 1, 1, m, 1);
                    colTrees[m].Update(row, row, next + 1, 1, n, 1);
                }
            }

        for (var row = n - 1; row >= 1; row--)
        for (var col = m - 1; col >= 1; col--)
            if (map[row, col] != 0)
            {
                // (row,col) 往右是什么范围呢？[left,right]
                var left = col + 1;
                var right = Math.Min(col + map[row, col], m);
                var next1 = rowTrees[row].Query(left, right, 1, m, 1);
                // (row,col) 往下是什么范围呢？[up,down]
                var up = row + 1;
                var down = Math.Min(row + map[row, col], n);
                var next2 = colTrees[col].Query(up, down, 1, n, 1);
                var next = Math.Min(next1, next2);
                if (next != int.MaxValue)
                {
                    rowTrees[row].Update(col, col, next + 1, 1, m, 1);
                    colTrees[col].Update(row, row, next + 1, 1, n, 1);
                }
            }

        return rowTrees[1].Query(1, 1, 1, m, 1);
    }

    // 为了测试
    private static int[,] RandomMatrix(int n, int m, int v)
    {
        var ans = new int[n, m];
        for (var i = 0; i < n; i++)
        for (var j = 0; j < m; j++)
            ans[i, j] = (int)(Utility.getRandomDouble * v);
        return ans;
    }

    // 为了测试
    public static void Run()
    {
        // 先展示一下线段树的用法，假设N=100
        // 初始化时，1~100所有位置的值都是系统最大
        Console.WriteLine("线段树展示开始");
        const int n1 = 100;
        var st = new SegmentTree(n1);
        // 查询8~19范围上的最小值
        Console.WriteLine(st.Query(8, 19, 1, n1, 1));
        // 把6~14范围上对应的值都修改成56
        st.Update(6, 14, 56, 1, n1, 1);
        // 查询8~19范围上的最小值
        Console.WriteLine(st.Query(8, 19, 1, n1, 1));
        // 以上是线段树的用法，你可以随意使用update和query方法
        // 线段树的详解请看体系学习班
        Console.WriteLine("线段树展示结束");

        // 以下为正式测试
        var len = 10;
        var value = 8;
        var testTimes = 10000;
        Console.WriteLine("对数器测试开始");
        for (var i = 0; i < testTimes; i++)
        {
            var n = (int)(Utility.getRandomDouble * len) + 1;
            var m = (int)(Utility.getRandomDouble * len) + 1;
            var map = RandomMatrix(n, m, value);
            var ans1 = Jump1(map);
            var ans2 = Jump2(map);
            if (ans1 != ans2) Console.WriteLine("出错了!");
        }

        Console.WriteLine("对数器测试结束");
    }

    // 区间查询最小值的线段树
    // 注意下标从1开始，不从0开始
    // 比如你传入size = 8
    // 则位置对应为1~8，而不是0~7
    private class SegmentTree
    {
        private readonly int[] _change;

        private readonly int[] _min;

        private readonly bool[] _updateArray;

        public SegmentTree(int size)
        {
            var n = size + 1;
            _min = new int[n << 2];
            _change = new int[n << 2];
            _updateArray = new bool[n << 2];
            Update(1, size, int.MaxValue, 1, size, 1);
        }

        private void PushUp(int rt)
        {
            _min[rt] = Math.Min(_min[rt << 1], _min[(rt << 1) | 1]);
        }

        private void PushDown(int rt, int ln, int rn)
        {
            if (_updateArray[rt])
            {
                _updateArray[rt << 1] = true;
                _updateArray[(rt << 1) | 1] = true;
                _change[rt << 1] = _change[rt];
                _change[(rt << 1) | 1] = _change[rt];
                _min[rt << 1] = _change[rt];
                _min[(rt << 1) | 1] = _change[rt];
                _updateArray[rt] = false;
            }
        }

        // 最后三个参数是固定的, 每次传入相同的值即可:
        // l = 1(固定)
        // r = size(你设置的线段树大小)
        // rt = 1(固定)
        public void Update(int left, int right, int c, int l, int r, int rt)
        {
            if (left <= l && r <= right)
            {
                _updateArray[rt] = true;
                _change[rt] = c;
                _min[rt] = c;
                return;
            }

            var mid = (l + r) >> 1;
            PushDown(rt, mid - l + 1, r - mid);
            if (left <= mid) Update(left, right, c, l, mid, rt << 1);
            if (right > mid) Update(left, right, c, mid + 1, r, (rt << 1) | 1);
            PushUp(rt);
        }

        // 最后三个参数是固定的, 每次传入相同的值即可:
        // l = 1(固定)
        // r = size(你设置的线段树大小)
        // rt = 1(固定)
        public int Query(int l1, int r1, int l, int r, int rt)
        {
            if (l1 <= l && r <= r1) return _min[rt];
            var mid = (l + r) >> 1;
            PushDown(rt, mid - l + 1, r - mid);
            var left = int.MaxValue;
            var right = int.MaxValue;
            if (l1 <= mid) left = Query(l1, r1, l, mid, rt << 1);
            if (r1 > mid) right = Query(l1, r1, mid + 1, r, (rt << 1) | 1);
            return Math.Min(left, right);
        }
    }
}