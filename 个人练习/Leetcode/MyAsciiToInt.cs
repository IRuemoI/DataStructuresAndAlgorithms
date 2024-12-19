namespace CustomTraining.Leetcode;

//leetcode:https://leetcode.cn/problems/ba-zi-fu-chuan-zhuan-huan-cheng-zheng-shu-lcof/
public class MyAsciiToInt
{
    public static int MyAsciiToIntCode(string str)
    {
        int n = str.Length;
        int i = 0;
        int sign = 1;
        long num = 0;
        if (n == 0) return 0;
        while (i < n && str[i] == ' ')
        {
            i++;
        }

        if (i == n) return 0;

        if (i < n && (str[i] == '+' || str[i] == '-'))
        {
            sign = str[i] == '+' ? 1 : -1;
            i++;
        }

        while (i < n)
        {
            if (char.IsDigit(str[i]))
            {
                num = num * 10 + str[i] - '0';
                if (num > int.MaxValue)
                {
                    return sign == 1 ? int.MaxValue : int.MinValue;
                }

                i++;
            }
            else
            {
                break;
            }
        }

        return (int)num * sign;
    }
}