namespace AdvancedTraining.Lesson08;

// 本题测试链接 : https://leetcode.cn/problems/basic-calculator-iii/
public class ExpressionCompute
{
    private static int Calculate(string str)
    {
        return F(str.ToCharArray(), 0)[0];
    }

    // 请从str[i...]往下算，遇到字符串终止位置或者右括号，就停止
    // 返回两个值，长度为2的数组
    // 0) 负责的这一段的结果是多少
    // 1) 负责的这一段计算到了哪个位置
    private static int[] F(char[] str, int i)
    {
        var que = new LinkedList<string>();
        var cur = 0;
        // 从i出发，开始撸串
        while (i < str.Length && str[i] != ')')
            if (str[i] >= '0' && str[i] <= '9')
            {
                cur = cur * 10 + str[i++] - '0';
            }
            else if (str[i] != '(')
            {
                // 遇到的是运算符号
                AddNum(que, cur);
                que.AddLast(str[i++].ToString());
                cur = 0;
            }
            else
            {
                // 遇到左括号了
                var bra = F(str, i + 1);
                cur = bra[0];
                i = bra[1] + 1;
            }

        AddNum(que, cur);
        return [GetNum(que), i];
    }

    private static void AddNum(LinkedList<string> que, int num)
    {
        if (que.Count > 0)
        {
            var top = que.Last!.Value;
            que.RemoveLast();
            if (top.Equals("+") || top.Equals("-"))
            {
                que.AddLast(top);
            }
            else
            {
                var temp = que.Last.Value;
                que.RemoveLast();
                var cur = Convert.ToInt32(temp);
                num = top.Equals("*") ? cur * num : cur / num;
            }
        }

        que.AddLast(num.ToString());
    }

    private static int GetNum(LinkedList<string> que)
    {
        var res = 0;
        var add = true;
        while (que.Count > 0)
        {
            var temp = que.First!.Value;
            que.RemoveFirst();
            var cur = temp;
            if (cur.Equals("+"))
            {
                add = true;
            }
            else if (cur.Equals("-"))
            {
                add = false;
            }
            else
            {
                var num = Convert.ToInt32(cur);
                res += add ? num : -num;
            }
        }

        return res;
    }
}