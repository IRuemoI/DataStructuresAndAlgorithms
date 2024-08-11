//测试通过

namespace Algorithms.Lesson11;

public class PaperFolding
{
    private static void PrintAllFolds(int n)
    {
        Process(1, n, true);
        Console.WriteLine();
    }

    // 当前你来了一个节点，脑海中想象的！
    // 这个节点在第i层，一共有N层，N固定不变的
    // 这个节点如果是凹的话，down = T
    // 这个节点如果是凸的话，down = F
    // 函数的功能：中序打印以你想象的节点为头的整棵树！
    private static void Process(int i, int n, bool down)
    {
        if (i > n) return;

        Process(i + 1, n, true);
        Console.Write(down ? "凹 " : "凸 ");
        Process(i + 1, n, false);
    }

    public static void Run()
    {
        var n = 4;
        PrintAllFolds(n);
    }
}