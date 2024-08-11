//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson45;

public class InsertS2MakeMostAlphabeticalOrder
{
    // 暴力方法
    private static string Right(string s1, string s2)
    {
        if (ReferenceEquals(s1, null) || s1.Length == 0) return s2;

        if (ReferenceEquals(s2, null) || s2.Length == 0) return s1;

        var p1 = s1 + s2;
        var p2 = s2 + s1;
        var ans = string.Compare(p1, p2, StringComparison.Ordinal) > 0 ? p1 : p2;
        for (var end = 1; end < s1.Length; end++)
        {
            var cur = s1.Substring(0, end) + s2 + s1.Substring(end);
            if (string.Compare(cur, ans, StringComparison.Ordinal) > 0) ans = cur;
        }

        return ans;
    }

    // 正式方法 O(N+M) + O(M^2)
    // N : s1长度
    // M : s2长度
    private static string MaxCombine(string s1, string s2)
    {
        if (ReferenceEquals(s1, null) || s1.Length == 0) return s2;

        if (ReferenceEquals(s2, null) || s2.Length == 0) return s1;

        var str1 = s1.ToCharArray();
        var str2 = s2.ToCharArray();
        var n = str1.Length;
        var m = str2.Length;
        int min = str1[0];
        int max = str1[0];
        for (var i = 1; i < n; i++)
        {
            min = Math.Min(min, str1[i]);
            max = Math.Max(max, str1[i]);
        }

        for (var i = 0; i < m; i++)
        {
            min = Math.Min(min, str2[i]);
            max = Math.Max(max, str2[i]);
        }

        var all = new int[n + m + 1];
        var index = 0;
        for (var i = 0; i < n; i++) all[index++] = str1[i] - min + 2;

        all[index++] = 1;
        for (var i = 0; i < m; i++) all[index++] = str2[i] - min + 2;

        var dc3 = new Dc3(all, max - min + 2);
        var rank = dc3.RankArray;
        var comp = n + 1;
        for (var i = 0; i < n; i++)
            if (rank[i] < rank[comp])
            {
                var best = BestSplit(s1, s2, i);
                return s1.Substring(0, best) + s2 + s1.Substring(best);
            }

        return s1 + s2;
    }

    private static int BestSplit(string s1, string s2, int first)
    {
        var n = s1.Length;
        var m = s2.Length;
        var end = n;
        for (int i = first, j = 0; i < n && j < m; i++, j++)
            if (s1[i] < s2[j])
            {
                end = i;
                break;
            }

        var bestPrefix = s2;
        var bestSplit = first;
        for (int i = first + 1, j = m - 1; i <= end; i++, j--)
        {
            var curPrefix = s1.Substring(first, i - first) + s2.Substring(0, j);
            if (string.Compare(curPrefix, bestPrefix, StringComparison.Ordinal) >= 0)
            {
                bestPrefix = curPrefix;
                bestSplit = i;
            }
        }

        return bestSplit;
    }

    //用于测试
    private static string RandomNumberString(int len, int range)
    {
        var str = new char[len];
        for (var i = 0; i < len; i++) str[i] = (char)((int)(Utility.GetRandomDouble * range) + '0');

        return new string(str);
    }

    //用于测试
    public static void Run()
    {
        var range = 10;
        var len = 50;
        var testTime = 100000;
        Console.WriteLine("功能测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var s1Len = (int)(Utility.GetRandomDouble * len);
            var s2Len = (int)(Utility.GetRandomDouble * len);
            var s1 = RandomNumberString(s1Len, range);
            var s2 = RandomNumberString(s2Len, range);
            var ans1 = Right(s1, s2);
            var ans2 = MaxCombine(s1, s2);
            if (!ans1.Equals(ans2))
            {
                Console.WriteLine("出错啦！");
                Console.WriteLine(s1);
                Console.WriteLine(s2);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                break;
            }
        }

        Console.WriteLine("功能测试结束");

        Console.WriteLine("==========");

        Console.WriteLine("性能测试开始");
        var s1Len1 = 1000000;
        var s2Len1 = 500;
        var s11 = RandomNumberString(s1Len1, range);
        var s21 = RandomNumberString(s2Len1, range);
        Utility.RestartStopwatch();
        MaxCombine(s11, s21);
        Console.WriteLine("运行时间 : " + Utility.GetStopwatchElapsedMilliseconds() + " ms");
        Console.WriteLine("性能测试结束");
    }

    private class Dc3
    {
        private readonly int[] _saArray;
        public readonly int[] RankArray;

        public Dc3(int[] nums, int max)
        {
            _saArray = Sa(nums, max);
            RankArray = Rank();
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

        private int[] Rank()
        {
            var n = _saArray.Length;
            var ans = new int[n];
            for (var i = 0; i < n; i++) ans[_saArray[i]] = i;

            return ans;
        }
    }
}