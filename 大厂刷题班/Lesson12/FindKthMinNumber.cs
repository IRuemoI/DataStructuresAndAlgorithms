//pass

namespace AdvancedTraining.Lesson12;

// 本题测试链接 : https://leetcode.cn/problems/median-of-two-sorted-arrays/
public class FindKthMinNumber
{
    //核心思想：通过将两个数组分为四个部分：第一第二数组，可能包含目标数组的区域，和不包含的部分，使用二分法查找。前提是两数组有序
    public static double FindMedianSortedArrays(int[] nums1, int[] nums2)
    {
        var size = nums1.Length + nums2.Length;
        var even = (size & 1) == 0; //true是偶数情况，false是奇数情况
        if (nums1.Length != 0 && nums2.Length != 0)
        {
            if (even)
                return (FindKthNum(nums1, nums2, size / 2) + FindKthNum(nums1, nums2, size / 2 + 1)) / 2D;
            return FindKthNum(nums1, nums2, size / 2 + 1);
        }

        if (nums1.Length != 0)
        {
            if (even)
                return (double)(nums1[(size - 1) / 2] + nums1[size / 2]) / 2;
            return nums1[size / 2];
        }

        if (nums2.Length != 0)
        {
            if (even)
                return (double)(nums2[(size - 1) / 2] + nums2[size / 2]) / 2;
            return nums2[size / 2];
        }

        return 0;
    }

    // 进阶问题 : 在两个都有序的数组中，找整体第K小的数
    // 可以做到O(log(Min(M,N)))
    private static int FindKthNum(int[] arr1, int[] arr2, int kth)
    {
        var longs = arr1.Length >= arr2.Length ? arr1 : arr2;
        var shorts = arr1.Length < arr2.Length ? arr1 : arr2;
        var l = longs.Length;
        var s = shorts.Length;
        if (kth <= s)
            // 1)
            return GetUpMedian(shorts, 0, kth - 1, longs, 0, kth - 1);
        if (kth > l)
        {
            // 3)
            if (shorts[kth - l - 1] >= longs[l - 1]) return shorts[kth - l - 1];
            if (longs[kth - s - 1] >= shorts[s - 1]) return longs[kth - s - 1];
            return GetUpMedian(shorts, kth - l, s - 1, longs, kth - s, l - 1);
        }

        // 2)  s < k <= l
        if (longs[kth - s - 1] >= shorts[s - 1]) return longs[kth - s - 1];
        return GetUpMedian(shorts, 0, s - 1, longs, kth - s, kth - 1);
    }


    // A[s1...e1]
    // B[s2...e2]
    // 一定等长！
    // 返回整体的，上中位数！8（4） 10（5） 12（6）
    private static int GetUpMedian(int[] a, int s1, int e1, int[] b, int s2, int e2)
    {
        while (s1 < e1)
        {
            // mid1 = s1 + (e1 - s1) >> 1
            var mid1 = (s1 + e1) / 2;
            var mid2 = (s2 + e2) / 2;
            if (a[mid1] == b[mid2]) return a[mid1];
            // 两个中点一定不等！
            if (((e1 - s1 + 1) & 1) == 1)
            {
                // 奇数长度
                if (a[mid1] > b[mid2])
                {
                    if (b[mid2] >= a[mid1 - 1]) return b[mid2];
                    e1 = mid1 - 1;
                    s2 = mid2 + 1;
                }
                else
                {
                    // A[mid1] < B[mid2]
                    if (a[mid1] >= b[mid2 - 1]) return a[mid1];
                    e2 = mid2 - 1;
                    s1 = mid1 + 1;
                }
            }
            else
            {
                // 偶数长度
                if (a[mid1] > b[mid2])
                {
                    e1 = mid1;
                    s2 = mid2 + 1;
                }
                else
                {
                    e2 = mid2;
                    s1 = mid1 + 1;
                }
            }
        }

        return Math.Min(a[s1], b[s2]);
    }
}