namespace AdvancedTraining.Lesson14;

public class MaxSubArraySumLessOrEqualK
{
    // 请返回arr中，求个子数组的累加和，是<=K的，并且是最大的。
    // 返回这个最大的累加和
    private static int GetMaxLessOrEqualK(int[] arr, int k)
    {
        // 记录i之前的，前缀和，按照有序表组织
        var set = new SortedSet<int> { 0 }; // 一个数也没有的时候，就已经有一个前缀和是0了
        var max = int.MinValue;
        var sum = 0;
        // 每一步的i，都求子数组必须以i结尾的情况下，求个子数组的累加和，是<=K的，并且是最大的
        foreach (var item in arr)
        {
            sum += item; // sum -> arr[0..i];
            max = Math.Max(max, sum - set.FirstOrDefault(x => x >= sum - k));

            set.Add(sum); // 当前的前缀和加入到set中去
        }

        return max;
    }
}