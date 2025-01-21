namespace AdvancedTraining.Lesson38;

//https://leetcode.cn/problems/find-all-numbers-disappeared-in-an-array/description/
public class FindAllNumbersDisappearedInAnArray //leetcode_0448
{
    private static IList<int> FindDisappearedNumbers(int[]? numbers)
    {
        IList<int> ans = new List<int>();
        if (numbers == null || numbers.Length == 0) return ans;
        var n = numbers.Length;
        for (var i = 0; i < n; i++)
            // 从i位置出发，去玩下标循环怼
            Walk(numbers, i);
        for (var i = 0; i < n; i++)
            if (numbers[i] != i + 1)
                ans.Add(i + 1);
        return ans;
    }

    private static void Walk(int[] numbers, int i)
    {
        while (numbers[i] != i + 1)
        {
            // 不断从i发货
            var nextI = numbers[i] - 1;
            if (numbers[nextI] == nextI + 1) break;
            (numbers[i], numbers[nextI]) = (numbers[nextI], numbers[i]);
        }
    }

    public static void Run()
    {
        Console.WriteLine(string.Join(",", FindDisappearedNumbers([4, 3, 2, 7, 8, 2, 3, 1]))); //输出[5,6]
    }
}