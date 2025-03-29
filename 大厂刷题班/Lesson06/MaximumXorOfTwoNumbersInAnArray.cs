//pass

namespace AdvancedTraining.Lesson06;

// 本题测试链接 : https://leetcode.cn/problems/maximum-xor-of-two-numbers-in-an-array/
public class MaximumXorOfTwoNumbersInAnArray
{
    private static int FindMaximumXor(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;
        var max = int.MinValue;
        var numTrie = new NumTrie();
        numTrie.Add(arr[0]);
        for (var i = 1; i < arr.Length; i++)
        {
            max = Math.Max(max, numTrie.MaxXor(arr[i]));
            numTrie.Add(arr[i]);
        }

        return max;
    }


    public static void Run()
    {
        Console.WriteLine(FindMaximumXor([14, 70, 53, 83, 49, 91, 36, 80, 92, 51, 66, 70])); //输出127
    }

    private class Node
    {
        public readonly Node?[] NextList = new Node[2];
    }

    private class NumTrie
    {
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

        public int MaxXor(int sum)
        {
            var cur = _head;
            var res = 0;
            for (var move = 31; move >= 0; move--)
            {
                var path = (sum >> move) & 1;
                var best = move == 31 ? path : path ^ 1;
                best = cur?.NextList[best] != null ? best : best ^ 1;
                res |= (path ^ best) << move;
                cur = cur?.NextList[best];
            }

            return res;
        }
    }
}