//位运算实现加减乘除

namespace AdvancedTraining.算法新手班补充;

public class ClacByBitOp
{
    // 加法: 无进位相加 + 进位信息 
    public int Add(int a, int b)
    {
        var carrylessSum = a;
        while (b != 0)
        {
            carrylessSum = a ^ b; // 无进位相加 
            b = (a & b) << 1; // 进位信息 
            a = carrylessSum;
        }

        return carrylessSum;
    }

    // 负数: 补码 (取反加一)
    public int NegNum(int n)
    {
        return ~n + 1;
    }

    // 减法 
    public int Minus(int a, int b)
    {
        return Add(a, NegNum(b));
    }

    // 乘法 
    public int Multiply(int a, int b)
    {
        var result = 0;
        while (b != 0)
        {
            if ((b & 1) != 0) result = Add(result, a);
            a <<= 1; // 左移 
            b >>= 1; // 右移 
        }

        return result;
    }

    // 判断是否为负数 
    public bool IsNeg(int n)
    {
        return n < 0;
    }

    // 除法 
    public int Div(int a, int b)
    {
        var x = IsNeg(a) ? NegNum(a) : a;
        var y = IsNeg(b) ? NegNum(b) : b;
        var result = 0;
        for (var i = 30; i >= 0; i = Minus(i, 1))
            if (x >> i >= y)
            {
                result = Add(result, 1 << i); // 将 1 << i 加到 result 上 
                x = Minus(x, y << i);
            }

        return IsNeg(a) ^ IsNeg(b) ? NegNum(result) : result;
    }

    // 除法 (处理边界条件)
    public int Divide(int dividend, int divisor)
    {
        if (dividend == int.MinValue && divisor == -1) return int.MaxValue; // 直接返回 int.MaxValue 

        if (divisor == int.MinValue) return 0;

        if (dividend == int.MinValue)
        {
            var ans = Div(Add(dividend, 1), divisor);
            return Add(ans, Div(Minus(dividend, Multiply(ans, divisor)), divisor));
        }

        return Div(dividend, divisor);
    }
}