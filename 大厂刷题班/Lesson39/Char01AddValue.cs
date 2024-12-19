namespace AdvancedTraining.Lesson39;

// 来自腾讯
// 给定一个只由0和1组成的字符串S，假设下标从1开始，规定i位置的字符价值V[i]计算方式如下 : 
// 1) i == 1时，V[i] = 1
// 2) i > 1时，如果S[i] != S[i-1]，V[i] = 1
// 3) i > 1时，如果S[i] == S[i-1]，V[i] = V[i-1] + 1
// 你可以随意删除S中的字符，返回整个S的最大价值
// 字符串长度<=5000
// https://blog.csdn.net/weixin_48502062/article/details/122149248
public class Char01AddValue
{
    private static int Max1(string s)
    {
        if (ReferenceEquals(s, null) || s.Length == 0) return 0;
        var str = s.ToCharArray();
        var arr = new int[str.Length];
        for (var i = 0; i < arr.Length; i++) arr[i] = str[i] == '0' ? 0 : 1;
        return Process1(arr, 0, 0, 0);
    }

    // 递归含义 :
    // 目前在arr[index...]上做选择, str[index...]的左边，最近的数字是lastNum
    // 并且lastNum所带的价值，已经拉高到baseValue
    // 返回在str[index...]上做选择，最终获得的最大价值
    // index -> 0 ~ 4999
    // lastNum -> 0 or 1
    // baseValue -> 1 ~ 5000
    // 5000 * 2 * 5000 -> 5 * 10^7(过!)
    private static int Process1(int[] arr, int index, int lastNum, int baseValue)
    {
        if (index == arr.Length) return 0;
        var curValue = lastNum == arr[index] ? baseValue + 1 : 1;
        // 当前index位置的字符保留
        var next1 = Process1(arr, index + 1, arr[index], curValue);
        // 当前index位置的字符不保留
        var next2 = Process1(arr, index + 1, lastNum, baseValue);
        return Math.Max(curValue + next1, next2);
    }

    // 请看体系学习班，动态规划章节，把上面的递归改成动态规划！看完必会

    public static void Run()
    {
        Console.WriteLine(Max1("000001100000")); //输出55
    }
}