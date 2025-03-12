namespace LeetBook;

class Program
{
    //冒泡排序
    public static void BubbleSort(IList<int>? array)
    {
        if (array == null || array.Count < 2) return;

        //对与每一趟冒泡都让每趟中的最大值移动到已排序区域的前一个位置
        for (int i = array.Count - 2; i >= 0; i--)
        {
            for (int j = 0; j <= i; j++)
            {
                //左侧未排序的区域的向左移动
                if (array[j] > array[j + 1]) (array[j], array[j + 1]) = (array[j + 1], array[j]);
            }
        }
    }

    static void Main(string[] args)
    {
        int[] list = [5,4,3,2,1,0];
        BubbleSort(list);
        Console.WriteLine("Hello, World!");
    }
}