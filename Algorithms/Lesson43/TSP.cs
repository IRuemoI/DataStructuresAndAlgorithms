#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson43;

public class Tsp
{
    private static int T1(int[,] matrix)
    {
        var n = matrix.Length; // 0...N-1
        // set
        // set.get(i) != null i这座城市在集合里
        // set.get(i) == null i这座城市不在集合里
        IList<int?> set = new List<int?>();
        for (var i = 0; i < n; i++) set.Add(1);

        return Func1(matrix, set, 0);
    }

    // 任何两座城市之间的距离，可以在matrix里面拿到
    // set中表示着哪些城市的集合，
    // start这座城一定在set里，
    // 从start出发，要把set中所有的城市过一遍，最终回到0这座城市，最小距离是多少
    private static int Func1(int[,] matrix, IList<int?> set, int start)
    {
        var cityNum = 0;
        foreach (var element in set)
            if (element != null)
                cityNum++;

        if (cityNum == 1) return matrix[start, 0];

        // cityNum > 1  不只start这一座城
        set[start] = null;
        var min = int.MaxValue;
        for (var i = 0; i < set.Count; i++)
            if (set[i] != null)
            {
                // start -> i i... -> 0
                var cur = matrix[start, i] + Func1(matrix, set, i);
                min = Math.Min(min, cur);
            }

        set[start] = 1;
        return min;
    }

    private static int T2(int[,] matrix)
    {
        var n = matrix.Length; // 0...N-1
        // 7座城 1111111
        var allCity = (1 << n) - 1;
        return F2(matrix, allCity, 0);
    }

    // 任何两座城市之间的距离，可以在matrix里面拿到
    // set中表示着哪些城市的集合，
    // start这座城一定在set里，
    // 从start出发，要把set中所有的城市过一遍，最终回到0这座城市，最小距离是多少
    private static int F2(int[,] matrix, int cityStatus, int start)
    {
        // cityStatus == cityStatux & (~cityStaus + 1)

        if (cityStatus == (cityStatus & (~cityStatus + 1))) return matrix[start, 0];

        // 把start位的1去掉，
        cityStatus &= ~(1 << start);
        var min = int.MaxValue;
        // 枚举所有的城市
        for (var move = 0; move < matrix.Length; move++)
            if ((cityStatus & (1 << move)) != 0)
            {
                var cur = matrix[start, move] + F2(matrix, cityStatus, move);
                min = Math.Min(min, cur);
            }

        cityStatus |= 1 << start;
        return min;
    }

    private static int T3(int[,] matrix)
    {
        var n = matrix.Length; // 0...N-1
        // 7座城 1111111
        var allCity = (1 << n) - 1;
        var dp = new int[1 << n, n];
        for (var i = 0; i < 1 << n; i++)
        for (var j = 0; j < n; j++)
            dp[i, j] = -1;

        return F3(matrix, allCity, 0, dp);
    }

    // 任何两座城市之间的距离，可以在matrix里面拿到
    // set中表示着哪些城市的集合，
    // start这座城一定在set里，
    // 从start出发，要把set中所有的城市过一遍，最终回到0这座城市，最小距离是多少
    private static int F3(int[,] matrix, int cityStatus, int start, int[,] dp)
    {
        if (dp[cityStatus, start] != -1) return dp[cityStatus, start];

        if (cityStatus == (cityStatus & (~cityStatus + 1)))
        {
            dp[cityStatus, start] = matrix[start, 0];
        }
        else
        {
            // 把start位的1去掉，
            cityStatus &= ~(1 << start);
            var min = int.MaxValue;
            // 枚举所有的城市
            for (var move = 0; move < matrix.Length; move++)
                if (move != start && (cityStatus & (1 << move)) != 0)
                {
                    var cur = matrix[start, move] + F3(matrix, cityStatus, move, dp);
                    min = Math.Min(min, cur);
                }

            cityStatus |= 1 << start;
            dp[cityStatus, start] = min;
        }

        return dp[cityStatus, start];
    }

    private static int T4(int[,] matrix)
    {
        var n = matrix.Length; // 0...N-1
        var statusNums = 1 << n;
        var dp = new int[statusNums, n];

        for (var status = 0; status < statusNums; status++)
        for (var start = 0; start < n; start++)
            if ((status & (1 << start)) != 0)
            {
                if (status == (status & (~status + 1)))
                {
                    dp[status, start] = matrix[start, 0];
                }
                else
                {
                    var min = int.MaxValue;
                    // start 城市在status里去掉之后，的状态
                    var preStatus = status & ~(1 << start);
                    // start -> i
                    for (var i = 0; i < n; i++)
                        if ((preStatus & (1 << i)) != 0)
                        {
                            var cur = matrix[start, i] + dp[preStatus, i];
                            min = Math.Min(min, cur);
                        }

                    dp[status, start] = min;
                }
            }

        return dp[statusNums - 1, 0];
    }

    // matrix[i,j] -> i城市到j城市的距离
    private static int Tsp1(int[,]? matrix, int origin)
    {
        if (matrix == null || matrix.Length < 2 || origin < 0 || origin >= matrix.Length) return 0;

        // 要考虑的集合
        var cities = new List<int?>();
        // cities[0] != null 表示0城在集合里
        // cities[i] != null 表示i城在集合里
        for (var i = 0; i < matrix.Length; i++) cities.Add(1);

        // null,1,1,1,1,1,1
        // origin城不参与集合
        cities[origin] = null;
        return Process(matrix, origin, cities, origin);
    }

    // matrix 所有距离，存在其中
    // origin 固定参数，唯一的目标
    // cities 要考虑的集合，一定不含有origin
    // 当前来到的城市是谁，cur
    private static int Process(int[,] matrix, int aim, List<int?> cities, int cur)
    {
        var hasCity = false; // 集团中还是否有城市
        var ans = int.MaxValue;
        for (var i = 0; i < cities.Count; i++)
            if (cities[i] != null)
            {
                hasCity = true;
                cities[i] = null;
                // matrix[cur,i] + f(i, 集团(去掉i) )
                ans = Math.Min(ans, matrix[cur, i] + Process(matrix, aim, cities, i));
                cities[i] = 1;
            }

        return hasCity ? ans : matrix[cur, aim];
    }

    // cities 里，一定含有cur这座城
    // 解决的是，集合从cur出发，通过集合里所有的城市，最终来到aim，最短距离
    private static int Process2(int[,] matrix, int aim, List<int?> cities, int cur)
    {
        if (cities.Count == 1) return matrix[cur, aim];

        cities[cur] = null;
        var ans = int.MaxValue;
        for (var i = 0; i < cities.Count; i++)
            if (cities[i] != null)
            {
                var dis = matrix[cur, i] + Process2(matrix, aim, cities, i);
                ans = Math.Min(ans, dis);
            }

        cities[cur] = 1;
        return ans;
    }

    private static int Tsp2(int[,]? matrix, int origin)
    {
        if (matrix == null || matrix.Length < 2 || origin < 0 || origin >= matrix.Length) return 0;

        var n = matrix.Length - 1; // 除去origin之后是n-1个点
        var s = 1 << n; // 状态数量
        var dp = new int[s, n];
        int iCity;
        for (var i = 0; i < n; i++)
        {
            iCity = i < origin ? i : i + 1;
            // 00000000 i
            dp[0, i] = matrix[iCity, origin];
        }

        for (var status = 1; status < s; status++)
            // 尝试每一种状态 status = 0 0 1 0 0 0 0 0 0
            // 下标 8 7 6 5 4 3 2 1 0
        for (var i = 0; i < n; i++)
        {
            // i 枚举的出发城市
            dp[status, i] = int.MaxValue;
            if (((1 << i) & status) != 0)
            {
                // 如果i这座城是可以枚举的，i = 6 ， i对应的原始城的编号，icity
                iCity = i < origin ? i : i + 1;
                for (var k = 0; k < n; k++)
                    // i 这一步连到的点，k
                    if (((1 << k) & status) != 0)
                    {
                        // i 这一步可以连到k
                        var kCity = k < origin ? k : k + 1;
                        dp[status, i] = Math.Min(dp[status, i], dp[status ^ (1 << i), k] + matrix[iCity, kCity]);
                    }
            }
        }

        var ans = int.MaxValue;
        for (var i = 0; i < n; i++)
        {
            iCity = i < origin ? i : i + 1;
            ans = Math.Min(ans, dp[s - 1, i] + matrix[origin, iCity]);
        }

        return ans;
    }

    private static int[,] GenerateGraph(int maxSize, int maxValue)
    {
        var len = (int)(Utility.getRandomDouble * maxSize) + 1;
        var matrix = new int[len, len];
        for (var i = 0; i < len; i++)
        for (var j = 0; j < len; j++)
            matrix[i, j] = (int)(Utility.getRandomDouble * maxValue) + 1;

        for (var i = 0; i < len; i++) matrix[i, i] = 0;

        return matrix;
    }

    public static void Run()
    {
        var len = 10;
        var value = 100;
        Console.WriteLine("功能测试开始");
        for (var i = 0; i < 20000; i++)
        {
            var tempMatrix = GenerateGraph(len, value);
            var origin = (int)(Utility.getRandomDouble * tempMatrix.Length);
            var ans1 = T3(tempMatrix);
            var ans2 = T4(tempMatrix);
            var ans3 = Tsp2(tempMatrix, origin);
            if (ans1 != ans2 || ans1 != ans3) Console.WriteLine("出错了！");
        }

        Console.WriteLine("功能测试结束");

        len = 22;
        Console.WriteLine("性能测试开始，数据规模 : " + len);
        var matrix = new int[len, len];
        for (var i = 0; i < len; i++)
        for (var j = 0; j < len; j++)
            matrix[i, j] = (int)(Utility.getRandomDouble * value) + 1;

        for (var i = 0; i < len; i++) matrix[i, i] = 0;


        Utility.RestartStopwatch();
        T4(matrix);

        Console.WriteLine("运行时间 : " + Utility.GetStopwatchElapsedMilliseconds() + " 毫秒");
        Console.WriteLine("性能测试结束");
    }
}