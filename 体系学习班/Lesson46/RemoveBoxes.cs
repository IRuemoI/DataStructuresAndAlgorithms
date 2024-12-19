//测试通过

namespace Algorithms.Lesson46;

// 本题测试链接 : https://leetcode.cn/problems/remove-boxes/
public class RemoveBoxes
{
    // arr[L...R]消除，而且前面跟着K个arr[L]这个数
    // 返回：所有东西都消掉，最大得分
    private static int Func1(int[] arr, int l, int r, int k)
    {
        if (l > r) return 0;
        var ans = Func1(arr, l + 1, r, 0) + (k + 1) * (k + 1);

        // 前面的K个X，和arr[L]数，合在一起了，现在有K+1个arr[L]位置的数
        for (var i = l + 1; i <= r; i++)
            if (arr[i] == arr[l])
                ans = Math.Max(ans, Func1(arr, l + 1, i - 1, 0) + Func1(arr, i, r, k + 1));
        return ans;
    }

    private static int RemoveBoxes1(int[] boxes)
    {
        var n = boxes.Length;
        var dp = new int[n, n, n];
        var ans = Process1(boxes, 0, n - 1, 0, dp);
        return ans;
    }

    private static int Process1(int[] boxes, int l, int r, int k, int[,,] dp)
    {
        if (l > r) return 0;
        if (dp[l, r, k] > 0) return dp[l, r, k];
        var ans = Process1(boxes, l + 1, r, 0, dp) + (k + 1) * (k + 1);
        for (var i = l + 1; i <= r; i++)
            if (boxes[i] == boxes[l])
                ans = Math.Max(ans, Process1(boxes, l + 1, i - 1, 0, dp) + Process1(boxes, i, r, k + 1, dp));
        dp[l, r, k] = ans;
        return ans;
    }

    private static int RemoveBoxes2(int[] boxes)
    {
        var n = boxes.Length;
        var dp = new int [n, n, n];
        var ans = Process2(boxes, 0, n - 1, 0, dp);
        return ans;
    }

    private static int Process2(int[] boxes, int l, int r, int k, int[,,] dp)
    {
        if (l > r) return 0;
        if (dp[l, r, k] > 0) return dp[l, r, k];
        // 找到开头，
        // 1,1,1,1,1,5
        // 3 4 5 6 7 8
        //         !
        var last = l;
        while (last + 1 <= r && boxes[last + 1] == boxes[l]) last++;
        // K个1     (K + last - L) last
        var pre = k + last - l;
        var ans = (pre + 1) * (pre + 1) + Process2(boxes, last + 1, r, 0, dp);
        for (var i = last + 2; i <= r; i++)
            if (boxes[i] == boxes[l] && boxes[i - 1] != boxes[l])
                ans = Math.Max(ans, Process2(boxes, last + 1, i - 1, 0, dp) + Process2(boxes, i, r, pre + 1, dp));
        dp[l, r, k] = ans;
        return ans;
    }

    public static void Run()
    {
        var boxes1 = new[] { 1, 3, 2, 2, 2, 3, 4, 3, 1 };
        Console.WriteLine(RemoveBoxes1(boxes1));
        Console.WriteLine(RemoveBoxes2(boxes1));
        Console.WriteLine("--------------");
        var boxes2 = new[] { 1, 1, 1 };
        Console.WriteLine(RemoveBoxes1(boxes2));
        Console.WriteLine(RemoveBoxes2(boxes2));
        Console.WriteLine("--------------");
        var boxes3 = new[] { 1 };
        Console.WriteLine(RemoveBoxes1(boxes3));
        Console.WriteLine(RemoveBoxes2(boxes3));
    }
}