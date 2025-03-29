//pass

namespace AdvancedTraining.Lesson01;

public class Near2Power
{
    // 已知n是正数
    // 返回大于等于，且最接近n的，2的某次方的值
    private static int TableSizeFor(int n)
    {
        n--; //若直接操作原值，当 n 是2的幂时会错误地返回 2n（例如输入8会错误返回16）
        n |= n >>> 1; // 将最高位1的右侧1个比特置1 
        n |= n >>> 2; // 将最高位1的右侧2个比特置1 
        n |= n >>> 4; // 将最高位1的右侧4个比特置1 
        n |= n >>> 8; // 将最高位1的右侧8个比特置1 
        n |= n >>> 16; // 将最高位1的右侧16个比特置1（覆盖32位整数）
        return n < 0 ? 1 : n + 1;
    }

    public static void Run()
    {
        const int cap = 120;
        Console.WriteLine(TableSizeFor(cap));
    }
}