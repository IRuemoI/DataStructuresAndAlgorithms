namespace AdvancedTraining.Lesson28;

public class LetterCombinationsOfAPhoneNumber //Problem_0017
{
    private static readonly char[][] Phone =
    [
        ['a', 'b', 'c'],
        ['d', 'e', 'f'],
        ['g', 'h', 'i'],
        ['j', 'k', 'l'],
        ['m', 'n', 'o'],
        ['p', 'q', 'r', 's'],
        ['t', 'u', 'v'],
        ['w', 'x', 'y', 'z']
    ];

    // "23"
    private static IList<string> LetterCombinations(string digits)
    {
        IList<string> ans = new List<string>();
        if (ReferenceEquals(digits, null) || digits.Length == 0) return ans;
        var str = digits.ToCharArray();
        var path = new char[str.Length];
        Process(str, 0, path, ans);
        return ans;
    }

    private static void Process(char[] str, int index, char[] path, IList<string> ans)
    {
        if (index == str.Length)
        {
            ans.Add(new string(path));
        }
        else
        {
            var cands = Phone[str[index] - '2'];
            foreach (var cur in cands)
            {
                path[index] = cur;
                Process(str, index + 1, path, ans);
            }
        }
    }

    public static void Run()
    {
        foreach (var item in
                 LetterCombinations("23"))
            Console.Write(item + ","); //输出：["ad","ae","af","bd","be","bf","cd","ce","cf"]
    }
}