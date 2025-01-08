namespace CustomTraining.Leetcode;

//leetcode:https://leetcode.cn/problems/shu-zi-xu-lie-zhong-mou-yi-wei-de-shu-zi-lcof/
public static class FindKthNumber
{
    public static int FindKthNumberCode(int k1)
    {
        long k = k1;
        int digit = 1;
        long start = 1;
        long count = 9;
        while (k > count)
        {
            digit += 1;
            k -= count;
            start *= 10;
            count = digit * start * 9;
        }

        long num = start + (k - 1) / digit;
        int temp = (int)((k - 1) % digit);
        return int.Parse(num.ToString().Substring(temp,1));
    }
}