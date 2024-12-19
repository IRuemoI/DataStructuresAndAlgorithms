#region

using Common.DataStructures.Heap;

#endregion

namespace AdvancedTraining.Lesson51;

public class CourseScheduleIii //leetcode_0630
{
    private static int ScheduleCourse(int[][] courses)
    {
        // courses[i]  = {花费，截止}
        Array.Sort(courses, (a, b) => a[1] - b[1]);
        // 花费时间的大根堆
        var heap = new Heap<int>((a, b) => b - a);
        // 时间点
        var time = 0;
        foreach (var c in courses)
            // 
            if (time + c[0] <= c[1])
            {
                // 当前时间 + 花费 <= 截止时间的
                heap.Push(c[0]);
                time += c[0];
            }
            else
            {
                // 当前时间 + 花费 > 截止时间的, 只有淘汰掉某课，当前的课才能进来！
                // 
                if (!heap.isEmpty && heap.Peek() > c[0])
                {
                    //					time -= heap.poll();
                    //					heap.add(c[0]);
                    //					time += c[0];
                    heap.Push(c[0]);
                    time += c[0] - heap.Pop();
                }
            }

        return heap.count;
    }

    public static void Run()
    {
        int[][] courses = [[100, 200], [200, 1300], [1000, 1250], [2000, 3200]];
        Console.WriteLine(ScheduleCourse(courses)); //3
    }
}