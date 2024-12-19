//pass
#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson23;

public class MaxAbsBetweenLeftAndRight
{
    private static int MaxAbs1(int[] arr)
    {
        var res = int.MinValue;
        for (var i = 0; i != arr.Length - 1; i++)
        {
            var maxLeft = int.MinValue;
            for (var j = 0; j != i + 1; j++) maxLeft = Math.Max(arr[j], maxLeft);
            var maxRight = int.MinValue;
            for (var j = i + 1; j != arr.Length; j++) maxRight = Math.Max(arr[j], maxRight);
            res = Math.Max(Math.Abs(maxLeft - maxRight), res);
        }

        return res;
    }

    private static int MaxAbs2(int[] arr)
    {
        var lArr = new int[arr.Length];
        var rArr = new int[arr.Length];
        lArr[0] = arr[0];
        rArr[arr.Length - 1] = arr[^1];
        for (var i = 1; i < arr.Length; i++) lArr[i] = Math.Max(lArr[i - 1], arr[i]);
        for (var i = arr.Length - 2; i > -1; i--) rArr[i] = Math.Max(rArr[i + 1], arr[i]);
        var max = 0;
        for (var i = 0; i < arr.Length - 1; i++) max = Math.Max(max, Math.Abs(lArr[i] - rArr[i + 1]));
        return max;
    }

    private static int MaxAbs3(int[] arr)
    {
        var max = int.MinValue;
        foreach (var item in arr)
            max = Math.Max(item, max);

        return max - Math.Min(arr[0], arr[^1]);
    }

    private static int[] GetRandomStringArray(int length)
    {
        var arr = new int[length];
        for (var i = 0; i != arr.Length; i++) arr[i] = (int)(Utility.getRandomDouble * 1000) - 499;
        return arr;
    }

    public static void Run()
    {
        var arr = GetRandomStringArray(200);
        Console.WriteLine(MaxAbs1(arr));
        Console.WriteLine(MaxAbs2(arr));
        Console.WriteLine(MaxAbs3(arr));
    }
}