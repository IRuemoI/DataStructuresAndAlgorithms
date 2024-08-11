#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson01;

public class MinSwapStep
{
    // 一个数组中只有两种字符'G'和'B'，
    // 可以让所有的G都放在左侧，所有的B都放在右侧
    // 或者可以让所有的G都放在右侧，所有的B都放在左侧
    // 但是只能在相邻字符之间进行交换操作，请问请问至少需要交换几次，
    private static int MinSteps1(string s)
    {
        if (s is null or "") return 0;
        var str = s.ToCharArray();
        var step1 = 0;
        var gi = 0;
        for (var i = 0; i < str.Length; i++)
            if (str[i] == 'G')
                step1 += i - gi++;
        var step2 = 0;
        var bi = 0;
        for (var i = 0; i < str.Length; i++)
            if (str[i] == 'B')
                step2 += i - bi++;
        return Math.Min(step1, step2);
    }

    // 可以让G在左，或者在右
    private static int MinSteps2(string s)
    {
        if (s is null or "") return 0;
        var str = s.ToCharArray();
        var step1 = 0;
        var step2 = 0;
        var gi = 0;
        var bi = 0;
        for (var i = 0; i < str.Length; i++)
            if (str[i] == 'G')
                // 当前的G，去左边   方案1
                step1 += i - gi++;
            else
                // 当前的B，去左边   方案2
                step2 += i - bi++;
        return Math.Min(step1, step2);
    }

    // 为了测试
    private static string RandomString(int maxLen)
    {
        var str = new char[(int)(Utility.GetRandomDouble * maxLen)];
        for (var i = 0; i < str.Length; i++) str[i] = Utility.GetRandomDouble < 0.5 ? 'G' : 'B';
        return new string(str);
    }

    public static void Run()
    {
        const int maxLen = 100;
        const int testTime = 1000000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var str = RandomString(maxLen);
            var ans1 = MinSteps1(str);
            var ans2 = MinSteps2(str);
            if (ans1 != ans2) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试结束");
    }
}