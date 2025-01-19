//pass
#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson06;

public class MaxXor
{
    // O(N^2)
    private static int MaxXorSubArray1(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        // 准备一个前缀异或和数组eor
        // eor[i] = arr[0...i]的异或结果
        var eor = new int[arr.Length];
        eor[0] = arr[0];
        // 生成eor数组，eor[i]代表arr[0..i]的异或和
        for (var i = 1; i < arr.Length; i++) eor[i] = eor[i - 1] ^ arr[i];
        var max = int.MinValue;
        for (var j = 0; j < arr.Length; j++)
        for (var i = 0; i <= j; i++)
            // 依次尝试arr[0..j]、arr[1..j]..arr[i..j]..arr[j..j]
            max = Math.Max(max, i == 0 ? eor[j] : eor[j] ^ eor[i - 1]);
        return max;
    }

    // O(N)
    private static int MaxXorSubArray2(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        var max = int.MinValue;
        // 0~i整体异或和
        var xor = 0;
        var numTrie = new NumTrie();
        numTrie.Add(0);
        foreach (var item in arr)
        {
            xor ^= item; // 0 ~ i
            max = Math.Max(max, numTrie.MaxXor(xor));
            numTrie.Add(xor);
        }

        return max;
    }

    //用于测试
    private static int[] GetRandomStringArray(int maxSize, int maxValue)
    {
        var arr = new int[(int)((maxSize + 1) * Utility.getRandomDouble)];
        for (var i = 0; i < arr.Length; i++)
            arr[i] = (int)((maxValue + 1) * Utility.getRandomDouble) - (int)(maxValue * Utility.getRandomDouble);
        return arr;
    }

    //用于测试
    private static void PrintArray(int[]? arr)
    {
        if (arr == null) return;
        foreach (var item in arr) Console.Write(item + " ");
        Console.WriteLine();
    }

    //用于测试
    public static void Run()
    {
        const int testTime = 5000;
        const int maxSize = 30;
        const int maxValue = 50;
        for (var i = 0; i < testTime; i++)
        {
            var arr = GetRandomStringArray(maxSize, maxValue);
            var comp = MaxXorSubArray1(arr);
            var res = MaxXorSubArray2(arr);
            if (res != comp)
            {
                Console.WriteLine("出错了");
                PrintArray(arr);
                Console.WriteLine(res);
                Console.WriteLine(comp);
                break;
            }
        }

        Console.WriteLine("测试通过");
    }

    // 前缀树的Node结构
    // nexts[0] -> 0方向的路
    // nexts[1] -> 1方向的路
    // nexts[0] == null 0方向上没路！
    // nexts[0] != null 0方向有路，可以跳下一个节点
    // nexts[1] == null 1方向上没路！
    // nexts[1] != null 1方向有路，可以跳下一个节点
    private class Node
    {
        public readonly Node?[] NextList = new Node[2];
    }

    // 基于本题，定制前缀树的实现
    private class NumTrie
    {
        // 头节点
        private readonly Node _head = new();

        public void Add(int newNum)
        {
            var cur = _head;
            for (var move = 31; move >= 0; move--)
            {
                var path = (newNum >> move) & 1;
                if (cur != null)
                {
                    cur.NextList[path] = cur.NextList[path] == null ? new Node() : cur.NextList[path];
                    cur = cur.NextList[path];
                }
            }
        }

        // 该结构之前收集了一票数字，并且建好了前缀树
        // num和 谁 ^ 最大的结果（把结果返回）
        public int MaxXor(int num)
        {
            var cur = _head;
            var ans = 0;
            for (var move = 31; move >= 0; move--)
            {
                // 取出num中第move位的状态，path只有两种值0就1，整数
                var path = (num >> move) & 1;
                // 期待遇到的东西
                var best = move == 31 ? path : path ^ 1;
                // 实际遇到的东西
                best = cur?.NextList[best] != null ? best : best ^ 1;
                // (path ^ best) 当前位位异或完的结果
                ans |= (path ^ best) << move;
                cur = cur?.NextList[best];
            }

            return ans;
        }
    }
}