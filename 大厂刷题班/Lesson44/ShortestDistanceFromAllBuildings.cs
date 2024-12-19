namespace AdvancedTraining.Lesson44;

//https://www.cnblogs.com/lishiblog/p/5691466.html
public class ShortestDistanceFromAllBuildings //leetcode_0317
{
    // 如果grid中0比较少，用这个方法比较好
    private static int ShortestDistance1(int[,] grid)
    {
        var ans = int.MaxValue;
        var n = grid.GetLength(0);
        var m = grid.GetLength(1);
        var buildings = 0;
        var positions = new Position[n, m];
        for (var i = 0; i < n; i++)
        for (var j = 0; j < m; j++)
        {
            if (grid[i, j] == 1) buildings++;
            positions[i, j] = new Position(i, j, grid[i, j]);
        }

        if (buildings == 0) return 0;
        for (var i = 0; i < n; i++)
        for (var j = 0; j < m; j++)
            ans = Math.Min(ans, Bfs(positions, buildings, i, j));
        return ans == int.MaxValue ? -1 : ans;
    }

    private static int Bfs(Position[,] positions, int buildings, int i, int j)
    {
        if (positions[i, j].V != 0) return int.MaxValue;
        var levels = new Dictionary<Position, int>();
        var queue = new LinkedList<Position>();
        var from = positions[i, j];
        levels[from] = 0;
        queue.AddLast(from);
        var ans = 0;
        var solved = 0;
        while (queue.Count > 0 && solved != buildings)
        {
            var cur = queue.First();
            queue.RemoveFirst();
            var level = levels[cur];
            if (cur.V == 1)
            {
                ans += level;
                solved++;
            }
            else
            {
                Add(queue, levels, positions, cur.R - 1, cur.C, level + 1);
                Add(queue, levels, positions, cur.R + 1, cur.C, level + 1);
                Add(queue, levels, positions, cur.R, cur.C - 1, level + 1);
                Add(queue, levels, positions, cur.R, cur.C + 1, level + 1);
            }
        }

        return solved == buildings ? ans : int.MaxValue;
    }

    private static void Add(LinkedList<Position> q, Dictionary<Position, int> l, Position[,] p, int i, int j, int level)
    {
        if (i >= 0 && i < p.Length && j >= 0 && j < p.GetLength(1) && p[i, j].V != 2 && !l.ContainsKey(p[i, j]))
        {
            l[p[i, j]] = level;
            q.AddLast(p[i, j]);
        }
    }

    // 如果grid中1比较少，用这个方法比较好
    private static int ShortestDistance2(int[,] grid)
    {
        var n = grid.GetLength(0);
        var m = grid.GetLength(1);
        var ones = 0;
        var zeros = 0;
        var infos = new Info[n, m];
        for (var i = 0; i < n; i++)
        for (var j = 0; j < m; j++)
            if (grid[i, j] == 1)
                infos[i, j] = new Info(i, j, 1, ones++);
            else if (grid[i, j] == 0)
                infos[i, j] = new Info(i, j, 0, zeros++);
            else
                infos[i, j] = new Info(i, j, 2, int.MaxValue);
        if (ones == 0) return 0;
        var distance = new int[ones, zeros];
        for (var i = 0; i < n; i++)
        for (var j = 0; j < m; j++)
            if (infos[i, j].V == 1)
                Bfs(infos, i, j, distance);
        var ans = int.MaxValue;
        for (var i = 0; i < zeros; i++)
        {
            var sum = 0;
            for (var j = 0; j < ones; j++)
                if (distance[j, i] == 0)
                {
                    sum = int.MaxValue;
                    break;
                }
                else
                {
                    sum += distance[j, i];
                }

            ans = Math.Min(ans, sum);
        }

        return ans == int.MaxValue ? -1 : ans;
    }

    private static void Bfs(Info[,] infos, int i, int j, int[,] distance)
    {
        var levels = new Dictionary<Info, int>();
        var queue = new LinkedList<Info>();
        var from = infos[i, j];
        Add(queue, levels, infos, from.R - 1, from.C, 1);
        Add(queue, levels, infos, from.R + 1, from.C, 1);
        Add(queue, levels, infos, from.R, from.C - 1, 1);
        Add(queue, levels, infos, from.R, from.C + 1, 1);
        while (queue.Count > 0)
        {
            var cur = queue.First();
            queue.RemoveFirst();
            var level = levels[cur];
            distance[from.T, cur.T] = level;
            Add(queue, levels, infos, cur.R - 1, cur.C, level + 1);
            Add(queue, levels, infos, cur.R + 1, cur.C, level + 1);
            Add(queue, levels, infos, cur.R, cur.C - 1, level + 1);
            Add(queue, levels, infos, cur.R, cur.C + 1, level + 1);
        }
    }

    private static void Add(LinkedList<Info> q, Dictionary<Info, int> l, Info[,] infos, int i, int j, int level)
    {
        if (i >= 0 && i < infos.Length && j >= 0 && j < infos.GetLength(1) && infos[i, j].V == 0 &&
            !l.ContainsKey(infos[i, j]))
        {
            l[infos[i, j]] = level;
            q.AddLast(infos[i, j]);
        }
    }

    // 方法三的大流程和方法二完全一样，从每一个1出发，而不从0出发
    // 运行时间快主要是因为常数优化，以下是优化点：
    // 1) 宽度优先遍历时，一次解决一层，不是一个一个遍历：
    // int size = que.size();
    // level++;
    // for (int k = 0; k < size; k++) { ... }
    // 2) pass的值每次减1何用？只有之前所有的1都到达的0，才有必要继续尝试的意思
    // 也就是说，如果某个1，自我封闭，之前的1根本到不了现在这个1附近的0，就没必要继续尝试了
    // if (nextr >= 0 && nextr < grid.length
    // && nextc >= 0 && nextc < grid[0].length
    // && grid[nextr,nextc] == pass)
    // 3) int[] trans = { 0, 1, 0, -1, 0 }; 的作用是迅速算出上、下、左、右
    // 4) 如果某个1在计算时，它周围已经没有pass值了，可以提前宣告1之间是不连通的
    // step = Bfs(grid, dist, i, j, pass--, trans);
    // if (step == Integer.MAX_VALUE) {
    // return -1;
    // }
    // 5) 最要的优化，每个1到某个0的距离是逐渐叠加的，每个1给所有的0叠一次（宽度优先遍历）
    // dist[nextr,nextc] += level;
    private static int ShortestDistance3(int[,] grid)
    {
        var dist = new int[grid.Length, grid.GetLength(1)];
        var pass = 0;
        var step = int.MaxValue;
        int[] trans = [0, 1, 0, -1, 0];
        for (var i = 0; i < grid.Length; i++)
        for (var j = 0; j < grid.GetLength(1); j++)
            if (grid[i, j] == 1)
            {
                step = Bfs(grid, dist, i, j, pass--, trans);
                if (step == int.MaxValue) return -1;
            }

        return step == int.MaxValue ? -1 : step;
    }

    // 原始矩阵是grid，但是所有的路(0)，被改了
    // 改成了啥？改成认为，pass才是路！原始矩阵中的1和2呢？不变！
    // dist，距离压缩表，之前的bfs，也就是之前每个1，走到某个0，总距离和都在dist里
    // row,col 宽度优先遍历的，出发点！
    // trans -> 炫技的，上下左右
    // 返回值代表，进行完这一遍bfs，压缩距离表中(dist)，最小值是谁？
    // 如果突然发现，无法联通！返回系统最大！
    private static int Bfs(int[,] grid, int[,] dist, int row, int col, int pass, int[] trans)
    {
        var que = new LinkedList<int[]>();
        que.AddLast(new[] { row, col });
        var level = 0;
        var ans = int.MaxValue;
        while (que.Count > 0)
        {
            var size = que.Count;
            level++;
            for (var k = 0; k < size; k++)
            {
                var node = que.First();
                que.RemoveFirst();
                for (var i = 1; i < trans.Length; i++)
                {
                    // 上下左右
                    var nextr = node[0] + trans[i - 1];
                    var nextc = node[1] + trans[i];
                    if (nextr >= 0 && nextr < grid.Length && nextc >= 0 && nextc < grid.GetLength(1) &&
                        grid[nextr, nextc] == pass)
                    {
                        que.AddLast(new[] { nextr, nextc });
                        dist[nextr, nextc] += level;
                        ans = Math.Min(ans, dist[nextr, nextc]);
                        grid[nextr, nextc]--;
                    }
                }
            }
        }

        return ans;
    }

    //todo:待修复
    public static void Run()
    {
        int[,] grid =
        {
            { 1, 0, 2, 0, 1 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 1, 0, 0 }
        };

        Console.WriteLine(ShortestDistance1(grid));
        Console.WriteLine(ShortestDistance2(grid));
        Console.WriteLine(ShortestDistance3(grid));
    }

    private class Position(int row, int col, int value)
    {
        public readonly int C = col;
        public readonly int R = row;
        public readonly int V = value;
    }

    private class Info(int row, int col, int value, int th)
    {
        public readonly int C = col;
        public readonly int R = row;
        public readonly int T = th;
        public readonly int V = value;
    }
}