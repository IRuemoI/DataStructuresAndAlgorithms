#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson35;

// 来自小红书
// 一场电影开始和结束时间可以用一个小数组来表示["07:30","12:00"]
// 已知有2000场电影开始和结束都在同一天，这一天从00:00开始到23:59结束
// 一定要选3场完全不冲突的电影来观看，返回最大的观影时间
// 如果无法选出3场完全不冲突的电影来观看，返回-1
public class WatchMovieMaxTime
{
    // 暴力方法，枚举前三场所有的可能全排列
    private static int MaxEnjoy1(int[][] movies)
    {
        if (movies.Length < 3) return -1;

        return Process1(movies, 0);
    }

    private static int Process1(int[][] movies, int index)
    {
        if (index == 3)
        {
            var start = 0;
            var watch = 0;
            for (var i = 0; i < 3; i++)
            {
                if (start > movies[i][0]) return -1;

                watch += movies[i][1] - movies[i][0];
                start = movies[i][1];
            }

            return watch;
        }

        var ans = -1;
        for (var i = index; i < movies.Length; i++)
        {
            Swap(movies, index, i);
            ans = Math.Max(ans, Process1(movies, index + 1));
            Swap(movies, index, i);
        }

        return ans;
    }

    private static void Swap(int[][] movies, int i, int j)
    {
        //交换矩阵数组的i行和j行
        for (var k = 0; k < movies.GetLength(1); k++) (movies[i][k], movies[j][k]) = (movies[j][k], movies[i][k]);
    }

    // 优化后的递归解
    private static int MaxEnjoy2(int[][] movies)
    {
        Array.Sort(movies, (a, b) => a[0] != b[0] ? a[0] - b[0] : a[1] - b[1]);
        return Process2(movies, 0, 0, 3);
    }

    private static int Process2(int[][] movies, int index, int time, int rest)
    {
        if (index == movies.Length) return rest == 0 ? 0 : -1;

        var p1 = Process2(movies, index + 1, time, rest);
        var next = movies[index][0] >= time && rest > 0 ? Process2(movies, index + 1, movies[index][1], rest - 1) : -1;
        var p2 = next != -1 ? movies[index][1] - movies[index][0] + next : -1;
        return Math.Max(p1, p2);
    }

    // 记忆化搜索的动态规划

    // 为了测试
    private static int[][] RandomMovies(int len, int time)
    {
        var movies = new int[len][];
        for (var i = 0; i < len; i++)
        {
            var a = (int)(Utility.GetRandomDouble * time);
            var b = (int)(Utility.GetRandomDouble * time);
            movies[i] =
            [
                Math.Min(a, b),
                Math.Max(a, b)
            ];
        }

        return movies;
    }

    public static void Run()
    {
        const int n = 10;
        const int t = 20;
        const int testTime = 10000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var len = (int)(Utility.GetRandomDouble * n) + 1;
            var movies = RandomMovies(len, t);
            var ans1 = MaxEnjoy1(movies);
            var ans2 = MaxEnjoy2(movies);
            if (ans1 != ans2)
            {
                for (var j = 0; j < movies.GetLength(0); j++) Console.WriteLine(movies[j][0] + " , " + movies[j][1]);

                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                Console.WriteLine("出错了");
            }
        }

        Console.WriteLine("测试结束");
    }
}