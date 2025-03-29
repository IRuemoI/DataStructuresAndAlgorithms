//通过测试

namespace Common.Algorithms.Sort;

public class ShellSort
{
    // 希尔排序

    private static void Code(int[] a)
    {
        var d = a.Length; //gap的值

        while (true)
        {
            d /= 2; //每次都将gap的值减半

            for (var x = 0; x < d; x++) //对于gap所分的每一个组
            for (var i = x + d; i < a.Length; i += d)
            {
                //进行插入排序
                var temp = a[i];

                int j;

                for (j = i - d; j >= 0 && a[j] > temp; j -= d) a[j + d] = a[j];

                a[j + d] = temp;
            }

            if (d == 1)
                //gap==1，跳出循环
                break;
        }
    }

    public static void Run()
    {
        int[] testList = [9, 8, 7, 6, 5, 4, 3, 2, 1];
        Console.WriteLine("希尔排序升序：");
        Code(testList);
        Console.WriteLine(string.Join(",", testList));
    }
}