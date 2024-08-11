//通过

namespace Algorithms.Lesson02;

public class SwapDemo
{
    public static void Run()
    {
        var a = 16;
        var b = 603;
        Console.WriteLine($"交换前a={a},b={b}");
        a ^= b;
        b ^= a;
        a ^= b;
        Console.WriteLine($"交换后a={a},b={b}\n");

        int[] arr = [3, 1, 100];
        const int i = 0;
        const int j = 0;
        arr[i] ^= arr[j];
        arr[j] ^= arr[i];
        arr[i] ^= arr[j];
        Console.WriteLine($"两个内存地址相同的变量交换后arr[i]={arr[i]}, arr[j]={arr[j]}\n");

        Console.WriteLine($"交换前arr[0]={arr[0]},arr[2]={arr[2]}");
        Swap(arr, 0, 2);
        Console.WriteLine($"交换后arr[0]={arr[0]},arr[2]={arr[2]}");
    }


    private static void Swap(int[] arr, int i, int j)
    {
        arr[i] ^= arr[j];
        arr[j] ^= arr[i];
        arr[i] ^= arr[j];
    }
}