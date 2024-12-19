//pass
namespace AdvancedTraining.Lesson30;

public class PascalTriangleIi //leetcode_0119
{
    private static IList<int> GetRow(int rowIndex)
    {
        var ans = new List<int>();
        for (var i = 0; i <= rowIndex; i++)
        {
            for (var j = i - 1; j > 0; j--) ans[j] = ans[j - 1] + ans[j];
            ans.Add(1);
        }

        return ans;
    }

    public static void Run()
    {
        Console.WriteLine(string.Join(",", GetRow(3))); //输出[1,3,3,1]
    }
}