namespace AdvancedTraining.Lesson36;

// 来自美团
// 给定一个数组arr，长度为N，做出一个结构，可以高效的做如下的查询
// 1) int QuerySum(L,R) : 查询arr[L...R]上的累加和
// 2) int QueryAim(L,R) : 查询arr[L...R]上的目标值，目标值定义如下：
//        假设arr[L...R]上的值为[a,b,c,d]，a+b+c+d = s
//        目标值为 : (s-a)^2 + (s-b)^2 + (s-c)^2 + (s-d)^2
// 3) int QueryMax(L,R) : 查询arr[L...R]上的最大值
// 要求：
// 1) 初始化该结构的时间复杂度不能超过O(N*logN)
// 2) 三个查询的时间复杂度不能超过O(logN)
// 3) 查询时，认为arr的下标从1开始，比如 : 
//    arr = [ 1, 1, 2, 3 ];
//    QuerySum(1, 3) -> 4
//    QueryAim(2, 4) -> 50
//    QueryMax(1, 4) -> 3
public class Query3Problems
{
    public static void Run()
    {
        int[] arr = [1, 1, 2, 3];
        var q = new Query(arr);
        Console.WriteLine(q.QuerySum(1, 3));
        Console.WriteLine(q.QueryAim(2, 4));
        Console.WriteLine(q.QueryMax(1, 4));
    }

    private class SegmentTree
    {
        private readonly int[] _change;

        private readonly int[] _max;

        private readonly bool[] _updateArray;

        public SegmentTree(int n)
        {
            _max = new int[n << 2];
            _change = new int[n << 2];
            _updateArray = new bool[n << 2];
            for (var i = 0; i < _max.Length; i++) _max[i] = int.MinValue;
        }

        private void PushUp(int rt)
        {
            _max[rt] = Math.Max(_max[rt << 1], _max[(rt << 1) | 1]);
        }

        // ln表示左子树元素结点个数，rn表示右子树结点个数
        private void PushDown(int rt, int ln, int rn)
        {
            if (_updateArray[rt])
            {
                _updateArray[rt << 1] = true;
                _updateArray[(rt << 1) | 1] = true;
                _change[rt << 1] = _change[rt];
                _change[(rt << 1) | 1] = _change[rt];
                _max[rt << 1] = _change[rt];
                _max[(rt << 1) | 1] = _change[rt];
                _updateArray[rt] = false;
            }
        }

        public void Update(int left, int right, int c, int l, int r, int rt)
        {
            if (left <= l && r <= right)
            {
                _updateArray[rt] = true;
                _change[rt] = c;
                _max[rt] = c;
                return;
            }

            var mid = (l + r) >> 1;
            PushDown(rt, mid - l + 1, r - mid);
            if (left <= mid) Update(left, right, c, l, mid, rt << 1);
            if (right > mid) Update(left, right, c, mid + 1, r, (rt << 1) | 1);
            PushUp(rt);
        }

        public int Query(int L, int R, int l, int r, int rt)
        {
            if (L <= l && r <= R) return _max[rt];
            var mid = (l + r) >> 1;
            PushDown(rt, mid - l + 1, r - mid);
            var left = 0;
            var right = 0;
            if (L <= mid) left = Query(L, R, l, mid, rt << 1);
            if (R > mid) right = Query(L, R, mid + 1, r, (rt << 1) | 1);
            return Math.Max(left, right);
        }
    }

    private class Query
    {
        private readonly int _m;
        private readonly SegmentTree _st;
        private readonly int[] _sum1;
        private readonly int[] _sum2;

        public Query(int[] arr)
        {
            var n = arr.Length;
            _m = arr.Length + 1;
            _sum1 = new int[_m];
            _sum2 = new int[_m];
            _st = new SegmentTree(_m);
            for (var i = 0; i < n; i++)
            {
                _sum1[i + 1] = _sum1[i] + arr[i];
                _sum2[i + 1] = _sum2[i] + arr[i] * arr[i];
                _st.Update(i + 1, i + 1, arr[i], 1, _m, 1);
            }
        }

        public int QuerySum(int l, int r)
        {
            return _sum1[r] - _sum1[l - 1];
        }

        public int QueryAim(int l, int r)
        {
            var sumPower2 = QuerySum(l, r);
            sumPower2 *= sumPower2;
            return _sum2[r] - _sum2[l - 1] + (r - l - 1) * sumPower2;
        }

        public int QueryMax(int l, int r)
        {
            return _st.Query(l, r, 1, _m, 1);
        }
    }
}