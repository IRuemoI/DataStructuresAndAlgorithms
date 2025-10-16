namespace AdvancedTraining.Lesson46;

public class PerfectRectangle //leetcode_0391
{
    private static bool IsRectangleCover(int[][] matrix)
    {
        if (matrix.Length == 0 || matrix[0].Length == 0) return false;
        var l = int.MaxValue;
        var r = int.MinValue;
        var d = int.MaxValue;
        var u = int.MinValue;
        var map = new Dictionary<int, Dictionary<int, int>>();
        var area = 0;
        foreach (var rect in matrix)
        {
            Add(map, rect[0], rect[1]);
            Add(map, rect[0], rect[3]);
            Add(map, rect[2], rect[1]);
            Add(map, rect[2], rect[3]);
            area += (rect[2] - rect[0]) * (rect[3] - rect[1]);
            l = Math.Min(rect[0], l);
            d = Math.Min(rect[1], d);
            r = Math.Max(rect[2], r);
            u = Math.Max(rect[3], u);
        }

        return CheckPoints(map, l, d, r, u) && area == (r - l) * (u - d);
    }

    private static void Add(Dictionary<int, Dictionary<int, int>> map, int row, int col)
    {
        if (!map.ContainsKey(row)) map[row] = new Dictionary<int, int>();
        map[row][col] = map[row].GetValueOrDefault(col, 0) + 1;
    }

    private static bool CheckPoints(Dictionary<int, Dictionary<int, int>> map, int l, int d, int r, int u)
    {
        if (map[l].GetValueOrDefault(d, 0) != 1 || map[l].GetValueOrDefault(u, 0) != 1 ||
            map[r].GetValueOrDefault(d, 0) != 1 || map[r].GetValueOrDefault(u, 0) != 1) return false;
        map[l].Remove(d);
        map[l].Remove(u);
        map[r].Remove(d);
        map[r].Remove(u);
        foreach (var key in map.Keys)
        foreach (var value in map[key].Values)
            if ((value & 1) != 0)
                return false;
        return true;
    }

    public static void Run()
    {
        int[][] m = [[1, 1, 3, 3], [3, 1, 4, 2], [3, 2, 4, 4], [1, 3, 2, 4], [2, 3, 3, 4]];
        Console.WriteLine(IsRectangleCover(m)); //输出True
    }
}