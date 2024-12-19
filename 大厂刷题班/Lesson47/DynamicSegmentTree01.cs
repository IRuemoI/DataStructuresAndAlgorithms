#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson47;

// 同时支持范围增加 + 范围修改 + 范围查询的动态开点线段树（累加和）
// 真的用到！才去建立
// 懒更新，及其所有的东西，和普通线段树，没有任何区别！
// todo:待整理
public class DynamicSegmentTree
{
    public static void Run()
    {
        const int n = 1000;
        const int value = 50;
        const int createTimes = 5000;
        const int operateTimes = 5000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < createTimes; i++)
        {
            var size = (int)(Utility.getRandomDouble * n) + 1;
            var dst = new DynamicSegmentTreeA(size);
            var right = new Right(size);
            for (var k = 0; k < operateTimes; k++)
            {
                var choose = Utility.getRandomDouble;
                if (choose < 0.333)
                {
                    var a = (int)(Utility.getRandomDouble * size) + 1;
                    var b = (int)(Utility.getRandomDouble * size) + 1;
                    var s = Math.Min(a, b);
                    var e = Math.Max(a, b);
                    var v = (int)(Utility.getRandomDouble * value);
                    dst.Update(s, e, v);
                    right.Update(s, e, v);
                }
                else if (choose < 0.666)
                {
                    var a = (int)(Utility.getRandomDouble * size) + 1;
                    var b = (int)(Utility.getRandomDouble * size) + 1;
                    var s = Math.Min(a, b);
                    var e = Math.Max(a, b);
                    var v = (int)(Utility.getRandomDouble * value);
                    dst.Add(s, e, v);
                    right.Add(s, e, v);
                }
                else
                {
                    var a = (int)(Utility.getRandomDouble * size) + 1;
                    var b = (int)(Utility.getRandomDouble * size) + 1;
                    var s = Math.Min(a, b);
                    var e = Math.Max(a, b);
                    var ans1 = dst.Query(s, e);
                    var ans2 = right.Query(s, e);
                    if (ans1 != ans2)
                    {
                        Console.WriteLine("出错了!");
                        Console.WriteLine(ans1);
                        Console.WriteLine(ans2);
                    }
                }
            }
        }

        Console.WriteLine("测试结束");
    }

    private class Node
    {
        public int Change;
        public int Lazy;
        public Node? Left;
        public Node? Right;
        public int Sum;
        public bool Update;
    }

    private class DynamicSegmentTreeA(int max)
    {
        private readonly Node? _root = new();
        private readonly int _size = max;

        internal void PushUp(Node c)
        {
            c.Sum = c.Left.Sum + c.Right.Sum;
        }

        private void PushDown(Node p, int ln, int rn)
        {
            if (p.Left == null) p.Left = new Node();
            if (p.Right == null) p.Right = new Node();
            if (p.Update)
            {
                p.Left.Update = true;
                p.Right.Update = true;
                p.Left.Change = p.Change;
                p.Right.Change = p.Change;
                p.Left.Lazy = 0;
                p.Right.Lazy = 0;
                p.Left.Sum = p.Change * ln;
                p.Right.Sum = p.Change * rn;
                p.Update = false;
            }

            if (p.Lazy != 0)
            {
                p.Left.Lazy += p.Lazy;
                p.Right.Lazy += p.Lazy;
                p.Left.Sum += p.Lazy * ln;
                p.Right.Sum += p.Lazy * rn;
                p.Lazy = 0;
            }
        }

        public void Update(int s, int e, int v)
        {
            Update(_root, 1, _size, s, e, v);
        }

        private void Update(Node c, int l, int r, int s, int e, int v)
        {
            if (s <= l && r <= e)
            {
                c.Update = true;
                c.Change = v;
                c.Sum = v * (r - l + 1);
                c.Lazy = 0;
            }
            else
            {
                var mid = (l + r) >> 1;
                PushDown(c, mid - l + 1, r - mid);
                if (s <= mid) Update(c.Left, l, mid, s, e, v);
                if (e > mid) Update(c.Right, mid + 1, r, s, e, v);
                PushUp(c);
            }
        }

        public void Add(int s, int e, int v)
        {
            Add(_root, 1, _size, s, e, v);
        }

        private void Add(Node c, int l, int r, int s, int e, int v)
        {
            if (s <= l && r <= e)
            {
                c.Sum += v * (r - l + 1);
                c.Lazy += v;
            }
            else
            {
                var mid = (l + r) >> 1;
                PushDown(c, mid - l + 1, r - mid);
                if (s <= mid) Add(c.Left, l, mid, s, e, v);
                if (e > mid) Add(c.Right, mid + 1, r, s, e, v);
                PushUp(c);
            }
        }

        public int Query(int s, int e)
        {
            return Query(_root, 1, _size, s, e);
        }

        private int Query(Node c, int l, int r, int s, int e)
        {
            if (s <= l && r <= e) return c.Sum;
            var mid = (l + r) >> 1;
            PushDown(c, mid - l + 1, r - mid);
            var ans = 0;
            if (s <= mid) ans += Query(c.Left, l, mid, s, e);
            if (e > mid) ans += Query(c.Right, mid + 1, r, s, e);
            return ans;
        }
    }

    private class Right
    {
        private readonly int[] _arr;

        public Right(int size)
        {
            _arr = new int[size + 1];
        }

        public void Add(int s, int e, int v)
        {
            for (var i = s; i <= e; i++) _arr[i] += v;
        }

        public void Update(int s, int e, int v)
        {
            for (var i = s; i <= e; i++) _arr[i] = v;
        }

        public int Query(int s, int e)
        {
            var sum = 0;
            for (var i = s; i <= e; i++) sum += _arr[i];
            return sum;
        }
    }
}