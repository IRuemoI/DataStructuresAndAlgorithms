namespace AdvancedTraining.Lesson28;

public class ValidParentheses //Problem_0020
{
    private static bool IsValid(string s)
    {
        if (ReferenceEquals(s, null) || s.Length == 0) return true;
        var str = s.ToCharArray();
        var n = str.Length;
        var stack = new char[n];
        var size = 0;
        for (var i = 0; i < n; i++)
        {
            var cha = str[i];
            if (cha == '(' || cha == '[' || cha == '{')
            {
                stack[size++] = cha == '(' ? ')' : cha == '[' ? ']' : '}';
            }
            else
            {
                if (size == 0) return false;
                var last = stack[--size];
                if (cha != last) return false;
            }
        }

        return size == 0;
    }

    public static void Run()
    {
        Console.WriteLine(IsValid("()[]{}"));

        Console.WriteLine(IsValid("(]"));
    }
}