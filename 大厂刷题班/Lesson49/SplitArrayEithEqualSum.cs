namespace AdvancedTraining.Lesson49;

//https://www.cnblogs.com/cnoodle/p/13527007.html
public class SplitArrayEithEqualSum //leetcode_0548
{
    private static bool SplitArray(int[] nums)
    {
        if (nums.Length < 7) return false;
        var sumLeftToRight = new int[nums.Length];
        var sumRightToLeft = new int[nums.Length];
        var s = 0;
        for (var i = 0; i < nums.Length; i++)
        {
            sumLeftToRight[i] = s;
            s += nums[i];
        }

        s = 0;
        for (var i = nums.Length - 1; i >= 0; i--)
        {
            sumRightToLeft[i] = s;
            s += nums[i];
        }

        for (var i = 1; i < nums.Length - 5; i++)
        for (var j = nums.Length - 2; j > i + 3; j--)
            if (sumLeftToRight[i] == sumRightToLeft[j] && Find(sumLeftToRight, sumRightToLeft, i, j))
                return true;
        return false;
    }

    private static bool Find(int[] sumLeftToRight, int[] sumRightToLeft, int l, int r)
    {
        var s = sumLeftToRight[l];
        var prefix = sumLeftToRight[l + 1];
        var suffix = sumRightToLeft[r - 1];
        for (var i = l + 2; i < r - 1; i++)
        {
            var s1 = sumLeftToRight[i] - prefix;
            var s2 = sumRightToLeft[i] - suffix;
            if (s1 == s2 && s1 == s) return true;
        }

        return false;
    }

    public static void Run()
    {
        Console.WriteLine(SplitArray([1, 2, 1, 2, 1, 2, 1])); //true
    }
}