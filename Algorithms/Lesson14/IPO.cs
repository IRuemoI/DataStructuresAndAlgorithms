//测试通过

#region

using Common.DataStructures.Heap;

#endregion

namespace Algorithms.Lesson14;

public class Ipo
{
    // 最多K个项目
    // W是初始资金
    // Profits[] Capital[] 一定等长
    // 返回最终最大的资金
    private static int FindMaximizedCapital(int k, int w, int[] profits, int[] capital)
    {
        // var minCostQ = new MinHeap<Project>((x, y) => x.C - y.C);
        // var maxProfitQ = new MinHeap<Project>((x, y) => y.C - x.C);
        var minCostQ = new Heap<Project>((x, y) => x.C - y.C);
        var maxProfitQ = new Heap<Project>((x, y) => x.C - y.C);


        for (var i = 0; i < profits.Length; i++) minCostQ.Push(new Project(profits[i], capital[i]));

        for (var i = 0; i < k; i++)
        {
            while (minCostQ.Count != 0 && minCostQ.Peek().C <= w) maxProfitQ.Push(minCostQ.Pop());

            if (maxProfitQ.Count == 0) return w;

            w += maxProfitQ.Pop().P;
        }

        return w;
    }

    public static void Run()
    {
        const int k1 = 2, w1 = 0;
        int[] profits1 = [1, 2, 3], capital1 = [0, 1, 1];
        Console.WriteLine(FindMaximizedCapital(k1, w1, profits1, capital1)); //输出4
        const int k2 = 3, w2 = 0;
        int[] profits2 = [1, 2, 3], capital2 = [0, 1, 2];
        Console.WriteLine(FindMaximizedCapital(k2, w2, profits2, capital2)); //输出6
    }

    private class Project(int p, int c)
    {
        public readonly int C = c;
        public readonly int P = p;
    }
}