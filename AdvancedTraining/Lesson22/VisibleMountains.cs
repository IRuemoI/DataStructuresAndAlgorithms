#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson22;

public class VisibleMountains
{
    private static int GetVisibleNum(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;
        var n = arr.Length;
        var maxIndex = 0;
        // 先在环中找到其中一个最大值的位置，哪一个都行
        for (var i = 0; i < n; i++) maxIndex = arr[maxIndex] < arr[i] ? i : maxIndex;
        var stack = new Stack<Record>();
        // 先把(最大值,1)这个记录放入stack中
        stack.Push(new Record(arr[maxIndex]));
        // 从最大值位置的下一个位置开始沿next方向遍历
        var index = NextIndex(maxIndex, n);
        // 用"小找大"的方式统计所有可见山峰对
        var res = 0;
        // 遍历阶段开始，当index再次回到maxIndex的时候，说明转了一圈，遍历阶段就结束
        while (index != maxIndex)
        {
            // 当前数要进入栈，判断会不会破坏第一维的数字从顶到底依次变大
            // 如果破坏了，就依次弹出栈顶记录，并计算山峰对数量
            while (stack.Peek().Value < arr[index])
            {
                var k = stack.Pop().Times;
                // 弹出记录为(X,K)，如果K==1，产生2对; 如果K>1，产生2*K + C(2,K)对。
                res += GetInternalSum(k) + 2 * k;
            }

            // 当前数字arr[index]要进入栈了，如果和当前栈顶数字一样就合并
            // 不一样就把记录(arr[index],1)放入栈中
            if (stack.Peek().Value == arr[index])
                stack.Peek().Times++;
            else
                // >
                stack.Push(new Record(arr[index]));
            index = NextIndex(index, n);
        }

        // 清算阶段开始了
        // 清算阶段的第1小阶段
        while (stack.Count > 2)
        {
            var times = stack.Pop().Times;
            res += GetInternalSum(times) + 2 * times;
        }

        // 清算阶段的第2小阶段
        if (stack.Count == 2)
        {
            var times = stack.Pop().Times;
            res += GetInternalSum(times) + (stack.Peek().Times == 1 ? times : 2 * times);
        }

        // 清算阶段的第3小阶段
        res += GetInternalSum(stack.Pop().Times);
        return res;
    }

    // 如果k==1返回0，如果k>1返回C(2,k)
    private static int GetInternalSum(int k)
    {
        return k == 1 ? 0 : k * (k - 1) / 2;
    }

    // 环形数组中当前位置为i，数组长度为size，返回i的下一个位置
    private static int NextIndex(int i, int size)
    {
        return i < size - 1 ? i + 1 : 0;
    }

    // 环形数组中当前位置为i，数组长度为size，返回i的上一个位置
    private static int LastIndex(int i, int size)
    {
        return i > 0 ? i - 1 : size - 1;
    }

    //用于测试, O(N^2)的解法，绝对正确
    private static int RightWay(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;
        var res = 0;
        var equalCounted = new HashSet<string>();
        for (var i = 0; i < arr.Length; i++)
            // 枚举从每一个位置出发，根据"小找大"原则能找到多少对儿，并且保证不重复找
            res += GetVisibleNumFromIndex(arr, i, equalCounted);
        return res;
    }

    //用于测试
    // 根据"小找大"的原则返回从index出发能找到多少对
    // 相等情况下，比如arr[1]==3，arr[5]==3
    // 之前如果从位置1找过位置5，那么等到从位置5出发时就不再找位置1（去重）
    // 之前找过的、所有相等情况的山峰对，都保存在了equalCounted中
    private static int GetVisibleNumFromIndex(int[] arr, int index, HashSet<string> equalCounted)
    {
        var res = 0;
        for (var i = 0; i < arr.Length; i++)
            if (i != index)
            {
                // 不找自己
                if (arr[i] == arr[index])
                {
                    var key = Math.Min(index, i) + "_" + Math.Max(index, i);
                    // 相等情况下，确保之前没找过这一对
                    if (equalCounted.Add(key) && IsVisible(arr, index, i)) res++;
                }
                else if (IsVisible(arr, index, i))
                {
                    // 不相等的情况下直接找
                    res++;
                }
            }

        return res;
    }

    //用于测试
    // 调用该函数的前提是，lowIndex和highIndex一定不是同一个位置
    // 在"小找大"的策略下，从lowIndex位置能不能看到highIndex位置
    // next方向或者last方向有一个能走通，就返回true，否则返回false
    private static bool IsVisible(int[] arr, int lowIndex, int highIndex)
    {
        if (arr[lowIndex] > arr[highIndex])
            // "大找小"的情况直接返回false
            return false;
        var size = arr.Length;
        var walkNext = true;
        var mid = NextIndex(lowIndex, size);
        // lowIndex通过next方向走到highIndex，沿途不能出现比arr[lowIndex]大的数
        while (mid != highIndex)
        {
            if (arr[mid] > arr[lowIndex])
            {
                walkNext = false; // next方向失败
                break;
            }

            mid = NextIndex(mid, size);
        }

        var walkLast = true;
        mid = LastIndex(lowIndex, size);
        // lowIndex通过last方向走到highIndex，沿途不能出现比arr[lowIndex]大的数
        while (mid != highIndex)
        {
            if (arr[mid] > arr[lowIndex])
            {
                walkLast = false; // last方向失败
                break;
            }

            mid = LastIndex(mid, size);
        }

        return walkNext || walkLast; // 有一个成功就是能相互看见
    }

    //用于测试
    private static int[] GetRandomArray(int size, int max)
    {
        var arr = new int[(int)(Utility.GetRandomDouble * size)];
        for (var i = 0; i < arr.Length; i++) arr[i] = (int)(Utility.GetRandomDouble * max);
        return arr;
    }

    //用于测试
    private static void PrintArray(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return;
        foreach (var item in arr)
            Console.Write(item + " ");

        Console.WriteLine();
    }

    public static void Run()
    {
        const int size = 10;
        const int max = 10;
        const int testTimes = 3000000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTimes; i++)
        {
            var arr = GetRandomArray(size, max);
            if (RightWay(arr) != GetVisibleNum(arr))
            {
                PrintArray(arr);
                Console.WriteLine(RightWay(arr));
                Console.WriteLine(GetVisibleNum(arr));
                break;
            }
        }

        Console.WriteLine("测试结束");
    }

    // 栈中放的记录，
    // value就是指，times是收集的个数
    private class Record(int value)
    {
        public readonly int Value = value;
        public int Times = 1;
    }
}