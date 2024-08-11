//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson45;

// 最长公共子串问题是面试常见题目之一
// 假设str1长度N，str2长度M
// 因为最优解的难度所限，一般在面试场上回答出O(N*M)的解法已经是比较优秀了
// 因为得到O(N*M)的解法，就已经需要用到动态规划了
// 但其实这个问题的最优解是O(N+M)，为了达到这个复杂度可是不容易
// 首先需要用到DC3算法得到后缀数组(sa)
// 进而用sa数组去生成height数组
// 而且在生成的时候，还有一个不回退的优化，都非常不容易理解
// 这就是后缀数组在面试算法中的地位 : 德高望重的噩梦
public class LongestCommonSubstringConquerByHeight
{
    private static int Lcs1(string s1, string s2)
    {
        if (ReferenceEquals(s1, null) || ReferenceEquals(s2, null) || s1.Length == 0 || s2.Length == 0) return 0;

        var str1 = s1.ToCharArray();
        var str2 = s2.ToCharArray();
        var row = 0;
        var col = str2.Length - 1;
        var max = 0;
        while (row < str1.Length)
        {
            var i = row;
            var j = col;
            var len = 0;
            while (i < str1.Length && j < str2.Length)
            {
                if (str1[i] != str2[j])
                    len = 0;
                else
                    len++;

                if (len > max) max = len;

                i++;
                j++;
            }

            if (col > 0)
                col--;
            else
                row++;
        }

        return max;
    }

    private static int Lcs2(string s1, string s2)
    {
        if (ReferenceEquals(s1, null) || ReferenceEquals(s2, null) || s1.Length == 0 || s2.Length == 0) return 0;

        var str1 = s1.ToCharArray();
        var str2 = s2.ToCharArray();
        var lengthN = str1.Length;
        var lengthM = str2.Length;
        int min = str1[0];
        int max = str1[0];
        for (var i = 1; i < lengthN; i++)
        {
            min = Math.Min(min, str1[i]);
            max = Math.Max(max, str1[i]);
        }

        for (var i = 0; i < lengthM; i++)
        {
            min = Math.Min(min, str2[i]);
            max = Math.Max(max, str2[i]);
        }

        var all = new int[lengthN + lengthM + 1];
        var index = 0;
        for (var i = 0; i < lengthN; i++) all[index++] = str1[i] - min + 2;

        all[index++] = 1;
        for (var i = 0; i < lengthM; i++) all[index++] = str2[i] - min + 2;

        var dc3 = new DC3(all, max - min + 2);
        var n = all.Length;
        var sa = dc3.sa_Conflict;
        var height = dc3.height_Conflict;
        var ans = 0;
        for (var i = 1; i < n; i++)
        {
            var y = sa[i - 1];
            var x = sa[i];
            if (Math.Min(x, y) < lengthN && Math.Max(x, y) > lengthN) ans = Math.Max(ans, height[i]);
        }

        return ans;
    }

    //用于测试
    private static string RandomNumberString(int len, int range)
    {
        var str = new char[len];
        for (var i = 0; i < len; i++) str[i] = (char)((int)(Utility.GetRandomDouble * range) + 'a');

        return new string(str);
    }

    public static void Run()
    {
        var len = 30;
        var range = 5;
        var testTime = 100000;
        Console.WriteLine("功能测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var n1 = (int)(Utility.GetRandomDouble * len);
            var n2 = (int)(Utility.GetRandomDouble * len);
            var str11 = RandomNumberString(n1, range);
            var str21 = RandomNumberString(n2, range);
            var ans11 = Lcs1(str11, str21);
            var ans21 = Lcs2(str11, str21);
            if (ans11 != ans21) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("功能测试结束");
        Console.WriteLine("==========");

        Console.WriteLine("性能测试开始");
        len = 80000;
        range = 26;

        var str1 = RandomNumberString(len, range);
        var str2 = RandomNumberString(len, range);

        Utility.RestartStopwatch();
        var ans1 = Lcs1(str1, str2);

        Console.WriteLine("方法1结果 : " + ans1 + " , 运行时间 : " + Utility.GetStopwatchElapsedMilliseconds() + " ms");

        Utility.RestartStopwatch();
        var ans2 = Lcs2(str1, str2);

        Console.WriteLine("方法2结果 : " + ans2 + " , 运行时间 : " + Utility.GetStopwatchElapsedMilliseconds() + " ms");

        Console.WriteLine("性能测试结束");
    }

    public class DC3
    {
        public int[] height_Conflict;

        public int[] rank_Conflict;
        public int[] sa_Conflict;

        public DC3(int[] nums, int max)
        {
            sa_Conflict = sa(nums, max);
            rank_Conflict = rank();
            height_Conflict = height(nums);
        }

        internal virtual int[] sa(int[] nums, int max)
        {
            var n = nums.Length;
            var arr = new int[n + 3];
            for (var i = 0; i < n; i++) arr[i] = nums[i];

            return skew(arr, n, max);
        }

        internal virtual int[] skew(int[] nums, int n, int K)
        {
            int n0 = (n + 2) / 3, n1 = (n + 1) / 3, n2 = n / 3, n02 = n0 + n2;
            int[] s12 = new int[n02 + 3], sa12 = new int[n02 + 3];
            for (int i = 0, j = 0; i < n + (n0 - n1); ++i)
                if (0 != i % 3)
                    s12[j++] = i;

            radixPass(nums, s12, sa12, 2, n02, K);
            radixPass(nums, sa12, s12, 1, n02, K);
            radixPass(nums, s12, sa12, 0, n02, K);
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
                sa12 = skew(s12, n02, name);
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

            radixPass(nums, s0, sa0, 0, n0, K);
            var sa = new int[n];
            for (int p = 0, t = n0 - n1, k = 0; k < n; k++)
            {
                var i = sa12[t] < n0 ? sa12[t] * 3 + 1 : (sa12[t] - n0) * 3 + 2;
                var j = sa0[p];
                if (sa12[t] < n0
                        ? leq(nums[i], s12[sa12[t] + n0], nums[j], s12[j / 3])
                        : leq(nums[i], nums[i + 1], s12[sa12[t] - n0 + 1], nums[j], nums[j + 1], s12[j / 3 + n0]))
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

        internal virtual void radixPass(int[] nums, int[] input, int[] output, int offset, int n, int k)
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

        internal virtual bool leq(int a1, int a2, int b1, int b2)
        {
            return a1 < b1 || (a1 == b1 && a2 <= b2);
        }

        internal virtual bool leq(int a1, int a2, int a3, int b1, int b2, int b3)
        {
            return a1 < b1 || (a1 == b1 && leq(a2, a3, b2, b3));
        }

        internal virtual int[] rank()
        {
            var n = sa_Conflict.Length;
            var ans = new int[n];
            for (var i = 0; i < n; i++) ans[sa_Conflict[i]] = i;

            return ans;
        }

        internal virtual int[] height(int[] s)
        {
            var n = s.Length;
            var ans = new int[n];
            // 依次求h[i] , k = 0
            for (int i = 0, k = 0; i < n; ++i)
                if (rank_Conflict[i] != 0)
                {
                    if (k > 0) --k;

                    var j = sa_Conflict[rank_Conflict[i] - 1];
                    while (i + k < n && j + k < n && s[i + k] == s[j + k]) ++k;

                    // h[i] = k
                    ans[rank_Conflict[i]] = k;
                }

            return ans;
        }
    }
}