#region

using Common.DataStructures.Heap;
using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson40;

public class MaxMeetingScore
{
    private static int MaxScore1(int[][] meetings)
    {
        Array.Sort(meetings, (a, b) => a[0] - b[0]);
        var path = new int[meetings.Length][];
        var size = 0;
        return Process1(meetings, 0, path, size);
    }

    private static int Process1(int[][] meetings, int index, int[][] path, int size)
    {
        if (index == meetings.Length)
        {
            var time = 0;
            var ans = 0;
            for (var i = 0; i < size; i++)
                if (time + 10 <= path[i][0])
                {
                    ans += path[i][1];
                    time += 10;
                }
                else
                {
                    return 0;
                }

            return ans;
        }

        var p1 = Process1(meetings, index + 1, path, size);
        path[size] = meetings[index];
        var p2 = Process1(meetings, index + 1, path, size + 1);
        // path[size] = null;
        return Math.Max(p1, p2);
    }

    private static int MaxScore2(int[][] meetings)
    {
        Array.Sort(meetings, (a, b) => a[0] - b[0]);
        var heap = new Heap<int>((x, y) => x - y);
        var time = 0;
        // 已经把所有会议，按照截止时间，从小到大，排序了！
        // 截止时间一样的，谁排前谁排后，无所谓
        foreach (var item in meetings)
            if (time + 10 <= item[0])
            {
                heap.Push(item[1]);
                time += 10;
            }
            else
            {
                if (heap.IsEmpty || heap.Peek() >= item[1]) continue;
                heap.Pop();
                heap.Push(item[1]);
            }

        var ans = 0;
        while (!heap.IsEmpty) ans += heap.Pop();

        return ans;
    }

    private static int[][] RandomMeetings(int n, int t, int s)
    {
        var ans = new int[n][];
        for (var i = 0; i < n; i++)
            ans[i] = new int[2]
            {
                (int)(Utility.GetRandomDouble * t) + 1,
                (int)(Utility.GetRandomDouble * s) + 1
            };

        return ans;
    }

    private static int[][] CopyMeetings(int[][] meetings)
    {
        var n = meetings.Length;
        var ans = new int[n][];
        for (var i = 0; i < n; i++)
            ans[i] = new int[2]
            {
                meetings[i][0],
                meetings[i][1]
            };

        return ans;
    }

    public static void Run()
    {
        const int n = 12;
        const int t = 100;
        const int s = 500;
        const int testTime = 10000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var size = (int)(Utility.GetRandomDouble * n) + 1;
            var meetings1 = RandomMeetings(size, t, s);
            var meetings2 = CopyMeetings(meetings1);
            var ans1 = MaxScore1(meetings1);
            var ans2 = MaxScore2(meetings2);
            if (ans1 != ans2)
            {
                Console.WriteLine("出错了!");
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
            }
        }

        Console.WriteLine("测试结束");
    }
}