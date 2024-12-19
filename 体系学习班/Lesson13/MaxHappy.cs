//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson13;

public class MaxHappy
{
    private static int MaxHappy1(Employee? boss)
    {
        if (boss == null) return 0;

        return Process1(boss, false);
    }

    // 当前来到的节点叫cur，
    // up表示cur的上级是否来，
    // 该函数含义：
    // 如果up为true，表示在cur上级已经确定来，的情况下，cur整棵树能够提供最大的快乐值是多少？
    // 如果up为false，表示在cur上级已经确定不来，的情况下，cur整棵树能够提供最大的快乐值是多少？
    private static int Process1(Employee cur, bool up)
    {
        if (up)
        {
            // 如果cur的上级来的话，cur没得选，只能不来
            var ans = 0;
            foreach (var next in cur.NextList) ans += Process1(next, false);

            return ans;
        }

        // 如果cur的上级不来的话，cur可以选，可以来也可以不来
        var p1 = cur.Happy;
        var p2 = 0;
        foreach (var next in cur.NextList)
        {
            p1 += Process1(next, true);
            p2 += Process1(next, false);
        }

        return Math.Max(p1, p2);
    }

    private static int MaxHappy2(Employee? head)
    {
        var allInfo = Process(head);
        return Math.Max(allInfo.No, allInfo.Yes);
    }

    private static Info Process(Employee? x)
    {
        if (x == null) return new Info(0, 0);

        var no = 0;
        var yes = x.Happy;
        foreach (var next in x.NextList)
        {
            var nextInfo = Process(next);
            no += Math.Max(nextInfo.No, nextInfo.Yes);
            yes += nextInfo.No;
        }

        return new Info(no, yes);
    }

    //用于测试
    private static Employee? GenerateBoss(int maxLevel, int maxNext, int maxHappy)
    {
        if (Utility.getRandomDouble < 0.02) return null;

        var boss = new Employee((int)(Utility.getRandomDouble * (maxHappy + 1)));
        GenerateNext(boss, 1, maxLevel, maxNext, maxHappy);
        return boss;
    }

    //用于测试
    private static void GenerateNext(Employee e, int level, int maxLevel, int maxNext, int maxHappy)
    {
        if (level > maxLevel) return;

        var nextSize = (int)(Utility.getRandomDouble * (maxNext + 1));
        for (var i = 0; i < nextSize; i++)
        {
            var next = new Employee((int)(Utility.getRandomDouble * (maxHappy + 1)));
            e.NextList.Add(next);
            GenerateNext(next, level + 1, maxLevel, maxNext, maxHappy);
        }
    }

    public static void Run()
    {
        var maxLevel = 4;
        const int maxNext = 7;
        var maxHappy = 100;
        var testTimes = 100000;
        for (var i = 0; i < testTimes; i++)
        {
            var boss = GenerateBoss(maxLevel, maxNext, maxHappy);
            if (MaxHappy1(boss) != MaxHappy2(boss)) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");
    }

    public class Employee
    {
        public readonly int Happy;
        public readonly List<Employee> NextList;

        public Employee(int h)
        {
            Happy = h;
            NextList = new List<Employee>();
        }
    }

    private class Info
    {
        public readonly int No;
        public readonly int Yes;

        public Info(int n, int y)
        {
            No = n;
            Yes = y;
        }
    }
}