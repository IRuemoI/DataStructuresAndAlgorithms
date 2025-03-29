//pass

namespace AdvancedTraining.Lesson09;

public class IsStepSum
{
    private static bool Code(int stepSum)
    {
        var l = 0;
        var r = stepSum;
        while (l <= r)
        {
            var m = l + ((r - l) >> 1);
            var cur = StepSum(m);
            if (cur == stepSum)
                return true;
            if (cur < stepSum)
                l = m + 1;
            else
                r = m - 1;
        }

        return false;
    }

    private static int StepSum(int num)
    {
        var sum = 0;
        while (num != 0)
        {
            sum += num;
            num /= 10;
        }

        return sum;
    }

    //用于测试
    private static Dictionary<int, int> GenerateStepSumNumberMap(int numMax)
    {
        var map = new Dictionary<int, int>();
        for (var i = 0; i <= numMax; i++) map[StepSum(i)] = i;
        return map;
    }

    //用于测试
    public static void Run()
    {
        const int max = 1000000;
        var maxStepSum = StepSum(max);
        var ans = GenerateStepSumNumberMap(max);
        Console.WriteLine("测试开始");
        for (var i = 0; i <= maxStepSum; i++)
            if (Code(i) ^ ans.ContainsKey(i))
                Console.WriteLine("出错了！");
        Console.WriteLine("测试结束");
    }
}