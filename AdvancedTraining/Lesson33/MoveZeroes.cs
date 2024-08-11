namespace AdvancedTraining.Lesson33;

//https://leetcode.cn/problems/move-zeroes/description/
public class MoveZeroes //Problem_0283
{
    private static void MoveZeroesCode(int[] numbers)
    {
        var to = 0;
        for (var i = 0; i < numbers.Length; i++)
            if (numbers[i] != 0)
                Swap(numbers, to++, i);
    }

    private static void Swap(int[] arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    public static void Run()
    {
        int[] arr = [0, 1, 0, 3, 12];
        MoveZeroesCode(arr);
        Console.WriteLine(string.Join(",", arr)); //输出[1,3,12,0,0]
    }
}