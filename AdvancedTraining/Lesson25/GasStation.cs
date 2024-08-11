namespace AdvancedTraining.Lesson25;

// 本题测试链接 : https://leetcode.cn/problems/gas-station/
// 注意本题的实现比leetcode上的问法更加通用
// leetcode只让返回其中一个良好出发点的位置
// 本题是返回结果数组，每一个出发点是否是良好出发点都求出来了
// 得到结果数组的过程，时间复杂度O(N)，额外空间复杂度O(1)
public class GasStation
{
    private static int CanCompleteCircuit(int[]? gas, int[] cost)
    {
        if (gas == null || gas.Length == 0) return -1;
        if (gas.Length == 1) return gas[0] < cost[0] ? -1 : 0;
        var good = Stations(cost, gas);
        for (var i = 0; i < gas.Length; i++)
            if (good != null && good[i])
                return i;
        return -1;
    }

    private static bool[]? Stations(int[]? cost, int[]? gas)
    {
        if (cost == null || gas == null || cost.Length < 2 || cost.Length != gas.Length) return null;
        var init = ChangeDisArrayGetInit(cost, gas);
        return init == -1 ? new bool[cost.Length] : EnlargeArea(cost, init);
    }

    private static int ChangeDisArrayGetInit(int[] dis, int[] oil)
    {
        var init = -1;
        for (var i = 0; i < dis.Length; i++)
        {
            dis[i] = oil[i] - dis[i];
            if (dis[i] >= 0) init = i;
        }

        return init;
    }

    private static bool[] EnlargeArea(int[] dis, int init)
    {
        var res = new bool[dis.Length];
        var start = init;
        var end = NextIndex(init, dis.Length);
        var need = 0;
        var rest = 0;
        do
        {
            // 当前来到的start已经在连通区域中，可以确定后续的开始点一定无法转完一圈
            if (start != init && start == LastIndex(end, dis.Length)) break;
            // 当前来到的start不在连通区域中，就扩充连通区域
            // start(5) ->  联通区的头部(7) -> 2
            // start(-2) -> 联通区的头部(7) -> 9
            if (dis[start] < need)
            {
                // 当前start无法接到连通区的头部
                need -= dis[start];
            }
            else
            {
                // 当前start可以接到连通区的头部，开始扩充连通区域的尾巴
                // start(7) -> 联通区的头部(5)
                rest += dis[start] - need;
                need = 0;
                while (rest >= 0 && end != start)
                {
                    rest += dis[end];
                    end = NextIndex(end, dis.Length);
                }

                // 如果连通区域已经覆盖整个环，当前的start是良好出发点，进入2阶段
                if (rest >= 0)
                {
                    res[start] = true;
                    ConnectGood(dis, LastIndex(start, dis.Length), init, res);
                    break;
                }
            }

            start = LastIndex(start, dis.Length);
        } while (start != init);

        return res;
    }

    // 已知start的next方向上有一个良好出发点
    // start如果可以达到这个良好出发点，那么从start出发一定可以转一圈
    private static void ConnectGood(int[] dis, int start, int init, bool[] res)
    {
        var need = 0;
        while (start != init)
        {
            if (dis[start] < need)
            {
                need -= dis[start];
            }
            else
            {
                res[start] = true;
                need = 0;
            }

            start = LastIndex(start, dis.Length);
        }
    }

    private static int LastIndex(int index, int size)
    {
        return index == 0 ? size - 1 : index - 1;
    }

    private static int NextIndex(int index, int size)
    {
        return index == size - 1 ? 0 : index + 1;
    }

    public static void Run()
    {
        Console.WriteLine(CanCompleteCircuit([1, 2, 3, 4, 5], [3, 4, 5, 1, 2])); //输出3
    }
}