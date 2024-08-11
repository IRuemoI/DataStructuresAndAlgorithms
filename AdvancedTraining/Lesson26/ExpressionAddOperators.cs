namespace AdvancedTraining.Lesson26;

// 本题测试链接 : https://leetcode.cn/problems/expression-add-operators/
public class ExpressionAddOperators
{
    private static List<string> AddOperators(string num, int target)
    {
        var ret = new List<string>();
        if (num.Length == 0) return ret;
        // 沿途的数字拷贝和+ - * 的决定，放在path里
        var path = new char[num.Length * 2 - 1];
        // num -> char[]
        var digits = num.ToCharArray();
        long n = 0;
        for (var i = 0; i < digits.Length; i++)
        {
            // 尝试0~i前缀作为第一部分
            n = n * 10 + digits[i] - '0';
            path[i] = digits[i];
            Dfs(ret, path, i + 1, 0, n, digits, i + 1, target); // 后续过程
            if (n == 0) break;
        }

        return ret;
    }

    // char[] digits 固定参数，字符类型数组，等同于num
    // int target 目标
    // char[] path 之前做的决定，已经从左往右依次填写的字符在其中，可能含有'0'~'9' 与 * - +
    // int len path[0..len-1]已经填写好，len是终止
    // int pos 字符类型数组num, 使用到了哪
    // left -> 前面固定的部分 cur -> 前一块
    // 默认 left + cur ...
    private static void Dfs(IList<string> res, char[] path, int len, long left, long cur, char[] num, int index,
        int aim)
    {
        if (index == num.Length)
        {
            if (left + cur == aim) res.Add(new string(path, 0, len));
            return;
        }

        long n = 0;
        var j = len + 1;
        for (var i = index; i < num.Length; i++)
        {
            // pos ~ i
            // 试每一个可能的前缀，作为第一个数字！
            // num[index...i] 作为第一个数字！
            n = n * 10 + num[i] - '0';
            path[j++] = num[i];
            path[len] = '+';
            Dfs(res, path, j, left + cur, n, num, i + 1, aim);
            path[len] = '-';
            Dfs(res, path, j, left + cur, -n, num, i + 1, aim);
            path[len] = '*';
            Dfs(res, path, j, left, cur * n, num, i + 1, aim);
            if (num[index] == '0') break;
        }
    }

    public static void Run()
    {
        foreach (var row in AddOperators("123", 6))
        {
            foreach (var item in row) Console.Write(item); //输出: ["1+2+3", "1*2*3"] 

            Console.WriteLine();
        }
    }
}