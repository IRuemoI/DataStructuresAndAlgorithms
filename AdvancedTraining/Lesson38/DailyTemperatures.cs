namespace AdvancedTraining.Lesson38;

public class DailyTemperatures //Problem_0739
{
    private static int[] DailyTemperaturesCode(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return [];

        var n = arr.Length;
        var ans = new int[n];
        var stack = new Stack<List<int>>();

        for (var i = 0; i < n; i++)
        {
            while (stack.Count > 0 && arr[stack.Peek()[0]] < arr[i])
            {
                var popIs = stack.Pop();
                foreach (var popI in popIs) ans[popI] = i - popI;
            }

            if (stack.Count > 0 && arr[stack.Peek()[0]] == arr[i])
            {
                stack.Peek().Add(i);
            }
            else
            {
                var list = new List<int> { i };
                stack.Push(list);
            }
        }

        return ans;
    }

    public static void Run()
    {
        Console.WriteLine(string.Join(",",
            DailyTemperaturesCode([73, 74, 75, 71, 69, 72, 76, 73]))); //输出: [1,1,4,2,1,1,0,0]
    }
}