namespace AdvancedTraining.Lesson09;

// 测试链接 : https://leetcode.cn/problems/remove-invalid-parentheses/
public class RemoveInvalidParentheses
{
    // 来自leetcode投票第一的答案，实现非常好，我们来赏析一下
    private static List<string> Code(string s)
    {
        var ans = new List<string>();
        var set = new HashSet<string>();
        Remove(s, ans, set, 0, 0, ['(', ')']);
        return ans;
    }

    // modifyIndex <= checkIndex
    // 只查s[checkIndex....]的部分，因为之前的一定已经调整对了
    // 但是之前的部分是怎么调整对的，调整到了哪？就是modifyIndex
    // 比如：
    // ( ( ) ( ) ) ) ...
    // 0 1 2 3 4 5 6
    // 一开始当然checkIndex = 0，modifyIndex = 0
    // 当查到6的时候，发现不对了，
    // 然后可以去掉2位置、4位置的 )，都可以
    // 如果去掉2位置的 ), 那么下一步就是
    // ( ( ( ) ) ) ...
    // 0 1 2 3 4 5 6
    // checkIndex = 6 ，modifyIndex = 2
    // 如果去掉4位置的 ), 那么下一步就是
    // ( ( ) ( ) ) ...
    // 0 1 2 3 4 5 6
    // checkIndex = 6 ，modifyIndex = 4
    // 也就是说，
    // checkIndex和modifyIndex，分别表示查的开始 和 调的开始，之前的都不用管了  par  (  )
    private static void Remove(string s, List<string> ans, HashSet<string> set, int checkIndex, int deleteIndex, char[] par)
    {
        var count = 0;
        for (var i = checkIndex; i < s.Length; i++)
        {
            if (s[i] == par[0]) count++;

            if (s[i] == par[1]) count--;

            // i check计数<0的第一个位置
            if (count < 0)
            {
                for (var j = deleteIndex; j <= i; j++)
                    if (s[j] == par[1] && (j == deleteIndex || s[j - 1] != par[1]))
                        Remove(s.Substring(0, j) + s.Substring(j + 1), ans, set, i, j, par);

                return;
            }
        }

        var reversed = new string(s.Reverse().ToArray());
        if (par[0] == '(')
            Remove(reversed, ans, set, 0, 0, new[] { ')', '(' });
        else if (set.Add(reversed))
            ans.Add(reversed);
    }

        public static void Run()
    {
        var result = Code("()())()"); // ["()()()", "(())()"]
        foreach (var item in result) Console.WriteLine(item);
    }
}