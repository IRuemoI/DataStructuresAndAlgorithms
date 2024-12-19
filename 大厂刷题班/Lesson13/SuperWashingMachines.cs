//pass
namespace AdvancedTraining.Lesson13;

// 本题测试链接 : https://leetcode.cn/problems/super-washing-machines/
public class SuperWashingMachines
{
    private static int FindMinMoves(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        var size = arr.Length;
        var sum = 0;
        for (var i = 0; i < size; i++) sum += arr[i];
        if (sum % size != 0) return -1;
        var avg = sum / size;
        var leftSum = 0;
        var ans = 0;
        for (var i = 0; i < arr.Length; i++)
        {
            var leftRest = leftSum - i * avg;
            var rightRest = sum - leftSum - arr[i] - (size - i - 1) * avg;
            if (leftRest < 0 && rightRest < 0)
                ans = Math.Max(ans, Math.Abs(leftRest) + Math.Abs(rightRest));
            else
                ans = Math.Max(ans, Math.Max(Math.Abs(leftRest), Math.Abs(rightRest)));
            leftSum += arr[i];
        }

        return ans;
    }

    public static void Run()
    {
        Console.WriteLine(FindMinMoves([1, 0, 5])); //输出3
    }
}