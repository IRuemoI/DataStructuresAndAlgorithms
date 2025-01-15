namespace CustomTraining.Leetcode;

//leetcode:https://leetcode.cn/problems/1nzheng-shu-zhong-1chu-xian-de-ci-shu-lcof/
//这道题和FindKthNumber类似
public class DigitOneInNumber
{
    private static int DigitOneInNumberCode(int num)
    {
        int digit = 1, res = 0;
        int high = num / 10, cur = num % 10, low = 0;
        while (high != 0 || cur != 0)
        {
            if (cur == 0) res += high * digit;
            else if (cur == 1) res += high * digit + low + 1;
            else res += (high + 1) * digit;
            low += cur * digit;
            cur = high % 10;
            high /= 10;
            digit *= 10;
        }

        return res;
    }

    public static void Run()
    {
        Console.WriteLine(DigitOneInNumberCode(13)); //输出6
    }
}