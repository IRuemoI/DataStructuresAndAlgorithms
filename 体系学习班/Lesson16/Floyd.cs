namespace Algorithms.Lesson16;

//todo:未通过
public class Floyd
{
    private const int MaxN = 101;
    private const int MaxM = 10001;
    private static readonly int[] Path = new int[MaxM];
    private static readonly int[,] Distance = new int[MaxN, MaxN];
    private static int _n, _m, _ans;

    // 初始时设置任意两点之间的最短距离为无穷大，表示任何路不存在
    private static void Build()
    {
        for (var i = 0; i < _n; i++)
        for (var j = 0; j < _n; j++)
            Distance[i, j] = int.MaxValue;
    }

    public static void Run()
    {
        using var reader = new StreamReader(new FileStream(
            @"D:\Code\CS\DataStructuresAndAlgorithms\体系学习班\Lesson\Lesson16\Floyd-input.txt", FileMode.Open));
        using var writer = new StreamWriter(new FileStream(
            @"D:\Code\CS\DataStructuresAndAlgorithms\体系学习班\Lesson\Lesson16\Floyd-output.txt", FileMode.OpenOrCreate));
        while (!reader.EndOfStream)
        {
            _n = int.Parse(reader.ReadLine()!);
            _m = int.Parse(reader.ReadLine()!);
            var paths = Array.ConvertAll(reader.ReadLine()!.Split(), int.Parse);
            for (var i = 0; i < _m; i++) Path[i] = paths[i] - 1;

            for (var i = 0; i < _n; i++)
            {
                var distances = Array.ConvertAll(reader.ReadLine()!.Split(), int.Parse);
                for (var j = 0; j < _n; j++) Distance[i, j] = distances[j];
            }

            _ans = 0;
            for (var i = 1; i < _m; i++) _ans += Distance[Path[i - 1], Path[i]];

            writer.WriteLine(_ans);
        }

        writer.Flush();
    }

    private static void Code()
    {
        // O(N^3)的过程
        // 枚举每个跳板
        // 注意，跳板要最先枚举！跳板要最先枚举！跳板要最先枚举！
        for (var bridge = 0; bridge < _n; bridge++) // 跳板
        for (var i = 0; i < _n; i++)
        for (var j = 0; j < _n; j++)
            // i -> .....bridge .... -> j
            // distance[i][j]能不能缩短
            // distance[i][j] = min ( distance[i][j] , distance[i][bridge] + distance[bridge][j])
            if (Distance[i, bridge] != int.MaxValue
                && Distance[bridge, j] != int.MaxValue
                && Distance[i, j] > Distance[i, bridge] + Distance[bridge, j])
                Distance[i, j] = Distance[i, bridge] + Distance[bridge, j];
    }
}