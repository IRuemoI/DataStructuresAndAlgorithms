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
        var res = new int[arr.Length, 2]; //第一个位置存左侧比它小的下标，第二个位置存右侧比他小的下标
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
        // 初始化结果数组：[N,0]存左侧最近更小索引，[N,1]存右侧最近更小索引 
        var res = new int[arr.Length, 2];
        // 单调栈：栈中元素是按值升序排列的索引列表，列表用于处理重复值 
        Stack<List<int>> stack = new();

        // 正向遍历数组元素 
        for (var i = 0; i < arr.Length; i++)
        {
            // 维护单调性：弹出所有比当前元素大的栈顶元素 
            while (stack.Count != 0 && arr[stack.Peek()[0]] > arr[i])
            {
                var popIs = stack.Pop(); // 获取待处理的索引列表 
                // 计算左侧最近更小值索引：栈空则为-1，否则取新栈顶最后一个元素 
                var leftLessIndex = stack.Count == 0 ? -1 : stack.Peek()[^1];

                // 为弹出的所有索引设置结果 
                foreach (var popI in popIs)
                {
                    res[popI, 0] = leftLessIndex; // 左侧更小索引 
                    res[popI, 1] = i; // 右侧更小索引即当前索引i 
                }
            }

            // 处理相等值：将当前索引加入已有列表，维持相同值的索引连续存储 
            if (stack.Count != 0 && arr[stack.Peek()[0]] == arr[i])
            {
                stack.Peek().Add(i);
            }
            // 处理新较小值：创建新列表压入栈 
            else
            {
                List<int> list = [i]; // C# 12集合表达式初始化 
                stack.Push(list);
            }
        }

        // 处理栈中剩余元素（右侧无更小值的情况）
        while (stack.Count != 0)
        {
            var popIs = stack.Pop();
            // 左侧更小索引处理逻辑与循环中相同 
            var leftLessIndex = stack.Count == 0 ? -1 : stack.Peek()[^1];
            foreach (var popI in popIs)
            {
                res[popI, 0] = leftLessIndex;
                res[popI, 1] = -1; // 右侧无更小值设为-1 
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
        var testTimes = 2000;
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