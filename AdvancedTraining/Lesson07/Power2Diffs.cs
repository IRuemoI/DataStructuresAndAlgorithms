#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson07;

public class Power2Diffs
{
    /*
     * 给定一个有序数组arr，其中值可能为正、负、0。 返回arr中每个数都平方之后不同的结果有多少种？
     *
     * 给定一个数组arr，先递减然后递增，返回arr中有多少个绝对值不同的数字？
     *
     */

    // 时间复杂度O(N)，额外空间复杂度O(N)
    private static int Diff1(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        var set = new HashSet<int>();
        foreach (var cur in arr) set.Add(cur * cur);
        return set.Count;
    }

    // 时间复杂度O(N)，额外空间复杂度O(1)
    private static int Diff2(int[] arr)
    {
        var n = arr.Length;
        var l = 0;
        var r = n - 1;
        var count = 0;
        while (l <= r)
        {
            count++;
            var leftAbs = Math.Abs(arr[l]);
            var rightAbs = Math.Abs(arr[r]);
            if (leftAbs < rightAbs)
            {
                while (r >= 0 && Math.Abs(arr[r]) == rightAbs) r--;
            }
            else if (leftAbs > rightAbs)
            {
                while (l < n && Math.Abs(arr[l]) == leftAbs) l++;
            }
            else
            {
                while (l < n && Math.Abs(arr[l]) == leftAbs) l++;
                while (r >= 0 && Math.Abs(arr[r]) == rightAbs) r--;
            }
        }

        return count;
    }

    //用于测试
    private static int[] randomSortedArray(int len, int value)
    {
        var ans = new int[(int)(Utility.getRandomDouble * len) + 1];
        for (var i = 0; i < ans.Length; i++)
            ans[i] = (int)(Utility.getRandomDouble * value) - (int)(Utility.getRandomDouble * value);
        Array.Sort(ans);
        return ans;
    }

    //用于测试
    private static void PrintArray(int[] arr)
    {
        foreach (var cur in arr) Console.Write(cur + " ");
        Console.WriteLine();
    }

    public static void Run()
    {
        var len = 100;
        var value = 500;
        var testTimes = 200000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTimes; i++)
        {
            var arr = randomSortedArray(len, value);
            var ans1 = Diff1(arr);
            var ans2 = Diff2(arr);
            if (ans1 != ans2)
            {
                PrintArray(arr);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                Console.WriteLine("出错啦！");
                break;
            }
        }

        Console.WriteLine("测试完成");
    }
}