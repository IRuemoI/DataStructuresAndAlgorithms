#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson16;

public class MergeRecord
{
    /*
     * 腾讯原题
     *
     * 给定整数power，给定一个数组arr，给定一个数组reverse。含义如下：
     * arr的长度一定是2的power次方，reverse中的每个值一定都在0~power范围。
     * 例如power = 2, arr = {3, 1, 4, 2}，reverse = {0, 1, 0, 2}
     * 任何一个在前的数字可以和任何一个在后的数组，构成一对数。可能是升序关系、相等关系或者降序关系。
     * 比如arr开始时有如下的降序对：(3,1)、(3,2)、(4,2)，一共3个。
     * 接下来根据reverse对arr进行调整：
     * reverse[0] = 0, 表示在arr中，划分每1(2的0次方)个数一组，然后每个小组内部逆序，那么arr变成
     * [3,1,4,2]，此时有3个逆序对。
     * reverse[1] = 1, 表示在arr中，划分每2(2的1次方)个数一组，然后每个小组内部逆序，那么arr变成
     * [1,3,2,4]，此时有1个逆序对
     * reverse[2] = 0, 表示在arr中，划分每1(2的0次方)个数一组，然后每个小组内部逆序，那么arr变成
     * [1,3,2,4]，此时有1个逆序对。
     * reverse[3] = 2, 表示在arr中，划分每4(2的2次方)个数一组，然后每个小组内部逆序，那么arr变成
     * [4,2,3,1]，此时有5个逆序对。
     * 所以返回[3,1,1,5]，表示每次调整之后的逆序对数量。
     *
     * 输入数据状况：
     * power的范围[0,20]
     * arr长度范围[1,10的7次方]
     * reverse长度范围[1,10的6次方]
     *
     * */

    private static int[] reversePair1(int[]? originArr, int[]? reverseArr, int power)
    {
        if (originArr == null || reverseArr == null) throw new Exception();
        var ans = new int[reverseArr.Length];
        for (var i = 0; i < reverseArr.Length; i++)
        {
            ReverseArray(originArr, 1 << reverseArr[i]);
            ans[i] = CountReversePair(originArr);
        }

        return ans;
    }

    private static void ReverseArray(int[] originArr, int teamSize)
    {
        if (teamSize < 2) return;
        for (var i = 0; i < originArr.Length; i += teamSize) ReversePart(originArr, i, i + teamSize - 1);
    }

    private static void ReversePart(int[] arr, int l, int r)
    {
        while (l < r)
        {
            var tmp = arr[l];
            arr[l++] = arr[r];
            arr[r--] = tmp;
        }
    }

    private static int CountReversePair(int[] originArr)
    {
        var ans = 0;
        for (var i = 0; i < originArr.Length; i++)
        for (var j = i + 1; j < originArr.Length; j++)
            if (originArr[i] > originArr[j])
                ans++;
        return ans;
    }

    private static int[] reversePair2(int[]? originArr, int[]? reverseArr, int power)
    {
        if (originArr == null || reverseArr == null) throw new Exception();
        var reverse = copyArray(originArr);
        if (reverse == null) throw new Exception();
        ReversePart(reverse, 0, reverse.Length - 1);
        var recordDown = new int[power + 1];
        var recordUp = new int[power + 1];
        Process(originArr, 0, originArr.Length - 1, power, recordDown);
        Process(reverse, 0, reverse.Length - 1, power, recordUp);
        var ans = new int[reverseArr.Length];
        for (var i = 0; i < reverseArr.Length; i++)
        {
            var curPower = reverseArr[i];
            for (var p = 1; p <= curPower; p++) (recordDown[p], recordUp[p]) = (recordUp[p], recordDown[p]);

            for (var p = 1; p <= power; p++) ans[i] += recordDown[p];
        }

        return ans;
    }

    // originArr[L...R]完成排序！
    // L...M左  M...R右  Merge
    // L...R  2的power次方

    private static void Process(int[] originArr, int l, int r, int power, int[] record)
    {
        if (l == r) return;
        var mid = l + ((r - l) >> 1);
        Process(originArr, l, mid, power - 1, record);
        Process(originArr, mid + 1, r, power - 1, record);
        record[power] += Merge(originArr, l, mid, r);
    }

    private static int Merge(int[] arr, int l, int m, int r)
    {
        var help = new int[r - l + 1];
        var i = 0;
        var p1 = l;
        var p2 = m + 1;
        var ans = 0;
        while (p1 <= m && p2 <= r)
        {
            ans += arr[p1] > arr[p2] ? m - p1 + 1 : 0;
            help[i++] = arr[p1] <= arr[p2] ? arr[p1++] : arr[p2++];
        }

        while (p1 <= m) help[i++] = arr[p1++];
        while (p2 <= r) help[i++] = arr[p2++];
        for (i = 0; i < help.Length; i++) arr[l + i] = help[i];
        return ans;
    }

    //用于测试
    private static int[] GenerateRandomOriginArray(int power, int value)
    {
        var ans = new int[1 << power];
        for (var i = 0; i < ans.Length; i++) ans[i] = (int)(Utility.getRandomDouble * value);
        return ans;
    }

    //用于测试
    private static int[] GenerateRandomReverseArray(int len, int power)
    {
        var ans = new int[len];
        for (var i = 0; i < ans.Length; i++) ans[i] = (int)(Utility.getRandomDouble * (power + 1));
        return ans;
    }

    //用于测试
    private static int[]? copyArray(int[]? arr)
    {
        if (arr == null) return null;
        var res = new int[arr.Length];
        for (var i = 0; i < arr.Length; i++) res[i] = arr[i];
        return res;
    }

    //用于测试
    private static bool IsEqual(int[]? arr1, int[]? arr2)
    {
        if ((arr1 == null && arr2 != null) || (arr1 != null && arr2 == null)) return false;
        if (arr1 == null && arr2 == null) return true;
        if (arr1?.Length != arr2?.Length) return false;
        for (var i = 0; i < arr1?.Length; i++)
            if (arr1[i] != arr2?[i])
                return false;
        return true;
    }

    public static void Run()
    {
        var powerMax = 8;
        var msizeMax = 10;
        var value = 30;
        var testTime = 50000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var power = (int)(Utility.getRandomDouble * powerMax) + 1;
            var msize = (int)(Utility.getRandomDouble * msizeMax) + 1;
            var originArr01 = GenerateRandomOriginArray(power, value);
            var originArr1 = copyArray(originArr01);
            var originArr2 = copyArray(originArr01);
            var reverseArr01 = GenerateRandomReverseArray(msize, power);
            var reverseArr1 = copyArray(reverseArr01);
            var reverseArr2 = copyArray(reverseArr01);
            var ans1 = reversePair1(originArr1, reverseArr1, power);
            var ans2 = reversePair2(originArr2, reverseArr2, power);
            if (!IsEqual(ans1, ans2)) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");

        powerMax = 20;
        msizeMax = 1000000;
        value = 1000;
        var originArr = GenerateRandomOriginArray(powerMax, value);
        var reverseArr = GenerateRandomReverseArray(msizeMax, powerMax);
        // int[] ans1 = reversePair1(originArr1, reverseArr1, powerMax);


        Utility.RestartStopwatch();
        reversePair2(originArr, reverseArr, powerMax);
        Console.WriteLine("run time : " + Utility.GetStopwatchElapsedMilliseconds() + " ms");
    }
}