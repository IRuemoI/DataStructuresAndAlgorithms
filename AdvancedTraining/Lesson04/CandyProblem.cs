namespace AdvancedTraining.Lesson04;

// 测试链接 : https://leetcode.cn/problems/candy/
public class CandyProblem
{
    // 这是原问题的优良解
    // 时间复杂度O(N)，额外空间复杂度O(N)
    private static int Candy1(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        var n = arr.Length;
        var left = new int[n];
        for (var i = 1; i < n; i++)
            if (arr[i - 1] < arr[i])
                left[i] = left[i - 1] + 1;

        var right = new int[n];
        for (var i = n - 2; i >= 0; i--)
            if (arr[i] > arr[i + 1])
                right[i] = right[i + 1] + 1;

        var ans = 0;
        for (var i = 0; i < n; i++) ans += Math.Max(left[i], right[i]);

        return ans + n;
    }

    // 这是原问题空间优化后的解
    // 时间复杂度O(N)，额外空间复杂度O(1)
    private static int Candy2(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        var index = NextMinIndex2(arr, 0);
        var res = RightCandies(0, index++);
        var lBase = 1;
        while (index != arr.Length)
            if (arr[index] > arr[index - 1])
            {
                res += ++lBase;
                index++;
            }
            else if (arr[index] < arr[index - 1])
            {
                var next = NextMinIndex2(arr, index - 1);
                var rCands = RightCandies(index - 1, next++);
                var rBase = next - index + 1;
                res += rCands + (rBase > lBase ? -lBase : -rBase);
                lBase = 1;
                index = next;
            }
            else
            {
                res += 1;
                lBase = 1;
                index++;
            }

        return res;
    }

    private static int NextMinIndex2(int[] arr, int start)
    {
        for (var i = start; i != arr.Length - 1; i++)
            if (arr[i] <= arr[i + 1])
                return i;

        return arr.Length - 1;
    }

    private static int RightCandies(int left, int right)
    {
        var n = right - left + 1;
        return n + n * (n - 1) / 2;
    }

    // 这是进阶问题的最优解，不要提交这个
    // 时间复杂度O(N), 额外空间复杂度O(1)
    private static int Candy3(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        var index = NextMinIndex3(arr, 0);
        var data = rightCandiesAndBase(arr, 0, index++);
        var res = data[0];
        var lBase = 1;
        var same = 1;
        while (index != arr.Length)
            if (arr[index] > arr[index - 1])
            {
                res += ++lBase;
                same = 1;
                index++;
            }
            else if (arr[index] < arr[index - 1])
            {
                var next = NextMinIndex3(arr, index - 1);
                data = rightCandiesAndBase(arr, index - 1, next++);
                if (data[1] <= lBase)
                    res += data[0] - data[1];
                else
                    res += -lBase * same + data[0] - data[1] + data[1] * same;
                index = next;
                lBase = 1;
                same = 1;
            }
            else
            {
                res += lBase;
                same++;
                index++;
            }

        return res;
    }

    private static int NextMinIndex3(int[] arr, int start)
    {
        for (var i = start; i != arr.Length - 1; i++)
            if (arr[i] < arr[i + 1])
                return i;

        return arr.Length - 1;
    }

    private static int[] rightCandiesAndBase(int[] arr, int left, int right)
    {
        var @base = 1;
        var candies = 1;
        for (var i = right - 1; i >= left; i--)
            if (arr[i] == arr[i + 1])
                candies += @base;
            else
                candies += ++@base;

        return [candies, @base];
    }

    public static void Run()
    {
        int[] test1 = [1, 2, 2];
        Console.WriteLine(Candy1(test1)); //输出4
        Console.WriteLine(Candy2(test1)); //输出4
        Console.WriteLine(Candy3(test1)); //输出5
    }
}