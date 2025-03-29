//pass

namespace AdvancedTraining.Lesson30;

public class PascalTriangle //leetcode_0118
{
    private static List<List<int>> Generate(int numRows)
    {
        var ans = new List<List<int>>();
        for (var i = 0; i < numRows; i++)
        {
            ans.Add(new List<int>());
            ans[i].Add(1);
        }

        for (var i = 1; i < numRows; i++)
        {
            for (var j = 1; j < i; j++) ans[i].Add(ans[i - 1][j - 1] + ans[i - 1][j]);
            ans[i].Add(1);
        }

        return ans;
    }

    public static void Run()
    {
        var result = Generate(5);
        foreach (var list in result) Console.WriteLine(string.Join(" ", list));
    }
}