namespace CustomTraining.Leetcode;

//leetcode:https://leetcode.cn/problems/remove-duplicates-from-sorted-array/solutions/728105/shan-chu-pai-xu-shu-zu-zhong-de-zhong-fu-tudo/
public static class RemoveDuplicates
{
    private static int RemoveDuplicatesCode(int[]? nums)
    {
        if (nums == null || nums.Length == 0) return 0;

        if (nums.Length == 1) return 1;

        var slow = 0;
        for (var i = 0; i < nums.Length; i++)
            if (nums[i] - nums[slow] > 0)
            {
                nums[slow + 1] = nums[i];
                slow++;
            }

        return slow + 1;
    }

    public static void Run()
    {
        int[] nums = [1, 1, 2];
        Console.WriteLine(RemoveDuplicatesCode(nums));
        Console.WriteLine(string.Join(",", nums));
    }
}