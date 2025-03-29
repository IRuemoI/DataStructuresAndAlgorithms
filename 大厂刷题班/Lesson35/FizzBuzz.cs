namespace AdvancedTraining.Lesson35;

//pass
public class FizzBuzz //leetcode_0412
{
    private static List<string> FizzBuzzCode(int n)
    {
        var ans = new List<string>();
        for (var i = 1; i <= n; i++)
            if (i % 15 == 0)
                ans.Add("FizzBuzz");
            else if (i % 5 == 0)
                ans.Add("Buzz");
            else if (i % 3 == 0)
                ans.Add("Fizz");
            else
                ans.Add(i.ToString());
        return ans;
    }

    public static void Run()
    {
        Console.WriteLine(string.Join(",", FizzBuzzCode(5))); //输出：["1","2","Fizz","4","Buzz"]
    }
}