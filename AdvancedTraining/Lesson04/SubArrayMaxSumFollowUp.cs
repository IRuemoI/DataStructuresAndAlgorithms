﻿namespace AdvancedTraining.Lesson04;

public class SubArrayMaxSumFollowUp
{
    private static int SubSqeMaxSumNoNext(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        if (arr.Length == 1) return arr[0];
        var dp = new int[arr.Length];
        // dp[i] : arr[0..i]挑选，满足不相邻设定的情况下，随意挑选，最大的累加和
        dp[0] = arr[0];
        dp[1] = Math.Max(arr[0], arr[1]);
        for (var i = 2; i < arr.Length; i++)
        {
            var p1 = dp[i - 1];
            var p2 = arr[i] + Math.Max(dp[i - 2], 0);
            dp[i] = Math.Max(p1, p2);
        }

        return dp[arr.Length - 1];
    }

    // 给定一个数组arr，在不能取相邻数的情况下，返回所有组合中的最大累加和
    // 思路：
    // 定义dp[i] : 表示arr[0...i]范围上，在不能取相邻数的情况下，返回所有组合中的最大累加和
    // 在arr[0...i]范围上，在不能取相邻数的情况下，得到的最大累加和，可能性分类：
    // 可能性 1) 选出的组合，不包含arr[i]。那么dp[i] = dp[i-1]
    // 比如，arr[0...i] = {3,4,-4}，最大累加和是不包含i位置数的时候
    //
    // 可能性 2) 选出的组合，只包含arr[i]。那么dp[i] = arr[i]
    // 比如，arr[0...i] = {-3,-4,4}，最大累加和是只包含i位置数的时候
    //
    // 可能性 3) 选出的组合，包含arr[i], 且包含arr[0...i-2]范围上的累加和。那么dp[i] = arr[i] + dp[i-2]
    // 比如，arr[0...i] = {3,1,4}，最大累加和是3和4组成的7，因为相邻不能选，所以i-1位置的数要跳过
    //
    // 综上所述：dp[i] = Max { dp[i-1], arr[i] , arr[i] + dp[i-2] }
    private static int MaxSum(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        var n = arr.Length;
        if (n == 1) return arr[0];
        if (n == 2) return Math.Max(arr[0], arr[1]);
        var dp = new int[n];
        dp[0] = arr[0];
        dp[1] = Math.Max(arr[0], arr[1]);
        for (var i = 2; i < n; i++) dp[i] = Math.Max(Math.Max(dp[i - 1], arr[i]), arr[i] + dp[i - 2]);
        return dp[n - 1];
    }
}