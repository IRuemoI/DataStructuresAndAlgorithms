namespace AdvancedTraining.Lesson35;
//pass
//https://leetcode.cn/problems/4sum-ii/description/
public class FourNumSumIi //leetcode_0454
{
    private static int FourSumCount(int[] a, int[] b, int[] c, int[] d)
    {
        var map = new Dictionary<int, int>();
        int sum;
        foreach (var itemI in a)
        foreach (var itemJ in b)
        {
            sum = itemI + itemJ;
            if (!map.TryAdd(sum, 1))
                map[sum] += 1;
        }

        var ans = 0;
        foreach (var itemI in c)
        foreach (var itemJ in d)
        {
            sum = itemI + itemJ;
            if (map.ContainsKey(-sum)) ans += map[-sum];
        }

        return ans;
    }

    public static void Run()
    {
        Console.WriteLine(FourSumCount([1, 2], [-2, -1], [-1, 2], [0, 2])); //输出2
    }
}