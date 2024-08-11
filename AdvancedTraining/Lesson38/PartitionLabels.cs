namespace AdvancedTraining.Lesson38;

//https://leetcode.cn/problems/partition-labels/description/
public class PartitionLabels //Problem_0763
{
    private static List<int> PartitionLabelsCode(string s)
    {
        var str = s.ToCharArray();
        var far = new int[26];
        for (var i = 0; i < str.Length; i++) far[str[i] - 'a'] = i;
        var ans = new List<int>();
        var left = 0;
        var right = far[str[0] - 'a'];
        for (var i = 1; i < str.Length; i++)
        {
            if (i > right)
            {
                ans.Add(right - left + 1);
                left = i;
            }

            right = Math.Max(right, far[str[i] - 'a']);
        }

        ans.Add(right - left + 1);
        return ans;
    }

    public static void Run()
    {
        Console.WriteLine(string.Join(",", PartitionLabelsCode("ababcbacadefegdehijhklij"))); //输出：[9,7,8]
    }
}