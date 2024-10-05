//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson14;

public class BestArrange
{
    // 暴力！所有情况都尝试！
    private static int BestArrange1(Program1[]? programs)
    {
        if (programs == null || programs.Length == 0) return 0;

        return Process(programs, 0, 0);
    }

    // 还剩下的会议都放在programs里
    // done之前已经安排了多少会议的数量
    // timeLine目前来到的时间点是什么

    // 目前来到timeLine的时间点，已经安排了done多的会议，剩下的会议programs可以自由安排
    // 返回能安排的最多会议数量
    private static int Process(Program1[] programs, int done, int timeLine)
    {
        if (programs.Length == 0) return done;

        // 还剩下会议
        var max = done;
        // 当前安排的会议是什么会，每一个都枚举
        for (var i = 0; i < programs.Length; i++)
            if (programs[i].Start >= timeLine)
            {
                var next = CopyButExcept(programs, i);
                max = Math.Max(max, Process(next, done + 1, programs[i].End));
            }

        return max;
    }

    private static Program1[] CopyButExcept(Program1[] programs, int i)
    {
        var ans = new Program1[programs.Length - 1];
        var index = 0;
        for (var k = 0; k < programs.Length; k++)
            if (k != i)
                ans[index++] = programs[k];

        return ans;
    }

    // 会议的开始时间和结束时间，都是数值，不会 < 0
    private static int BestArrange2(Program1[] programs)
    {
        Array.Sort(programs, (a, b) => a.End - b.End);
        var timeLine = 0;
        var result = 0;
        // 依次遍历每一个会议，结束时间早的会议先遍历
        foreach (var element in programs)
            if (timeLine <= element.Start)
            {
                result++;
                timeLine = element.End;
            }

        return result;
    }

    //用于测试
    private static Program1[] GeneratePrograms(int programSize, int timeMax)
    {
        var ans = new Program1[(int)(Utility.getRandomDouble * (programSize + 1))];
        for (var i = 0; i < ans.Length; i++)
        {
            var r1 = (int)(Utility.getRandomDouble * (timeMax + 1));
            var r2 = (int)(Utility.getRandomDouble * (timeMax + 1));
            if (r1 == r2)
                ans[i] = new Program1(r1, r1 + 1);
            else
                ans[i] = new Program1(Math.Min(r1, r2), Math.Max(r1, r2));
        }

        return ans;
    }

    public static void Run()
    {
        var programSize = 12;
        var timeMax = 20;
        var timeTimes = 1000000;
        for (var i = 0; i < timeTimes; i++)
        {
            var programs = GeneratePrograms(programSize, timeMax);
            if (BestArrange1(programs) != BestArrange2(programs)) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");
    }

    private class Program1
    {
        public readonly int End;
        public readonly int Start;

        public Program1(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}