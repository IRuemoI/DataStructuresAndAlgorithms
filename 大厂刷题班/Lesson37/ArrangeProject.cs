namespace AdvancedTraining.Lesson37;

// 来自网易
// 刚入职网易互娱，新人mini项目便如火如荼的开展起来。为了更好的项目协作与管理，
// 小易决定将学到的甘特图知识用于mini项目时间预估。小易先把项目中每一项工作以任务的形式列举出来，
// 每项任务有一个预计花费时间与前置任务表，必须完成了该任务的前置任务才能着手去做该任务。
// 作为经验PM，小易把任务划分得井井有条，保证没有前置任务或者前置任务全数完成的任务，都可以同时进行。
// 小易给出了这样一个任务表，请作为程序的你计算需要至少多长时间才能完成所有任务。
// 输入第一行为一个正整数T，表示数据组数。
// 对于接下来每组数据，第一行为一个正整数N，表示一共有N项任务。
// 接下来N行，每行先有两个整数Di和Ki，表示完成第i个任务的预计花费时间为Di天，该任务有Ki个前置任务。
// 之后为Ki个整数Mj，表示第Mj个任务是第i个任务的前置任务。
// 数据范围：对于所有数据，满足1<=T<=3, 1<=N, Mj<=100000, 0<=Di<=1000, 0<=sum(Ki)<=N*2。
// https://www.cnblogs.com/moonfdd/p/17395282.html
public class ArrangeProject
{
    private static int DayCount(int[][] numbers, int[] days, int[] headCount)
    {
        var head = CountHead(headCount);
        var maxDay = 0;
        var countDay = new int[days.Length];
        while (head.Count > 0)
        {
            var cur = head.First();
            head.RemoveFirst();
            countDay[cur] += days[cur];
            for (var j = 0; j < numbers[cur].Length; j++)
            {
                headCount[numbers[cur][j]]--;
                if (headCount[numbers[cur][j]] == 0) head.AddLast(numbers[cur][j]);
                countDay[numbers[cur][j]] = Math.Max(countDay[numbers[cur][j]], countDay[cur]);
            }
        }

        foreach (var item in countDay)
            maxDay = Math.Max(maxDay, item);

        return maxDay;
    }

    private static LinkedList<int> CountHead(int[] headCount)
    {
        var queue = new LinkedList<int>();
        for (var i = 0; i < headCount.Length; i++)
            if (headCount[i] == 0)
                queue.AddLast(i); // 没有前驱任务
        return queue;
    }

    public static void Run()
    {
        int[][] numbers = [[1, 2, 3], [4, 5, 6]];
        int[] days = [2];
        int[] headCount = [3];
        Console.WriteLine(DayCount(numbers, days, headCount));
    }
}