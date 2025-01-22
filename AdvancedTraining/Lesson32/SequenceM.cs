#region

using Common.Utilities;

#endregion
//pass
namespace AdvancedTraining.Lesson32;

// 给定一个数组arr，arr[i] = j，表示第i号试题的难度为j。给定一个非负数M
// 想出一张卷子，对于任何相邻的两道题目，前一题的难度不能超过后一题的难度+M
// 返回所有可能的卷子种数
public class SequenceM
{
    // 纯暴力方法，生成所有排列，一个一个验证
    private static int Ways1(int[]? arr, int m)
    {
        if (arr == null || arr.Length == 0) return 0;
        return Process(arr, 0, m);
    }

    private static int Process(int[] arr, int index, int m)
    {
        if (index == arr.Length)
        {
            for (var i = 1; i < index; i++)
                if (arr[i - 1] > arr[i] + m)
                    return 0;
            return 1;
        }

        var ans = 0;
        for (var i = index; i < arr.Length; i++)
        {
            Swap(arr, index, i);
            ans += Process(arr, index + 1, m);
            Swap(arr, index, i);
        }

        return ans;
    }

    private static void Swap(int[] arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    // 时间复杂度O(N * logN)
    // 从左往右的动态规划 + 范围上二分
    private static int Ways2(int[]? arr, int m)
    {
        if (arr == null || arr.Length == 0) return 0;
        Array.Sort(arr);
        var all = 1;
        for (var i = 1; i < arr.Length; i++) all *= (Num(arr, i - 1, arr[i] - m) + 1);
        return all;
    }

    // arr[0..r]上返回>=t的数有几个, 二分的方法
    // 找到 >=t 最左的位置a, 然后返回r - a + 1就是个数
    private static int Num(int[] arr, int r, int t)
    {
        var i = 0;
        var j = r;
        var a = r + 1;
        while (i <= j)
        {
            var m = (i + j) / 2;
            if (arr[m] >= t)
            {
                a = m;
                j = m - 1;
            }
            else
            {
                i = m + 1;
            }
        }

        return r - a + 1;
    }

    // 时间复杂度O(N * logV)
    // 从左往右的动态规划 + IndexTree
    private static int Ways3(int[]? arr, int m)
    {
        if (arr == null || arr.Length == 0) return 0;
        var max = int.MinValue;
        var min = int.MaxValue;
        foreach (var num in arr)
        {
            max = Math.Max(max, num);
            min = Math.Min(min, num);
        }

        var iTree = new IndexTree(max - min + 2);
        Array.Sort(arr);
        var all = 1;
        iTree.Add(arr[0] - min + 1, 1);
        for (var i = 1; i < arr.Length; i++)
        {
            var a = arr[i] - min + 1;
            var b = i - (a - m - 1 >= 1 ? iTree.Sum(a - m - 1) : 0);
            all = all * (b + 1);
            iTree.Add(a, 1);
        }

        return all;
    }

    // 为了测试
    private static int[] randomArray(int len, int value)
    {
        var arr = new int[len];
        for (var i = 0; i < len; i++) arr[i] = (int)(Utility.getRandomDouble * (value + 1));
        return arr;
    }

    // 为了测试
    public static void Run()
    {
        const int n = 10;
        const int value = 20;
        const int testTimes = 1000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTimes; i++)
        {
            var len = (int)(Utility.getRandomDouble * (n + 1));
            var arr = randomArray(len, value);
            var m = (int)(Utility.getRandomDouble * (value + 1));
            var ans1 = Ways1(arr, m);
            var ans2 = Ways2(arr, m);
            var ans3 = Ways3(arr, m);
            if (ans1 != ans2 || ans1 != ans3)
            {
                Console.WriteLine("出错了!");
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                Console.WriteLine(ans3);
            }
        }

        Console.WriteLine("测试结束");
    }

    // 注意开始下标是1，不是0
    private class IndexTree
    {
        private readonly int _n;

        private readonly int[] _tree;

        public IndexTree(int size)
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
}