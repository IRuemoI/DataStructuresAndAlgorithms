namespace Algorithms.Lesson31;

public class SegmentTree
{
    // arr[]为原序列的信息从0开始，但在arr里是从1开始的
    // sum[]模拟线段树维护区间和
    // lazy[]为累加和懒惰标记
    // change[]为更新的值
    // update[]为更新慵懒标记
    private readonly int[] _arr;
    private readonly int[] _change;
    private readonly int[] _lazy;
    private readonly int[] _sum;
    private readonly bool[] _update;

    public SegmentTree(int[] origin)
    {
        var maxN = origin.Length + 1;
        _arr = new int[maxN]; // arr[0] 不用 从1开始使用
        for (var i = 1; i < maxN; i++) _arr[i] = origin[i - 1];

        _sum = new int[maxN << 2]; // 用来支持脑补概念中，某一个范围的累加和信息
        _lazy = new int[maxN << 2]; // 用来支持脑补概念中，某一个范围沒有往下傳遞的纍加任務
        _change = new int[maxN << 2]; // 用来支持脑补概念中，某一个范围有没有更新操作的任务
        _update = new bool[maxN << 2]; // 用来支持脑补概念中，某一个范围更新任务，更新成了什么
    }

    private void PushUp(int rt)
    {
        _sum[rt] = _sum[rt << 1] + _sum[(rt << 1) | 1];
    }

    // 之前的，所有懒增加，和懒更新，从父范围，发给左右两个子范围
    // 分发策略是什么
    // ln表示左子树元素结点个数，rn表示右子树结点个数
    private void PushDown(int rt, int ln, int rn)
    {
        if (_update[rt])
        {
            _update[rt << 1] = true;
            _update[(rt << 1) | 1] = true;
            _change[rt << 1] = _change[rt];
            _change[(rt << 1) | 1] = _change[rt];
            _lazy[rt << 1] = 0;
            _lazy[(rt << 1) | 1] = 0;
            _sum[rt << 1] = _change[rt] * ln;
            _sum[(rt << 1) | 1] = _change[rt] * rn;
            _update[rt] = false;
        }

        if (_lazy[rt] != 0)
        {
            _lazy[rt << 1] += _lazy[rt];
            _sum[rt << 1] += _lazy[rt] * ln;
            _lazy[(rt << 1) | 1] += _lazy[rt];
            _sum[(rt << 1) | 1] += _lazy[rt] * rn;
            _lazy[rt] = 0;
        }
    }

    // 在初始化阶段，先把sum数组，填好
    // 在arr[l~r]范围上，去build，1~N，
    // rt : 这个范围在sum中的下标
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


    // L~R  所有的值变成C
    // l~r  rt
    public void Update(int l1, int r1, int c, int l, int r, int rt)
    {
        if (l1 <= l && r <= r1)
        {
            _update[rt] = true;
            _change[rt] = c;
            _sum[rt] = c * (r - l + 1);
            _lazy[rt] = 0;
            return;
        }

        // 当前任务躲不掉，无法懒更新，要往下发
        var mid = (l + r) >> 1;
        PushDown(rt, mid - l + 1, r - mid);
        if (l1 <= mid) Update(l1, r1, c, l, mid, rt << 1);

        if (r1 > mid) Update(l1, r1, c, mid + 1, r, (rt << 1) | 1);

        PushUp(rt);
    }

    // L~R, C 任务！
    // rt，l~r
    public void Add(int l1, int r1, int c, int l, int r, int rt)
    {
        // 任务如果把此时的范围全包了！
        if (l1 <= l && r <= r1)
        {
            _sum[rt] += c * (r - l + 1);
            _lazy[rt] += c;
            return;
        }

        // 任务没有把你全包！
        // l  r  mid = (l+r)/2
        var mid = (l + r) >> 1;
        PushDown(rt, mid - l + 1, r - mid);
        // L~R
        if (l1 <= mid) Add(l1, r1, c, l, mid, rt << 1);

        if (r1 > mid) Add(l1, r1, c, mid + 1, r, (rt << 1) | 1);

        PushUp(rt);
    }

    // 1~6 累加和是多少？ 1~8 rt
    public long Query(int l1, int r1, int l, int r, int rt)
    {
        if (l1 <= l && r <= r1) return _sum[rt];

        var mid = (l + r) >> 1;
        PushDown(rt, mid - l + 1, r - mid);
        long ans = 0;
        if (l1 <= mid) ans += Query(l1, r1, l, mid, rt << 1);

        if (r1 > mid) ans += Query(l1, r1, mid + 1, r, (rt << 1) | 1);

        return ans;
    }
}

public static class SegmentTreeTest
{
    private static int[] GenerateRandomArray(int len, int max)
    {
        var size = (int)(new Random().NextDouble() * len) + 1;
        var origin = new int[size];
        for (var i = 0; i < size; i++)
            origin[i] = (int)(new Random().NextDouble() * max) - (int)(new Random().NextDouble() * max);

        return origin;
    }

    private static bool Test()
    {
        var len = 100;
        var max = 1000;
        var testTimes = 5000;
        var addOrUpdateTimes = 1000;
        var queryTimes = 500;
        for (var i = 0; i < testTimes; i++)
        {
            var origin = GenerateRandomArray(len, max);
            var seg = new SegmentTree(origin);
            const int s = 1;
            var n = origin.Length;
            var root = 1;
            seg.Build(s, n, root);
            var rig = new Right(origin);
            for (var j = 0; j < addOrUpdateTimes; j++)
            {
                var num1 = (int)(new Random().NextDouble() * n) + 1;
                var num2 = (int)(new Random().NextDouble() * n) + 1;
                var l = Math.Min(num1, num2);
                var r = Math.Max(num1, num2);
                var c = (int)(new Random().NextDouble() * max) - (int)(new Random().NextDouble() * max);
                if (new Random().NextDouble() < 0.5)
                {
                    seg.Add(l, r, c, s, n, root);
                    rig.Add(l, r, c);
                }
                else
                {
                    seg.Update(l, r, c, s, n, root);
                    rig.Update(l, r, c);
                }
            }

            for (var k = 0; k < queryTimes; k++)
            {
                var num1 = (int)(new Random().NextDouble() * n) + 1;
                var num2 = (int)(new Random().NextDouble() * n) + 1;
                var l = Math.Min(num1, num2);
                var r = Math.Max(num1, num2);
                var ans1 = seg.Query(l, r, s, n, root);
                var ans2 = rig.Query(l, r);
                if (ans1 != ans2) return false;
            }
        }

        return true;
    }

    public static void Run()
    {
        int[] origin = [2, 1, 1, 2, 3, 4, 5];
        var seg = new SegmentTree(origin);
        const int s = 1; // 整个区间的开始位置，规定从1开始，不从0开始 -> 固定
        var n = origin.Length; // 整个区间的结束位置，规定能到N，不是N-1 -> 固定
        var root = 1; // 整棵树的头节点位置，规定是1，不是0 -> 固定
        const int l = 2; // 操作区间的开始位置 -> 可变
        const int r = 5; // 操作区间的结束位置 -> 可变
        const int c = 4; // 要加的数字或者要更新的数字 -> 可变
        // 区间生成，必须在[S,N]整个范围上build
        seg.Build(s, n, root);
        // 区间修改，可以改变L、R和C的值，其他值不可改变
        seg.Add(l, r, c, s, n, root);
        // 区间更新，可以改变L、R和C的值，其他值不可改变
        seg.Update(l, r, c, s, n, root);
        // 区间查询，可以改变L和R的值，其他值不可改变
        var sum = seg.Query(l, r, s, n, root);
        Console.WriteLine(sum);

        Console.WriteLine("对数器测试开始...");
        Console.WriteLine("测试结果 : " + (Test() ? "通过" : "未通过"));
    }

    private class Right
    {
        private readonly int[] _arr;

        public Right(int[] origin)
        {
            _arr = new int[origin.Length + 1];
            for (var i = 0; i < origin.Length; i++) _arr[i + 1] = origin[i];
        }

        public void Update(int l, int r, int c)
        {
            for (var i = l; i <= r; i++) _arr[i] = c;
        }

        public void Add(int l, int r, int c)
        {
            for (var i = l; i <= r; i++) _arr[i] += c;
        }

        public long Query(int l, int r)
        {
            long ans = 0;
            for (var i = l; i <= r; i++) ans += _arr[i];

            return ans;
        }
    }
}