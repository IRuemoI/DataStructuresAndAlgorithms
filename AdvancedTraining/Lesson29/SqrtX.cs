namespace AdvancedTraining.Lesson29;

public class SqrtX //Problem_0069
{
    // x一定非负，输入可以保证
    private static int MySqrt(int x)
    {
        if (x == 0) return 0;
        if (x < 3) return 1;
        // x >= 3
        long ans = 1;
        long l = 1;
        long r = x;
        while (l <= r)
        {
            var m = (l + r) / 2;
            if (m * m <= x)
            {
                ans = m;
                l = m + 1;
            }
            else
            {
                r = m - 1;
            }
        }

        return (int)ans;
    }


    public static void Run()
    {
        Console.WriteLine(MySqrt(8)); //输出2
    }
}