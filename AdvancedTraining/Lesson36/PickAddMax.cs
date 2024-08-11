#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson36;

// 来自腾讯
// 给定一个数组arr，当拿走某个数a的时候，其他所有的数都+a
// 请返回最终所有数都拿走的最大分数
// 比如: [2,3,1]
// 当拿走3时，获得3分，数组变成[5,4]
// 当拿走5时，获得5分，数组变成[9]
// 当拿走9时，获得9分，数组变成[]
// 这是最大的拿取方式，返回总分17
public class PickAddMax
{
    // 最优解
    private static int Pick(int[] arr)
    {
        Array.Sort(arr);
        var ans = 0;
        for (var i = arr.Length - 1; i >= 0; i--) ans = (ans << 1) + arr[i];
        return ans;
    }

    // 纯暴力方法，为了测试
    private static int Test(int[] arr)
    {
        if (arr.Length == 1) return arr[0];
        var ans = 0;
        for (var i = 0; i < arr.Length; i++)
        {
            var rest = removeAddOthers(arr, i);
            ans = Math.Max(ans, arr[i] + Test(rest));
        }

        return ans;
    }

    // 为了测试
    private static int[] removeAddOthers(int[] arr, int i)
    {
        var rest = new int[arr.Length - 1];
        var ri = 0;
        for (var j = 0; j < i; j++) rest[ri++] = arr[j] + arr[i];
        for (var j = i + 1; j < arr.Length; j++) rest[ri++] = arr[j] + arr[i];
        return rest;
    }

    // 为了测试
    private static int[] randomArray(int len, int value)
    {
        var arr = new int[len];
        for (var i = 0; i < len; i++) arr[i] = (int)(Utility.GetRandomDouble * value) + 1;
        return arr;
    }

    // 为了测试
    public static void Run()
    {
        const int n = 7;
        const int v = 10;
        const int testTime = 10000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var len = (int)(Utility.GetRandomDouble * n) + 1;
            var arr = randomArray(len, v);
            var ans1 = Pick(arr);
            var ans2 = Test(arr);
            if (ans1 != ans2)
            {
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                Console.WriteLine("出错了!");
            }
        }

        Console.WriteLine("测试结束");
    }
}