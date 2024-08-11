namespace AdvancedTraining.Lesson36;

// 来自腾讯
// 给定一个正数数组arr，代表每个人的体重。给定一个正数limit代表船的载重，所有船都是同样的载重量
// 每个人的体重都一定不大于船的载重
// 要求：
// 1, 可以1个人单独一搜船
// 2, 一艘船如果坐2人，两个人的体重相加需要是偶数，且总体重不能超过船的载重
// 3, 一艘船最多坐2人
// 返回如果想所有人同时坐船，船的最小数量
//https://www.cnblogs.com/moonfdd/p/17395289.html
public class MinBoatEvenNumbers
{
    private static int MinBoat(int[] arr, int limit)
    {
        Array.Sort(arr);
        var odd = 0;
        var even = 0;
        foreach (var num in arr)
            if ((num & 1) == 0)
                even++;
            else
                odd++;
        var odds = new int[odd];
        var evens = new int[even];
        for (var i = arr.Length - 1; i >= 0; i--)
            if ((arr[i] & 1) == 0)
                evens[--even] = arr[i];
            else
                odds[--odd] = arr[i];
        return Min(odds, limit) + Min(evens, limit);
    }

    private static int Min(int[]? arr, int limit)
    {
        if (arr == null || arr.Length == 0) return 0;
        var n = arr.Length;
        if (arr[n - 1] > limit) return -1;
        var lessR = -1;
        for (var i = n - 1; i >= 0; i--)
            if (arr[i] <= limit / 2)
            {
                lessR = i;
                break;
            }

        if (lessR == -1) return n;
        var l = lessR;
        var r = lessR + 1;
        var noUsed = 0;
        while (l >= 0)
        {
            var solved = 0;
            while (r < n && arr[l] + arr[r] <= limit)
            {
                r++;
                solved++;
            }

            if (solved == 0)
            {
                noUsed++;
                l--;
            }
            else
            {
                l = Math.Max(-1, l - solved);
            }
        }

        var all = lessR + 1;
        var used = all - noUsed;
        var moreUnsolved = n - all - used;
        return used + ((noUsed + 1) >> 1) + moreUnsolved;
    }

    public static void Run()
    {
        Console.WriteLine(MinBoat([1, 2, 3, 4, 5], 5)); //输出4
    }
}