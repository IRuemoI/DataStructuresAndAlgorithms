namespace CustomTraining.Leetcode;

//leetcode:https://leetcode.cn/problems/gou-jian-cheng-ji-shu-zu-lcof/
public class StatisticalResult
{
    private static int[] StatisticalResultCode(int[] arrayA)
    {
        var length = arrayA.Length;
        if (length == 0) return [];

        var arrayB = new int[length];
        arrayB[0] = 1;
        int i;
        for (i = 1; i < length; i++) arrayB[i] = arrayB[i - 1] * arrayA[i - 1];

        var temp = 1;
        for (i = length - 2; i >= 0; i--)
        {
            temp *= arrayA[i + 1];
            arrayB[i] *= temp;
        }

        return arrayB;
    }

    public static void Run()
    {
        int[] arrayA = [1, 2, 3, 4, 5];
        var result = StatisticalResultCode(arrayA);
        Console.WriteLine(string.Join(",", result));
    }
}