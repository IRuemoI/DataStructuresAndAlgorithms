namespace Algorithms.Lesson31;

public static class FallingSquares
{
    private static Dictionary<int, int> Index(int[][] positions)
    {
        SortedSet<int> pos = new();
        foreach (var arr in positions)
        {
            pos.Add(arr[0]);
            pos.Add(arr[0] + arr[1] - 1);
        }

        Dictionary<int, int> map = new();
        var count = 0;
        foreach (var index in pos) map.Add(index, ++count);

        return map;
    }

    public static List<int> Code(int[][] positions)
    {
        var map = Index(positions);
        var n = map.Count;
        var segmentTree = new SegmentTree(n);
        var max = 0;
        List<int> res = new();
        // 每落一个正方形，收集一下，所有东西组成的图像，最高高度是什么
        foreach (var arr in positions)
        {
            var l = map[arr[0]];
            var r = map[arr[0] + arr[1] - 1];
            var height = segmentTree.Query(l, r, 1, n, 1) + arr[1];
            max = Math.Max(max, height);
            res.Add(max);
            segmentTree.Update(l, r, height, 1, n, 1);
        }

        return res;
    }

    private class SegmentTree
    {
        private readonly int[] _change;
        private readonly int[] _max;
        private readonly bool[] _update;

        public SegmentTree(int size)
        {
            var n = size + 1;
            _max = new int[n << 2];

            _change = new int[n << 2];
            _update = new bool[n << 2];
        }

        private void PushUp(int rt)
        {
            _max[rt] = Math.Max(_max[rt << 1], _max[(rt << 1) | 1]);
        }

        // ln表示左子树元素结点个数，rn表示右子树结点个数
        private void PushDown(int rt)
        {
            if (_update[rt])
            {
                _update[rt << 1] = true;
                _update[(rt << 1) | 1] = true;
                _change[rt << 1] = _change[rt];
                _change[(rt << 1) | 1] = _change[rt];
                _max[rt << 1] = _change[rt];
                _max[(rt << 1) | 1] = _change[rt];
                _update[rt] = false;
            }
        }

        public void Update(int l1, int r1, int c, int l, int r, int rt)
        {
            if (l1 <= l && r <= r1)
            {
                _update[rt] = true;
                _change[rt] = c;
                _max[rt] = c;
                return;
            }

            var mid = (l + r) >> 1;
            PushDown(rt);
            if (l1 <= mid) Update(l1, r1, c, l, mid, rt << 1);

            if (r1 > mid) Update(l1, r1, c, mid + 1, r, (rt << 1) | 1);

            PushUp(rt);
        }

        public int Query(int l1, int r1, int l, int r, int rt)
        {
            if (l1 <= l && r <= r1) return _max[rt];

            var mid = (l + r) >> 1;
            PushDown(rt);
            var left = 0;
            var right = 0;
            if (l1 <= mid) left = Query(l1, r1, l, mid, rt << 1);

            if (r1 > mid) right = Query(l1, r1, mid + 1, r, (rt << 1) | 1);

            return Math.Max(left, right);
        }
    }
}

public class FallingSquaresTest
{
    public static void Run()
    {
        int[][] positions1 =
        [
            [1, 2],
            [2, 3],
            [6, 1]
        ];

        int[][] positions2 =
        [
            [100, 100],
            [200, 100]
        ];

        foreach (var item in FallingSquares.Code(positions1)) //[2,5,5]
            Console.WriteLine(item);

        Console.WriteLine("----------------------------");

        foreach (var item in FallingSquares.Code(positions2)) //[100,100]
            Console.WriteLine(item);
    }
}