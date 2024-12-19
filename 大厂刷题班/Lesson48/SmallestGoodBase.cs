namespace AdvancedTraining.Lesson48;

//pass
public class SmallestGoodBase //leetcode_0483
{
    // ""4651" -> 4651
    private static string Code(string n)
    {
        var num = Convert.ToInt64(n);
        // n这个数，需要从m位开始试，固定位数，一定要有m位！
        for (var m = (int)(Math.Log(num + 1) / Math.Log(2)); m > 2; m--)
        {
            // num开m次方
            var l = (long)Math.Pow(num, 1.0 / m);
            var r = (long)Math.Pow(num, 1.0 / (m - 1)) + 1L;
            while (l <= r)
            {
                var k = l + ((r - l) >> 1);
                var sum = 0L;
                var @base = 1L;
                for (var i = 0; i < m && sum <= num; i++)
                {
                    sum += @base;
                    @base *= k;
                }

                if (sum < num)
                    l = k + 1;
                else if (sum > num)
                    r = k - 1;
                else
                    return k.ToString();
            }
        }

        return (num - 1).ToString();
    }
}