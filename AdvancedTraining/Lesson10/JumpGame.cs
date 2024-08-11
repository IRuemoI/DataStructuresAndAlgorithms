namespace AdvancedTraining.Lesson10;

// 本题测试链接 : https://leetcode.cn/problems/Jump-game-ii/
public class JumpGame
{
    private static int Jump(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        var step = 0;
        var cur = 0;
        var next = 0;
        for (var i = 0; i < arr.Length; i++)
        {
            if (cur < i)
            {
                step++;
                cur = next;
            }

            next = Math.Max(next, i + arr[i]);
        }

        return step;
    }

    public static void Run()
    {
        Console.WriteLine(Jump([2, 3, 1, 1, 4])); //输出2
    }
}