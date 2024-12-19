namespace AdvancedTraining.Lesson33;
//pass
public class ProductOfArrayExceptSelf //leetcode_0238
{
    private static int[] ProductExceptSelfCode(int[] numbers)
    {
        var n = numbers.Length;
        var ans = new int[n];
        ans[0] = numbers[0];
        for (var i = 1; i < n; i++) ans[i] = ans[i - 1] * numbers[i];
        var right = 1;
        for (var i = n - 1; i > 0; i--)
        {
            ans[i] = ans[i - 1] * right;
            right *= numbers[i];
        }

        ans[0] = right;
        return ans;
    }

    // 扩展 : 如果仅仅是不能用除号，把结果直接填在nums里呢？
    // 解法：数一共几个0；每一个位得到结果就是，a / b，位运算替代 /，之前的课讲过（算法新手班）

    public static void Run()
    {
        Console.WriteLine(string.Join(",", ProductExceptSelfCode([-1, 1, 0, -3, 3]))); // 输出: [0,0,9,0,0]
    }
}