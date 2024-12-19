namespace AdvancedTraining.Lesson10;

// 本题测试链接 : https://leetcode-cn.com/problems/boolean-evaluation-lcci/
public class BooleanEvaluation
{
    private static int CountEval0(string express, int desired)
    {
        if (express is null or "") return 0;
        var exp = express.ToCharArray();
        var n = exp.Length;
        var dp = new Info[n, n];
        var allInfo = Func(exp, 0, exp.Length - 1, dp);
        if (allInfo != null) return desired == 1 ? allInfo.T : allInfo.F1;
        throw new Exception("desired is null");
    }

    // 限制:
    // L...R上，一定有奇数个字符
    // L位置的字符和R位置的字符，非0即1，不能是逻辑符号！
    // 返回str[L...R]这一段，为true的方法数，和false的方法数
    private static Info? Func(char[] str, int l, int r, Info?[,] dp)
    {
        if (dp[l, r] != null) return dp[l, r];
        var t = 0;
        var f = 0;
        if (l == r)
        {
            t = str[l] == '1' ? 1 : 0;
            f = str[l] == '0' ? 1 : 0;
        }
        else
        {
            // L..R >=3
            // 每一个种逻辑符号，split枚举的东西
            // 都去试试最后结合
            for (var split = l + 1; split < r; split += 2)
            {
                var leftInfo = Func(str, l, split - 1, dp);
                var rightInfo = Func(str, split + 1, r, dp);
                if (leftInfo != null && rightInfo != null)
                {
                    var a = leftInfo.T;
                    var b = leftInfo.F1;
                    var c = rightInfo.T;
                    var d = rightInfo.F1;
                    switch (str[split])
                    {
                        case '&':
                            t += a * c;
                            f += b * c + b * d + a * d;
                            break;
                        case '|':
                            t += a * c + a * d + b * c;
                            f += b * d;
                            break;
                        case '^':
                            t += a * d + b * c;
                            f += a * c + b * d;
                            break;
                    }
                }
            }
        }

        dp[l, r] = new Info(t, f);
        return dp[l, r];
    }

    private static int CountEval1(string express, int desired)
    {
        if (express is null or "") return 0;
        var exp = express.ToCharArray();
        return F(exp, desired, 0, exp.Length - 1);
    }

    private static int F(char[] str, int desired, int l, int r)
    {
        if (l == r)
        {
            if (str[l] == '1')
                return desired;
            return desired ^ 1;
        }

        var res = 0;
        if (desired == 1)
            for (var i = l + 1; i < r; i += 2)
                switch (str[i])
                {
                    case '&':
                        res += F(str, 1, l, i - 1) * F(str, 1, i + 1, r);
                        break;
                    case '|':
                        res += F(str, 1, l, i - 1) * F(str, 0, i + 1, r);
                        res += F(str, 0, l, i - 1) * F(str, 1, i + 1, r);
                        res += F(str, 1, l, i - 1) * F(str, 1, i + 1, r);
                        break;
                    case '^':
                        res += F(str, 1, l, i - 1) * F(str, 0, i + 1, r);
                        res += F(str, 0, l, i - 1) * F(str, 1, i + 1, r);
                        break;
                }
        else
            for (var i = l + 1; i < r; i += 2)
                switch (str[i])
                {
                    case '&':
                        res += F(str, 0, l, i - 1) * F(str, 1, i + 1, r);
                        res += F(str, 1, l, i - 1) * F(str, 0, i + 1, r);
                        res += F(str, 0, l, i - 1) * F(str, 0, i + 1, r);
                        break;
                    case '|':
                        res += F(str, 0, l, i - 1) * F(str, 0, i + 1, r);
                        break;
                    case '^':
                        res += F(str, 1, l, i - 1) * F(str, 1, i + 1, r);
                        res += F(str, 0, l, i - 1) * F(str, 0, i + 1, r);
                        break;
                }

        return res;
    }

    private static int CountEval2(string express, int desired)
    {
        if (express is null or "") return 0;
        var exp = express.ToCharArray();
        var n = exp.Length;
        var dp = new int[2, n, n];
        dp[0, 0, 0] = exp[0] == '0' ? 1 : 0;
        dp[1, 0, 0] = dp[0, 0, 0] ^ 1;
        for (var i = 2; i < exp.Length; i += 2)
        {
            dp[0, i, i] = exp[i] == '1' ? 0 : 1;
            dp[1, i, i] = exp[i] == '0' ? 0 : 1;
            for (var j = i - 2; j >= 0; j -= 2)
            for (var k = j; k < i; k += 2)
                if (exp[k + 1] == '&')
                {
                    dp[1, j, i] += dp[1, j, k] * dp[1, k + 2, i];
                    dp[0, j, i] += (dp[0, j, k] + dp[1, j, k]) * dp[0, k + 2, i] + dp[0, j, k] * dp[1, k + 2, i];
                }
                else if (exp[k + 1] == '|')
                {
                    dp[1, j, i] += (dp[0, j, k] + dp[1, j, k]) * dp[1, k + 2, i] + dp[1, j, k] * dp[0, k + 2, i];
                    dp[0, j, i] += dp[0, j, k] * dp[0, k + 2, i];
                }
                else
                {
                    dp[1, j, i] += dp[0, j, k] * dp[1, k + 2, i] + dp[1, j, k] * dp[0, k + 2, i];
                    dp[0, j, i] += dp[0, j, k] * dp[0, k + 2, i] + dp[1, j, k] * dp[1, k + 2, i];
                }
        }

        return dp[desired, 0, n - 1];
    }

    public static void Run()
    {
        Console.WriteLine(CountEval0("1^0|0|1", 0)); //输出2
        Console.WriteLine(CountEval1("1^0|0|1", 0)); //输出2
        Console.WriteLine(CountEval2("1^0|0|1", 0)); //输出2
    }

    private class Info(int tr, int fa)
    {
        public readonly int F1 = fa;
        public readonly int T = tr;
    }
}