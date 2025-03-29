//pass

namespace AdvancedTraining.Lesson09;

// 本题测试链接 : https://leetcode.cn/problems/russian-doll-envelopes/
public class EnvelopesProblem
{
    private static int MaxEnvelopes(int[][] matrix)
    {
        var arr = Sort(matrix);
        var ends = new int[matrix.Length];
        ends[0] = arr[0].H;
        var right = 0;
        for (var i = 1; i < arr.Length; i++)
        {
            var l = 0;
            var r = right;
            while (l <= r)
            {
                var m = (l + r) / 2;
                if (arr[i].H > ends[m])
                    l = m + 1;
                else
                    r = m - 1;
            }

            right = Math.Max(right, l);
            ends[l] = arr[i].H;
        }

        return right + 1;
    }

    private static Envelope[] Sort(int[][] matrix)
    {
        var res = new Envelope[matrix.Length];
        for (var i = 0; i < matrix.Length; i++) res[i] = new Envelope(matrix[i][0], matrix[i][1]);
        Array.Sort(res, new EnvelopeComparator());
        return res;
    }

    public static void Run()
    {
        Console.WriteLine(MaxEnvelopes([[5, 4], [6, 4], [6, 7], [2, 3]])); //输出3
    }

    private class Envelope(int weight, int height)
    {
        public readonly int H = height;
        public readonly int L = weight;
    }

    private class EnvelopeComparator : IComparer<Envelope>
    {
        public int Compare(Envelope? o1, Envelope? o2)
        {
            return o2 != null && o1 != null && o1.L != o2.L ? o1.L - o2.L : o2!.H - o1!.H;
        }
    }
}