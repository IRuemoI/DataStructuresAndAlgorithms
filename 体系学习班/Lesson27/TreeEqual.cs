//通过

namespace Algorithms.Lesson27;

public class TreeEqual
{
    private static bool ContainsTree1(Node? big, Node? small)
    {
        if (small == null) return true;

        if (big == null) return false;

        if (IsSameValueStructure(big, small)) return true;

        return ContainsTree1(big.Left, small) || ContainsTree1(big.Right, small);
    }

    private static bool IsSameValueStructure(Node? head1, Node? head2)
    {
        if (head1 == null && head2 != null) return false;

        if (head1 != null && head2 == null) return false;

        if (head1 == null && head2 == null) return true;

        if (head1?.Value != head2?.Value) return false;

        return IsSameValueStructure(head1?.Left, head2?.Left)
               && IsSameValueStructure(head1?.Right, head2?.Right);
    }

    private static bool ContainsTree2(Node? big, Node? small)
    {
        if (small == null) return true;

        if (big == null) return false;

        var b = PreSerial(big);
        var s = PreSerial(small);
        var str = new string[b.Count];
        for (var i = 0; i < str.Length; i++) str[i] = b[i] ?? string.Empty;

        var match = new string[s.Count];
        for (var i = 0; i < match.Length; i++) match[i] = s[i] ?? string.Empty;

        return GetIndexOf(str, match) != -1;
    }

    private static List<string?> PreSerial(Node head)
    {
        List<string?> ans = new();
        Pres(head, ans);
        return ans;
    }

    private static void Pres(Node? head, List<string?> ans)
    {
        if (head == null)
        {
            ans.Add(null);
        }
        else
        {
            ans.Add(head.Value.ToString());
            Pres(head.Left, ans);
            Pres(head.Right, ans);
        }
    }

    private static int GetIndexOf(string[]? str1, string[]? str2)
    {
        if (str1 == null || str2 == null || str1.Length < 1 || str1.Length < str2.Length) return -1;

        var x = 0;
        var y = 0;
        var next = GetNextArray(str2);
        while (x < str1.Length && y < str2.Length)
            if (IsEqual(str1[x], str2[y]))
            {
                x++;
                y++;
            }
            else if (next[y] == -1)
            {
                x++;
            }
            else
            {
                y = next[y];
            }

        return y == str2.Length ? x - y : -1;
    }

    private static int[] GetNextArray(string[] ms)
    {
        if (ms.Length == 1) return new[] { -1 };

        var next = new int[ms.Length];
        next[0] = -1;
        next[1] = 0;
        var i = 2;
        var cn = 0;
        while (i < next.Length)
            if (IsEqual(ms[i - 1], ms[cn]))
                next[i++] = ++cn;
            else if (cn > 0)
                cn = next[cn];
            else
                next[i++] = 0;

        return next;
    }

    private static bool IsEqual(string? a, string? b)
    {
        if (a == null && b == null) return true;

        if (a == null || b == null)
            return false;
        return a.Equals(b);
    }

    
    private static Node? GenerateRandomBst(int maxLevel, int maxValue)
    {
        return Generate(1, maxLevel, maxValue);
    }

    
    private static Node? Generate(int level, int maxLevel, int maxValue)
    {
        if (level > maxLevel || new Random().NextDouble() < 0.5) return null;

        var head = new Node((int)(new Random().NextDouble() * maxValue))
        {
            Left = Generate(level + 1, maxLevel, maxValue),
            Right = Generate(level + 1, maxLevel, maxValue)
        };
        return head;
    }

    public static void Run()
    {
        const int bigTreeLevel = 7;
        const int smallTreeLevel = 4;
        const int nodeMaxValue = 5;
        const int testTimes = 100000;
        Console.WriteLine("开始测试");
        for (var i = 0; i < testTimes; i++)
        {
            var big = GenerateRandomBst(bigTreeLevel, nodeMaxValue);
            var small = GenerateRandomBst(smallTreeLevel, nodeMaxValue);
            var ans1 = ContainsTree1(big, small);
            var ans2 = ContainsTree2(big, small);
            if (ans1 != ans2) Console.WriteLine("出错了！");
        }

        Console.WriteLine("测试完成");
    }

    private class Node
    {
        public readonly int Value;
        public Node? Left;
        public Node? Right;

        public Node(int v)
        {
            Value = v;
        }
    }
}