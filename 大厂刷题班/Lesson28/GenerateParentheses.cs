//pass
namespace AdvancedTraining.Lesson28;

public class GenerateParentheses //leetcode_0022
{
    private static IList<string> GenerateParenthesis(int n)
    {
        var path = new char[n << 1];
        IList<string> ans = new List<string>();
        Process(path, 0, 0, n, ans);
        return ans;
    }


    // path 做的决定  path[0....index-1]做完决定的！
    // path[index.....] 还没做决定，当前轮到index位置做决定！

    private static void Process(char[] path, int index, int leftMinusRight, int leftRest, IList<string> ans)
    {
        if (index == path.Length)
        {
            ans.Add(new string(path));
        }
        else
        {
            // index (   )
            if (leftRest > 0)
            {
                path[index] = '(';
                Process(path, index + 1, leftMinusRight + 1, leftRest - 1, ans);
            }

            if (leftMinusRight > 0)
            {
                path[index] = ')';
                Process(path, index + 1, leftMinusRight - 1, leftRest, ans);
            }
        }
    }

    // 不剪枝的做法
    private static IList<string> GenerateParenthesis2(int n)
    {
        var path = new char[n << 1];
        IList<string> ans = new List<string>();
        Process2(path, 0, ans);
        return ans;
    }

    private static void Process2(char[] path, int index, IList<string> ans)
    {
        if (index == path.Length)
        {
            if (IsValid(path)) ans.Add(new string(path));
        }
        else
        {
            path[index] = '(';
            Process2(path, index + 1, ans);
            path[index] = ')';
            Process2(path, index + 1, ans);
        }
    }

    private static bool IsValid(char[] path)
    {
        var count = 0;
        foreach (var cha in path)
        {
            if (cha == '(')
                count++;
            else
                count--;
            if (count < 0) return false;
        }

        return count == 0;
    }

    public static void Run()
    {
        var ans = GenerateParenthesis(3);
        foreach (var s in ans) Console.Write(s + ",");

        Console.WriteLine("\n-------------");

        ans = GenerateParenthesis2(3);
        foreach (var s in ans) Console.Write(s + ",");
    }
}