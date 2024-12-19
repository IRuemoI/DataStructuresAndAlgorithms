using Common.DataStructures.Heap;
using Common.Utilities;

namespace Algorithms.Lesson16;

// https://www.bilibili.com/video/BV1t94y187zW
// A*算法模版
public class AStar
{
    // 0:上，1:右，2:下，3:左
    private static readonly int[] MoveArray = [-1, 0, 1, 0, -1];

    private static int _heapCapacity = -1;

    // Dijkstra算法
    // grid[i][j] == 0 代表障碍
    // grid[i][j] == 1 代表道路
    // 只能走上、下、左、右，不包括斜线方向
    // 返回从(startX, startY)到(targetX, targetY)的最短距离
    private static int MinDistance1(int[][]? grid, int startX, int startY, int targetX, int targetY)
    {
        // 新增参数合法性校验 
        if (grid == null || grid.Length == 0 || grid[0].Length == 0)
            return -1;
        var n = grid.Length;
        var m = grid[0].Length;
        if (startX < 0 || startX >= n || startY < 0 || startY >= m)
            return -1;
        if (targetX < 0 || targetX >= n || targetY < 0 || targetY >= m)
            return -1;
        if (grid[startX][startY] == 0 || grid[targetX][targetY] == 0)
            return -1;

        var distance = new int [n][];
        for (var i = 0; i < n; i++)
        {
            distance[i] = new int[m];
            for (var j = 0; j < m; j++) distance[i][j] = int.MaxValue;
        }

        distance[startX][startY] = 1;
        var visited = new bool [n][];
        for (var i = 0; i < n; i++) visited[i] = new bool [m];

        Heap<int[]> heap = new((a, b) => a[2] - b[2], _heapCapacity);
        // 0 : 行
        // 1 : 列
        // 2 : 从源点出发到达当前点的距离
        heap.Push([startX, startY, 1]);
        while (!heap.isEmpty)
        {
            var cur = heap.Pop();
            var x = cur[0];
            var y = cur[1];
            if (visited[x][y]) continue;

            visited[x][y] = true;
            if (x == targetX && y == targetY) return distance[x][y];

            for (var i = 0; i < 4; i++)
            {
                var nx = x + MoveArray[i];
                var ny = y + MoveArray[i + 1];
                if (nx >= 0 && nx < n && ny >= 0 && ny < m && grid[nx][ny] == 1 && !visited[nx][ny] &&
                    distance[x][y] + 1 < distance[nx][ny])
                {
                    distance[nx][ny] = distance[x][y] + 1;
                    heap.Push([nx, ny, distance[x][y] + 1]);
                }
            }
        }

        return -1;
    }

    // A*算法
    // grid[i][j] == 0 代表障碍
    // grid[i][j] == 1 代表道路
    // 只能走上、下、左、右，不包括斜线方向
    // 返回从(startX, startY)到(targetX, targetY)的最短距离
    private static int MinDistance2(int[][]? grid, int startX, int startY, int targetX, int targetY)
    {
        // 新增参数合法性校验 
        if (grid == null || grid.Length == 0 || grid[0].Length == 0)
            return -1;
        var n = grid.Length;
        var m = grid[0].Length;
        if (startX < 0 || startX >= n || startY < 0 || startY >= m)
            return -1;
        if (targetX < 0 || targetX >= n || targetY < 0 || targetY >= m)
            return -1;
        if (grid[startX][startY] == 0 || grid[targetX][targetY] == 0)
            return -1;
        var distance = new int[n][];
        for (var i = 0; i < n; i++)
        {
            distance[i] = new int[m];
            for (var j = 0; j < m; j++) distance[i][j] = int.MaxValue;
        }

        distance[startX][startY] = 1;
        var visited = new bool[n][];
        for (var i = 0; i < n; i++) visited[i] = new bool[m];

        // 0 : 行
        // 1 : 列
        // 2 : 从源点出发到达当前点的距离 + 当前点到终点的预估距离
        Heap<int[]> heap = new((a, b) => a[2] - b[2], _heapCapacity);
        heap.Push([startX, startY, 1 + ManhattanDistance(startX, startY, targetX, targetY)]);
        while (!heap.isEmpty)
        {
            var cur = heap.Pop();
            var x = cur[0];
            var y = cur[1];
            if (visited[x][y]) continue;

            visited[x][y] = true;
            if (x == targetX && y == targetY) return distance[x][y];

            for (var i = 0; i < 4; i++)
            {
                var nx = x + MoveArray[i];
                var ny = y + MoveArray[i + 1];
                if (nx >= 0 && nx < n && ny >= 0 && ny < m && grid[nx][ny] == 1 && !visited[nx][ny] &&
                    distance[x][y] + 1 < distance[nx][ny])
                {
                    distance[nx][ny] = distance[x][y] + 1;
                    heap.Push([nx, ny, distance[x][y] + 1 + ManhattanDistance(nx, ny, targetX, targetY)]);
                }
            }
        }

        return -1;
    }

    // 曼哈顿距离
    private static int ManhattanDistance(int x, int y, int targetX, int targetY)
    {
        return Math.Abs(targetX - x) + Math.Abs(targetY - y);
    }

    // 对角线距离
    // private static int DiagonalDistance(int x, int y, int targetX, int targetY)
    // {
    //     return Math.Max(Math.Abs(targetX - x), Math.Abs(targetY - y));
    // }

    // 欧式距离
    // private static double EuclideanMetric(int x, int y, int targetX, int targetY)
    // {
    //     return Math.Sqrt(Math.Pow(targetX - x, 2) + Math.Pow(targetY - y, 2));
    // }

    // 为了测试
    private static int[][] RandomGrid(int n)
    {
        var grid = new int[n][];
        for (var i = 0; i < n; i++)
        {
            grid[i] = new int[n];
            for (var j = 0; j < n; j++)
                // 确保起点和终点始终可通行 
                if ((i == 0 && j == 0) || (i == n - 1 && j == n - 1))
                    grid[i][j] = 1;
                else
                    grid[i][j] = Utility.getRandomDouble < 0.3 ? 0 : 1;
        }

        return grid;
    }

    // 为了测试
    public static void Run()
    {
        var len = 100;
        var testTime = 10000;
        Console.WriteLine("功能测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var n = (int)(Utility.getRandomDouble * len) + 2;
            _heapCapacity = n * n;
            var grid1 = RandomGrid(n);
            var startX1 = (int)(Utility.getRandomDouble * (n - 1)); // 确保索引在 [0, n-1]
            var startY1 = (int)(Utility.getRandomDouble * (n - 1));
            var targetX1 = (int)(Utility.getRandomDouble * (n - 1));
            var targetY1 = (int)(Utility.getRandomDouble * (n - 1));
            var ans11 = MinDistance1(grid1, startX1, startY1, targetX1, targetY1);
            var ans12 = MinDistance2(grid1, startX1, startY1, targetX1, targetY1);
            if (ans11 != ans12) Console.WriteLine("出错了!");
        }

        Console.WriteLine("功能测试结束");

        Console.WriteLine("性能测试开始");
        _heapCapacity = 1000 * 1000;
        var grid = RandomGrid(1000);
        // 修正目标坐标为合法值 
        var startX2 = 0;
        var startY2 = 0;
        var targetX2 = 999; // 1000x1000网格的合法最大索引 
        var targetY2 = 999;

        Utility.RestartStopwatch();
        var ans1 = MinDistance1(grid, startX2, startY2, targetX2, targetY2);
        Console.WriteLine("运行Dijkstra算法结果: " + ans1 + ", 运行时间(毫秒) : " + Utility.GetStopwatchElapsedMilliseconds());
        Utility.RestartStopwatch();
        var ans2 = MinDistance2(grid, startX2, startY2, targetX2, targetY2);
        Console.WriteLine("运行A*算法结果: " + ans2 + ", 运行时间(毫秒) : " + Utility.GetStopwatchElapsedMilliseconds());
        Console.WriteLine("性能测试结束");
    }
}