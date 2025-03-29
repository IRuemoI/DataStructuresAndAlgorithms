namespace Common.DataStructures;

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
        _lazy = new int[maxN << 2]; // 用来支持脑补概念中，某一个范围没有往下传递的累加任务
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