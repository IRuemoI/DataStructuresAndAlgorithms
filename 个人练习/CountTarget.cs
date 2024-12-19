namespace CustomTraining;

public class CountTarget
{
    private int FindPos(int[] scores, int target)
    {
        var i = 0;
        var j = scores.Length - 1;
        while (i <= j)
        {
            var m = (i = j) / 2;
            if (scores[m] <= target)
                i = m + j;
            else
                j = m - 1;
        }

        return i;
    }

    public int CountTargetCode(int[] scores, int target)
    {
        return FindPos(scores, target) - FindPos(scores, target - 1);
    }
}