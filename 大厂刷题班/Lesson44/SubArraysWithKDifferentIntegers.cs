namespace AdvancedTraining.Lesson44;

//https://leetcode.cn/problems/subarrays-with-k-different-integers/description/
public class SubArraysWithKDifferentIntegers //leetcode_0992
{
    // nums 数组，题目规定，nums中的数字，不会超过nums的长度
    // [ ]长度为5，0~5
    private static int SubArraysWithKDistinct1(int[] numbers, int k)
    {
        var n = numbers.Length;
        // k-1种数的窗口词频统计
        var lessCounts = new int[n + 1];
        // k种数的窗口词频统计
        var equalCounts = new int[n + 1];
        var lessLeft = 0;
        var equalLeft = 0;
        var lessKinds = 0;
        var equalKinds = 0;
        var ans = 0;
        for (var r = 0; r < n; r++)
        {
            // 当前刚来到r位置！
            if (lessCounts[numbers[r]] == 0) lessKinds++;
            if (equalCounts[numbers[r]] == 0) equalKinds++;
            lessCounts[numbers[r]]++;
            equalCounts[numbers[r]]++;
            while (lessKinds == k)
            {
                if (lessCounts[numbers[lessLeft]] == 1) lessKinds--;
                lessCounts[numbers[lessLeft++]]--;
            }

            while (equalKinds > k)
            {
                if (equalCounts[numbers[equalLeft]] == 1) equalKinds--;
                equalCounts[numbers[equalLeft++]]--;
            }

            ans += lessLeft - equalLeft;
        }

        return ans;
    }

    private static int SubArraysWithKDistinct2(int[] arr, int k)
    {
        return NumbersMostK(arr, k) - NumbersMostK(arr, k - 1);
    }

    private static int NumbersMostK(int[] arr, int k)
    {
        int i = 0, res = 0;
        var count = new Dictionary<int, int?>();
        for (var j = 0; j < arr.Length; ++j)
        {
            if ((count[arr[j]] != null ? count[arr[j]] : 0) == 0) k--;
            count[arr[j]] = (count[arr[j]] != null ? count[arr[j]] : 0) + 1;
            while (k < 0)
            {
                count[arr[i]] = count[arr[i]] - 1;
                if (count[arr[i]] == 0) k++;
                i++;
            }

            res += j - i + 1;
        }

        return res;
    }

    //todo:待修复
    public static void Run()
    {
        Console.WriteLine(SubArraysWithKDistinct1([1, 2, 1, 2, 3], 2)); //输出7
        Console.WriteLine(SubArraysWithKDistinct2([1, 2, 1, 2, 3], 2)); //输出7
    }
}