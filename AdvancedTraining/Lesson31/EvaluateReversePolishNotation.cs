namespace AdvancedTraining.Lesson31;

public class EvaluateReversePolishNotation //Problem_0150
{
    private static int EvalRpn(string[] tokens)
    {
        var stack = new Stack<int>();
        foreach (var str in tokens)
            if (str.Equals("+") || str.Equals("-") || str.Equals("*") || str.Equals("/"))
                Compute(stack, str);
            else
                stack.Push(Convert.ToInt32(str));
        return stack.Peek();
    }

    private static void Compute(Stack<int> stack, string op)
    {
        var num2 = stack.Pop();
        var num1 = stack.Pop();
        var ans = 0;
        switch (op)
        {
            case "+":
                ans = num1 + num2;
                break;
            case "-":
                ans = num1 - num2;
                break;
            case "*":
                ans = num1 * num2;
                break;
            case "/":
                ans = num1 / num2;
                break;
        }

        stack.Push(ans);
    }

    public static void Run()
    {
        string[] tokens = ["2", "1", "+", "3", "*"];
        Console.WriteLine(EvalRpn(tokens)); //输出9
    }
}