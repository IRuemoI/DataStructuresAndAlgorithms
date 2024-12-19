namespace AdvancedTraining.Lesson51;

// leetcode题目 : https://leetcode-cn.com/problems/programmable-robot/
public class ProgrammableRobot
{
    // 此处为一轮以内，x和y最大能移动的步数，对应的2的几次方
    // 比如本题，x和y最大能移动1000步，就对应2的10次方
    // 如果换一个数据量，x和y最大能移动5000步，就对应2的13次方
    // 只需要根据数据量修改这一个变量，剩下的代码不需要调整
    private const int Bit = 10;

    // 如果，x和y最大能移动的步数，对应2的bit次方
    // 那么一个坐标(x,y)，所有的可能性就是：(2 ^ bit) ^ 2 = 2 ^ (bit * 2)
    // 也就是，(1 << (bit << 1))个状态，记为bits
    private const int Bits = 1 << (Bit << 1);

    // 为了表示下bits个状态，需要几个整数？
    // 32位只需要一个整数，所以bits个状态，需要bits / 32 个整数
    // 即整型长度需要 : bits >> 5
    private static readonly int[] Set = new int[Bits >> 5];

    private static bool Robot1(string command, int[][] obstacles, int x, int y)
    {
        var x1 = 0;
        var y2 = 0;
        var set = new HashSet<int> { 0 };
        foreach (var c in command)
        {
            x1 += c == 'R' ? 1 : 0;
            y2 += c == 'U' ? 1 : 0;
            set.Add((x1 << 10) | y2);
        }

        // 不考虑任何额外的点，机器人能不能到达，(x，y)
        if (!Meet1(x, y, x1, y2, set)) return false;
        foreach (var ob in obstacles)
            // ob[0] ob[1]
            if (ob[0] <= x && ob[1] <= y && Meet1(ob[0], ob[1], x1, y2, set))
                return false;
        return true;
    }

    // 一轮以内，X，往右一共有几个单位
    // Y, 往上一共有几个单位
    // set, 一轮以内的所有可能性
    // (x,y)要去的点
    // 机器人从(0,0)位置，能不能走到(x,y)
    private static bool Meet1(int x, int y, int x1, int y1, HashSet<int> set)
    {
        if (x1 == 0)
            // Y != 0 往上肯定走了！
            return x == 0;
        if (y1 == 0) return y == 0;
        // 至少几轮？
        var atLeast = Math.Min(x / x1, y / y1);
        // 经历过最少轮数后，x剩多少？
        var rx = x - atLeast * x1;
        // 经历过最少轮数后，y剩多少？
        var ry = y - atLeast * y1;
        return set.Contains((rx << 10) | ry);
    }

    private static bool Robot2(string command, int[][] obstacles, int x, int y)
    {
        Array.Fill(Set, 0);
        Set[0] = 1;
        var x1 = 0;
        var y1 = 0;
        foreach (var c in command)
        {
            x1 += c == 'R' ? 1 : 0;
            y1 += c == 'U' ? 1 : 0;
            Add((x1 << 10) | y1);
        }

        if (!Meet2(x, y, x1, y1)) return false;
        foreach (var ob in obstacles)
            if (ob[0] <= x && ob[1] <= y && Meet2(ob[0], ob[1], x1, y1))
                return false;
        return true;
    }

    private static bool Meet2(int x, int y, int x1, int y1)
    {
        if (x1 == 0) return x == 0;
        if (y1 == 0) return y == 0;
        var atLeast = Math.Min(x / x1, y / y1);
        var rx = x - atLeast * x1;
        var ry = y - atLeast * y1;
        return Contains((rx << 10) | ry);
    }

    private static void Add(int status)
    {
        Set[status >> 5] |= 1 << (status & 31);
    }

    private static bool Contains(int status)
    {
        return status < Bits && (Set[status >> 5] & (1 << (status & 31))) != 0;
    }

    // int num -> 32位的状态
    // 请打印这32位状态
    private static void PrintBinary(int num)
    {
        for (var i = 31; i >= 0; i--) Console.Write((num & (1 << i)) != 0 ? "1" : "0");
        Console.WriteLine();
    }

    public static void Run()
    {
        var x = 7;
        PrintBinary(x);

        var y = 4;

        PrintBinary(y);

        // x_y 组合！
        var c = (x << 10) | y;
        PrintBinary(c);

        Console.WriteLine(c);

        // 0 ~ 1048575 任何一个数，bit来表示的！
        //		int[] set = new int[32768];
        //		set[0] = int  32 位   0~31这些数出现过没出现过
        //		set[1] = int  32 位   32~63这些数出现过没出现过

        // 0 ~ 1048575
        //		int[] set = new int[32768];
        //		int num = 738473; // 32 bit int
        ////		set[  734873 / 32   ] // 734873 % 32
        //		boolean exist = (set[num / 32] & (1 << (num % 32))) != 0;
    }
}