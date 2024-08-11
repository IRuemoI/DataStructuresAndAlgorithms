namespace AdvancedTraining.Lesson49;

public class ArithmeticSlicesIiSubsequence //Problem_0446
{
    // 时间复杂度是O(N^2)，最优解的时间复杂度
    private static int NumberOfArithmeticSlices(int[] arr)
    {
        var n = arr.Length;
        var ans = 0;
        var maps = new List<Dictionary<int, int>>();
        for (var i = 0; i < n; i++)
        {
            maps.Add(new Dictionary<int, int>());
            //  ....j...i（结尾）
            for (var j = i - 1; j >= 0; j--)
            {
                var diff = arr[i] - (long)arr[j];
                if (diff is <= int.MinValue or > int.MaxValue) continue;
                var dif = (int)diff;
                var count = maps[j][dif];
                ans += count;
                maps[i][dif] = maps[i][dif] + count + 1;
            }
        }

        return ans;
    }
}