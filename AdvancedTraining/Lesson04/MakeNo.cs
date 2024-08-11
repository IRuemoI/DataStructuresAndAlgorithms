namespace AdvancedTraining.Lesson04;

public class MakeNo
{
    // 生成长度为size的达标数组
    // 达标：对于任意的 i<k<j，满足 [i] + [j] != [k] * 2
    private static int[] Code(int size)
    {
        if (size == 1) return [1];
        // size
        // 一半长达标来
        // 7 : 4
        // 8 : 4
        // [4个奇数] [3个偶]
        var halfSize = (size + 1) / 2;
        var @base = Code(halfSize);
        // base -> 等长奇数达标来
        // base -> 等长偶数达标来
        var ans = new int[size];
        var index = 0;
        for (; index < halfSize; index++) ans[index] = @base[index] * 2 - 1;
        for (var i = 0; index < size; index++, i++) ans[index] = @base[i] * 2;
        return ans;
    }

    // 检验函数
    private static bool IsValid(int[] arr)
    {
        var n = arr.Length;
        for (var i = 0; i < n; i++)
        for (var k = i + 1; k < n; k++)
        for (var j = k + 1; j < n; j++)
            if (arr[i] + arr[j] == 2 * arr[k])
                return false;
        return true;
    }

    public static void Run()
    {
        Console.WriteLine("测试开始");
        for (var n = 1; n < 100; n++)
        {
            var arr = Code(n);
            if (!IsValid(arr)) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试结束");
        Console.WriteLine(IsValid(Code(1042)));
        Console.WriteLine(IsValid(Code(2981)));
    }
}