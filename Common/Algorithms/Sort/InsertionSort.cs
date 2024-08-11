//测试通过

namespace Common.Algorithms.Sort;

public abstract class InsertionSort
{
    private static void InsertionSortCode(int[]? list)
    {
        if (list == null || list.Length == 1) return;
        //对于插入排序(升序)：  
        //将数组分为左右两部分，左侧时已排序的部分[0~i],右侧是未排序的部分[(i+1)~list.Length-1]。  
        //每趟将右侧无序部分的第一个元素向左有序部分移动(交换)直到这个元素不小于左侧的元素停止，将目标值移动到这个索引  
        for (var i = 1; i < list.Length; i++)
        for (var j = i - 1; j >= 0; j--)
            if (list[j] > list[j + 1])
                (list[j], list[j + 1]) = (list[j + 1], list[j]);
    }

    public static void Run()
    {
        int[] testList = [54, 26, 93, 17, 77, 31, 44, 55, 20];
        Console.WriteLine("插入排序升序：");
        InsertionSortCode(testList);
        Console.WriteLine(string.Join(",", testList));
    }
}