#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson41;

// 来自小红书
// 有四种诗的韵律分别为: AABB、ABAB、ABBA、AAAA
// 比如 : 1 1 3 3就属于AABB型的韵律、6 6 6 6就属于AAAA型的韵律等等
// 一个数组arr，当然可以生成很多的子序列，如果某个子序列一直以韵律的方式连接起来，我们称这样的子序列是有效的
// 比如, arr = { 1, 1, 15, 1, 34, 1, 2, 67, 3, 3, 2, 4, 15, 3, 17, 4, 3, 7, 52, 7, 81, 9, 9 }
// arr的一个子序列为{1, 1, 1, 1, 2, 3, 3, 2, 4, 3, 4, 3, 7, 7, 9, 9}
// 其中1, 1, 1, 1是AAAA、2, 3, 3, 2是ABBA、4, 3, 4, 3是ABAB、7, 7, 9, 9是AABB
// 可以看到，整个子序列一直以韵律的方式连接起来，所以这个子序列是有效的
// 给定一个数组arr, 返回最长的有效子序列长度
// 题目限制 : arr长度 <= 4000, arr中的值<= 10^9
// 离散化之后，arr长度 <= 4000,  arr中的值<= 4000
public class PoemProblem
{
    // arr[i.....]符合规则连接的最长子序列长度
    //	private static int Zuo(int[] arr, int i) {
    //		if (i + 4 > arr.length) {
    //			return 0;
    //		}
    //		// 最终的，符合规则连接的最长子序列长度，就是不要i位置的字符
    //		int p0 = Zuo(arr, i + 1);
    //		// p1使用for循环搞定的！
    //		int p1 = 找到arr[i..s]是最短的，且能搞出AABB来的(4个) + Zuo(arr, s + 1);
    //		// p2使用for循环搞定的！
    //		int p2 = 找到arr[i..t]是最短的，且能搞出ABAB来的(4个) + Zuo(arr, t + 1);
    //		// p3使用for循环搞定的！
    //		int p3 = 找到arr[i..k]是最短的，且能搞出ABBA来的(4个) + Zuo(arr, k + 1);
    //		// p4没用
    //		int p4 = 找到arr[i..f]是最短的，且能搞出AAAA来的(4个) + Zuo(arr, f + 1);
    //		return p0~p4的最大值
    //	}

    // AABB
    // ABAB
    // ABBA
    // AAAA
    private static int MaxLen1(int[]? arr)
    {
        if (arr == null || arr.Length < 4) return 0;
        var path = new int[arr.Length];
        return Process1(arr, 0, path, 0);
    }

    private static int Process1(int[] arr, int index, int[] path, int size)
    {
        if (index == arr.Length)
        {
            if (size % 4 != 0) return 0;

            for (var i = 0; i < size; i += 4)
                if (!Valid(path, i))
                    return 0;
            return size;
        }

        var p1 = Process1(arr, index + 1, path, size);
        path[size] = arr[index];
        var p2 = Process1(arr, index + 1, path, size + 1);
        return Math.Max(p1, p2);
    }

    private static bool Valid(int[] p, int i)
    {
        // AABB
        // ABAB
        // ABBA
        // AAAA
        return (p[i] == p[i + 1] && p[i + 2] == p[i + 3]) ||
               (p[i] == p[i + 2] && p[i + 1] == p[i + 3] && p[i] != p[i + 1]) ||
               (p[i] == p[i + 3] && p[i + 1] == p[i + 2] && p[i] != p[i + 1]);
    }

    // 0 : [3,6,9]
    // 1 : [2,7,13]
    // 2 : [23]
    // [
    // [3,6,9]
    // ]
    private static int MaxLen2(int[]? arr)
    {
        if (arr == null || arr.Length < 4) return 0;
        var n = arr.Length;
        var sorted = new int[n];
        Array.Copy(arr, sorted, n);
        Array.Sort(sorted);
        var vmap = new Dictionary<int, int>();
        var index = 0;
        vmap[sorted[0]] = index++;
        for (var i = 1; i < n; i++)
            if (sorted[i] != sorted[i - 1])
                vmap[sorted[i]] = index++;
        var sizeArr = new int[index];
        for (var i = 0; i < n; i++)
        {
            arr[i] = vmap[arr[i]];
            sizeArr[arr[i]]++;
        }

        var imap = new int[index][];
        for (var i = 0; i < index; i++) imap[i] = new int[sizeArr[i]];
        for (var i = n - 1; i >= 0; i--) imap[arr[i]][--sizeArr[arr[i]]] = i;
        return Process2(arr, imap, 0);
    }

    // AABB
    // ABAB
    // ABBA
    // AAAA
    private static int Process2(int[] varr, int[][] imap, int i)
    {
        if (i + 4 > varr.Length) return 0;
        var p0 = Process2(varr, imap, i + 1);
        // AABB
        var p1 = 0;
        var rightClosedP1A2 = RightClosed(imap, varr[i], i);
        if (rightClosedP1A2 != -1)
            for (var next = rightClosedP1A2 + 1; next < varr.Length; next++)
                if (varr[i] != varr[next])
                {
                    var rightClosedP1B2 = RightClosed(imap, varr[next], next);
                    if (rightClosedP1B2 != -1) p1 = Math.Max(p1, 4 + Process2(varr, imap, rightClosedP1B2 + 1));
                }

        // ABAB
        var p2 = 0;
        for (var p2B1 = i + 1; p2B1 < varr.Length; p2B1++)
            if (varr[i] != varr[p2B1])
            {
                var rightClosedP2A2 = RightClosed(imap, varr[i], p2B1);
                if (rightClosedP2A2 != -1)
                {
                    var rightClosedP2B2 = RightClosed(imap, varr[p2B1], rightClosedP2A2);
                    if (rightClosedP2B2 != -1) p2 = Math.Max(p2, 4 + Process2(varr, imap, rightClosedP2B2 + 1));
                }
            }

        // ABBA
        var p3 = 0;
        for (var p3B1 = i + 1; p3B1 < varr.Length; p3B1++)
            if (varr[i] != varr[p3B1])
            {
                var rightClosedP3B2 = RightClosed(imap, varr[p3B1], p3B1);
                if (rightClosedP3B2 != -1)
                {
                    var rightClosedP3A2 = RightClosed(imap, varr[i], rightClosedP3B2);
                    if (rightClosedP3A2 != -1) p3 = Math.Max(p3, 4 + Process2(varr, imap, rightClosedP3A2 + 1));
                }
            }

        // AAAA
        var p4 = 0;
        var rightClosedP4A2 = RightClosed(imap, varr[i], i);
        var rightClosedP4A3 = rightClosedP4A2 == -1 ? -1 : RightClosed(imap, varr[i], rightClosedP4A2);
        var rightClosedP4A4 = rightClosedP4A3 == -1 ? -1 : RightClosed(imap, varr[i], rightClosedP4A3);
        if (rightClosedP4A4 != -1) p4 = Math.Max(p4, 4 + Process2(varr, imap, rightClosedP4A4 + 1));
        return Math.Max(p0, Math.Max(Math.Max(p1, p2), Math.Max(p3, p4)));
    }

    private static int RightClosed(int[][] imap, int v, int i)
    {
        var left = 0;
        var right = imap[v].Length - 1;
        var ans = -1;
        while (left <= right)
        {
            var mid = (left + right) / 2;
            if (imap[v][mid] <= i)
            {
                left = mid + 1;
            }
            else
            {
                ans = mid;
                right = mid - 1;
            }
        }

        return ans == -1 ? -1 : imap[v][ans];
    }

    private static int MaxLen3(int[]? arr)
    {
        if (arr == null || arr.Length < 4) return 0;
        var n = arr.Length;
        var sorted = new int[n];

        Array.Copy(arr, sorted, n);
        Array.Sort(sorted);
        var vmap = new Dictionary<int, int>();
        var index = 0;
        vmap[sorted[0]] = index++;
        for (var i = 1; i < n; i++)
            if (sorted[i] != sorted[i - 1])
                vmap[sorted[i]] = index++;
        var sizeArr = new int[index];
        for (var i = 0; i < n; i++)
        {
            arr[i] = vmap[arr[i]];
            sizeArr[arr[i]]++;
        }

        var imap = new int[index][];
        for (var i = 0; i < index; i++) imap[i] = new int[sizeArr[i]];
        for (var i = n - 1; i >= 0; i--) imap[arr[i]][--sizeArr[arr[i]]] = i;
        var dp = new int[n + 1];
        for (var i = n - 4; i >= 0; i--)
        {
            var p0 = dp[i + 1];
            // AABB
            var p1 = 0;
            var rightClosedP1A2 = RightClosed(imap, arr[i], i);
            if (rightClosedP1A2 != -1)
                for (var next = rightClosedP1A2 + 1; next < arr.Length; next++)
                    if (arr[i] != arr[next])
                    {
                        var rightClosedP1B2 = RightClosed(imap, arr[next], next);
                        if (rightClosedP1B2 != -1) p1 = Math.Max(p1, 4 + dp[rightClosedP1B2 + 1]);
                    }

            // ABAB
            var p2 = 0;
            for (var p2B1 = i + 1; p2B1 < arr.Length; p2B1++)
                if (arr[i] != arr[p2B1])
                {
                    var rightClosedP2A2 = RightClosed(imap, arr[i], p2B1);
                    if (rightClosedP2A2 != -1)
                    {
                        var rightClosedP2B2 = RightClosed(imap, arr[p2B1], rightClosedP2A2);
                        if (rightClosedP2B2 != -1) p2 = Math.Max(p2, 4 + dp[rightClosedP2B2 + 1]);
                    }
                }

            // ABBA
            var p3 = 0;
            for (var p3B1 = i + 1; p3B1 < arr.Length; p3B1++)
                if (arr[i] != arr[p3B1])
                {
                    var rightClosedP3B2 = RightClosed(imap, arr[p3B1], p3B1);
                    if (rightClosedP3B2 != -1)
                    {
                        var rightClosedP3A2 = RightClosed(imap, arr[i], rightClosedP3B2);
                        if (rightClosedP3A2 != -1) p3 = Math.Max(p3, 4 + dp[rightClosedP3A2 + 1]);
                    }
                }

            // AAAA
            var p4 = 0;
            var rightClosedP4A2 = RightClosed(imap, arr[i], i);
            var rightClosedP4A3 = rightClosedP4A2 == -1 ? -1 : RightClosed(imap, arr[i], rightClosedP4A2);
            var rightClosedP4A4 = rightClosedP4A3 == -1 ? -1 : RightClosed(imap, arr[i], rightClosedP4A3);
            if (rightClosedP4A4 != -1) p4 = Math.Max(p4, 4 + dp[rightClosedP4A4 + 1]);
            dp[i] = Math.Max(p0, Math.Max(Math.Max(p1, p2), Math.Max(p3, p4)));
        }

        return dp[0];
    }

    // 课堂有同学提出了贪心策略（这题还真是有贪心策略），是正确的
    // AABB
    // ABAB
    // ABBA
    // AAAA
    // 先看前三个规则：AABB、ABAB、ABBA
    // 首先A、A、B、B的全排列为:
    // AABB -> AABB
    // ABAB -> ABAB
    // ABBA -> ABBA
    // BBAA -> 等同于AABB，因为A和B谁在前、谁在后都算是 : AABB的范式
    // BABA -> 等同于ABAB，因为A和B谁在前、谁在后都算是 : ABAB的范式
    // BAAB -> 等同于ABBA，因为A和B谁在前、谁在后都算是 : ABBA的范式
    // 也就是说，AABB、ABAB、ABBA这三个规则，可以这么用：
    // 只要有两个不同的数，都出现2次，那么这一共4个数就一定符合韵律规则。
    // 所以：
    // 1) 当来到arr中的一个数字num的时候，
    // 如果num已经出现了2次了, 只要之前还有一个和num不同的数，
    // 也出现了两次，则一定符合了某个规则, 长度直接+4，然后清空所有的统计
    // 2) 当来到arr中的一个数字num的时候,
    // 如果num已经出现了4次了(规则四), 长度直接+4，然后清空所有的统计
    // 但是如果我去掉某个规则，该贪心直接报废，比如韵律规则变成:
    // AABB、ABAB、AAAA
    // 因为少了ABBA, 所以上面的化简不成立了, 得重新分析新规则下的贪心策略
    // 而尝试的方法就更通用(也就是maxLen3)，只是减少一个分支而已
    // 这个贪心费了很多心思，值得点赞！
    private static int MaxLen4(int[] arr)
    {
        // 统计某个数(key)，出现的次数(value)
        var map = new Dictionary<int, int?>();
        // tow代表目前有多少数出现了2次
        var two = 0;
        // ans代表目前符合韵律链接的子序列增长到了多长
        var ans = 0;
        // 当前的num出现了几次
        foreach (var num in arr)
        {
            // 对当前的num，做次数统计
            map[num] = (map[num] != null ? map[num] : 0) + 1;
            // 把num出现的次数拿出来
            var numTimes = map[num];
            // 如果num刚刚出现了2次, 那么目前出现了2次的数，的数量，需要增加1个
            two += numTimes == 2 ? 1 : 0;
            // 下面的if代表 :
            // 如果目前有2个数出现2次了，可以连接了
            // 如果目前有1个数出现4次了，可以连接了
            if (two == 2 || numTimes == 4)
            {
                ans += 4;
                map.Clear();
                two = 0;
            }
        }

        return ans;
    }

    // 为了测试
    private static int[] RandomArray(int len, int value)
    {
        var arr = new int[len];
        for (var i = 0; i < len; i++) arr[i] = (int)(Utility.GetRandomDouble * value);
        return arr;
    }

    // 为了测试
    //todo:待修复
    public static void Run()
    {
        // 1111 2332 4343 7799
        int[] test = [1, 1, 15, 1, 34, 1, 2, 67, 3, 3, 2, 4, 15, 3, 17, 4, 3, 7, 52, 7, 81, 9, 9];
        Console.WriteLine(MaxLen1(test));
        Console.WriteLine(MaxLen2(test));
        Console.WriteLine(MaxLen3(test));
        Console.WriteLine(MaxLen4(test));
        Console.WriteLine("===========");

        var len = 16;
        var value = 10;
        var arr = RandomArray(len, value);
        var arr1 = new int[arr.Length];
        Array.Copy(arr, arr1, arr.Length);
        var arr2 = new int[arr.Length];
        Array.Copy(arr, arr2, arr.Length);
        var arr3 = new int[arr.Length];
        Array.Copy(arr, arr3, arr.Length);
        var arr4 = new int[arr.Length];
        Array.Copy(arr, arr4, arr.Length);
        Console.WriteLine(MaxLen1(arr1));
        Console.WriteLine(MaxLen2(arr2));
        Console.WriteLine(MaxLen3(arr3));
        Console.WriteLine(MaxLen4(arr4));

        Console.WriteLine("===========");

        var longArr = RandomArray(4000, 20);
        Utility.RestartStopwatch();
        Console.WriteLine(MaxLen3(longArr));

        Console.WriteLine("运行时间(毫秒) : " + Utility.GetStopwatchElapsedMilliseconds());
        Console.WriteLine("===========");

        Utility.RestartStopwatch();
        Console.WriteLine(MaxLen4(longArr));

        Console.WriteLine("运行时间(毫秒) : " + Utility.GetStopwatchElapsedMilliseconds());
        Console.WriteLine("===========");
    }
}