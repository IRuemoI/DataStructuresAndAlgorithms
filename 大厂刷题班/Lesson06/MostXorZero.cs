//pass
#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson06;

public class MostXorZero
{
    // 暴力方法
    private static int Comparator(int[] arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        var N = arr.Length;
        var eor = new int[N];
        eor[0] = arr[0];
        for (var i = 1; i < N; i++) eor[i] = eor[i - 1] ^ arr[i];
        return Process(eor, 1, new List<int>());
    }

    // index去决定：前一坨部分，结不结束！
    // 如果结束！就把index放入到parts里去
    // 如果不结束，就不放
    private static int Process(int[] eor, int index, List<int> parts)
    {
        var ans = 0;
        if (index == eor.Length)
        {
            parts.Add(eor.Length);
            ans = EorZeroParts(eor, parts);
            parts.RemoveAt(parts.Count - 1);
        }
        else
        {
            var p1 = Process(eor, index + 1, parts);
            parts.Add(index);
            var p2 = Process(eor, index + 1, parts);
            parts.RemoveAt(parts.Count - 1);
            ans = Math.Max(p1, p2);
        }

        return ans;
    }

    private static int EorZeroParts(int[] eor, List<int> parts)
    {
        var L = 0;
        var ans = 0;
        foreach (var end in parts)
        {
            if ((eor[end - 1] ^ (L == 0 ? 0 : eor[L - 1])) == 0) ans++;
            L = end;
        }

        return ans;
    }

    // 时间复杂度O(N)的方法
    private static int MostXor(int[] arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        var N = arr.Length;
        var dp = new int[N];

        // key 某一个前缀异或和
        // value 这个前缀异或和上次出现的位置(最晚！)
        var map = new Dictionary<int, int>();
        map[0] = -1;
        // 0~i整体的异或和
        var xor = 0;
        for (var i = 0; i < N; i++)
        {
            xor ^= arr[i];
            if (map.ContainsKey(xor))
            {
                // 可能性2
                var pre = map[xor];
                dp[i] = pre == -1 ? 1 : dp[pre] + 1;
            }

            if (i > 0) dp[i] = Math.Max(dp[i - 1], dp[i]);
            map[xor] = i;
        }

        return dp[N - 1];
    }

    //用于测试
    private static int[] GetRandomStringArray(int maxSize, int maxValue)
    {
        var arr = new int[(int)((maxSize + 1) * Utility.getRandomDouble)];
        for (var i = 0; i < arr.Length; i++) arr[i] = (int)((maxValue + 1) * Utility.getRandomDouble);
        return arr;
    }

    //用于测试
    private static void PrintArray(int[] arr)
    {
        if (arr == null) return;
        for (var i = 0; i < arr.Length; i++) Console.Write(arr[i] + " ");
        Console.WriteLine();
    }

    //用于测试
    public static void Run()
    {
        var testTime = 5000;
        var maxSize = 12;
        var maxValue = 5;
        var succeed = true;
        for (var i = 0; i < testTime; i++)
        {
            var arr = GetRandomStringArray(maxSize, maxValue);
            var res = MostXor(arr);
            var comp = Comparator(arr);
            if (res != comp)
            {
                succeed = false;
                PrintArray(arr);
                Console.WriteLine(res);
                Console.WriteLine(comp);
                break;
            }
        }

        Console.WriteLine(succeed ? "测试通过" : "出现错误");
    }
}