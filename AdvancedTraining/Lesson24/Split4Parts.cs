#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson24;

public class Split4Parts
{
    private static bool CanSplits1(int[]? arr)
    {
        if (arr == null || arr.Length < 7) return false;
        var set = new HashSet<string>();
        var sum = 0;
        foreach (var item in arr)
            sum += item;

        var leftSum = arr[0];
        for (var i = 1; i < arr.Length - 1; i++)
        {
            set.Add(leftSum + "_" + (sum - leftSum - arr[i]));
            leftSum += arr[i];
        }

        var l = 1;
        var lSum = arr[0];
        var r = arr.Length - 2;
        var rSum = arr[^1];
        while (l < r - 3)
            if (lSum == rSum)
            {
                var lkey = (lSum * 2 + arr[l]).ToString();
                var rkey = (rSum * 2 + arr[r]).ToString();
                if (set.Contains(lkey + "_" + rkey)) return true;
                lSum += arr[l++];
            }
            else if (lSum < rSum)
            {
                lSum += arr[l++];
            }
            else
            {
                rSum += arr[r--];
            }

        return false;
    }

    private static bool CanSplits2(int[]? arr)
    {
        if (arr == null || arr.Length < 7) return false;
        // key 某一个累加和， value出现的位置
        var map = new Dictionary<int, int>();
        var sum = arr[0];
        for (var i = 1; i < arr.Length; i++)
        {
            map[sum] = i;
            sum += arr[i];
        }

        var lSum = arr[0]; // 第一刀左侧的累加和
        for (var s1 = 1; s1 < arr.Length - 5; s1++)
        {
            // s1是第一刀的位置
            var checkSum = lSum * 2 + arr[s1]; // 100 x 100   100*2 + x
            if (map.TryGetValue(checkSum, out var s2))
            {
                checkSum += lSum + arr[s2];
                if (map.TryGetValue(checkSum, out var s3))
                    // 100 * 3 + x + y
                    if (checkSum + arr[s3] + lSum == sum)
                        return true;
            }

            lSum += arr[s1];
        }

        return false;
    }

    private static int[] GenerateRondomArray()
    {
        var res = new int[(int)(Utility.GetRandomDouble * 10) + 7];
        for (var i = 0; i < res.Length; i++) res[i] = (int)(Utility.GetRandomDouble * 10) + 1;
        return res;
    }

    public static void Run()
    {
        Console.WriteLine("测试开始");
        var testTime = 3000000;
        for (var i = 0; i < testTime; i++)
        {
            var arr = GenerateRondomArray();
            if (CanSplits1(arr) ^ CanSplits2(arr)) Console.WriteLine("Error");
        }

        Console.WriteLine("测试结束");
    }
}