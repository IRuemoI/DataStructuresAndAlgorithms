//测试通过

namespace Algorithms.Lesson25;

public class MonotonousStack
{
    // arr = [ 3, 1, 2, 3]
    //         0  1  2  3
    //  [
    //     0 : [-1,  1]
    //     1 : [-1, -1]
    //     2 : [ 1, -1]
    //     3 : [ 2, -1]
    //  ]
    private static int[,] GetNearLessNoRepeat(int[] arr)
    {
        var res = new int[arr.Length, 2];
        // 只存位置！
        Stack<int> stack = new();
        for (var i = 0; i < arr.Length; i++)
        {
            // 当遍历到i位置的数，arr[i]
            while (stack.Count != 0 && arr[stack.Peek()] > arr[i])
            {
                var j = stack.Pop();
                var leftLessIndex = stack.Count == 0 ? -1 : stack.Peek();
                res[j, 0] = leftLessIndex;
                res[j, 1] = i;
            }

            stack.Push(i);
        }

        while (stack.Count != 0)
        {
            var j = stack.Pop();
            var leftLessIndex = stack.Count == 0 ? -1 : stack.Peek();
            res[j, 0] = leftLessIndex;
            res[j, 1] = -1;
        }

        return res;
    }

    private static int[,] GetNearLess(int[] arr)
    {
        var res = new int[arr.Length, 2];
        Stack<List<int>> stack = new();
        for (var i = 0; i < arr.Length; i++)
        {
            // i -> arr[i] 进栈
            while (stack.Count != 0 && arr[stack.Peek()[0]] > arr[i])
            {
                var popIs = stack.Pop();
                var leftLessIndex = stack.Count == 0 ? -1 : stack.Peek()[stack.Peek().Count - 1];
                foreach (var popI in popIs)
                {
                    res[popI, 0] = leftLessIndex;
                    res[popI, 1] = i;
                }
            }

            if (stack.Count != 0 && arr[stack.Peek()[0]] == arr[i])
            {
                stack.Peek().Add(i);
            }
            else
            {
                List<int> list = new() { i };
                stack.Push(list);
            }
        }

        while (stack.Count != 0)
        {
            var popIs = stack.Pop();
            var leftLessIndex = stack.Count == 0 ? -1 : stack.Peek()[stack.Peek().Count - 1];
            foreach (var popI in popIs)
            {
                res[popI, 0] = leftLessIndex;
                res[popI, 1] = -1;
            }
        }

        return res;
    }

    //用于测试
    private static int[] GetRandomArrayNoRepeat(int size)
    {
        var arr = new int[(int)(new Random().NextDouble() * size) + 1];
        for (var i = 0; i < arr.Length; i++) arr[i] = i;

        for (var i = 0; i < arr.Length; i++)
        {
            var swapIndex = (int)(new Random().NextDouble() * arr.Length);
            (arr[swapIndex], arr[i]) = (arr[i], arr[swapIndex]);
        }

        return arr;
    }

    //用于测试
    private static int[] GetRandomArray(int size, int max)
    {
        var arr = new int[(int)(new Random().NextDouble() * size) + 1];
        for (var i = 0; i < arr.Length; i++)
            arr[i] = (int)(new Random().NextDouble() * max) - (int)(new Random().NextDouble() * max);

        return arr;
    }

    //用于测试
    private static int[,] RightWay(int[] arr)
    {
        var res = new int[arr.Length, 2];
        for (var i = 0; i < arr.Length; i++)
        {
            var leftLessIndex = -1;
            var rightLessIndex = -1;
            var cur = i - 1;
            while (cur >= 0)
            {
                if (arr[cur] < arr[i])
                {
                    leftLessIndex = cur;
                    break;
                }

                cur--;
            }

            cur = i + 1;
            while (cur < arr.Length)
            {
                if (arr[cur] < arr[i])
                {
                    rightLessIndex = cur;
                    break;
                }

                cur++;
            }

            res[i, 0] = leftLessIndex;
            res[i, 1] = rightLessIndex;
        }

        return res;
    }

    //用于测试
    private static bool IsEqual(int[,] res1, int[,] res2)
    {
        if (res1.GetLength(0) != res2.GetLength(0)) return false;

        for (var i = 0; i < res1.GetLength(0); i++)
            if (res1[i, 0] != res2[i, 0] || res1[i, 1] != res2[i, 1])
                return false;

        return true;
    }

    //用于测试
    private static void PrintArray(int[] arr)
    {
        foreach (var element in arr) Console.Write(element + " ");

        Console.WriteLine();
    }

    public static void Run()
    {
        var size = 10;
        var max = 20;
        var testTimes = 2000000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTimes; i++)
        {
            var arr1 = GetRandomArrayNoRepeat(size);
            var arr2 = GetRandomArray(size, max);
            if (!IsEqual(GetNearLessNoRepeat(arr1), RightWay(arr1)))
            {
                Console.WriteLine("出错啦！");
                PrintArray(arr1);
                break;
            }

            if (!IsEqual(GetNearLess(arr2), RightWay(arr2)))
            {
                Console.WriteLine("出错啦！");
                PrintArray(arr2);
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}