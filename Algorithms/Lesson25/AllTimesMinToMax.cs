//通过

namespace Algorithms.Lesson25;

public class AllTimesMinToMax
{
    private static int Max1(int[] arr)
    {
        var max = int.MinValue;
        for (var i = 0; i < arr.Length; i++)
        for (var j = i; j < arr.Length; j++)
        {
            var minNum = int.MaxValue;
            var sum = 0;
            for (var k = i; k <= j; k++)
            {
                sum += arr[k];
                minNum = Math.Min(minNum, arr[k]);
            }

            max = Math.Max(max, minNum * sum);
        }

        return max;
    }

    private static int Max2(int[] arr)
    {
        var size = arr.Length;
        var sums = new int[size];
        sums[0] = arr[0];
        for (var i = 1; i < size; i++) sums[i] = sums[i - 1] + arr[i];

        var max = int.MinValue;
        var stack = new Stack<int>();
        for (var i = 0; i < size; i++)
        {
            while (stack.Count != 0 && arr[stack.Peek()] >= arr[i])
            {
                var j = stack.Pop();
                max = Math.Max(max, (stack.Count == 0 ? sums[i - 1] : sums[i - 1] - sums[stack.Peek()]) * arr[j]);
            }

            stack.Push(i);
        }

        while (stack.Count != 0)
        {
            var j = stack.Pop();
            max = Math.Max(max, (stack.Count == 0 ? sums[size - 1] : sums[size - 1] - sums[stack.Peek()]) * arr[j]);
        }

        return max;
    }

    private static int[] GenerateRandomArray()
    {
        var arr = new int[(int)(new Random().NextDouble() * 20) + 10];
        for (var i = 0; i < arr.Length; i++) arr[i] = (int)(new Random().NextDouble() * 101);

        return arr;
    }

    public static void Run()
    {
        var testTimes = 2000000;
        Console.WriteLine("开始测试");
        for (var i = 0; i < testTimes; i++)
        {
            var arr = GenerateRandomArray();
            if (Max1(arr) != Max2(arr))
            {
                Console.WriteLine("出错了！!");
                break;
            }
        }

        Console.WriteLine("测试完成");
    }
}