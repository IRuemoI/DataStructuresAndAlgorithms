//pass
namespace AdvancedTraining.Lesson01;

public class Near2Power
{
    // 已知n是正数
    // 返回大于等于，且最接近n的，2的某次方的值
    private static int TableSizeFor(int n)
    {
        n--;
        n |= n >>> 1;
        n |= n >>> 2;
        n |= n >>> 4;
        n |= n >>> 8;
        n |= n >>> 16;
        return n < 0 ? 1 : n + 1;
    }

    public static void Run()
    {
        const int cap = 120;
        Console.WriteLine(TableSizeFor(cap));
    }
}