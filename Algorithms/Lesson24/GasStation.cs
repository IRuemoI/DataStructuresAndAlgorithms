//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson24;

// 测试链接：https://leetcode.cn/problems/gas-station
public static class GasStation
{
    // 这个方法的时间复杂度O(N)，额外空间复杂度O(N)
    public static int CanCompleteCircuit(int[] gas, int[] cost)
    {
        var good = GoodArray(gas, cost);
        for (var i = 0; i < gas.Length; i++)
            if (good[i])
                return i;

        return -1;
    }

    private static bool[] GoodArray(int[] g, int[] c)
    {
        var n = g.Length;
        var m = n << 1;
        var arr = new int[m];
        for (var i = 0; i < n; i++)
        {
            arr[i] = g[i] - c[i];
            arr[i + n] = g[i] - c[i];
        }

        for (var i = 1; i < m; i++) arr[i] += arr[i - 1];

        LinkedList<int> w = new();
        for (var i = 0; i < n; i++)
        {
            while (w.Count != 0 && arr[w.Last()] >= arr[i]) w.RemoveLast();

            w.AddLast(i);
        }

        var ans = new bool[n];
        for (int offset = 0, i = 0, j = n; j < m; offset = arr[i++], j++)
        {
            if (arr[w.First()] - offset >= 0) ans[i] = true;

            if (w.First() == i) w.RemoveFirst();

            while (w.Count != 0 && arr[w.Last()] >= arr[j]) w.RemoveLast();

            w.AddLast(j);
        }

        return ans;
    }
}

public class GasStationTest
{
    public static void Run()
    {
        int[] gas1 = [1, 2, 3, 4, 5];
        int[] cost1 = [3, 4, 5, 1, 2]; //3
        int[] gas2 = [2, 3, 4];
        int[] cost2 = [3, 4, 3]; //-1


        Utility.RestartStopwatch();
        Console.WriteLine("方法1结果:" + GasStation.CanCompleteCircuit(gas1, cost1) + ",耗时:" +
                          Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法1结果:" + GasStation.CanCompleteCircuit(gas2, cost2) + ",耗时:" +
                          Utility.GetStopwatchElapsedMilliseconds() + "ms");
    }
}