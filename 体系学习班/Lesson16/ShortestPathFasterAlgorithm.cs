namespace Algorithms.Lesson16;

// Bellman-Ford + SPFA优化模版（洛谷）
// 给定一个 n个点的有向图，请求出图中是否存在从顶点 1 出发能到达的负环
// 负环的定义是：一条边权之和为负数的回路。
// 测试链接 : https://www.luogu.com.cn/problem/P3385
// 请同学们务必参考如下代码中关于输入、输出的处理
// 这是输入输出处理效率很高的写法
// 提交以下所有代码，把主类名改成Main，可以直接通过
public class ShortestPathFasterAlgorithm
{
    private const int MaxN = 2001;

    private const int MaxM = 6001;

    // SPFA需要
    private const int MaxQ = 4000001;

    // 链式前向星建图需要
    private static readonly int[] Head = new int[MaxN];
    private static readonly int[] Next = new int[MaxM];
    private static readonly int[] To = new int[MaxM];
    private static readonly int[] Weight = new int[MaxM];

    private static int _cnt;

    // 源点出发到每个节点的距离表
    private static readonly int[] Distance = new int[MaxN];

    // 节点被松弛的次数
    private static readonly int[] UpdateCnt = new int[MaxN];

    // 哪些节点被松弛了放入队列
    private static readonly int[] Queue = new int[MaxQ];

    private static int _l, _r;

    // 节点是否已经在队列中
    private static readonly bool[] Enter = new bool[MaxN];

    // todo:这里的填充可能有问题
    private static void Build(int n)
    {
        _cnt = 1;
        _l = _r = 0;
        Array.Fill(Head, 0, 0, n);
        Array.Fill(Enter, false, 0, n);
        Array.Fill(Distance, int.MaxValue, 0, n);
        Array.Fill(UpdateCnt, 0, 0, n);
    }

    private static void AddEdge(int u, int v, int w)
    {
        Next[_cnt] = Head[u];
        To[_cnt] = v;
        Weight[_cnt] = w;
        Head[u] = _cnt++;
    }

    public static void Run()
    {
        var br = new StreamReader(new FileStream(
            @"D:\Code\CS\DataStructuresAndAlgorithms\体系学习班\Lesson16\SPFA-input.txt", FileMode.Open));
        var @out = new StreamWriter(new FileStream(
            @"D:\Code\CS\DataStructuresAndAlgorithms\体系学习班\Lesson16\SPFA-output.txt", FileMode.OpenOrCreate));
        var cases = int.Parse(br.ReadLine() ?? string.Empty);
        for (var i = 0; i < cases; i++)
        {
            var token = br.ReadLine()!.Split();
            var n = int.Parse(token[0]);
            var m = int.Parse(token[1]);
            Build(n);
            for (var j = 0; j < m; j++)
            {
                token = br.ReadLine()!.Split();
                var u = int.Parse(token[0]);
                var v = int.Parse(token[1]);
                var w = int.Parse(token[2]);
                if (w >= 0)
                {
                    AddEdge(u, v, w);
                    AddEdge(v, u, w);
                }
                else
                {
                    AddEdge(u, v, w);
                }
            }

            @out.WriteLine(Spfa(n) ? "YES" : "NO");
        }

        @out.Flush();
        @out.Close();
        br.Close();
        Console.WriteLine("运行结束");
    }

    // Bellman-Ford + SPFA优化的模版
    private static bool Spfa(int n)
    {
        Distance[1] = 0;
        UpdateCnt[1]++;
        Queue[_r++] = 1;
        Enter[1] = true;
        while (_l < _r)
        {
            var u = Queue[_l++];
            Enter[u] = false;
            for (var ei = Head[u]; ei > 0; ei = Next[ei])
            {
                var v = To[ei];
                var w = Weight[ei];
                if (Distance[u] + w < Distance[v])
                {
                    Distance[v] = Distance[u] + w;
                    if (!Enter[v])
                    {
                        if (UpdateCnt[v]++ == n) return true;

                        Queue[_r++] = v;
                        Enter[v] = true;
                    }
                }
            }
        }

        return false;
    }
}