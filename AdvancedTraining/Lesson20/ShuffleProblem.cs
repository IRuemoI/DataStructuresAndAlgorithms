#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson20;

public class ShuffleProblem
{
    // 数组的长度为len，调整前的位置是i，返回调整之后的位置
    // 下标不从0开始，从1开始
    private static int ModifyIndex1(int i, int len)
    {
        if (i <= len / 2)
            return 2 * i;
        return 2 * (i - len / 2) - 1;
    }

    // 数组的长度为len，调整前的位置是i，返回调整之后的位置
    // 下标不从0开始，从1开始
    private static int ModifyIndex2(int i, int len)
    {
        return 2 * i % (len + 1);
    }

    // 主函数
    // 数组必须不为空，且长度为偶数
    private static void Shuffle(int[]? arr)
    {
        if (arr != null && arr.Length != 0 && (arr.Length & 1) == 0) Shuffle(arr, 0, arr.Length - 1);
    }

    // 在arr[L..R]上做完美洗牌的调整（arr[L..R]范围上一定要是偶数个数字）
    private static void Shuffle(int[] arr, int l, int r)
    {
        while (r - l + 1 > 0)
        {
            // 切成一块一块的解决，每一块的长度满足(3^k)-1
            var len = r - l + 1;
            var @base = 3;
            var k = 1;
            // 计算小于等于len并且是离len最近的，满足(3^k)-1的数
            // 也就是找到最大的k，满足3^k <= len+1
            while (@base <= (len + 1) / 3)
            {
                // base > (N+1)/3
                @base *= 3;
                k++;
            }

            // 3^k -1
            // 当前要解决长度为base-1的块，一半就是再除2
            var half = (@base - 1) / 2;
            // [L..R]的中点位置
            var mid = (l + r) / 2;
            // 要旋转的左部分为[L+half...mid], 右部分为arr[mid+1..mid+half]
            // 注意在这里，arr下标是从0开始的
            Rotate(arr, l + half, mid, mid + half);
            // 旋转完成后，从L开始算起，长度为base-1的部分进行下标连续推
            Cycles(arr, l, @base - 1, k);
            // 解决了前base-1的部分，剩下的部分继续处理
            l = l + @base - 1; // L ->     [] [+1...R]
        }
    }

    // 从start位置开始，往右len的长度这一段，做下标连续推
    // 出发位置依次为1,3,9...
    private static void Cycles(int[] arr, int start, int len, int k)
    {
        // 找到每一个出发位置trigger，一共k个
        // 每一个trigger都进行下标连续推
        // 出发位置是从1开始算的，而数组下标是从0开始算的。
        for (int i = 0, trigger = 1; i < k; i++, trigger *= 3)
        {
            var preValue = arr[trigger + start - 1];
            var cur = ModifyIndex2(trigger, len);
            while (cur != trigger)
            {
                (arr[cur + start - 1], preValue) = (preValue, arr[cur + start - 1]);
                cur = ModifyIndex2(cur, len);
            }

            arr[cur + start - 1] = preValue;
        }
    }

    // [L..M]为左部分，[M+1..R]为右部分，左右两部分互换
    private static void Rotate(int[] arr, int l, int m, int r)
    {
        Reverse(arr, l, m);
        Reverse(arr, m + 1, r);
        Reverse(arr, l, r);
    }

    // [L..R]做逆序调整
    private static void Reverse(int[] arr, int l, int r)
    {
        while (l < r)
        {
            var tmp = arr[l];
            arr[l++] = arr[r];
            arr[r--] = tmp;
        }
    }

    private static void WiggleSort(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return;
        // 假设这个排序是额外空间复杂度O(1)的，当然系统提供的排序并不是，你可以自己实现一个堆排序
        Array.Sort(arr);
        if ((arr.Length & 1) == 1)
        {
            Shuffle(arr, 1, arr.Length - 1);
        }
        else
        {
            Shuffle(arr, 0, arr.Length - 1);
            for (var i = 0; i < arr.Length; i += 2) (arr[i], arr[i + 1]) = (arr[i + 1], arr[i]);
        }
    }

    //用于测试
    private static bool IsValidWiggle(int[] arr)
    {
        for (var i = 1; i < arr.Length; i++)
        {
            if ((i & 1) == 1 && arr[i] < arr[i - 1]) return false;
            if ((i & 1) == 0 && arr[i] > arr[i - 1]) return false;
        }

        return true;
    }

    //用于测试
    private static void PrintArray(int[] arr)
    {
        foreach (var item in arr) Console.Write(item + " ");

        Console.WriteLine();
    }

    //用于测试
    private static int[] GenerateArray()
    {
        var len = (int)(Utility.GetRandomDouble * 10) * 2;
        var arr = new int[len];
        for (var i = 0; i < len; i++) arr[i] = (int)(Utility.GetRandomDouble * 100);
        return arr;
    }

    public static void Run()
    {
        for (var i = 0; i < 5000000; i++)
        {
            var arr = GenerateArray();
            WiggleSort(arr);
            if (!IsValidWiggle(arr))
            {
                Console.WriteLine("o出错");
                PrintArray(arr);
                break;
            }
        }
    }
}