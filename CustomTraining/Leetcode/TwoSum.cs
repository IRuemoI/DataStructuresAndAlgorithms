namespace CustomTraining.Leetcode;

//leetcode:https://leetcode.cn/problems/he-wei-sde-liang-ge-shu-zi-lcof/
public static class TwoSum
{
    private static int[] TwoSumCode(int[] price, int target)
    {
        var i = 0;
        var j = price.Length - 1;
        while (i < j)
        {
            var sum = price[i] + price[j];
            if (sum < target) i++;
            else if (sum > target) j--;
            else if (sum == target) return [price[i], price[j]];
        }

        return [];
    }

    public static void Run()
    {
        //int[] price1 = [3, 9, 12, 15];
        //int target1 = 18;
        int[] price2 = [8, 21, 27, 34, 52, 66];
        var target2 = 61;
        //Console.WriteLine(string.Join(",", TwoSumCode(price1, target1)));
        Console.WriteLine(string.Join(",", TwoSumCode(price2, target2)));
    }
}