//pass
namespace AdvancedTraining.Lesson20;
// 本题为leetcode原题
// 测试链接：https://leetcode.cn/problems/largest-component-size-by-common-factor/
// 方法1会超时，但是方法2直接通过
public class LargestComponentSizeByCommonFactor
{
    private static int LargestComponentSize1(int[] arr)
    {
        var n = arr.Length;
        var set = new UnionFind(n);
        for (var i = 0; i < n; i++)
        for (var j = i + 1; j < n; j++)
            if (Gcd(arr[i], arr[j]) != 1)
                set.Union(i, j);
        return set.MaxSize();
    }

    private static int LargestComponentSize2(int[] arr)
    {
        var n = arr.Length;
        // arr中，N个位置，在并查集初始时，每个位置自己是一个集合
        var unionFind = new UnionFind(n);
        //      key 某个因子   value 哪个位置拥有这个因子
        var fatorsMap = new Dictionary<int, int>();
        for (var i = 0; i < n; i++)
        {
            var num = arr[i];
            // 求出根号N， -> limit
            var limit = (int)Math.Sqrt(num);
            for (var j = 1; j <= limit; j++)
                if (num % j == 0)
                {
                    if (j != 1)
                        if (!fatorsMap.TryAdd(j, i))
                            unionFind.Union(fatorsMap[j], i);

                    var other = num / j;
                    if (other != 1)
                        if (!fatorsMap.TryAdd(other, i))
                            unionFind.Union(fatorsMap[other], i);
                }
        }

        return unionFind.MaxSize();
    }

    // O(1)
    // m,n 要是正数，不能有任何一个等于0
    private static int Gcd(int a, int b)
    {
        return b == 0 ? a : Gcd(b, a % b);
    }

    public static void Run()
    {
        Console.WriteLine(LargestComponentSize1([2, 3, 6, 7, 4, 12, 21, 39])); //输出8
        Console.WriteLine(LargestComponentSize1([2, 3, 6, 7, 4, 12, 21, 39])); //输出8
    }

    private class UnionFind
    {
        private readonly int[] _help;
        private readonly int[] _parents;
        private readonly int[] _sizes;

        public UnionFind(int n)
        {
            _parents = new int[n];
            _sizes = new int[n];
            _help = new int[n];
            for (var i = 0; i < n; i++)
            {
                _parents[i] = i;
                _sizes[i] = 1;
            }
        }

        public int MaxSize()
        {
            var ans = 0;
            foreach (var size in _sizes) ans = Math.Max(ans, size);
            return ans;
        }

        private int Find(int i)
        {
            var hi = 0;
            while (i != _parents[i])
            {
                _help[hi++] = i;
                i = _parents[i];
            }

            for (hi--; hi >= 0; hi--) _parents[_help[hi]] = i;
            return i;
        }

        public void Union(int i, int j)
        {
            var f1 = Find(i);
            var f2 = Find(j);
            if (f1 != f2)
            {
                var big = _sizes[f1] >= _sizes[f2] ? f1 : f1;
                var small = big == f1 ? f2 : f1;
                _parents[small] = big;
                _sizes[big] = _sizes[f1] + _sizes[f2];
            }
        }
    }
}