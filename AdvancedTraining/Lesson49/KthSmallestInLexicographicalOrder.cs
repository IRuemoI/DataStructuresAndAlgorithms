namespace AdvancedTraining.Lesson49;

// 这道题在leetcode上，所有题解都只能做到O( (logN) 平方)的解
// 我们课上讲的是O(logN)的解
// 打败所有题解
public class KthSmallestInLexicographicalOrder //Problem_0440
{
    private static readonly int[] Offset =
    [
        0, 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000
    ];

    private static readonly int[] Number =
    [
        0, 1, 11, 111, 1111, 11111, 111111, 1111111, 11111111, 111111111, 1111111111
    ];

    private static int FindKthNumber(int n, int k)
    {
        // 数字num，有几位，len位
        // 65237, 5位，len = 5
        var len = Len(n);
        // 65237, 开头数字，6，first
        var first = n / Offset[len];
        // 65237，左边有几个？
        var left = (first - 1) * Number[len];
        int pick;
        int already;
        if (k <= left)
        {
            // k / a 向上取整-> (k + a - 1) / a
            pick = (k + Number[len] - 1) / Number[len];
            already = (pick - 1) * Number[len];
            return Kth((pick + 1) * Offset[len] - 1, len, k - already);
        }

        var mid = Number[len - 1] + n % Offset[len] + 1;
        if (k - left <= mid) return Kth(n, len, k - left);
        k -= left + mid;
        len--;
        pick = (k + Number[len] - 1) / Number[len] + first;
        already = (pick - first - 1) * Number[len];
        return Kth((pick + 1) * Offset[len] - 1, len, k - already);
    }

    private static int Len(int n)
    {
        var len = 0;
        while (n != 0)
        {
            n /= 10;
            len++;
        }

        return len;
    }

    private static int Kth(int max, int len, int kth)
    {
        // 中间范围还管不管的着！
        // 有任何一步，中间位置没命中，左或者右命中了，那以后就都管不着了！
        // 但是开始时，肯定是管的着的！
        var closeToMax = true;
        var ans = max / Offset[len];
        while (--kth > 0)
        {
            max %= Offset[len--];
            int pick;
            if (!closeToMax)
            {
                pick = (kth - 1) / Number[len];
                ans = ans * 10 + pick;
                kth -= pick * Number[len];
            }
            else
            {
                var first = max / Offset[len];
                var left = first * Number[len];
                if (kth <= left)
                {
                    closeToMax = false;
                    pick = (kth - 1) / Number[len];
                    ans = ans * 10 + pick;
                    kth -= pick * Number[len];
                    continue;
                }

                kth -= left;
                var mid = Number[len - 1] + max % Offset[len] + 1;
                if (kth <= mid)
                {
                    ans = ans * 10 + first;
                    continue;
                }

                closeToMax = false;
                kth -= mid;
                len--;
                pick = (kth + Number[len] - 1) / Number[len] + first;
                ans = ans * 10 + pick;
                kth -= (pick - first - 1) * Number[len];
            }
        }

        return ans;
    }
}