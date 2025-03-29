//测试通过

namespace Algorithms.Lesson05;

// https://leetcode.cn/problems/count-of-range-sum/
public class CountOfRangeSum
{
    private static int GetResult(int[] array, int lower, int upper)
    {
        var prefixSum = new long[array.Length];
        prefixSum[0] = array[0];
        for (var i = 1; i < array.Length; i++) prefixSum[i] = prefixSum[i - 1] + array[i];

        return Process(prefixSum, 0, prefixSum.Length - 1, lower, upper);
    }

    private static int Process(long[] prefixSum, int leftEdge, int rightEdge, int lower, int upper)
    {
        if (leftEdge == rightEdge) return prefixSum[leftEdge] >= lower && prefixSum[leftEdge] <= upper ? 1 : 0;
        var middle = leftEdge + ((rightEdge - leftEdge) >> 1);
        return Process(prefixSum, leftEdge, middle, lower, upper) +
               Process(prefixSum, middle + 1, rightEdge, lower, upper) +
               Merge(prefixSum, leftEdge, middle, rightEdge, lower, upper);
    }

    private static int Merge(long[] prefixSum, int leftEdge, int middle, int rightEdge, int lower, int upper)
    {
        #region 统计符合标准的情况

        var result = 0;
        var windowLeftEdge = leftEdge;
        var windowRightEdge = leftEdge;
        //对于所有的右组元素
        for (var i = middle + 1; i <= rightEdge; i++)
        {
            //获取比较的标准
            var min = prefixSum[i] - upper;
            var max = prefixSum[i] - lower;
            //因为当prefixSum[windowRightEdge] = max时也是满足条件的，窗口右侧还需要向右扩
            while (windowRightEdge <= middle && prefixSum[windowRightEdge] <= max) windowRightEdge++;
            //而当prefixSum[windowLeftEdge] = min时，窗口左侧已经不需要向右扩了
            while (windowLeftEdge <= middle && prefixSum[windowLeftEdge] < min) windowLeftEdge++;
            result += windowRightEdge - windowLeftEdge; //加上本轮符合条件的次数
        }

        #endregion

        #region 合并的基本操作

        var help = new long[rightEdge - leftEdge + 1];
        var helpIndex = 0;
        var leftPartIndex = leftEdge;
        var rightPartIndex = middle + 1;
        //比较左右两部分的元素，每次将较小的元素放入help数组中
        while (leftPartIndex <= middle && rightPartIndex <= rightEdge)
            help[helpIndex++] = prefixSum[leftPartIndex] < prefixSum[rightPartIndex]
                ? prefixSum[leftPartIndex++]
                : prefixSum[rightPartIndex++];
        //复制左半部分还有剩余的元素
        if (leftPartIndex <= middle) Array.Copy(prefixSum, leftPartIndex, help, helpIndex, middle - leftPartIndex + 1);
        //复制右半部分还有剩余的元素
        if (rightPartIndex <= rightEdge)
            Array.Copy(prefixSum, rightPartIndex, help, helpIndex, rightEdge - rightPartIndex + 1);
        //将排好序的help数组拷贝到原数组
        Array.Copy(help, 0, prefixSum, leftEdge, help.Length);

        #endregion

        return result;
    }

    public static void Run()
    {
        int[] array = [0, 0];
        Console.WriteLine(GetResult(array, 0, 0));

        array = [-2, 5, -1];
        Console.WriteLine(GetResult(array, -2, 2));
    }
}