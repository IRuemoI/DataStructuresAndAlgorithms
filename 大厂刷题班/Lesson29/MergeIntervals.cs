//pass
namespace AdvancedTraining.Lesson29;

public class MergeIntervals //leetcode_0056
{
    private static int[][] Merge(int[][] intervals)
    {
        if (intervals.Length == 0) return [];

        Array.Sort(intervals, (a, b) => a[0].CompareTo(b[0]));

        var s = intervals[0][0];
        var e = intervals[0][1];
        var mergedIntervals = new List<int[]>();

        for (var i = 1; i < intervals.Length; i++)
            if (intervals[i][0] > e)
            {
                mergedIntervals.Add([s, e]);
                s = intervals[i][0];
                e = intervals[i][1];
            }
            else
            {
                e = Math.Max(e, intervals[i][1]);
            }

        mergedIntervals.Add([s, e]);

        return mergedIntervals.ToArray();
    }

    public static void Run()
    {
        foreach (var row in Merge([[1, 3], [2, 6], [8, 10], [15, 18]]))
        {
            foreach (var item in row) Console.Write(item + ",");

            Console.WriteLine();
        }
    }
}