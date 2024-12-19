namespace Algorithms.Lesson17;

/// <summary>
///     逆序一个栈，只能使用递归，不能使用额外的数据结构
/// </summary>
public class ReverseStackUsingRecursive
{
    private static void Reverse(Stack<int> stack)
    {
        if (stack.Count == 0) return;

        var i = Process(stack);
        Reverse(stack);
        stack.Push(i);
    }

    // 栈底元素移除掉
    // 上面的元素盖下来
    // 返回移除掉的栈底元素
    private static int Process(Stack<int> stack)
    {
        var result = stack.Pop();
        if (stack.Count == 0) return result;

        var last = Process(stack);
        stack.Push(result);
        return last;
    }

    public static void Run()
    {
        var test = new Stack<int>();
        test.Push(1);
        test.Push(2);
        test.Push(3);
        test.Push(4);
        test.Push(5);
        Reverse(test);
        while (test.Count > 0) Console.WriteLine(test.Pop());
    }
}