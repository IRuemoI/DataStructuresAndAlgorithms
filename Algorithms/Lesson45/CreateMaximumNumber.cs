//测试通过

namespace Algorithms.Lesson45;

// 测试链接: https://leetcode.cn/problems/create-maximum-number/
public class CreateMaximumNumber
{
    private static int[]? MaxNumber1(int[] nums1, int[] nums2, int k)
    {
        var len1 = nums1.Length;
        var len2 = nums2.Length;
        if (k < 0 || k > len1 + len2) return null;

        var res = new int[k];
        var dp1 = GetDp(nums1); // 生成dp1这个表，以后从nums1中，只要固定拿N个数，
        var dp2 = GetDp(nums2);
        // get1 从arr1里拿的数量
        // K - get1 从arr2里拿的数量
        for (var get1 = Math.Max(0, k - len2); get1 <= Math.Min(k, len1); get1++)
        {
            // arr1 挑 get1个，怎么得到一个最优结果
            var pick1 = MaxPick(nums1, dp1, get1);
            var pick2 = MaxPick(nums2, dp2, k - get1);
            var merge = Merge(pick1, pick2);
            res = PreMoreThanLast(res, 0, merge, 0) ? res : merge;
        }

        return res;
    }

    private static int[] Merge(int[] nums1, int[] nums2)
    {
        var k = nums1.Length + nums2.Length;
        var ans = new int[k];
        for (int i = 0, j = 0, r = 0; r < k; ++r)
            ans[r] = PreMoreThanLast(nums1, i, nums2, j) ? nums1[i++] : nums2[j++];

        return ans;
    }

    private static bool PreMoreThanLast(int[] nums1, int i, int[] nums2, int j)
    {
        while (i < nums1.Length && j < nums2.Length && nums1[i] == nums2[j])
        {
            i++;
            j++;
        }

        return j == nums2.Length || (i < nums1.Length && nums1[i] > nums2[j]);
    }

    private static int[]? MaxNumber2(int[] nums1, int[] nums2, int k)
    {
        var len1 = nums1.Length;
        var len2 = nums2.Length;
        if (k < 0 || k > len1 + len2) return null;

        var res = new int[k];
        var dp1 = GetDp(nums1);
        var dp2 = GetDp(nums2);
        for (var get1 = Math.Max(0, k - len2); get1 <= Math.Min(k, len1); get1++)
        {
            var pick1 = MaxPick(nums1, dp1, get1);
            var pick2 = MaxPick(nums2, dp2, k - get1);
            var merge = MergeBySuffixArray(pick1, pick2);
            res = MoreThan(res, merge) ? res : merge;
        }

        return res;
    }

    private static bool MoreThan(int[] pre, int[] last)
    {
        var i = 0;
        var j = 0;
        while (i < pre.Length && j < last.Length && pre[i] == last[j])
        {
            i++;
            j++;
        }

        return j == last.Length || (i < pre.Length && pre[i] > last[j]);
    }

    private static int[] MergeBySuffixArray(int[] nums1, int[] nums2)
    {
        var size1 = nums1.Length;
        var size2 = nums2.Length;
        var nums = new int[size1 + 1 + size2];
        for (var i = 0; i < size1; i++) nums[i] = nums1[i] + 2;

        nums[size1] = 1;
        for (var j = 0; j < size2; j++) nums[j + size1 + 1] = nums2[j] + 2;

        var dc3 = new Dc3(nums, 11);
        var rank = dc3.RankArray;
        var ans = new int[size1 + size2];
        var i1 = 0;
        var i2 = 0;
        var r = 0;
        while (i1 < size1 && i2 < size2) ans[r++] = rank[i1] > rank[i2 + size1 + 1] ? nums1[i1++] : nums2[i2++];

        while (i1 < size1) ans[r++] = nums1[i1++];

        while (i2 < size2) ans[r++] = nums2[i2++];

        return ans;
    }

    private static int[,] GetDp(int[] arr)
    {
        var size = arr.Length; // 0~N-1
        var pick = arr.Length + 1; // 1 ~ N
        var dp = new int[size, pick];
        // get 不从0开始，因为拿0个无意义
        for (var get = 1; get < pick; get++)
        {
            // 1 ~ N
            var maxIndex = size - get;
            // i~N-1
            for (var i = size - get; i >= 0; i--)
            {
                if (arr[i] >= arr[maxIndex]) maxIndex = i;

                dp[i, get] = maxIndex;
            }
        }

        return dp;
    }

    private static int[] MaxPick(int[] arr, int[,] dp, int pick)
    {
        var res = new int[pick];
        for (int resIndex = 0, dpRow = 0; pick > 0; pick--, resIndex++)
        {
            res[resIndex] = arr[dp[dpRow, pick]];
            dpRow = dp[dpRow, pick] + 1;
        }

        return res;
    }

    private class Dc3
    {
        private readonly int[] _sa;
        public readonly int[] RankArray;

        public Dc3(int[] nums, int max)
        {
            _sa = Sa(nums, max);
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

        private static void RadixPass(int[] nums, int[] input, int[] output, int offset, int n, int k)
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

        private static bool Leq(int a1, int a2, int b1, int b2)
        {
            return a1 < b1 || (a1 == b1 && a2 <= b2);
        }

        private static bool Leq(int a1, int a2, int a3, int b1, int b2, int b3)
        {
            return a1 < b1 || (a1 == b1 && Leq(a2, a3, b2, b3));
        }

        private int[] Rank()
        {
            var n = _sa.Length;
            var ans = new int[n];
            for (var i = 0; i < n; i++) ans[_sa[i]] = i;

            return ans;
        }
    }
}