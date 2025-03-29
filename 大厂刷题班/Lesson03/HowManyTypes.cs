//pass

#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson03;

public class HowManyTypes
{
    /*
     * 只由小写字母（a~z）组成的一批字符串，都放在字符类型的数组String[] arr中，
     * 如果其中某两个字符串，所含有的字符种类完全一样，就将两个字符串算作一类 比如：baacba和bac就算作一类
     * 虽然长度不一样，但是所含字符的种类完全一样（a、b、c） 返回arr中有多少类？
     *
     */

    private static int Types1(string[] arr)
    {
        var types = new HashSet<string>();
        foreach (var str in arr)
        {
            var chs = str.ToCharArray();
            var map = new bool[26];
            foreach (var item in chs) map[item - 'a'] = true;

            var key = "";
            for (var i = 0; i < 26; i++)
                if (map[i])
                    key += ((char)(i + 'a')).ToString();
            types.Add(key);
        }

        return types.Count;
    }

    private static int Types2(string[] arr)
    {
        var types = new HashSet<int>();
        foreach (var str in arr)
        {
            var chs = str.ToCharArray();
            var key = 0;
            foreach (var item in chs) key |= 1 << (item - 'a');

            types.Add(key);
        }

        return types.Count;
    }

    //用于测试
    private static string[] GetRandomStringArray(int possibilities, int strMaxSize, int arrMaxSize)
    {
        var ans = new string[(int)(Utility.getRandomDouble * arrMaxSize) + 1];
        for (var i = 0; i < ans.Length; i++) ans[i] = GetRandomString(possibilities, strMaxSize);
        return ans;
    }

    //用于测试
    private static string GetRandomString(int possibilities, int strMaxSize)
    {
        var ans = new char[(int)(Utility.getRandomDouble * strMaxSize) + 1];
        for (var i = 0; i < ans.Length; i++) ans[i] = (char)((int)(Utility.getRandomDouble * possibilities) + 'a');
        return new string(ans);
    }

    public static void Run()
    {
        var possibilities = 5;
        var strMaxSize = 10;
        var arrMaxSize = 100;
        var testTimes = 5000;
        Console.WriteLine("test begin, test time : " + testTimes);
        for (var i = 0; i < testTimes; i++)
        {
            var arr = GetRandomStringArray(possibilities, strMaxSize, arrMaxSize);
            var ans1 = Types1(arr);
            var ans2 = Types2(arr);
            if (ans1 != ans2) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");
    }
}