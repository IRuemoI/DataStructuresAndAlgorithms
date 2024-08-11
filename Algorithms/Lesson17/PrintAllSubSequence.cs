//测试通过

namespace Algorithms.Lesson17;

/// <summary>
///     打印字符串的所有子序列
/// </summary>
public class PrintAllSubSequence
{
    // s -> "abc" ->
    private static List<string> Subs(string s)
    {
        var str = s.ToCharArray();
        var path = "";
        List<string> ans = new();
        Process1(str, 0, ans, path);
        return ans;
    }

    // str 固定参数
    // 来到了str[index]字符，index是位置
    // str[0..index-1]已经走过了！之前的决定，都在path上
    // 之前的决定已经不能改变了，就是path
    // str[index....]还能决定，之前已经确定，而后面还能自由选择的话，
    // 把所有生成的子序列，放入到ans里去
    private static void Process1(char[] str, int index, List<string> ans, string path)
    {
        if (index == str.Length)
        {
            ans.Add(path);
            return;
        }

        // 没有要index位置的字符
        Process1(str, index + 1, ans, path);
        // 要了index位置的字符
        Process1(str, index + 1, ans, path + str[index]);
    }

    private static List<string> SubsNoRepeat(string s)
    {
        var str = s.ToCharArray();
        var path = "";
        HashSet<string> set = new();
        Process2(str, 0, set, path);
        List<string> ans = new();
        foreach (var cur in set) ans.Add(cur);

        return ans;
    }

    private static void Process2(char[] str, int index, HashSet<string> set, string path)
    {
        if (index == str.Length)
        {
            set.Add(path);
            return;
        }

        var no = path;
        Process2(str, index + 1, set, no);
        var yes = path + str[index];
        Process2(str, index + 1, set, yes);
    }

    public static void Run()
    {
        var test = "orange";
        var ans1 = Subs(test);
        var ans2 = SubsNoRepeat(test);

        foreach (var str in ans1) Console.WriteLine(str);

        Console.WriteLine("=================");
        foreach (var str in ans2) Console.WriteLine(str);

        Console.WriteLine("=================");
    }
}