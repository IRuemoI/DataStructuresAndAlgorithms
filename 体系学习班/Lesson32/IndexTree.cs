//通过

namespace Algorithms.Lesson32;

public class IndexTree
{
    public static void Run()
    {
        const int n = 100;
        const int v = 100;
        var testTime = 20000;
        var tree = new IndexTree1(n);
        var test = new Right(n);
        Console.WriteLine("开始测试");
        for (var i = 0; i < testTime; i++)
        {
            var index = (int)(new Random().NextDouble() * n) + 1;
            if (new Random().NextDouble() <= 0.5)
            {
                var add = (int)(new Random().NextDouble() * v);
                tree.Add(index, add);
                test.Add(index, add);
            }
            else
            {
                if (tree.Sum(index) != test.Sum(index)) Console.WriteLine("出错了！");
            }
        }

        Console.WriteLine("测试完成");
    }

    private class IndexTree1
    {
        private readonly int _n;
        private readonly int[] _tree;

        public IndexTree1(int size)
        {
            _n = size;
            _tree = new int[_n + 1];
        }

        public int Sum(int index)
        {
            var ret = 0;
            while (index > 0)
            {
                ret += _tree[index];
                index -= index & -index;
            }

            return ret;
        }

        public void Add(int index, int d)
        {
            while (index <= _n)
            {
                _tree[index] += d;
                index += index & -index;
            }
        }
    }

    private class Right
    {
        private readonly int[] _nums;

        public Right(int size)
        {
            var n = size + 1;
            _nums = new int[n + 1];
        }

        public int Sum(int index)
        {
            var ret = 0;
            for (var i = 1; i <= index; i++) ret += _nums[i];

            return ret;
        }

        public void Add(int index, int d)
        {
            _nums[index] += d;
        }
    }
}