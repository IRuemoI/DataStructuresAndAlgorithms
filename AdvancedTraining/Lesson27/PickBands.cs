#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson27;

public class PickBands
{
    private static int _min = int.MaxValue;

    // 每一个项目都有三个数，[a,b,c]表示这个项目a和b乐队参演，花费为c
    // 每一个乐队可能在多个项目里都出现了，但是只能挑一次
    // nums是可以挑选的项目数量，所以一定会有nums*2只乐队被挑选出来
    // 乐队的全部数量一定是nums*2，且标号一定是0 ~ nums*2-1
    // 返回一共挑nums轮(也就意味着一定请到所有的乐队)，最少花费是多少？
    private static int MinCost(int[][]? programs, int numbers)
    {
        if (numbers == 0 || programs == null || programs.Length == 0) return 0;
        var size = Clean(programs);
        var map1 = init(1 << (numbers << 1));
        int[] map2;
        if ((numbers & 1) == 0)
        {
            // nums = 8 , 4
            F(programs, size, 0, 0, 0, numbers >> 1, map1);
            map2 = map1;
        }
        else
        {
            // nums == 7 4 -> map1 3 -> map2
            F(programs, size, 0, 0, 0, numbers >> 1, map1);
            map2 = init(1 << (numbers << 1));
            F(programs, size, 0, 0, 0, numbers - (numbers >> 1), map2);
        }

        // 16 mask : 0..00 1111.1111(16个)
        // 12 mask : 0..00 1111.1111(12个)
        var mask = (1 << (numbers << 1)) - 1;
        var ans = int.MaxValue;
        for (var i = 0; i < map1.Length; i++)
            if (map1[i] != int.MaxValue && map2[mask & ~i] != int.MaxValue)
                ans = Math.Min(ans, map1[i] + map2[mask & ~i]);
        return ans == int.MaxValue ? -1 : ans;
    }

    // [
    // [9, 1, 100]
    // [2, 9 , 50]
    // ...
    // ]
    private static int Clean(int[][] programs)
    {
        int x;
        int y;
        foreach (var p in programs)
        {
            x = Math.Min(p[0], p[1]);
            y = Math.Max(p[0], p[1]);
            p[0] = x;
            p[1] = y;
        }

        Array.Sort(programs, (a, b) => a[0] != b[0] ? a[0] - b[0] : a[1] != b[1] ? a[1] - b[1] : a[2] - b[2]);
        x = programs[0][0];
        y = programs[0][1];
        var n = programs.Length;
        // (0, 1, ? )
        for (var i = 1; i < n; i++)
            if (programs[i][0] == x && programs[i][1] == y)
            {
                programs[i] = null;
            }
            else
            {
                x = programs[i][0];
                y = programs[i][1];
            }

        var size = 1;
        for (var i = 1; i < n; i++)
            if (programs[i] != null)
                programs[size++] = programs[i];
        // programs[0...size-1]
        return size;
    }

    private static int[] init(int size)
    {
        var arr = new int[size];
        for (var i = 0; i < size; i++) arr[i] = int.MaxValue;
        return arr;
    }

    private static void F(int[][] programs, int size, int index, int status, int cost, int rest, int[] map)
    {
        if (rest == 0)
        {
            map[status] = Math.Min(map[status], cost);
        }
        else
        {
            if (index != size)
            {
                F(programs, size, index + 1, status, cost, rest, map);
                var pick = 0 | (1 << programs[index][0]) | (1 << programs[index][1]);
                if ((pick & status) == 0)
                    F(programs, size, index + 1, status | pick, cost + programs[index][2], rest - 1, map);
            }
        }
    }

    //	// 如果nums，2 * nums 只乐队
    //	// (1 << (nums << 1)) - 1
    //	// programs 洗数据 size
    //	// nums = 8   16只乐队
    //	
    //	// Process(programs, size, (1 << (nums << 1)) - 1, 0, 4, 0, 0)
    //	
    //	private static int MinCost = Integer.MAX_VALUE;
    //	
    //	private static int[] map = new int[1 << 16]; // map初始化全变成系统最大
    //	
    //	
    //
    //	private static void Process(int[][] programs, int size, int index, int rest, int pick, int cost) {
    //		if (rest == 0) {
    //			
    //			map[pick] = Math.min(map[pick], cost);
    //			
    //		} else { // 还有项目可挑
    //			if (index != size) {
    //				// 不考虑当前的项目！programs[index];
    //				Process(programs, size, index + 1, rest, pick, cost);
    //				// 考虑当前的项目！programs[index];
    //				int x = programs[index][0];
    //				int y = programs[index][1];
    //				int cur = (1 << x) | (1 << y);
    //				if ((pick & cur) == 0) { // 终于可以考虑了！
    //					Process(programs, size, index + 1, rest - 1, pick | cur, cost + programs[index][2]);
    //				}
    //			}
    //		}
    //	}
    //	
    //	
    //	private static void Zuo(int[] arr, int index, int rest) {
    //		if(rest == 0) {
    //			停止
    //		}
    //		if(index != arr.length) {
    //			Zuo(arr, index + 1, rest);
    //			Zuo(arr, index + 1, rest - 1);
    //		}
    //	}

    // 为了测试
    private static int Right(int[][] programs, int nums)
    {
        _min = int.MaxValue;
        R(programs, 0, nums, 0, 0);
        return _min == int.MaxValue ? -1 : _min;
    }

    private static void R(int[][] programs, int index, int rest, int pick, int cost)
    {
        if (rest == 0)
        {
            _min = Math.Min(_min, cost);
        }
        else
        {
            if (index < programs.Length)
            {
                R(programs, index + 1, rest, pick, cost);
                var cur = (1 << programs[index][0]) | (1 << programs[index][1]);
                if ((pick & cur) == 0) R(programs, index + 1, rest - 1, pick | cur, cost + programs[index][2]);
            }
        }
    }

    // 为了测试
    private static int[][] RandomPrograms(int argN, int argV)
    {
        var nums = argN << 1;
        var n = nums * (nums - 1);
        var programs = new int[n][];
        for (var i = 0; i < programs.Length; i++) programs[i] = new int[3];
        for (var i = 0; i < n; i++)
        {
            var a = (int)(Utility.getRandomDouble * nums);
            int b;
            do
            {
                b = (int)(Utility.getRandomDouble * nums);
            } while (b == a);

            programs[i][0] = a;
            programs[i][1] = b;
            programs[i][2] = (int)(Utility.getRandomDouble * argV) + 1;
        }

        return programs;
    }

    // 为了测试
    public static void Run()
    {
        const int n = 4;
        const int v = 100;
        const int t = 10000;

        Console.WriteLine("测试开始");
        for (var i = 0; i < t; i++)
        {
            var nums = (int)(Utility.getRandomDouble * n) + 1;
            var programs = RandomPrograms(nums, v);
            var ans1 = Right(programs, nums);
            var ans2 = MinCost(programs, nums);
            if (ans1 != ans2)
            {
                Console.WriteLine("出错啦！");
                break;
            }
        }

        Console.WriteLine("测试结束");

        var programs1 = RandomPrograms(7, v);
        Utility.RestartStopwatch();
        Right(programs1, 7);

        Console.WriteLine("right方法，在nums=7时候的运行时间(毫秒) : " + Utility.GetStopwatchElapsedMilliseconds());

        programs1 = RandomPrograms(10, v);
        Utility.RestartStopwatch();
        MinCost(programs1, 10);

        Console.WriteLine("minCost方法，在nums=10时候的运行时间(毫秒) : " + Utility.GetStopwatchElapsedMilliseconds());
    }
}