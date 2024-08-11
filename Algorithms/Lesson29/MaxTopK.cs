//测试通过

namespace Algorithms.Lesson29;

public class MaxTopK
{
    // 时间复杂度O(N*logN)
    // 排序+收集
    private static int[] MaxTopK1(int[]? arr, int k)
    {
        if (arr == null || arr.Length == 0) return Array.Empty<int>();

        var n = arr.Length;
        k = Math.Min(n, k);
        Array.Sort(arr);
        var ans = new int[k];
        for (int i = n - 1, j = 0; j < k; i--, j++) ans[j] = arr[i];

        return ans;
    }

    // 方法二，时间复杂度O(N + K*logN)
    // 解释：堆
    private static int[] MaxTopK2(int[]? arr, int k)
    {
        if (arr == null || arr.Length == 0) return Array.Empty<int>();

        var n = arr.Length;
        k = Math.Min(n, k);
        // 从底向上建堆，时间复杂度O(N)
        for (var i = n - 1; i >= 0; i--) Heapify(arr, i, n);

        // 只把前K个数放在arr末尾，然后收集，O(K*logN)
        var heapSize = n;
        Swap(arr, 0, --heapSize);
        var count = 1;
        while (heapSize > 0 && count < k)
        {
            Heapify(arr, 0, heapSize);
            Swap(arr, 0, --heapSize);
            count++;
        }

        var ans = new int[k];
        for (int i = n - 1, j = 0; j < k; i--, j++) ans[j] = arr[i];

        return ans;
    }

    private static void HeapInsert(int[] arr, int index)
    {
        while (arr[index] > arr[(index - 1) / 2])
        {
            Swap(arr, index, (index - 1) / 2);
            index = (index - 1) / 2;
        }
    }

    private static void Heapify(int[] arr, int index, int heapSize)
    {
        var left = index * 2 + 1;
        while (left < heapSize)
        {
            var largest = left + 1 < heapSize && arr[left + 1] > arr[left] ? left + 1 : left;
            largest = arr[largest] > arr[index] ? largest : index;
            if (largest == index) break;

            Swap(arr, largest, index);
            index = largest;
            left = index * 2 + 1;
        }
    }

    private static void Swap(int[] arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    // 方法三，时间复杂度O(n + k * logk)
    private static int[] MaxTopK3(int[]? arr, int k)
    {
        if (arr == null || arr.Length == 0) return Array.Empty<int>();

        var n = arr.Length;
        k = Math.Min(n, k);
        // O(N)
        var num = MinKth(arr, n - k);
        var ans = new int[k];
        var index = 0;
        for (var i = 0; i < n; i++)
            if (arr[i] > num)
                ans[index++] = arr[i];

        for (; index < k; index++) ans[index] = num;

        // O(k*logk)
        Array.Sort(ans);
        for (int l = 0, r = k - 1; l < r; l++, r--) Swap(ans, l, r);

        return ans;
    }

    // 时间复杂度O(N)
    private static int MinKth(int[] arr, int index)
    {
        var l = 0;
        var r = arr.Length - 1;
        while (l < r)
        {
            var pivot = arr[l + (int)(new Random().NextDouble() * (r - l + 1))];
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

        return new[] { less + 1, more - 1 };
    }

    //用于测试
    private static int[] GenerateRandomArray(int maxSize, int maxValue)
    {
        var arr = new int[(int)((maxSize + 1) * new Random().NextDouble())];
        for (var i = 0; i < arr.Length; i++)
            // [-? , +?]
            arr[i] = (int)((maxValue + 1) * new Random().NextDouble()) - (int)(maxValue * new Random().NextDouble());

        return arr;
    }

    //用于测试
    private static int[]? CopyArray(int[]? arr)
    {
        if (arr == null) return null;

        var res = new int[arr.Length];
        for (var i = 0; i < arr.Length; i++) res[i] = arr[i];

        return res;
    }

    //用于测试
    private static bool IsEqual(int[]? arr1, int[]? arr2)
    {
        if ((arr1 == null && arr2 != null) || (arr1 != null && arr2 == null)) return false;

        if (arr1 == null && arr2 == null) return true;

        if (arr1?.Length != arr2?.Length) return false;

        for (var i = 0; i < arr1?.Length; i++)
            if (arr1[i] != arr2?[i])
                return false;

        return true;
    }

    //用于测试
    private static void PrintArray(int[]? arr)
    {
        if (arr == null) return;

        foreach (var element in arr) Console.Write(element + " ");

        Console.WriteLine();
    }

    // 生成随机数组测试
    public static void Run()
    {
        var testTime = 50000;
        var maxSize = 100;
        var maxValue = 100;
        var pass = true;
        Console.WriteLine("测试开始，没有打印出错信息说明测试通过");
        for (var i = 0; i < testTime; i++)
        {
            var k = (int)(new Random().NextDouble() * maxSize) + 1;
            var arr = GenerateRandomArray(maxSize, maxValue);

            var arr1 = CopyArray(arr);
            var arr2 = CopyArray(arr);
            var arr3 = CopyArray(arr);

            var ans1 = MaxTopK1(arr1, k);
            var ans2 = MaxTopK2(arr2, k);
            var ans3 = MaxTopK3(arr3, k);
            if (!IsEqual(ans1, ans2) || !IsEqual(ans1, ans3))
            {
                pass = false;
                Console.WriteLine("出错了！");
                PrintArray(ans1);
                PrintArray(ans2);
                PrintArray(ans3);
                break;
            }
        }

        Console.WriteLine("测试结束了，测试了" + testTime + "组，是否所有测试用例都通过？" + (pass ? "是" : "否"));
    }
}