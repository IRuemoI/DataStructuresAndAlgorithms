//pass
#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson01;

public class CordCoverMaxPoint
{
    private static int MaxPoint1(int[] arr, int l)
    {
        var res = 1;
        for (var i = 0; i < arr.Length; i++)
        {
            var nearest = NearestIndex(arr, i, arr[i] - l);
            res = Math.Max(res, i - nearest + 1);
        }

        return res;
    }

    private static int NearestIndex(int[] arr, int r, int value)
    {
        var l = 0;
        var index = r;
        while (l <= r)
        {
            var mid = l + ((r - l) >> 1);
            if (arr[mid] >= value)
            {
                index = mid;
                r = mid - 1;
            }
            else
            {
                l = mid + 1;
            }
        }

        return index;
    }

    private static int MaxPoint2(int[] arr, int l)
    {
        var left = 0;
        var right = 0;
        var n = arr.Length;
        var max = 0;
        while (left < n)
        {
            while (right < n && arr[right] - arr[left] <= l) right++;
            max = Math.Max(max, right - left++);
        }

        return max;
    }

    //用于测试
    private static int Test(int[] arr, int l)
    {
        var max = 0;
        for (var i = 0; i < arr.Length; i++)
        {
            var pre = i - 1;
            while (pre >= 0 && arr[i] - arr[pre] <= l) pre--;
            max = Math.Max(max, i - pre);
        }

        return max;
    }

    //用于测试
    private static int[] GenerateArray(int len, int max)
    {
        var ans = new int[(int)(Utility.getRandomDouble * len) + 1];
        for (var i = 0; i < ans.Length; i++) ans[i] = (int)(Utility.getRandomDouble * max);
        Array.Sort(ans);
        return ans;
    }

    public static void Run()
    {
        var len = 100;
        var max = 1000;
        var testTime = 100000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var l = (int)(Utility.getRandomDouble * max);
            var arr = GenerateArray(len, max);
            var ans1 = MaxPoint1(arr, l);
            var ans2 = MaxPoint2(arr, l);
            var ans3 = Test(arr, l);
            if (ans1 != ans2 || ans2 != ans3)
            {
                Console.WriteLine("出错啦！");
                break;
            }
        }
    }
}