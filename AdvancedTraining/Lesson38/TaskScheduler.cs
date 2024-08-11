namespace AdvancedTraining.Lesson38;

public class TaskScheduler //Problem_0621
{
    // ['A', 'B', 'A']
    private static int LeastInterval(char[] tasks, int free)
    {
        var count = new int[256];
        // 出现最多次的任务，到底是出现了几次
        var maxCount = 0;
        foreach (var task in tasks)
        {
            count[task]++;
            maxCount = Math.Max(maxCount, count[task]);
        }

        // 有多少种任务，都出现最多次
        var maxKinds = 0;
        for (var task = 0; task < 256; task++)
            if (count[task] == maxCount)
                maxKinds++;
        // maxKinds : 有多少种任务，都出现最多次
        // maxCount : 最多次，是几次？
        // 砍掉最后一组剩余的任务数
        var tasksExceptFinalTeam = tasks.Length - maxKinds;
        var spaces = (free + 1) * (maxCount - 1);
        // 到底几个空格最终会留下！
        var restSpaces = Math.Max(0, spaces - tasksExceptFinalTeam);
        return tasks.Length + restSpaces;
        // return Math.max(tasks.length, ((n + 1) * (maxCount - 1) + maxKinds));
    }

    public static void Run()
    {
        Console.WriteLine(LeastInterval(['A', 'A', 'A', 'B', 'B', 'B'], 2)); //输出8
    }
}