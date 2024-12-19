//pass
namespace AdvancedTraining.Lesson07;

// 测试链接 : https://leetcode.cn/problems/maximum-gap/
public class MaxGap
{
    private static int MaximumGap(int[]? numbers)
    {
        if (numbers == null || numbers.Length < 2) return 0;
        var len = numbers.Length;
        var min = int.MaxValue;
        var max = int.MinValue;
        for (var i = 0; i < len; i++)
        {
            min = Math.Min(min, numbers[i]);
            max = Math.Max(max, numbers[i]);
        }

        if (min == max) return 0;
        var hasNum = new bool[len + 1];
        var maxArray = new int[len + 1];
        var minArray = new int[len + 1];
        for (var j = 0; j < len; j++)
        {
            var bid = Bucket(numbers[j], len, min, max);
            minArray[bid] = hasNum[bid] ? Math.Min(minArray[bid], numbers[j]) : numbers[j];
            maxArray[bid] = hasNum[bid] ? Math.Max(maxArray[bid], numbers[j]) : numbers[j];
            hasNum[bid] = true;
        }

        var res = 0;
        var lastMax = maxArray[0];
        var i1 = 1;
        for (; i1 <= len; i1++)
            if (hasNum[i1])
            {
                res = Math.Max(res, minArray[i1] - lastMax);
                lastMax = maxArray[i1];
            }

        return res;
    }

    private static int Bucket(long num, long len, long min, long max)
    {
        return (int)((num - min) * len / (max - min));
    }


    public static void Run()
    {
        Console.WriteLine(MaximumGap([3, 6, 9, 1])); //输出3
    }
}