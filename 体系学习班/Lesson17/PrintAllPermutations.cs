namespace Algorithms.Lesson17;

/// <summary>
///     打印字符串的全排列
/// </summary>
public class PrintAllPermutations
{
    private static List<string> Permutation1(string s)
    {
        List<string> ans = new();
        if (string.IsNullOrEmpty(s)) return ans;

        var str = s.ToCharArray();
        var rest = new List<char>();
        foreach (var cha in str) rest.Add(cha);

        var path = "";
        Process1(rest, path, ans);
        return ans;
    }

    private static void Process1(List<char> rest, string path, List<string> ans)
    {
        if (rest.Count == 0)
        {
            ans.Add(path);
        }
        else
        {
            var n = rest.Count;
            for (var i = 0; i < n; i++)
            {
                var cur = rest[i];
                //回复现场
                rest.RemoveAt(i);
                Process1(rest, path + cur, ans);
                rest.Insert(i, cur);
            }
        }
    }

    private static List<string> Permutation2(string s)
    {
        List<string> ans = new();
        if (string.IsNullOrEmpty(s)) return ans;

        var str = s.ToCharArray();
        Process2(str, 0, ans);
        return ans;
    }

    private static void Process2(char[] str, int index, List<string> ans)
    {
        if (index == str.Length)
            ans.Add(new string(str));
        else
            for (var i = index; i < str.Length; i++)
            {
                Swap(str, index, i);
                Process2(str, index + 1, ans);
                Swap(str, index, i);
            }
    }

    private static List<string> Permutation3(string s)
    {
        List<string> ans = new();
        if (string.IsNullOrEmpty(s)) return ans;

        var str = s.ToCharArray();
        Process3(str, 0, ans);
        return ans;
    }

    private static void Process3(char[] str, int index, List<string> ans)
    {
        if (index == str.Length)
        {
            ans.Add(new string(str));
        }
        else
        {
            var visited = new bool[256];
            for (var i = index; i < str.Length; i++)
                if (!visited[str[i]]) //去重，一种剪枝思想
                {
                    visited[str[i]] = true;
                    Swap(str, index, i);
                    Process3(str, index + 1, ans);
                    Swap(str, index, i);
                }
        }
    }

    private static void Swap(char[] chs, int i, int j)
    {
        (chs[i], chs[j]) = (chs[j], chs[i]);
    }

    public static void Run()
    {
        var s = "old";
        var ans1 = Permutation1(s);
        foreach (var str in ans1) Console.WriteLine(str);

        Console.WriteLine("=======");
        var ans2 = Permutation2(s);
        foreach (var str in ans2) Console.WriteLine(str);

        Console.WriteLine("=======");
        var ans3 = Permutation3(s);
        foreach (var str in ans3) Console.WriteLine(str);
    }
}