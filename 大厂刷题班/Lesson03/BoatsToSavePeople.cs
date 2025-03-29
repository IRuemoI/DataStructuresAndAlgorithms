//pass

namespace AdvancedTraining.Lesson03;

// 给定一个正数数组arr，代表若干人的体重
// 再给定一个正数limit，表示所有船共同拥有的载重量
// 每艘船最多坐两人，且不能超过载重
// 想让所有的人同时过河，并且用最好的分配方法让船尽量少
// 返回最少的船数
// 测试链接 : https://leetcode.cn/problems/boats-to-save-people/
public class BoatsToSavePeople
{
    private static int NumRescueBoats(int[]? arr, int limit)
    {
        if (arr == null || arr.Length == 0) return 0;
        var n = arr.Length;
        Array.Sort(arr);
        if (arr[n - 1] > limit) return -1;
        var lessR = -1;
        for (var i = n - 1; i >= 0; i--)
            if (arr[i] <= limit / 2)
            {
                lessR = i;
                break;
            }

        if (lessR == -1) return n;
        var l = lessR;
        var r = lessR + 1;
        var noUsed = 0;
        while (l >= 0)
        {
            var solved = 0;
            while (r < n && arr[l] + arr[r] <= limit)
            {
                r++;
                solved++;
            }

            if (solved == 0)
            {
                noUsed++;
                l--;
            }
            else
            {
                l = Math.Max(-1, l - solved);
            }
        }

        var all = lessR + 1;
        var used = all - noUsed;
        var moreUnsolved = n - all - used;
        return used + ((noUsed + 1) >> 1) + moreUnsolved;
    }


    public static void Run()
    {
        Console.WriteLine(NumRescueBoats([3, 2, 2, 1], 3)); //输出3
    }
}