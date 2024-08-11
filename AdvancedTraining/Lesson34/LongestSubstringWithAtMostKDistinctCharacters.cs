namespace AdvancedTraining.Lesson34;

//https://blog.csdn.net/zhizhengguan/article/details/126262037
public class LongestSubstringWithAtMostKDistinctCharacters //Problem_0340
{
    private static int LengthOfLongestSubstringKDistinct(string s, int k)
    {
        if (ReferenceEquals(s, null) || s.Length == 0 || k < 1) return 0;
        var str = s.ToCharArray();
        var n = str.Length;
        var count = new int[256];
        var diff = 0;
        var r = 0;
        var ans = 0;
        for (var i = 0; i < n; i++)
        {
            // R 窗口的右边界
            while (r < n && (diff < k || (diff == k && count[str[r]] > 0)))
            {
                diff += count[str[r]] == 0 ? 1 : 0;
                count[str[r++]]++;
            }

            // R 来到违规的第一个位置
            ans = Math.Max(ans, r - i);
            diff -= count[str[i]] == 1 ? 1 : 0;
            count[str[i]]--;
        }

        return ans;
    }


    public static void Run()
    {
        Console.WriteLine(LengthOfLongestSubstringKDistinct("eceba", 2)); //输出3
    }
}