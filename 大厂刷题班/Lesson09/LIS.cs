//pass
namespace AdvancedTraining.Lesson09;

// 本题测试链接 : https://leetcode.cn/problems/longest-increasing-subsequence
public class Lis
{
    private static int LengthOfLis(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        var ends = new int[arr.Length];
        ends[0] = arr[0];
        var right = 0;
        var max = 1;
        for (var i = 1; i < arr.Length; i++)
        {
            var l = 0;
            var r = right;
            while (l <= r)
            {
                var m = (l + r) / 2;
                if (arr[i] > ends[m])
                    l = m + 1;
                else
                    r = m - 1;
            }

            right = Math.Max(right, l);
            ends[l] = arr[i];
            max = Math.Max(max, l + 1);
        }

        return max;
    }

    public static void Run()
    {
        Console.WriteLine(LengthOfLis([10, 9, 2, 5, 3, 7, 101, 18])); //输出4
    }
}