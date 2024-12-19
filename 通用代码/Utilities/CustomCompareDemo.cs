namespace Common.Utilities;

public class CustomCompareDemo
{
    #region 使用接口实现

    private class CustomKeyComparer : IComparer<string> // 假设键是字符串类型  
    {
        public int Compare(string? x, string? y)
        {
            // 自定义排序逻辑  
            // 例如，按照字符串长度排序
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var lengthComparison = x.Length.CompareTo(y.Length);
            // 如果字符串长度不同，则较长的字符串较大;否则，使用普通的字符串比较
            return lengthComparison != 0 ? lengthComparison : string.Compare(x, y, StringComparison.Ordinal);
        }
    }

    public static void Run1()
    {
        // 使用自定义比较器创建 SortedDictionary  
        var sortedDict = new SortedDictionary<string, int>(new CustomKeyComparer())
        {
            { "apple", 1 },
            { "banana", 2 },
            { "cherry", 3 }
        };

        // 输出将按照字符串长度排序  
        foreach (var kvp in sortedDict) Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
    }

    #endregion

    #region 使用接口实现

    private static int CompareByLength(string? x, string? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;
        var lengthComparison = x.Length.CompareTo(y.Length);
        // 如果字符串长度不同，则较长的字符串较大;否则，使用普通的字符串比较
        return lengthComparison != 0 ? lengthComparison : string.Compare(x, y, StringComparison.Ordinal);
    }

    public static void Run2()
    {
        var dinosaurs = new List<string?>
        {
            "Pachycephalosaurus",
            "Amargasaurus",
            "",
            null,
            "Mamenchisaurus",
            "Deinonychus"
        };
        Display(dinosaurs);

        Console.WriteLine("\nSort with generic Comparison<string> delegate:");
        dinosaurs.Sort(CompareByLength);
        Display(dinosaurs);
    }

    private static void Display(List<string?> list)
    {
        Console.WriteLine();
        foreach (var s in list)
            if (s == null)
                Console.WriteLine("(null)");
            else
                Console.WriteLine("\"{0}\"", s);
    }

    #endregion
}