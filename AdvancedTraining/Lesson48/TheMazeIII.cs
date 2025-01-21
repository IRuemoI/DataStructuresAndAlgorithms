namespace AdvancedTraining.Lesson48;

public class TheMazeIii //leetcode_0499
{
    private static readonly int[][] To =
    [
        [1, 0],
        [0, -1],
        [0, 1],
        [-1, 0],
        [0, 0]
    ];

    private static readonly string[] Re = ["d", "l", "r", "u"];

    private static string FindShortestWay(int[,] maze, int[] ball, int[] hole)
    {
        var n = maze.GetLength(0);
        var m = maze.GetLength(1);
        Node[] q1 = new Node[n * m], q2 = new Node[n * m];
        int s1 = 0, s2 = 0;
        var visited = new bool[maze.GetLength(0), maze.GetLength(1), 4];
        s1 = Spread(maze, n, m, new Node(ball[0], ball[1], 4, ""), visited, q1, s1);
        while (s1 != 0)
        {
            for (var i = 0; i < s1; i++)
            {
                var cur = q1[i];
                if (hole[0] == cur.R && hole[1] == cur.C) return cur.P;
                s2 = Spread(maze, n, m, cur, visited, q2, s2);
            }

            (q1, q2) = (q2, q1);
            s1 = s2;
            s2 = 0;
        }

        return "impossible";
    }

    // maze迷宫，走的格子
    // n 行数
    // m 列数
    // 当前来到的节点，cur -> (r,c) 方向 路径（决定）
    // v [行,列,方向] 一个格子，其实在宽度有限遍历时，是4个点！
    // q 下一层的队列
    // s 下一层队列填到了哪，size
    // 当前点cur，该分裂分裂，该继续走继续走，所产生的一下层的点，进入q，s++
    // 返回值：q增长到了哪？返回size -> s
    private static int Spread(int[,] maze, int n, int m, Node cur, bool[,,] v, Node[] q, int s)
    {
        var d = cur.D;
        var r = cur.R + To[d][0];
        var c = cur.C + To[d][1];
        // 分裂去！
        if (d == 4 || r < 0 || r == n || c < 0 || c == m || maze[r, c] != 0)
        {
            for (var i = 0; i < 4; i++)
                if (i != d)
                {
                    r = cur.R + To[i][0];
                    c = cur.C + To[i][1];
                    if (r >= 0 && r < n && c >= 0 && c < m && maze[r, c] == 0 && !v[r, c, i])
                    {
                        v[r, c, i] = true;
                        var next = new Node(r, c, i, cur.P + Re[i]);
                        q[s++] = next;
                    }
                }
        }
        else
        {
            // 不分裂！继续走！
            if (!v[r, c, d])
            {
                v[r, c, d] = true;
                q[s++] = new Node(r, c, d, cur.P);
            }
        }

        return s;
    }

    // 节点：来到了哪？(r,c)这个位置
    // 从哪个方向来的！d -> 0 1 2 3 4
    // 之前做了什么决定让你来到这个位置。
    private class Node(int row, int col, int dir, string path)
    {
        public readonly int C = col;
        public readonly int D = dir;
        public readonly string P = path;
        public readonly int R = row;
    }
}