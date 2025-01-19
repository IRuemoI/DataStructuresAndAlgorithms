//pass
namespace AdvancedTraining.Lesson12;

// 本题测试链接 : https://leetcode.cn/problems/longest-consecutive-sequence/
public class LongestConsecutive
{
    private static int Code(int[] numbers)
    {
        var map = new Dictionary<int, int>();
        var len = 0;
        foreach (var num in numbers)
            if (map.TryAdd(num, 1))
            {
                var preLen = map.ContainsKey(num - 1) ? map[num - 1] : 0;
                var posLen = map.ContainsKey(num + 1) ? map[num + 1] : 0;
                var all = preLen + posLen + 1;
                map[num - preLen] = all;
                map[num + posLen] = all;
                len = Math.Max(len, all);
            }

        return len;
    }

    public static void Run()
    {
        Console.WriteLine(Code([100, 4, 200, 1, 3, 2])); //输出4
    }
}