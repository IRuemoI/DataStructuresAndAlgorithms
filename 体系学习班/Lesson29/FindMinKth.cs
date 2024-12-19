//测试通过

#region

using Common.DataStructures.Heap;

#endregion

namespace Algorithms.Lesson29;

public class FindMinKth
{
    // 利用大根堆，时间复杂度O(N*logK)
    private static int MinKth1(int[] arr, int k)
    {
        var maxHeap = new Heap<int>((x, y) => y - x);
        for (var i = 0; i < k; i++) maxHeap.Push(arr[i]);

        for (var i = k; i < arr.Length; i++)
            if (arr[i] < maxHeap.Peek())
            {
                maxHeap.Pop();
                maxHeap.Push(arr[i]);
            }

        return maxHeap.Peek();
    }

    // 改写快排，时间复杂度O(N)
    // k >= 1
    private static int MinKth2(int[] array, int k)
    {
        var arr = CopyArray(array);
        return Process2(arr, 0, arr.Length - 1, k - 1);
    }

    private static int[] CopyArray(int[] arr)
    {
        var ans = new int[arr.Length];
        for (var i = 0; i != ans.Length; i++) ans[i] = arr[i];

        return ans;
    }

    // arr 第k小的数
    // process2(arr, 0, N-1, k-1)
    // arr[L..R]  范围上，如果排序的话(不是真的去排序)，找位于index的数
    // index [L..R]
    private static int Process2(int[] arr, int l, int r, int index)
    {
        if (l == r)
            // L = =R ==INDEX
            return arr[l];

        // 不止一个数  L +  [0, R -L]
        var pivot = arr[l + (int)(new Random().NextDouble() * (r - l + 1))];
        var range = Partition(arr, l, r, pivot);
        if (index >= range[0] && index <= range[1])
            return arr[index];
        if (index < range[0])
            return Process2(arr, l, range[0] - 1, index);
        return Process2(arr, range[1] + 1, r, index);
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

        return new[] { less + 1, more - 1 };
    }

    private static void Swap(int[] arr, int i1, int i2)
    {
        (arr[i1], arr[i2]) = (arr[i2], arr[i1]);
    }

    // 利用bfprt算法，时间复杂度O(N)
    private static int MinKth3(int[] array, int k)
    {
        var arr = CopyArray(array);
        return Bfprt(arr, 0, arr.Length - 1, k - 1);
    }

    // arr[L..R]  如果排序的话，位于index位置的数，是什么，返回
    private static int Bfprt(int[] arr, int l, int r, int index)
    {
        if (l == r) return arr[l];

        // L...R  每五个数一组
        // 每一个小组内部排好序
        // 小组的中位数组成新数组
        // 这个新数组的中位数返回
        var pivot = MedianOfMedians(arr, l, r);
        var range = Partition(arr, l, r, pivot);
        if (index >= range[0] && index <= range[1])
            return arr[index];
        if (index < range[0])
            return Bfprt(arr, l, range[0] - 1, index);
        return Bfprt(arr, range[1] + 1, r, index);
    }

    // arr[L...R]  五个数一组
    // 每个小组内部排序
    // 每个小组中位数领出来，组成marr
    // marr中的中位数，返回
    private static int MedianOfMedians(int[] arr, int l, int r)
    {
        var size = r - l + 1;
        var offset = size % 5 == 0 ? 0 : 1;
        var mArr = new int[size / 5 + offset];
        for (var team = 0; team < mArr.Length; team++)
        {
            var teamFirst = l + team * 5;
            // L ... L + 4
            // L +5 ... L +9
            // L +10....L+14
            mArr[team] = GetMedian(arr, teamFirst, Math.Min(r, teamFirst + 4));
        }

        // marr中，找到中位数
        // marr(0, marr.len - 1,  mArr.Length / 2 )
        return Bfprt(mArr, 0, mArr.Length - 1, mArr.Length / 2);
    }

    private static int GetMedian(int[] arr, int l, int r)
    {
        InsertionSort(arr, l, r);
        return arr[(l + r) / 2];
    }

    private static void InsertionSort(int[] arr, int l, int r)
    {
        for (var i = l + 1; i <= r; i++)
        for (var j = i - 1; j >= l && arr[j] > arr[j + 1]; j--)
            Swap(arr, j, j + 1);
    }

    //用于测试
    private static int[] GenerateRandomArray(int maxSize, int maxValue)
    {
        var arr = new int[(int)(new Random().NextDouble() * maxSize) + 1];
        for (var i = 0; i < arr.Length; i++) arr[i] = (int)(new Random().NextDouble() * (maxValue + 1));

        return arr;
    }

    public static void Run()
    {
        var testTime = 10000;
        var maxSize = 100;
        var maxValue = 100;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var arr = GenerateRandomArray(maxSize, maxValue);
            var k = (int)(new Random().NextDouble() * arr.Length) + 1;
            var ans1 = MinKth1(arr, k);
            var ans2 = MinKth2(arr, k);
            var ans3 = MinKth3(arr, k);
            if (ans1 != ans2 || ans2 != ans3) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");
    }
}