using System.Text.RegularExpressions;

namespace CustomTraining;

public class RegExTest
{
    public static void Run()
    {
        const string inputText = "Hello 123 World 456, this is a test 789 string.";
        const string pattern = @"\d+"; // 正则表达式模式，匹配一个或多个数字

        // 使用 Regex.Match() 查找第一个匹配项
        var firstMatch = Regex.Match(inputText, pattern);
        Console.WriteLine("首个匹配项: " + firstMatch.Value);

        // 使用 Regex.Matches() 查找所有匹配项
        var allMatches = Regex.Matches(inputText, pattern);
        Console.WriteLine("全部比配项:");
        foreach (Match match in allMatches) Console.Write(match.Value + ",");
        Console.WriteLine();

        // 使用 Regex.Replace() 替换所有匹配的数字为 "number"
        var replacedText = Regex.Replace(inputText, pattern, "number");
        Console.WriteLine("文本替换后: " + replacedText);

        // 使用 Regex.Split() 根据匹配的数字分割字符串
        var splitText = Regex.Split(inputText, pattern);
        Console.WriteLine("文本分隔后:");
        foreach (var segment in splitText) Console.WriteLine(segment);
    }
}