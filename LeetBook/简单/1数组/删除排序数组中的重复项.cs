namespace LeetBook.简单._1数组;

public static class 删除排序数组中的重复项
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
        int[] arr = [1, 1, 2];
        Console.WriteLine(RemoveDuplicatesCode(arr));
        Console.WriteLine(string.Join(",", arr));
    }
}