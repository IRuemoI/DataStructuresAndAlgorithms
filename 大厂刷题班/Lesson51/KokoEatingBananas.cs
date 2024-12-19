namespace AdvancedTraining.Lesson51;

//和leetcode
public class KokoEatingBananas //leetcode_0875
{
    private static int MinEatingSpeed(int[] piles, int h)
    {
        var l = 1;
        var r = 0;
        foreach (var pile in piles) r = Math.Max(r, pile);
        var ans = 0;
        while (l <= r)
        {
            var m = l + ((r - l) >> 1);
            if (Hours(piles, m) <= h)
            {
                ans = m;
                r = m - 1;
            }
            else
            {
                l = m + 1;
            }
        }

        return ans;
    }

    private static int Hours(int[] piles, int speed)
    {
        var ans = 0;
        var offset = speed - 1;
        foreach (var pile in piles) ans += (pile + offset) / speed;
        return ans;
    }

    public static void Run()
    {
        var piles = new[] { 805306368, 805306368, 805306368 };
        var h = 1000000000;
        Console.WriteLine(MinEatingSpeed(piles, h)); //3
    }
}