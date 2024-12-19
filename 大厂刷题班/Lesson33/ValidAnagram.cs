namespace AdvancedTraining.Lesson33;

//https://leetcode.cn/problems/valid-anagram/description/
public class ValidAnagram //leetcode_0242
{
    private static bool IsAnagram(string s, string t)
    {
        if (s.Length != t.Length) return false;
        var str1 = s.ToCharArray();
        var str2 = t.ToCharArray();
        var count = new int[256];
        foreach (var cha in str1) count[cha]++;
        foreach (var cha in str2)
            if (--count[cha] < 0)
                return false;
        return true;
    }

    public static void Run()
    {
        Console.WriteLine(IsAnagram("anagram", "nagaram")); //输出True
    }
}