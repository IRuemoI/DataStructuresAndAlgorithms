#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson34;

//https://leetcode.cn/problems/wiggle-sort-ii/description/
public class WiggleSortIi //Problem_0324
{
    // 时间复杂度O(N)，额外空间复杂度O(1)
    private static void WiggleSort(int[]? numbers)
    {
        if (numbers == null || numbers.Length < 2) return;
        var n = numbers.Length;
        // 小 中 右
        FindIndexNum(numbers, 0, numbers.Length - 1, n / 2);
        if ((n & 1) == 0)
        {
            // R L -> L R
            Shuffle(numbers, 0, numbers.Length - 1);
            // R1 L1 R2 L2 R3 L3 R4 L4
            // L4 R4 L3 R3 L2 R2 L1 R1 -> 代码中的方式，可以的！
            // L1 R1 L2 R2 L3 R3 L4 R4 -> 课上的分析，是不行的！不能两两交换！
            Reverse(numbers, 0, numbers.Length - 1);
            // 做个实验，如果把上一行的code注释掉(reverse过程)，然后跑下面注释掉的for循环代码
            // for循环的代码就是两两交换，会发现对数器报错，说明两两交换是不行的, 必须整体逆序
            //			for (int i = 0; i < nums.length; i += 2) {
            //				Swap(nums, i, i + 1);
            //			}
        }
        else
        {
            Shuffle(numbers, 1, numbers.Length - 1);
        }
    }

    private static int FindIndexNum(int[] arr, int l, int r, int index)
    {
        while (l < r)
        {
            var pivot = arr[l + (int)(Utility.getRandomDouble * (r - l + 1))];
            var range = Partition(arr, l, r, pivot);
            if (index >= range[0] && index <= range[1])
                return arr[index];
            if (index < range[0])
                r = range[0] - 1;
            else
                l = range[1] + 1;
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

    private static void Shuffle(int[] nums, int l, int r)
    {
        while (r - l + 1 > 0)
        {
            var lenAndOne = r - l + 2;
            var bloom = 3;
            var k = 1;
            while (bloom <= lenAndOne / 3)
            {
                bloom *= 3;
                k++;
            }

            var m = (bloom - 1) / 2;
            var mid = (l + r) / 2;
            Rotate(nums, l + m, mid, mid + m);
            Cycles(nums, l - 1, bloom, k);
            l = l + bloom - 1;
        }
    }

    private static void Cycles(int[] numbers, int @base, int bloom, int k)
    {
        for (int i = 0, trigger = 1; i < k; i++, trigger *= 3)
        {
            var next = 2 * trigger % bloom;
            var cur = next;
            var record = numbers[next + @base];
            numbers[next + @base] = numbers[trigger + @base];
            while (cur != trigger)
            {
                next = 2 * cur % bloom;
                var tmp = numbers[next + @base];
                numbers[next + @base] = record;
                cur = next;
                record = tmp;
            }
        }
    }

    private static void Rotate(int[] arr, int l, int m, int r)
    {
        Reverse(arr, l, m);
        Reverse(arr, m + 1, r);
        Reverse(arr, l, r);
    }

    private static void Reverse(int[] arr, int l, int r)
    {
        while (l < r) Swap(arr, l++, r--);
    }

    private static void Swap(int[] nums, int i, int j)
    {
        (nums[i], nums[j]) = (nums[j], nums[i]);
    }

    // 为了测试，暴力方法
    // 把arr全排列尝试一遍，
    // 其中任何一个排列能做到 < > < > ... 返回true;
    // 如果没有任何一个排列能做到，返回false;
    private static bool Test(int[] arr)
    {
        return Process(arr, 0);
    }

    // 为了测试
    private static bool Process(int[] arr, int index)
    {
        if (index == arr.Length) return Valid(arr);
        for (var i = index; i < arr.Length; i++)
        {
            Swap(arr, index, i);
            if (Process(arr, index + 1)) return true;
            Swap(arr, index, i);
        }

        return false;
    }

    // 为了测试
    private static bool Valid(int[] arr)
    {
        var more = true;
        for (var i = 1; i < arr.Length; i++)
        {
            if ((more && arr[i - 1] >= arr[i]) || (!more && arr[i - 1] <= arr[i])) return false;
            more = !more;
        }

        return true;
    }

    // 为了测试
    private static int[] randomArray(int n, int v)
    {
        var ans = new int[n];
        for (var i = 0; i < n; i++) ans[i] = (int)(Utility.getRandomDouble * v);
        return ans;
    }

    // 为了测试
    private static int[] copyArray(int[] arr)
    {
        var ans = new int[arr.Length];
        for (var i = 0; i < arr.Length; i++) ans[i] = arr[i];
        return ans;
    }

    // 为了测试
    public static void Run()
    {
        const int n1 = 10;
        const int v = 10;
        const int testTime = 10000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var n = (int)(Utility.getRandomDouble * n1) + 1;
            var arr1 = randomArray(n, v);
            var arr2 = copyArray(arr1);
            WiggleSort(arr1);
            if (Valid(arr1) != Test(arr2)) Console.WriteLine("出错了!");
        }

        Console.WriteLine("测试结束");
    }
}