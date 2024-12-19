using System.Diagnostics;

namespace Common;

// 使用泛型来处理输入和输出，可以是单一类型或元组
public abstract class QuestionTemplate<TInput, TOutput>(int testCount = 100) where TInput : ICloneable?, TOutput?
{
    private readonly Stopwatch _stopwatch = new();
    private int _errorCount;

    /// <summary>
    ///     执行测试
    /// </summary>
    protected void RunTests()
    {
        _stopwatch.Start();

        for (var i = 0; i < testCount; i++)
            try
            {
                var testData = GenerateInputArgs();
                var args = CopyArgs(testData);
                //Console.WriteLine(ReferenceEquals(testData, args));
                var result = QuestionSolution((TInput?)args);
                var expected = ExpectedResult(testData);

                if (CheckResult(result, expected)) continue;
                Console.WriteLine($"Test {i + 1}: {testData}");
                _errorCount++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test {i + 1} threw exception: {ex.Message}");
                _errorCount++;
            }

        _stopwatch.Stop();
        Console.WriteLine($"本次测试共{testCount}次，未通过{_errorCount}次，总用时{_stopwatch.Elapsed.TotalMilliseconds}毫秒");
    }

    /// <summary>
    ///     问题的解决方案
    /// </summary>
    /// <param name="testData">测试数据</param>
    protected abstract TOutput? QuestionSolution(TInput? testData);

    /// <summary>
    ///     生成输入参数
    /// </summary>
    protected abstract TInput? GenerateInputArgs();

    /// <summary>
    ///     复制生成的参数
    /// </summary>
    /// <param name="args">生成的参数</param>
    private static object? CopyArgs(TInput? args)
    {
        if (args == null) return null;
        return (TInput)args.Clone();
    }

    // 获取预期结果
    /// <summary>
    ///     获取预期结果
    /// </summary>
    /// <param name="testData">生成的参数拷贝</param>
    protected abstract TOutput? ExpectedResult(TInput? testData);

    /// <summary>
    ///     检查结果
    /// </summary>
    /// <param name="result">解决方案结果</param>
    /// <param name="expected">预期的结果</param>
    /// <returns>是否相同</returns>
    protected abstract bool CheckResult(TOutput? result, TOutput? expected);
}

// // 例如，如果一个题目需要两个整数输入，可以这样定义测试类
// public class Leetcode123Test : LeetcodeTestBase<(int, int), int>
// {
//     protected override (int, int) GenerateTestData()
//     {
//         return (1, 2);
//     }
//
//     protected override int QuestionSolution((int, int) testData)
//     {
//         //return YourLeetcodeSolution(testData.Item1, testData.Item2);
//         return 3;
//     }
//
//     protected override int ExpectedResult((int, int) testData)
//     {
//         return 3; // 假设预期结果是3
//     }
//
//     protected override bool CheckResult(int result, int expected)
//     {
//         return result == expected;
//     }
// }
//
// // 或者，如果一个题目需要一个整数和一个字符串作为输入，可以这样定义测试类
// public class Leetcode456Test : LeetcodeTestBase<(int, string), List<int>>
// {
//     protected override (int, string) GenerateTestData()
//     {
//         return (1, "hello");
//     }
//
//     protected override List<int> QuestionSolution((int, string) testData)
//     {
//         //return YourLeetcodeSolution(testData.Item1, testData.Item2);
//         return [1, 2, 3];
//     }
//
//     protected override List<int> ExpectedResult((int, string) testData)
//     {
//         return [1, 2, 3]; // 假设预期结果是一个整数列表
//     }
//
//     protected override bool CheckResult(List<int> result, List<int> expected)
//     {
//         return result.SequenceEqual(expected);
//     }
// }