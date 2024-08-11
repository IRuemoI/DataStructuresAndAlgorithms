namespace AdvancedTraining.Lesson32;

//https://blog.csdn.net/qq_44631615/article/details/138450068
public class MissingRanges //Problem_0163
{
    private static IList<string> FindMissingRanges(int[] numbers, int lower, int upper)
    {
        IList<string> ans = new List<string>();
        foreach (var num in numbers)
        {
            if (num > lower) ans.Add(Miss(lower, num - 1));
            if (num == upper) return ans;
            lower = num + 1;
        }

        if (lower <= upper) ans.Add(Miss(lower, upper));
        return ans;
    }

    // 生成"lower->upper"的字符串，如果lower==upper，只用生成"lower"
    private static string Miss(int lower, int upper)
    {
        var left = lower.ToString();
        var right = "";
        if (upper > lower) right = "->" + upper;
        return left + right;
    }

    public static void Run()
    {
        int[] numbers = [0, 1, 3, 50, 75];
        var lower = 0;
        var upper = 99;

        Console.WriteLine(string.Join(", ", FindMissingRanges(numbers, lower, upper)));
    }
}