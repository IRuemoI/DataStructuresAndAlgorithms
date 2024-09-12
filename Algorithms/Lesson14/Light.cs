//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson14;

public class Light
{
    private static int MinLight1(string road)
    {
        if (string.IsNullOrEmpty(road)) return 0;
        return Process(road.ToCharArray(), 0, new HashSet<int>());
    }

    // str[index....]位置，自由选择放灯还是不放灯
    // str[0..index-1]位置呢？已经做完决定了，那些放了灯的位置，存在lights里
    // 要求选出能照亮所有.的方案，并且在这些有效的方案中，返回最少需要几个灯
    private static int Process(char[] str, int index, HashSet<int> lights)
    {
        if (index == str.Length)
        {
            // 结束的时候
            for (var i = 0; i < str.Length; i++)
                if (str[i] != 'X')
                    // 当前位置是点的话
                    if (!lights.Contains(i - 1) && !lights.Contains(i) && !lights.Contains(i + 1))
                        return int.MaxValue;

            return lights.Count;
        }

        // str还没结束
        // i X .
        var no = Process(str, index + 1, lights);
        var yes = int.MaxValue;
        if (str[index] == 'O')
        {
            lights.Add(index);
            yes = Process(str, index + 1, lights);
            lights.Remove(index);
        }

        return Math.Min(no, yes);
    }

    private static int MinLight2(string? road)
    {
        var str = road?.ToCharArray();
        var i = 0;
        var light = 0;
        while (i < str?.Length)
            if (str[i] == 'X')
            {
                i++;
            }
            else
            {
                light++;
                if (i + 1 == str.Length) break;

                // 有i位置  i+ 1   X  O
                if (str[i + 1] == 'X')
                    i = i + 2;
                else
                    i = i + 3;
            }

        return light;
    }

    //用于测试
    private static string RandomString(int len)
    {
        var res = new char[(int)(Utility.GetRandomDouble * len) + 1];
        for (var i = 0; i < res.Length; i++) res[i] = Utility.GetRandomDouble < 0.5 ? 'X' : 'O';

        return new string(res);
    }

    public static void Run()
    {
        var len = 20;
        var testTime = 10000;
        for (var i = 0; i < testTime; i++)
        {
            var test = RandomString(len);
            var ans1 = MinLight1(test);
            var ans2 = MinLight2(test);
            if (ans1 != ans2) Console.WriteLine("出错  ans1:{0} ans2:{1}", ans1, ans2);
        }

        Console.WriteLine("测试完成");
    }
}