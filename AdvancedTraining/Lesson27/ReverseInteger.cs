namespace AdvancedTraining.Lesson27;

public class ReverseInteger //Problem_0007
{
    private static int Reverse(int x)
    {
        var neg = ((x >>> 31) & 1) == 1;
        x = neg ? x : -x;
        var m = int.MinValue / 10;
        var o = int.MinValue % 10;
        var res = 0;
        while (x != 0)
        {
            if (res < m || (res == m && x % 10 < o)) return 0;
            res = res * 10 + x % 10;
            x /= 10;
        }

        return neg ? res : Math.Abs(res);
    }
}