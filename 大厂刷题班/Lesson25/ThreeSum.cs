//pass

namespace AdvancedTraining.Lesson25;

// 本题测试链接 : https://leetcode.cn/problems/3sum/
public class ThreeSum
{
    private static IList<IList<int>> ThreeSumCode(int[] numbers)
    {
        Array.Sort(numbers);
        var n = numbers.Length;
        IList<IList<int>> ans = new List<IList<int>>();
        for (var i = n - 1; i > 1; i--)
            // 三元组最后一个数，是arr[i]   之前....二元组 + arr[i]
            if (i == n - 1 || numbers[i] != numbers[i + 1])
            {
                var nexts = TwoSum(numbers, i - 1, -numbers[i]);
                foreach (var cur in nexts)
                {
                    cur.Add(numbers[i]);
                    ans.Add(cur);
                }
            }

        return ans;
    }

    // nums[0...end]这个范围上，有多少个不同二元组，相加==target，全返回
    // {-1,5}     K = 4
    // {1, 3}
    private static IList<IList<int>> TwoSum(int[] numbers, int end, int target)
    {
        var l = 0;
        var r = end;
        IList<IList<int>> ans = new List<IList<int>>();
        while (l < r)
            if (numbers[l] + numbers[r] > target)
            {
                r--;
            }
            else if (numbers[l] + numbers[r] < target)
            {
                l++;
            }
            else
            {
                // nums[L] + nums[R] == target
                if (l == 0 || numbers[l - 1] != numbers[l])
                {
                    IList<int> cur = new List<int>();
                    cur.Add(numbers[l]);
                    cur.Add(numbers[r]);
                    ans.Add(cur);
                }

                l++;
            }

        return ans;
    }

    private static int FindPairs(int[] numbers, int k)
    {
        Array.Sort(numbers);
        int left = 0, right = 1;
        var result = 0;
        while (left < numbers.Length && right < numbers.Length)
            if (left == right || numbers[right] - numbers[left] < k)
            {
                right++;
            }
            else if (numbers[right] - numbers[left] > k)
            {
                left++;
            }
            else
            {
                left++;
                result++;
                while (left < numbers.Length && numbers[left] == numbers[left - 1]) left++;
            }

        return result;
    }

    public static void Run()
    {
        foreach (var row in ThreeSumCode([-1, 0, 1, 2, -1, -4]))
        {
            foreach (var item in row) Console.Write(item + ",");

            Console.WriteLine(); //输出：[[-1,-1,2],[-1,0,1]]
        }
    }
}