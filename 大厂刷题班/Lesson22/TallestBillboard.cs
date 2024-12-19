//pass
namespace AdvancedTraining.Lesson22;

// 本题测试链接 : https://leetcode.cn/problems/tallest-billboard/
public class TallestBillboard
{
    private static int Code(int[] rods)
    {
        // key 集合对的某个差  
        // value 满足差值为key的集合对中，最好的一对，较小集合的累加和  
        // 较大 -> value + key  
        Dictionary<int, int> dp = new()
        {
            { 0, 0 } // 空集 和 空集  
        };

        foreach (var num in rods)
            if (num != 0)
            {
                // cur 内部数据完全和dp一样  
                var cur = new Dictionary<int, int>(dp);
                foreach (var d in cur.Keys)
                {
                    var diffMore = cur[d]; // 最好的一对，较小集合的累加和  
                    // x决定放入，比较大的那个  
                    if (!dp.ContainsKey(d + num))
                        dp.Add(d + num, Math.Max(diffMore, dp.ContainsKey(num + d) ? dp[num + d] : 0));
                    else
                        dp[d + num] = Math.Max(diffMore, dp.ContainsKey(num + d) ? dp[num + d] : 0);

                    // x决定放入，比较小的那个  
                    // 新的差值 Math.Abs(x - d)  
                    // 之前差值为Math.Abs(x - d)，的那一对，就要和这一对，决策一下  
                    // 之前那一对，较小集合的累加和diffXD  
                    var diffXd = dp.ContainsKey(Math.Abs(num - d)) ? dp[Math.Abs(num - d)] : 0;
                    if (d >= num) // x决定放入比较小的那个, 但是放入之后，没有超过这一对较大的那个  
                    {
                        if (!dp.ContainsKey(d - num))
                            dp.Add(d - num, Math.Max(diffMore + num, diffXd));
                        else
                            dp[d - num] = Math.Max(diffMore + num, diffXd);
                    }
                    else // x决定放入比较小的那个, 但是放入之后，没有超过这一对较大的那个  
                    {
                        if (!dp.ContainsKey(num - d))
                            dp.Add(num - d, Math.Max(diffMore + d, diffXd));
                        else
                            dp[num - d] = Math.Max(diffMore + d, diffXd);
                    }
                }
            }

        return dp.GetValueOrDefault(0); // 确保dp中有键0，否则返回0  
    }

    public static void Run()
    {
        Console.WriteLine(Code([1, 2, 3, 6])); //输出6
    }
}