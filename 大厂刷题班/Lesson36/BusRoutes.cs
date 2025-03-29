namespace AdvancedTraining.Lesson36;

//pass
// 来自三七互娱
// Leetcode原题 : https://leetcode.cn/problems/bus-routes/description/
public class BusRoutes
{
    // 0 : [1,3,7,0]
    // 1 : [7,9,6,2]
    // ....
    // 返回：返回换乘几次+1 -> 返回一共坐了多少条线的公交。
    private static int NumBusesToDestination(int[][] routes, int source, int target)
    {
        if (source == target) return 0;
        var n = routes.Length;
        // key : 车站
        // value : list -> 该车站拥有哪些线路！
        var map = new Dictionary<int, List<int>>();
        for (var i = 0; i < n; i++)
        for (var j = 0; j < routes[i].Length; j++)
        {
            if (!map.ContainsKey(routes[i][j])) map[routes[i][j]] = new List<int>();
            map[routes[i][j]].Add(i);
        }

        var queue = new List<int>();
        var set = new bool[n];
        foreach (var route in map[source])
        {
            queue.Add(route);
            set[route] = true;
        }

        var len = 1;
        while (queue.Count > 0)
        {
            var nextLevel = new List<int>();
            foreach (var route in queue)
            {
                var bus = routes[route];
                foreach (var station in bus)
                {
                    if (station == target) return len;
                    foreach (var nextRoute in map[station])
                        if (!set[nextRoute])
                        {
                            nextLevel.Add(nextRoute);
                            set[nextRoute] = true;
                        }
                }
            }

            queue = nextLevel;
            len++;
        }

        return -1;
    }

    public static void Run()
    {
        Console.WriteLine(NumBusesToDestination([[1, 2, 7], [3, 7], [6, 7]], 1, 6)); //输出2
    }
}