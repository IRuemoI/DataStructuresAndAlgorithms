namespace AdvancedTraining.Lesson52;

public class KEmptySlots //leetcode_0683
{
    private static int KEmptySlots1(int[] bulbs, int k)
    {
        var n = bulbs.Length;
        var days = new int[n];
        for (var i = 0; i < n; i++) days[bulbs[i] - 1] = i + 1;
        var ans = int.MaxValue;
        if (k == 0)
        {
            for (var i = 1; i < n; i++) ans = Math.Min(ans, Math.Max(days[i - 1], days[i]));
        }
        else
        {
            var minq = new int[n];
            var l = 0;
            var r = -1;
            for (var i = 1; i < n && i < k; i++)
            {
                while (l <= r && days[minq[r]] >= days[i]) r--;
                minq[++r] = i;
            }

            for (int i = 1, j = k; j < n - 1; i++, j++)
            {
                while (l <= r && days[minq[r]] >= days[j]) r--;
                minq[++r] = j;
                var cur = Math.Max(days[i - 1], days[j + 1]);
                if (days[minq[l]] > cur) ans = Math.Min(ans, cur);
                if (i == minq[l]) l++;
            }
        }

        return ans == int.MaxValue ? -1 : ans;
    }

    private static int KEmptySlots2(int[] bulbs, int k)
    {
        var n = bulbs.Length;
        var days = new int[n];
        for (var i = 0; i < n; i++) days[bulbs[i] - 1] = i + 1;
        var ans = int.MaxValue;
        for (int left = 0, mid = 1, right = k + 1; right < n; mid++)
            // 验证指针mid
            // mid 永远不和left撞上的！
            // 1) mid在left和right中间验证的时候，没通过！
            // 2) mid是撞上right的时候
            if (days[mid] <= Math.Max(days[left], days[right]))
            {
                //				if(mid == right) { // left...right 达标的！
                //					int cur = Math.max(days[left], days[right]);
                //					ans = Math.min(ans, cur);
                //					left  = mid;
                //					right =  mid + k + 1;
                //				} else { // 验证不通过！
                //					left  = mid;
                //					right =  mid + k + 1;
                //				}
                if (mid == right)
                    // 收答案！
                    ans = Math.Min(ans, Math.Max(days[left], days[right]));
                left = mid;
                right = mid + k + 1;
            }

        return ans == int.MaxValue ? -1 : ans;
    }

    public static void Run()
    {
        var bulbs = new[] { 1, 3, 2 };
        var k = 1;
        Console.WriteLine(KEmptySlots1(bulbs, k)); //2
        Console.WriteLine(KEmptySlots2(bulbs, k)); //2
    }
}