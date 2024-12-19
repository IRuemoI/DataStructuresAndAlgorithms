//测试通过

namespace Common.Algorithms.Sort;

public class BubbleSort
{
    private static void Code(int[]? list)
    {
        if (list == null || list.Length == 1) return;
        //对于冒泡排序(升序)：
        //将数组分为左侧未排序的部分(0~i-1)，和右侧已排序的部分(i~list.Length-1)
        //每次比较时将较大值交换到右侧，完成一趟排序后使得最大值位于未排序部分的最右侧，之后想做扩大已排序的部分。
        for (var i = list.Length - 1; i > 0; i--)
        for (var j = 0; j < i; j++)
            //如果未排序的部分中当前数小于下一个数
            if (list[j] > list[j + 1])
                (list[j], list[j + 1]) = (list[j + 1], list[j]); //让较大的数字右移
    }

    public static void Run()
    {
        int[] testList = [54, 26, 93, 17, 77, 31, 44, 55, 20];
        Console.WriteLine("冒泡排序升序：");
        Code(testList);
        Console.WriteLine(string.Join(",", testList));
    }
}