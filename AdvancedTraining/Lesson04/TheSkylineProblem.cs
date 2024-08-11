namespace AdvancedTraining.Lesson04;

// 本题测试链接 : https://leetcode.cn/problems/the-skyline-problem/
public class TheSkylineProblem
{
    private static List<List<int>> GetSkyline(int[][] matrix)
    {
        var nodes = new Node[matrix.Length * 2];
        for (var i = 0; i < matrix.Length; i++)
        {
            nodes[i * 2] = new Node(matrix[i][0], true, matrix[i][2]);
            nodes[i * 2 + 1] = new Node(matrix[i][1], false, matrix[i][2]);
        }

        Array.Sort(nodes, (a, b) => a.X - b.X);
        // key  高度  value 次数
        var mapHeightTimes = new SortedDictionary<int, int>();
        var mapXHeight = new SortedDictionary<int, int>();
        foreach (var item in nodes)
        {
            if (item.IsAdd)
            {
                if (!mapHeightTimes.TryAdd(item.H, 1))
                    mapHeightTimes[item.H] = mapHeightTimes[item.H] + 1;
            }
            else
            {
                if (mapHeightTimes[item.H] == 1)
                    mapHeightTimes.Remove(item.H);
                else
                    mapHeightTimes[item.H] -= 1;
            }

            if (mapHeightTimes.Count == 0)
                mapXHeight[item.X] = 0;
            else
                mapXHeight[item.X] = mapHeightTimes.Last().Key;
        }

        var ans = new List<List<int>>();
        foreach (var item in mapXHeight)
        {
            var curX = item.Key;
            var curMaxHeight = item.Value;
            if (ans.Count == 0 || ans[^1][1] != curMaxHeight)
            {
                var tempList = new List<int>
                {
                    curX,
                    curMaxHeight
                };
                ans.Add(tempList);
            }
        }

        return ans;
    }

    public static void Run()
    {
        var result = GetSkyline([[2, 9, 10], [3, 7, 15], [5, 12, 12], [15, 20, 10], [19, 24, 8]]);

        foreach (var resultRow in result)
        {
            foreach (var item in resultRow) Console.Write(item + ",");

            Console.WriteLine();
        }
        //输出[[2,10],[3,15],[7,12],[12,0],[15,10],[20,8],[24,0]]
    }

    private class Node(int x, bool isAdd, int h)
    {
        public readonly int H = h;
        public readonly bool IsAdd = isAdd;
        public readonly int X = x;
    }
}