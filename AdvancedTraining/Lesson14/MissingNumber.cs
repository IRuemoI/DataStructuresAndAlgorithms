namespace AdvancedTraining.Lesson14;

// 测试链接：https://leetcode.cn/problems/first-missing-positive/
public class MissingNumber
{
    private static int FirstMissingPositive(int[] arr)
    {
        // l是盯着的位置
        // 0 ~ L-1有效区
        var l = 0;
        var r = arr.Length;
        while (l != r)
            if (arr[l] == l + 1)
                l++;
            else if (arr[l] <= l || arr[l] > r || arr[arr[l] - 1] == arr[l])
                // 垃圾的情况
                Swap(arr, l, --r);
            else
                Swap(arr, l, arr[l] - 1);
        return l + 1;
    }

    private static void Swap(int[] arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    public static void Run()
    {
        Console.WriteLine(FirstMissingPositive([3, 4, -1, 1])); //输出2
    }
}