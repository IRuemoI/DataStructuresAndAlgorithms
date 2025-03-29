//pass

namespace AdvancedTraining.Lesson06;

public class Nim
{
    // 保证arr是正数数组
    private static void PrintWinner(int[] arr)
    {
        var eor = 0;
        foreach (var num in arr) eor ^= num;
        Console.WriteLine(eor == 0 ? "后手赢" : "先手赢");
    }
}