//pass
namespace AdvancedTraining.Lesson25;

// 本题测试链接: https://leetcode.cn/problems/max-points-on-a-line/
public class MaxPointsOnALine
{
    // [
    //    [1,3]
    //    [4,9]
    //    [5,7]
    //   ]

    private static int MaxPoints(int[][]? points)
    {
        if (points == null) return 0;
        if (points.Length <= 2) return points.Length;
        // key = 3
        // value = {7 , 10}  -> 斜率为3/7的点 有10个
        //         {5,  15}  -> 斜率为3/5的点 有15个
        IDictionary<int, IDictionary<int, int>> map = new Dictionary<int, IDictionary<int, int>>();
        var result = 0;
        for (var i = 0; i < points.Length; i++)
        {
            map.Clear();
            var samePosition = 1;
            var sameX = 0;
            var sameY = 0;
            var line = 0;
            for (var j = i + 1; j < points.Length; j++)
            {
                // i号点，和j号点，的斜率关系
                var x = points[j][0] - points[i][0];
                var y = points[j][1] - points[i][1];
                if (x == 0 && y == 0)
                {
                    samePosition++;
                }
                else if (x == 0)
                {
                    sameX++;
                }
                else if (y == 0)
                {
                    sameY++;
                }
                else
                {
                    // 普通斜率
                    var gcd = Gcd(x, y);
                    x /= gcd;
                    y /= gcd;
                    // x / y
                    if (!map.ContainsKey(x)) map[x] = new Dictionary<int, int>();
                    if (!map[x].ContainsKey(y)) map[x][y] = 0;
                    map[x][y] = map[x][y] + 1;
                    line = Math.Max(line, map[x][y]);
                }
            }

            result = Math.Max(result, Math.Max(Math.Max(sameX, sameY), line) + samePosition);
        }

        return result;
    }

    // 保证初始调用的时候，a和b不等于0
    // O(1)
    private static int Gcd(int a, int b)
    {
        return b == 0 ? a : Gcd(b, a % b);
    }

    public static void Run()
    {
        Console.WriteLine(MaxPoints([[1, 1], [3, 2], [5, 3], [4, 1], [2, 3], [1, 4]])); //输出4
    }
}