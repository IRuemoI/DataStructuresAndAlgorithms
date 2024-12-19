//测试通过

namespace Common.Algorithms.Sort;

public class SelectionSort
{
    private static void Code(int[]? list)
    {
        if (list == null || list.Length == 1) return;
        //对于插入排序(升序)：
        //将数组分为左右两部分，左侧是已排序的部分[0~i]，右侧是为排序的部分[(i+1)~(list.Length-1)]。
        //每次将右侧未排序部分中的最小值放入左侧已排序的右边界。直到排序完成
        for (var i = 0; i < list.Length - 1; i++)
        {
            var minIndex = i; //每次用左端已排序的最大值作为对比右侧的第一个最小值
            for (var j = i + 1; j < list.Length; j++)
                if (list[j] < list[minIndex])
                    minIndex = j;

            (list[i], list[minIndex]) = (list[minIndex], list[i]);
        }
    }

    public static void Run()
    {
        int[] testList = [9, 8, 7, 6, 5, 4, 3, 2, 1];
        Console.WriteLine("选择排序升序：");
        Code(testList);
        Console.WriteLine(string.Join(",", testList));
    }
}