namespace Common.Algorithms.Sort;

public class CountSort
{
    private static void Code(int[] arr)
    {
        if (arr.Length < 2) return;

        //找出数组中的最大值和最小值
        var max = int.MinValue;
        var min = int.MaxValue;
        foreach (var element in arr)
        {
            max = Math.Max(max, element);
            min = Math.Min(min, element);
        }

        //申请合适大小的"桶"，作为计数数组
        var bucket = new int[max - min + 1];
        //把数组中的所有元素放入桶中
        foreach (var element in arr)
        {
            bucket[element - min]++;//可以处理负数的情况
        }

        var index = 0; //定义用于写入数组时所使用的指针
        //把桶中的元素按照从小到大的索引从桶中取出放入元素数组(降序排序时逆向写入)
        for (var i = 0; i < bucket.Length; i++)
        {
            while (bucket[i]-- > 0)
            {
                arr[index++] = min + i;
            }
        }
    }
    
    public static void Run()
    {
        int[] testList = [54, 26, 93, 17, 77, 31, 44, 55, 20];
        Console.WriteLine("计数排序升序：");
        Code(testList);
        Console.WriteLine(string.Join(",", testList));
    }
}