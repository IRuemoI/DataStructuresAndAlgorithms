namespace AdvancedTraining.Lesson35;

public class NumberOfLongestIncreasingSubsequence //Problem_0673
{
    // 好理解的方法，时间复杂度O(N^2)
    private static int FindNumberOfLis1(int[]? numbers)
    {
        if (numbers == null || numbers.Length == 0) return 0;

        var n = numbers.Length;
        var lens = new int[n];
        var cnts = new int[n];
        lens[0] = 1;
        cnts[0] = 1;
        var maxLen = 1;
        var allCnt = 1;
        for (var i = 1; i < n; i++)
        {
            var preLen = 0;
            var preCnt = 1;
            for (var j = 0; j < i; j++)
            {
                if (numbers[j] >= numbers[i] || preLen > lens[j]) continue;

                if (preLen < lens[j])
                {
                    preLen = lens[j];
                    preCnt = cnts[j];
                }
                else
                {
                    preCnt += cnts[j];
                }
            }

            lens[i] = preLen + 1;
            cnts[i] = preCnt;
            if (maxLen < lens[i])
            {
                maxLen = lens[i];
                allCnt = cnts[i];
            }
            else if (maxLen == lens[i])
            {
                allCnt += cnts[i];
            }
        }

        return allCnt;
    }

    // 优化后的最优解，时间复杂度O(N*logN)
    private static int FindNumberOfLis2(int[]? numbers)
    {
        if (numbers == null || numbers.Length == 0) return 0;

        var dp = new List<SortedDictionary<int, int>>();
        foreach (var num in numbers)
        {
            // num之前的长度，num到哪个长度len+1
            var len = Search(dp, num);
            // cnt : 最终要去加底下的记录，才是应该填入的value
            int cnt;
            if (len == 0)
            {
                cnt = 1;
            }
            else
            {
                var p = dp[len - 1];
                cnt = p.First().Value - p[p.FirstOrDefault(x => x.Key >= num).Value];
            }

            if (len == dp.Count)
            {
                dp.Add(new SortedDictionary<int, int>());
                dp[len][num] = cnt;
            }
            else
            {
                dp[len][num] = dp[len].First().Value + cnt;
            }
        }

        return dp[^1].First().Value;
    }

    // 二分查找，返回>=num最左的位置
    private static int Search(List<SortedDictionary<int, int>> dp, int num)
    {
        int l = 0, r = dp.Count - 1;
        var ans = dp.Count;
        while (l <= r)
        {
            var m = (l + r) / 2;
            if (dp[m].First().Key >= num)
            {
                ans = m;
                r = m - 1;
            }
            else
            {
                l = m + 1;
            }
        }

        return ans;
    }

    //todo:待修复
    public static void Run()
    {
        Console.WriteLine(FindNumberOfLis1([1, 3, 5, 4, 7])); //输出2
        Console.WriteLine(FindNumberOfLis2([1, 3, 5, 4, 7])); //输出2
    }
}