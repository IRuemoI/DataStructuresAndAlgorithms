﻿namespace AdvancedTraining.Lesson35;

// 来自网易
// 给定一个正数数组arr，表示每个小朋友的得分
// 任何两个相邻的小朋友，如果得分一样，怎么分糖果无所谓，但如果得分不一样，分数大的一定要比分数少的多拿一些糖果
// 假设所有的小朋友坐成一个环形，返回在不破坏上一条规则的情况下，需要的最少糖果数
public class CircleCandy
{
    private static int MinCandy(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        if (arr.Length == 1) return 1;
        var n = arr.Length;
        var minIndex = 0;
        for (var i = 0; i < n; i++)
            if (arr[i] <= arr[LastIndex(i, n)] && arr[i] <= arr[NextIndex(i, n)])
            {
                minIndex = i;
                break;
            }

        var nums = new int[n + 1];
        for (var i = 0; i <= n; i++, minIndex = NextIndex(minIndex, n)) nums[i] = arr[minIndex];
        var left = new int[n + 1];
        left[0] = 1;
        for (var i = 1; i <= n; i++) left[i] = nums[i] > nums[i - 1] ? left[i - 1] + 1 : 1;
        var right = new int[n + 1];
        right[n] = 1;
        for (var i = n - 1; i >= 0; i--) right[i] = nums[i] > nums[i + 1] ? right[i + 1] + 1 : 1;
        var ans = 0;
        for (var i = 0; i < n; i++) ans += Math.Max(left[i], right[i]);
        return ans;
    }

    private static int NextIndex(int i, int n)
    {
        return i == n - 1 ? 0 : i + 1;
    }

    private static int LastIndex(int i, int n)
    {
        return i == 0 ? n - 1 : i - 1;
    }

    public static void Run()
    {
        int[] arr = [3, 4, 2, 3, 2];
        Console.WriteLine(MinCandy(arr));
    }
}