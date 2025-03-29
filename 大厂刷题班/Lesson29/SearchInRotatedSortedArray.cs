//pass

namespace AdvancedTraining.Lesson29;

public class SearchInRotatedSortedArray //leetcode_0033
{
    // arr，原本是有序数组，旋转过，而且左部分长度不知道
    // 找num
    // num所在的位置返回
    private static int Search(int[] arr, int num)
    {
        var l = 0;
        var r = arr.Length - 1;
        while (l <= r)
        {
            // M = L + ((R - L) >> 1)
            var m = (l + r) / 2;
            if (arr[m] == num) return m;
            // arr[M] != num
            // [L] == [M] == [R] != num 无法二分
            if (arr[l] == arr[m] && arr[m] == arr[r])
            {
                while (l != m && arr[l] == arr[m]) l++;
                // 1) L == M L...M 一路都相等
                // 2) 从L到M终于找到了一个不等的位置
                if (l == m)
                {
                    // L...M 一路都相等
                    l = m + 1;
                    continue;
                }
            }

            // ...
            // arr[M] != num
            // [L] [M] [R] 不都一样的情况, 如何二分的逻辑
            if (arr[l] != arr[m])
            {
                if (arr[m] > arr[l])
                {
                    // L...M 一定有序
                    if (num >= arr[l] && num < arr[m])
                        //  3  [L] == 1    [M]   = 5   L...M - 1
                        r = m - 1;
                    else
                        // 9    [L] == 2    [M]   =  7   M... R
                        l = m + 1;
                }
                else
                {
                    // [L] > [M]    L....M  存在断点
                    if (num > arr[m] && num <= arr[r])
                        l = m + 1;
                    else
                        r = m - 1;
                }
            }
            else
            {
                // [L] [M] [R] 不都一样，  [L] === [M] -> [M]!=[R]
                if (arr[m] < arr[r])
                {
                    if (num > arr[m] && num <= arr[r])
                        l = m + 1;
                    else
                        r = m - 1;
                }
                else
                {
                    if (num >= arr[l] && num < arr[m])
                        r = m - 1;
                    else
                        l = m + 1;
                }
            }
        }

        return -1;
    }

    public static void Run()
    {
        Console.WriteLine(Search([4, 5, 6, 7, 0, 1, 2], 0)); //输出4
    }
}