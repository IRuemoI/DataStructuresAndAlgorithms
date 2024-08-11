namespace AdvancedTraining.Lesson38;

// 360笔试题
// 长城守卫军
// 题目描述：
// 长城上有连成一排的n个烽火台，每个烽火台都有士兵驻守。
// 第i个烽火台驻守着ai个士兵，相邻峰火台的距离为1。另外，有m位将军，
// 每位将军可以驻守一个峰火台，每个烽火台可以有多个将军驻守，
// 将军可以影响所有距离他驻守的峰火台小于等于x的烽火台。
// 每个烽火台的基础战斗力为士兵数，另外，每个能影响此烽火台的将军都能使这个烽火台的战斗力提升k。
// 长城的战斗力为所有烽火台的战斗力的最小值。
// 请问长城的最大战斗力可以是多少？
// 输入描述
// 第一行四个正整数n,m,x,k(1<=x<=n<=10^5,0<=m<=10^5,1<=k<=10^5)
// 第二行n个整数ai(0<=ai<=10^5)
// 输出描述 仅一行，一个整数，表示长城的最大战斗力
// 样例输入
// 5 2 1 2
// 4 4 2 4 4
// 样例输出
// 6
public class GreatWall
{
    private static int MaxForce(int[] wall, int m, int x, int k)
    {
        long l = 0;
        long r = 0;
        foreach (var num in wall) r = Math.Max(r, num);
        r += m * k;
        long ans = 0;
        while (l <= r)
        {
            var m1 = (l + r) / 2;
            if (Can(wall, m, x, k, m1))
            {
                ans = m1;
                l = m1 + 1;
            }
            else
            {
                r = m1 - 1;
            }
        }

        return (int)ans;
    }

    private static bool Can(int[] wall, int m, int x, int k, long limit)
    {
        var n = wall.Length;
        // 注意：下标从1开始
        var st = new SegmentTree(wall);
        st.Build(1, n, 1);
        var need = 0;
        for (var i = 0; i < n; i++)
        {
            // 注意：下标从1开始
            var cur = st.Query(i + 1, i + 1, 1, n, 1);
            if (cur < limit)
            {
                var add = (int)((limit - cur + k - 1) / k);
                need += add;
                if (need > m) return false;
                st.Add(i + 1, Math.Min(i + x, n), add * k, 1, n, 1);
            }
        }

        return true;
    }

    public static void Run()
    {
        int[] wall = [3, 1, 1, 1, 3];
        const int m = 2;
        const int x = 3;
        const int k = 1;
        Console.WriteLine(MaxForce(wall, m, x, k));
    }

    private class SegmentTree
    {
        private readonly int[] _arr;
        private readonly int[] _change;
        private readonly int[] _lazy;

        private readonly int[] _sum;

        private readonly bool[] _updateArray;

        public SegmentTree(int[] origin)
        {
            var maxn = origin.Length + 1;
            _arr = new int[maxn];
            for (var i = 1; i < maxn; i++) _arr[i] = origin[i - 1];
            _sum = new int[maxn << 2];
            _lazy = new int[maxn << 2];
            _change = new int[maxn << 2];
            _updateArray = new bool[maxn << 2];
        }

        private void PushUp(int rt)
        {
            _sum[rt] = _sum[rt << 1] + _sum[(rt << 1) | 1];
        }

        private void PushDown(int rt, int ln, int rn)
        {
            if (_updateArray[rt])
            {
                _updateArray[rt << 1] = true;
                _updateArray[(rt << 1) | 1] = true;
                _change[rt << 1] = _change[rt];
                _change[(rt << 1) | 1] = _change[rt];
                _lazy[rt << 1] = 0;
                _lazy[(rt << 1) | 1] = 0;
                _sum[rt << 1] = _change[rt] * ln;
                _sum[(rt << 1) | 1] = _change[rt] * rn;
                _updateArray[rt] = false;
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

        public void Update(int left, int right, int c, int l, int r, int rt)
        {
            if (left <= l && r <= right)
            {
                _updateArray[rt] = true;
                _change[rt] = c;
                _sum[rt] = c * (r - l + 1);
                _lazy[rt] = 0;
                return;
            }

            var mid = (l + r) >> 1;
            PushDown(rt, mid - l + 1, r - mid);
            if (left <= mid) Update(left, right, c, l, mid, rt << 1);
            if (right > mid) Update(left, right, c, mid + 1, r, (rt << 1) | 1);
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

        public long Query(int left, int right, int l, int r, int rt)
        {
            if (left <= l && r <= right) return _sum[rt];
            var mid = (l + r) >> 1;
            PushDown(rt, mid - l + 1, r - mid);
            long ans = 0;
            if (left <= mid) ans += Query(left, right, l, mid, rt << 1);
            if (right > mid) ans += Query(left, right, mid + 1, r, (rt << 1) | 1);
            return ans;
        }
    }
}