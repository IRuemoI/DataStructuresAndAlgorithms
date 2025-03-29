#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson01;

/*
 * 给定两个数组x和hp，长度都是N。
 * x数组一定是有序的，x[i]表示i号怪兽在x轴上的位置；hp数组不要求有序，hp[i]表示i号怪兽的血量
 * 为了方便起见，可以认为x数组和hp数组中没有负数。
 * 再给定一个正数range，表示如果法师释放技能的范围长度, 被打到的每只怪兽损失1点血量。
 * 返回要把所有怪兽血量清空，至少需要释放多少次aoe技能？
 * 三个参数：int[] x, int[] hp, int range
 * 返回：int 次数
 * */
public class AoeQuestion
{
    // 纯暴力解法
    private static int MinAoe1(int[] x, int[] hp, int range)
    {
        var n = x.Length;
        var coverLeft = new int[n];
        var coverRight = new int[n];
        var left = 0;
        var right = 0;
        for (var i = 0; i < n; i++)
        {
            while (x[i] - x[left] > range)
                left++;
            while (right < n && x[right] - x[i] <= range)
                right++;
            coverLeft[i] = left;
            coverRight[i] = right - 1;
        }

        return Process(hp, coverLeft, coverRight);
    }

    private static int Process(int[] hp, int[] coverLeft, int[] coverRight)
    {
        var n = hp.Length;
        var ans = int.MaxValue;
        for (var i = 0; i < n; i++)
        for (var f = coverLeft[i]; f <= coverRight[i]; f++)
            if (hp[f] > 0)
            {
                var next = Aoe(hp, coverLeft[i], coverRight[i]);
                ans = Math.Min(ans, 1 + Process(next, coverLeft, coverRight));
                break;
            }

        return ans == int.MaxValue ? 0 : ans;
    }

    private static int[] Aoe(int[] hp, int l, int r)
    {
        var n = hp.Length;
        var next = new int[n];
        for (var i = 0; i < n; i++)
            next[i] = hp[i];
        for (var i = l; i <= r; i++)
            next[i] -= next[i] > 0 ? 1 : 0;
        return next;
    }

    // 贪心策略：永远让最左边缘以最优的方式(AOE尽可能往右扩，最让最左边缘盖住目前怪的最左)变成0，也就是选择：
    // 一定能覆盖到最左边缘, 但是尽量靠右的中心点
    // 等到最左边缘变成0之后，再去找下一个最左边缘...
    private static int MinAoe2(int[] x, int[] hp, int range)
    {
        var n = x.Length;
        var ans = 0;
        for (var i = 0; i < n; i++)
            if (hp[i] > 0)
            {
                var triggerPost = i;
                while (triggerPost < n && x[triggerPost] - x[i] <= range)
                    triggerPost++;
                ans += hp[i];
                Aoe(x, hp, i, triggerPost - 1, range);
            }

        return ans;
    }

    private static void Aoe(int[] x, int[] hp, int l, int trigger, int range)
    {
        var n = x.Length;
        var rPost = trigger;
        while (rPost < n && x[rPost] - x[trigger] <= range)
            rPost++;
        var minus = hp[l];
        for (var i = l; i < rPost; i++)
            hp[i] = Math.Max(0, hp[i] - minus);
    }

    // 贪心策略和方法二一样，但是需要用线段树，可优化成O(N * logN)的方法，
    private static int MinAoe3(int[] x, int[] hp, int range)
    {
        var n = x.Length;
        // coverLeft[i]：如果以i为中心点放技能，左侧能影响到哪，下标从1开始，不从0开始
        // coverRight[i]：如果以i为中心点放技能，右侧能影响到哪，下标从1开始，不从0开始
        // coverLeft和coverRight数组，0位置弃而不用
        // 举个例子，比如 :
        // x = [1,2,5,7,9,12,15], range = 3
        // 下标: 1 2 3 4 5 6 7
        // 以1位置做中心点: 能覆盖位置:1,2 -> [1..2]
        // 以2位置做中心点: 能覆盖位置:1,2,3 -> [1..3]
        // 以3位置做中心点: 能覆盖位置:2,3,4 -> [2..4]
        // 以4位置做中心点: 能覆盖位置:3,4,5 -> [3..5]
        // 以5位置做中心点: 能覆盖位置:4,5,6 -> [4..6]
        // 以6位置做中心点: 能覆盖位置:5,6,7 -> [5..7]
        // 以7位置做中心点: 能覆盖位置:6,7 -> [6..7]
        // 可以看出如果从左往右，依次求每个位置的左边界(left)和左边界(right)，是可以不回退的！
        var coverLeft = new int[n + 1];
        var coverRight = new int[n + 1];
        var left = 0;
        var right = 0;
        // 从左往右，不回退的依次求每个位置的左边界(left)和左边界(right)，记录到coverLeft和coverRight里
        for (var i = 0; i < n; i++)
        {
            while (x[i] - x[left] > range)
                left++;
            while (right < n && x[right] - x[i] <= range)
                right++;
            coverLeft[i + 1] = left + 1;
            coverRight[i + 1] = right;
        }

        // best[i]: 如果i是最左边缘点，选哪个点做技能中心点最好，下标从1开始，不从0开始
        // 与上面同理，依然可以不回退
        var best = new int[n + 1];
        var trigger = 0;
        for (var i = 0; i < n; i++)
        {
            while (trigger < n && x[trigger] - x[i] <= range)
                trigger++;
            best[i + 1] = trigger;
        }

        var st = new SegmentTree(hp);
        st.Build(1, n, 1);
        var ans = 0;
        // 整体思路：
        // 当前左边缘点从i位置开始(注意0位置已经弃而不用了)，
        // 目标是把左边缘的怪物杀死，但是放技能的位置当然是尽可能远离左边缘点，但是又保证能覆盖住
        // best[i] : 放技能的位置当然是尽可能远离左边缘点i，但是又保证能覆盖住，
        // 请问这个中心在哪？就是best的含义，之前求过了。
        // 然后在这个中心点，放技能，放几次技能呢？左边缘点还剩多少血，就放几次技能，
        // 这样能保证刚好杀死左边缘点。
        // 然后向右继续寻找下一个没有死的左边缘点。
        for (var i = 1; i <= n; i++)
        {
            // 查询当前i位置，还有没有怪物存活
            var leftEdge = st.Query(i, i, 1, n, 1);
            // 如果还有血量(leftEdge > 0)，说明有存活。此时，放技能
            // 如果没有血了(leftEdge <= 0)，说明当前边缘点不需要考虑了，换下一个i
            if (leftEdge > 0)
            {
                // t = best[i]: 在哪放技能最值
                // l = coverLeft[t]: 如果在t放技能的话，左边界影响到哪
                // r = coverRight[t]: 如果在t放技能的话，右边界影响到哪
                // 就在t放技能，放leftEdge次，这样左边缘点恰好被杀死
                ans += (int)leftEdge;
                var t = best[i];
                var l = coverLeft[t];
                var r = coverRight[t];
                // 同时[l...r]整个范围，所有的怪物都会扣除掉leftEdge的血量，因为AOE嘛！
                st.Add(l, r, (int)-leftEdge, 1, n, 1);
            }
        }

        return ans;
    }

    //用于测试
    private static int[] RandomArray(int n, int valueMax)
    {
        var ans = new int[n];
        for (var i = 0; i < n; i++)
            ans[i] = (int)(Utility.getRandomDouble * valueMax) + 1;
        return ans;
    }

    //用于测试
    private static int[] CopyArray(int[] arr)
    {
        var n = arr.Length;
        var ans = new int[n];
        for (var i = 0; i < n; i++)
            ans[i] = arr[i];
        return ans;
    }

    public static void Run()
    {
        const int n = 50;
        const int valueMax = 100;
        const int h = 10;
        const int r = 5;
        const int time = 1;
        Console.WriteLine("测试开始");
        for (var i = 0; i < time; i++)
        {
            var len = (int)(Utility.getRandomDouble * n) + 1;
            var x = RandomArray(len, valueMax);
            Array.Sort(x);
            var hp = RandomArray(len, h);
            var range = (int)(Utility.getRandomDouble * r) + 1;
            var x2 = CopyArray(x);
            var hp2 = CopyArray(hp);
            var ans2 = MinAoe2(x2, hp2, range);
            // 已经测过下面注释掉的内容，注意minAoe1非常慢，
            // 所以想加入对比需要把数据量(n, valueMax, h, r, time)改小
            var x1 = CopyArray(x);
            var hp1 = CopyArray(hp);
            var ans1 = MinAoe1(x1, hp1, range);
            if (ans1 != ans2)
            {
                Console.WriteLine("出错");
                Console.WriteLine(ans1 + "," + ans2);
            }

            var x3 = CopyArray(x);
            var hp3 = CopyArray(hp);
            var ans3 = MinAoe3(x3, hp3, range);
            if (ans2 != ans3)
            {
                Console.WriteLine("出错啦！");
                Console.WriteLine(ans2 + "," + ans3);
            }
        }

        Console.WriteLine("测试结束");
    }

    private class SegmentTree
    {
        private readonly int[] _arr;
        private readonly int[] _change;

        private readonly int[] _lazy;

        // arr[]为原序列的信息从0开始，但在arr里是从1开始的
        // sum[]模拟线段树维护区间和
        // lazy[]为累加懒惰标记
        // change[]为更新的值
        // Update[]为更新慵懒标记

        private readonly int[] _sum;

        private readonly bool[] _updateArray;

        public SegmentTree(int[] origin)
        {
            var maxN = origin.Length + 1;
            _arr = new int[maxN]; // arr[0] 不用 从1开始使用
            for (var i = 1; i < maxN; i++)
                _arr[i] = origin[i - 1];
            _sum = new int[maxN << 2]; // 用来支持脑补概念中，某一个范围的累加和信息

            _lazy = new int[maxN << 2]; // 用来支持脑补概念中，某一个范围沒有往下傳遞的纍加任務
            _change = new int[maxN << 2]; // 用来支持脑补概念中，某一个范围有没有更新操作的任务
            _updateArray = new bool[maxN << 2]; // 用来支持脑补概念中，某一个范围更新任务，更新成了什么
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

            // 当前任务躲不掉，无法懒更新，要往下发
            var mid = (l + r) >> 1;
            PushDown(rt, mid - l + 1, r - mid);
            if (left <= mid)
                Update(left, right, c, l, mid, rt << 1);
            if (right > mid)
                Update(left, right, c, mid + 1, r, (rt << 1) | 1);
            PushUp(rt);
        }

        // L..R -> 任务范围 ,所有的值累加上C
        // l,r -> 表达的范围
        // rt 去哪找l，r范围上的信息
        public void Add(int left, int right, int c, int l, int r, int rt)
        {
            // 任务的范围彻底覆盖了，当前表达的范围
            if (left <= l && r <= right)
            {
                _sum[rt] += c * (r - l + 1);
                _lazy[rt] += c;
                return;
            }

            // 任务并没有把l...r全包住
            // 要把当前任务往下发
            // 任务 L, R 没有把本身表达范围 l,r 彻底包住
            var mid = (l + r) >> 1; // l..mid (rt << 1) mid+1...r(rt << 1 | 1)
            // 下发之前所有攒的懒任务
            PushDown(rt, mid - l + 1, r - mid);
            // 左孩子是否需要接到任务
            if (left <= mid)
                Add(left, right, c, l, mid, rt << 1);
            // 右孩子是否需要接到任务
            if (right > mid)
                Add(left, right, c, mid + 1, r, (rt << 1) | 1);
            // 左右孩子做完任务后，我更新我的sum信息
            PushUp(rt);
        }

        // 1~6 累加和是多少？ 1~8 rt
        public long Query(int left, int right, int l, int r, int rt)
        {
            if (left <= l && r <= right)
                return _sum[rt];
            var mid = (l + r) >> 1;
            PushDown(rt, mid - l + 1, r - mid);
            long ans = 0;
            if (left <= mid)
                ans += Query(left, right, l, mid, rt << 1);
            if (right > mid)
                ans += Query(left, right, mid + 1, r, (rt << 1) | 1);
            return ans;
        }
    }
}