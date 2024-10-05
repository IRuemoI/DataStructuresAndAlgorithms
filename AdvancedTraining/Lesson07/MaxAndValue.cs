#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson07;

// 给定一个正数组成的数组，长度一定大于1，求数组中哪两个数与的结果最大
public class MaxAndValue
{
    // O(N^2)的暴力解
    private static int MaxAndValue1(int[] arr)
    {
        var n = arr.Length;
        var max = int.MinValue;
        for (var i = 0; i < n; i++)
        for (var j = i + 1; j < n; j++)
            max = Math.Max(max, arr[i] & arr[j]);
        return max;
    }

    // O(N)的解
    // 因为是正数，所以不用考虑符号位(31位)
    // 首先来到30位，假设剩余的数字有N个(整体)，看看这一位是1的数，有几个
    // 如果有0个、或者1个
    // 说明不管怎么在数组中选择，任何两个数&的结果在第30位上都不可能有1了
    // 答案在第30位上的状态一定是0，
    // 保留剩余的N个数，继续考察第29位，谁也不淘汰(因为谁也不行，干脆接受30位上没有1的事实)
    // 如果有2个，
    // 说明答案就是这两个数(直接返回答案)，因为别的数在第30位都没有1，就这两个数有。
    // 如果有>2个，比如K个
    // 说明答案一定只用在这K个数中去选择某两个数，因为别的数在第30位都没有1，就这K个数有。
    // 答案在第30位上的状态一定是1，
    // 只把这K个数作为剩余的数，继续考察第29位，其他数都淘汰掉
    // .....
    // 现在来到i位，假设剩余的数字有M个，看看这一位是1的数，有几个
    // 如果有0个、或者1个
    // 说明不管怎么在M个数中选择，任何两个数&的结果在第i位上都不可能有1了
    // 答案在第i位上的状态一定是0，
    // 保留剩余的M个数，继续考察第i-1位
    // 如果有2个，
    // 说明答案就是这两个数(直接返回答案)，因为别的数在第i位都没有1，就这两个数有。
    // 如果有>2个，比如K个
    // 说明答案一定只用在这K个数中去选择某两个数，因为别的数在第i位都没有1，就这K个数有。
    // 答案在第i位上的状态一定是1，
    // 只把这K个数作为剩余的数，继续考察第i-1位，其他数都淘汰掉
    private static int MaxAndValue2(int[] arr)
    {
        // arr[0...M-1]  arr[M....]
        var m = arr.Length;
        var ans = 0;
        for (var bit = 30; bit >= 0; bit--)
        {
            // arr[0...M-1] arr[M...]
            var i = 0;
            var tmp = m;
            while (i < m)
                // arr[0...M-1]
                if ((arr[i] & (1 << bit)) == 0)
                    Swap(arr, i, --m);
                else
                    i++;
            if (m == 2)
                // arr[0,1]
                return arr[0] & arr[1];
            if (m < 2)
                m = tmp;
            else
                // > 2个数  bit位上有1
                ans |= 1 << bit;
        }

        return ans;
    }

    private static void Swap(int[] arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    private static int[] GetRandomArray(int size, int range)
    {
        var arr = new int[size];
        for (var i = 0; i < size; i++) arr[i] = (int)(Utility.getRandomDouble * range) + 1;
        return arr;
    }

    public static void Run()
    {
        const int maxSize = 50;
        const int range = 30;
        const int testTime = 1000000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var size = (int)(Utility.getRandomDouble * maxSize) + 2;
            var arr = GetRandomArray(size, range);
            var ans1 = MaxAndValue1(arr);
            var ans2 = MaxAndValue2(arr);
            if (ans1 != ans2) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试结束");
    }
}