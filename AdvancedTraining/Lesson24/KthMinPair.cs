#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson24;

public class KthMinPair
{
    // O(N^2 * log (N^2))的复杂度，你肯定过不了
    // 返回的int[] 长度是2，{3,1} int[2] = [3,1]
    private static int[]? KthMinPair1(int[] arr, int k)
    {
        var n = arr.Length;
        if (k > n * n) return null;
        var pairs = new Pair[n * n];
        var index = 0;
        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
            pairs[index++] = new Pair(arr[i], arr[j]);
        Array.Sort(pairs, new PairComparator());
        return new[] { pairs[k - 1].X, pairs[k - 1].Y };
    }

    // O(N*logN)的复杂度，你肯定过了
    private static int[]? kthMinPair2(int[] arr, int k)
    {
        var n = arr.Length;
        if (k > n * n) return null;
        // O(N*logN)
        Array.Sort(arr);
        // 第K小的数值对，第一维数字，是什么 是arr中
        var firstNum = arr[(k - 1) / n];
        var lessFirstNumSize = 0; // 数出比firstNum小的数有几个
        var firstNumSize = 0; // 数出==firstNum的数有几个
        // <= firstNum
        for (var i = 0; i < n && arr[i] <= firstNum; i++)
            if (arr[i] < firstNum)
                lessFirstNumSize++;
            else
                firstNumSize++;
        var rest = k - lessFirstNumSize * n;
        if (firstNumSize != 0)
            return [firstNum, arr[(rest - 1) / firstNumSize]];
        throw new InvalidOperationException("不能除以0");
    }

    // O(N)的复杂度，你肯定蒙了
    private static int[]? kthMinPair3(int[] arr, int k)
    {
        var n = arr.Length;
        if (k > n * n) return null;
        // 在无序数组中，找到第K小的数，返回值
        // 第K小，以1作为开始
        var firstNum = GetMinKth(arr, (k - 1) / n);
        // 第1维数字
        var lessFirstNumSize = 0;
        var firstNumSize = 0;
        for (var i = 0; i < n; i++)
        {
            if (arr[i] < firstNum) lessFirstNumSize++;
            if (arr[i] == firstNum) firstNumSize++;
        }

        var rest = k - lessFirstNumSize * n;
        if (firstNumSize != 0)
            return [firstNum, GetMinKth(arr, (rest - 1) / firstNumSize)];
        throw new InvalidOperationException("不能除以0");
    }

    // 改写快排，时间复杂度O(N)
    // 在无序数组arr中，找到，如果排序的话，arr[index]的数是什么？
    private static int GetMinKth(int[] arr, int index)
    {
        var l = 0;
        var r = arr.Length - 1;
        while (l < r)
        {
            var pivot = arr[l + (int)(Utility.GetRandomDouble * (r - l + 1))];
            var range = Partition(arr, l, r, pivot);
            if (index < range[0])
                r = range[0] - 1;
            else if (index > range[1])
                l = range[1] + 1;
            else
                return pivot;
        }

        return arr[l];
    }

    private static int[] Partition(int[] arr, int l, int r, int pivot)
    {
        var less = l - 1;
        var more = r + 1;
        var cur = l;
        while (cur < more)
            if (arr[cur] < pivot)
                Swap(arr, ++less, cur++);
            else if (arr[cur] > pivot)
                Swap(arr, cur, --more);
            else
                cur++;
        return [less + 1, more - 1];
    }

    private static void Swap(int[] arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    // 为了测试，生成值也随机，长度也随机的随机数组
    private static int[] GetRandomArray(int max, int len)
    {
        var arr = new int[(int)(Utility.GetRandomDouble * len) + 1];
        for (var i = 0; i < arr.Length; i++)
            arr[i] = (int)(Utility.GetRandomDouble * max) - (int)(Utility.GetRandomDouble * max);
        return arr;
    }

    // 为了测试
    private static int[]? CopyArray(int[]? arr)
    {
        if (arr == null) return null;
        var res = new int[arr.Length];
        for (var i = 0; i < arr.Length; i++) res[i] = arr[i];
        return res;
    }

    // 随机测试了百万组，保证三种方法都是对的
    public static void Run()
    {
        var max = 100;
        var len = 30;
        var testTimes = 100000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTimes; i++)
        {
            var arr = GetRandomArray(max, len);
            var arr1 = CopyArray(arr);
            var arr2 = CopyArray(arr);
            var arr3 = CopyArray(arr);
            var n = arr.Length * arr.Length;
            var k = (int)(Utility.GetRandomDouble * n) + 1;
            if (arr1 != null && arr2 != null && arr3 != null)
            {
                var ans1 = KthMinPair1(arr1, k);
                var ans2 = kthMinPair2(arr2, k);
                var ans3 = kthMinPair3(arr3, k);
                if (ans3 != null && ans2 != null && ans1 != null && (
                        ans1[0] != ans2[0] ||
                        ans2[0] != ans3[0] ||
                        ans1[1] != ans2[1] ||
                        ans2[1] != ans3[1]))
                    Console.WriteLine("出错啦！");
            }
        }

        Console.WriteLine("测试完成");
    }

    private class Pair(int a, int b)
    {
        public readonly int X = a;
        public readonly int Y = b;
    }

    private class PairComparator : IComparer<Pair>
    {
        public int Compare(Pair? arg0, Pair? arg1)
        {
            if (arg0 == null || arg1 == null) throw new Exception();
            return arg0.X != arg1.X ? arg0.X - arg1.X : arg0.Y - arg1.Y;
        }
    }
}