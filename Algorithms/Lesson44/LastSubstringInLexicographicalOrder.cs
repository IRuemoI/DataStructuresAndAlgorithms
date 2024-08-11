namespace Algorithms.Lesson44;

// 测试链接: https://leetcode.cn/problems/last-substring-in-lexicographical-order/
public static class LastSubstringInLexicographicalOrder
{
    public static void Run()
    {
        Console.WriteLine(LastSubstring("abab")); //bab
        Console.WriteLine(LastSubstring("leetcode")); //tcode
    }

    private static string? LastSubstring(string? s)
    {
        if (ReferenceEquals(s, null) || s.Length == 0) return s;

        var n = s.Length;
        var str = s.ToCharArray();
        var min = int.MaxValue;
        var max = int.MinValue;
        foreach (var cha in str)
        {
            min = Math.Min(min, cha);
            max = Math.Max(max, cha);
        }

        var arr = new int[n];
        for (var i = 0; i < n; i++) arr[i] = str[i] - min + 1;

        var dc3 = new Dc3(arr, max - min + 1);
        return s.Substring(dc3.SaArray[n - 1]);
    }

    private class Dc3
    {
        public readonly int[] SaArray;

        public Dc3(int[] nums, int max)
        {
            SaArray = Sa(nums, max);
        }

        private int[] Sa(int[] nums, int max)
        {
            var n = nums.Length;
            var arr = new int[n + 3];
            for (var i = 0; i < n; i++) arr[i] = nums[i];

            return Skew(arr, n, max);
        }

        private int[] Skew(int[] nums, int n, int targetK)
        {
            int n0 = (n + 2) / 3, n1 = (n + 1) / 3, n2 = n / 3, n02 = n0 + n2;
            int[] s12 = new int[n02 + 3], sa12 = new int[n02 + 3];
            for (int i = 0, j = 0; i < n + (n0 - n1); ++i)
                if (0 != i % 3)
                    s12[j++] = i;

            RadixPass(nums, s12, sa12, 2, n02, targetK);
            RadixPass(nums, sa12, s12, 1, n02, targetK);
            RadixPass(nums, s12, sa12, 0, n02, targetK);
            int name = 0, c0 = -1, c1 = -1, c2 = -1;
            for (var i = 0; i < n02; ++i)
            {
                if (c0 != nums[sa12[i]] || c1 != nums[sa12[i] + 1] || c2 != nums[sa12[i] + 2])
                {
                    name++;
                    c0 = nums[sa12[i]];
                    c1 = nums[sa12[i] + 1];
                    c2 = nums[sa12[i] + 2];
                }

                if (1 == sa12[i] % 3)
                    s12[sa12[i] / 3] = name;
                else
                    s12[sa12[i] / 3 + n0] = name;
            }

            if (name < n02)
            {
                sa12 = Skew(s12, n02, name);
                for (var i = 0; i < n02; i++) s12[sa12[i]] = i + 1;
            }
            else
            {
                for (var i = 0; i < n02; i++) sa12[s12[i] - 1] = i;
            }

            int[] s0 = new int[n0], sa0 = new int[n0];
            for (int i = 0, j = 0; i < n02; i++)
                if (sa12[i] < n0)
                    s0[j++] = 3 * sa12[i];

            RadixPass(nums, s0, sa0, 0, n0, targetK);
            var sa = new int[n];
            for (int p = 0, t = n0 - n1, k = 0; k < n; k++)
            {
                var i = sa12[t] < n0 ? sa12[t] * 3 + 1 : (sa12[t] - n0) * 3 + 2;
                var j = sa0[p];
                if (sa12[t] < n0
                        ? Leq(nums[i], s12[sa12[t] + n0], nums[j], s12[j / 3])
                        : Leq(nums[i], nums[i + 1], s12[sa12[t] - n0 + 1], nums[j], nums[j + 1], s12[j / 3 + n0]))
                {
                    sa[k] = i;
                    t++;
                    if (t == n02)
                        for (k++; p < n0; p++, k++)
                            sa[k] = sa0[p];
                }
                else
                {
                    sa[k] = j;
                    p++;
                    if (p == n0)
                        for (k++; t < n02; t++, k++)
                            sa[k] = sa12[t] < n0 ? sa12[t] * 3 + 1 : (sa12[t] - n0) * 3 + 2;
                }
            }

            return sa;
        }

        private void RadixPass(int[] nums, int[] input, int[] output, int offset, int n, int k)
        {
            var cnt = new int[k + 1];
            for (var i = 0; i < n; ++i) cnt[nums[input[i] + offset]]++;

            for (int i = 0, sum = 0; i < cnt.Length; ++i)
            {
                var t = cnt[i];
                cnt[i] = sum;
                sum += t;
            }

            for (var i = 0; i < n; ++i) output[cnt[nums[input[i] + offset]]++] = input[i];
        }

        private bool Leq(int a1, int a2, int b1, int b2)
        {
            return a1 < b1 || (a1 == b1 && a2 <= b2);
        }

        private bool Leq(int a1, int a2, int a3, int b1, int b2, int b3)
        {
            return a1 < b1 || (a1 == b1 && Leq(a2, a3, b2, b3));
        }
    }
}