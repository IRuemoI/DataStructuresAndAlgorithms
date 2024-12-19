namespace Algorithms.Lesson01;

//局部最小问题，无序数组中找到一个局部最小即可
public class BsAwesome
{
    private static int GetLessIndex(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return -1;

        if (arr.Length == 1 || arr[0] < arr[1]) return 0;

        if (arr[^1] < arr[^2]) return arr.Length - 1;

        var left = 0;
        var right = arr.Length - 1;
        //数组的开始情况是两端的数都大于相邻的数
        while (left < right)
        {
            var middle = left + ((right - left) >> 1); //需要注意C#的右移的优先级低于加法，所以需要加括号
            if (arr[middle - 1] < arr[middle]) //左侧能组成U型
                right = middle - 1; //舍去右侧
            else if (arr[middle] > arr[middle + 1]) //右侧能组成U型
                left = middle + 1; //舍去左侧
            else
                return middle; //局部最小值数组相邻是不相等的，在这个是分支中同时满足了小于右侧两侧的条件，也就是局部最小值的索引
        }

        return left; //理论上返回左或者右都一样
    }

    public static void Run()
    {
        Console.WriteLine(GetLessIndex([6, 1, 2, 1, -1])); //4
        Console.WriteLine(GetLessIndex([2, 1, 3])); //1
        Console.WriteLine(GetLessIndex([10, 1, 5, 2])); //3
    }
}