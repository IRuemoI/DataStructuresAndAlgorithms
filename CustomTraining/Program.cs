//todo:进度 https://www.bilibili.com/video/BV1sovaemEhi/?t=2146

namespace CustomTraining;

public static class Program
{
    public static void Main()
    {
        var min = int.MaxValue;//刚开始是最大值，表示无效


        Console.WriteLine(min + (min == int.MaxValue ? 0 : 1));
    }
}