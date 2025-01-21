namespace AdvancedTraining.Lesson18;

// 本题测试链接 : https://leetcode.cn/problems/shortest-bridge/
public class ShortestBridge
{
    private static int Code(int[][] m)
    {
        var row = m.GetLength(0);
        var column = m.GetLength(1);
        var all = row * column;
        var island = 0;
        var curs = new int[all];
        var nextList = new int[all];
        int[][] records = [new int[all], new int[all]];
        for (var i = 0; i < row; i++)
        for (var j = 0; j < column; j++)
            if (m[i][j] == 1)
            {
                // 当前位置发现了1！
                // 把这一片的1，都变成2，同时，抓上来了，这一片1组成的初始队列
                // curs, 把这一片的1到自己的距离，都设置成1了，records
                var queueSize = Infect(m, i, j, row, column, curs, 0, records[island]);
                var v = 1;
                while (queueSize != 0)
                {
                    v++;
                    // curs里面的点，上下左右，records[点]==0， nexts
                    queueSize = Bfs(row, column, all, v, curs, queueSize, nextList, records[island]);
                    (curs, nextList) = (nextList, curs);
                }

                island++;
            }

        var min = int.MaxValue;
        for (var i = 0; i < all; i++) min = Math.Min(min, records[0][i] + records[1][i]);
        return min - 3;
    }

    // 当前来到m[i][j] , 总行数是N，总列数是M
    // m[i][j]感染出去(找到这一片岛所有的1),把每一个1的坐标，放入到int[] curs队列！
    // 1 (a,b) -> curs[index++] = (a * M + b)
    // 1 (c,d) -> curs[index++] = (c * M + d)
    // 二维已经变成一维了， 1 (a,b) -> a * M + b
    // 设置距离record[a * M +b ] = 1
    private static int Infect(int[][] m, int i, int j, int n, int m1, int[] curs, int index, int[] record)
    {
        if (i < 0 || i == n || j < 0 || j == m1 || m[i][j] != 1) return index;
        // m[i][j] 不越界，且m[i][j] == 1
        m[i][j] = 2;
        var p = i * m1 + j;
        record[p] = 1;
        // 收集到不同的1
        curs[index++] = p;
        index = Infect(m, i - 1, j, n, m1, curs, index, record);
        index = Infect(m, i + 1, j, n, m1, curs, index, record);
        index = Infect(m, i, j - 1, n, m1, curs, index, record);
        index = Infect(m, i, j + 1, n, m1, curs, index, record);
        return index;
    }

    // 二维原始矩阵中，N总行数，M总列数
    // all 总 all = N * M
    // V 要生成的是第几层 curs V-1 nexts V
    // record里面拿距离
    private static int Bfs(int n, int m, int all, int v, int[] curs, int size, int[] nexts, int[] record)
    {
        var nextI = 0; // 我要生成的下一层队列成长到哪了？
        for (var i = 0; i < size; i++)
        {
            // curs[i] -> 一个位置
            var up = curs[i] < m ? -1 : curs[i] - m;
            var down = curs[i] + m >= all ? -1 : curs[i] + m;
            var left = curs[i] % m == 0 ? -1 : curs[i] - 1;
            var right = curs[i] % m == m - 1 ? -1 : curs[i] + 1;
            if (up != -1 && record[up] == 0)
            {
                record[up] = v;
                nexts[nextI++] = up;
            }

            if (down != -1 && record[down] == 0)
            {
                record[down] = v;
                nexts[nextI++] = down;
            }

            if (left != -1 && record[left] == 0)
            {
                record[left] = v;
                nexts[nextI++] = left;
            }

            if (right != -1 && record[right] == 0)
            {
                record[right] = v;
                nexts[nextI++] = right;
            }
        }

        return nextI;
    }

    public static void Run()
    {
        Console.WriteLine(Code([
            [1, 1, 1, 1, 1],
            [1, 0, 0, 0, 1],
            [1, 0, 1, 0, 1],
            [1, 0, 0, 0, 1],
            [1, 1, 1, 1, 1]
        ])); //输出1
    }
}