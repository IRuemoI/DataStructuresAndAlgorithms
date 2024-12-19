namespace AdvancedTraining.Lesson36;
//pass

// 来自美团
// () 分值为2
// (()) 分值为3
// ((())) 分值为4
// 也就是说，每包裹一层，分数就是里面的分值+1
// ()() 分值为2 * 2
// (())() 分值为3 * 2
// 也就是说，每连接一段，分数就是各部分相乘，以下是一个结合起来的例子
// (()())()(()) -> (2 * 2 + 1) * 2 * 3 -> 30
// 给定一个括号字符串str，已知str一定是正确的括号结合，不会有违规嵌套
// 返回分数
public class ComputeExpressionValue
{
    private static int Sores(string s)
    {
        return Compute(s.ToCharArray(), 0)[0];
    }

    // s[i.....] 遇到 ')' 或者 终止位置  停！
    // 返回值：int[]  长度就是2
    // 0 ：分数是多少
    // 1 : 来到了什么位置停的！
    private static int[] Compute(char[] s, int i)
    {
        if (s[i] == ')') return [1, i];
        var ans = 1;
        while (i < s.Length && s[i] != ')')
        {
            var info = Compute(s, i + 1);
            ans *= info[0] + 1;
            i = info[1] + 1;
        }

        return [ans, i];
    }

    public static void Run()
    {
        var str1 = "(()())()(())";
        Console.WriteLine(Sores(str1));

        // (()()) + (((()))) + ((())())
        // (()()) -> 2 * 2 + 1 -> 5
        // (((()))) -> 5
        // ((())()) -> ((2 + 1) * 2) + 1 -> 7
        // 所以下面的结果应该是175
        var str2 = "(()())(((())))((())())";
        Console.WriteLine(Sores(str2));

        // (()()()) + (()(()))
        // (()()()) -> 2 * 2 * 2 + 1 -> 9
        // (()(())) -> 2 * 3 + 1 -> 7
        // 所以下面的结果应该是63
        var str3 = "(()()())(()(()))";
        Console.WriteLine(Sores(str3));
    }
}