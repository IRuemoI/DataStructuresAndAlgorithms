//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson13;

public class LowestLexicography
{
    private static string LowestString1(string?[] strings)
    {
        if (strings.Length == 0) return "";

        var ans = Process(strings);
        return ans.Count == 0 ? "" : ans.First();
    }

    // strings中所有字符串全排列，返回所有可能的结果
    private static SortedSet<string> Process(string?[] strings)
    {
        SortedSet<string> ans = new();
        if (strings is { Length: 0 })
        {
            ans.Add("");
            return ans;
        }

        for (var i = 0; i < strings.Length; i++)
        {
            var first = strings[i];
            var nextArray = RemoveIndexString(strings, i);
            var next = Process(nextArray);
            foreach (var cur in next) ans.Add(first + cur);
        }

        return ans;
    }

    // {"abc", "cks", "bct"}
    // 0 1 2
    // removeIndexString(arr , 1) -> {"abc", "bct"}
    private static string[] RemoveIndexString(string?[] arr, int index)
    {
        var n = arr.Length;
        var ans = new string[n - 1];
        var ansIndex = 0;
        for (var i = 0; i < n; i++)
            if (i != index)
                ans[ansIndex++] = arr[i]!;

        return ans;
    }

    private static string LowestString2(string?[] strings)
    {
        if (strings.Length == 0) return "";
        var res = "";
        foreach (var element in strings) res += element;

        return res;
    }

    //用于测试
    private static string? GenerateRandomString(int strLen)
    {
        var ans = new char[(int)(Utility.GetRandomDouble * strLen) + 1];
        for (var i = 0; i < ans.Length; i++)
        {
            var value = (int)(Utility.GetRandomDouble * 5);
            ans[i] = Utility.GetRandomDouble <= 0.5 ? (char)(65 + value) : (char)(97 + value);
        }

        return ans.ToString();
    }

    //用于测试
    private static string?[] GenerateRandomStringArray(int arrLen, int strLen)
    {
        var ans = new string?[(int)(Utility.GetRandomDouble * arrLen) + 1];
        for (var i = 0; i < ans.Length; i++) ans[i] = GenerateRandomString(strLen);

        return ans;
    }

    //用于测试
    private static string?[] CopyStringArray(string?[] arr)
    {
        var ans = new string?[arr.Length];
        for (var i = 0; i < ans.Length; i++) ans[i] = arr[i];

        return ans;
    }

    public static void Run()
    {
        var arrLen = 6;
        var strLen = 5;
        var testTimes = 10000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTimes; i++)
        {
            var arr1 = GenerateRandomStringArray(arrLen, strLen);
            var arr2 = CopyStringArray(arr1);
            if (!LowestString1(arr1).Equals(LowestString2(arr2)))
            {
                foreach (var str in arr1) Console.Write(str + ",");

                Console.WriteLine();
                Console.WriteLine("出错啦！");
            }
        }

        Console.WriteLine("测试完成");
    }

    public class MyComparator : IComparable<string>
    {
        public int CompareTo(string? other)
        {
            return string.Compare(this + other, other + this, StringComparison.Ordinal);
        }
    }
}