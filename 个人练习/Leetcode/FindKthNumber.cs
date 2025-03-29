namespace CustomTraining.Leetcode;

//leetcode:https://leetcode.cn/problems/shu-zi-xu-lie-zhong-mou-yi-wei-de-shu-zi-lcof/
public static class FindKthNumber
{
    //这道题的难点在于需要构造一个这样的字符串：1-n的数组通过追加的方式组成字符串。最终的结果可以获得这个字符串的第k个元素即可。
    //当然通过暴力创建字符串的方式是低效的，我们要做的就是模拟这个填充的过程，跳过不必要的部分
    public static int FindKthNumberCode(int k1)
    {
        long k = k1;
        var digit = 1;
        long start = 1;
        long count = 9;
        while (k > count)
        {
            digit += 1;
            k -= count;
            start *= 10;
            count = digit * start * 9;
        }

        var num = start + (k - 1) / digit;
        var temp = (int)((k - 1) % digit);
        return int.Parse(num.ToString().Substring(temp, 1));
    }
}