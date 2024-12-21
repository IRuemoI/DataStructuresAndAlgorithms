namespace CustomTraining.Leetcode;

//leetcode:https://leetcode.cn/problems/zuo-xuan-zhuan-zi-fu-chuan-lcof/
public class DynamicPassword
{
    private static string DynamicPasswordCode(string password, int target)
    {
        return password[target..] + password[..target];
    }

    public static void Run()
    {
        var password = "lrloseumgh";
        var target = 6;
        Console.WriteLine(DynamicPasswordCode(password, target));
    }
}