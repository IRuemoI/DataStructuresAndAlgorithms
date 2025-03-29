namespace AdvancedTraining.Lesson42;

public class SelfCrossing //leetcode_0335
{
    private static bool IsSelfCrossing(int[]? x)
    {
        if (x == null || x.Length < 4) return false;
        if ((x.Length > 3 && x[2] <= x[0] && x[3] >= x[1]) ||
            (x.Length > 4 && ((x[3] <= x[1] && x[4] >= x[2]) || (x[3] == x[1] && x[0] + x[4] >= x[2])))) return true;
        for (var i = 5; i < x.Length; i++)
            if (x[i - 1] <= x[i - 3] && (x[i] >= x[i - 2] ||
                                         (x[i - 2] >= x[i - 4] && x[i - 5] + x[i - 1] >= x[i - 3] &&
                                          x[i - 4] + x[i] >= x[i - 2])))
                return true;
        return false;
    }

    public static void Run()
    {
        int[] arr = [2, 1, 1, 2];
        Console.WriteLine(IsSelfCrossing(arr)); //true
    }
}