namespace AdvancedTraining.Lesson45;

public class PartitionArrayIntoTwoArraysToMinimizeSumDifference //leetcode_2035
{
    private static int MinimumDifference(int[] arr)
    {
        var size = arr.Length;
        var half = size >> 1;
        var lmap = new Dictionary<int, SortedSet<int>>();
        Process(arr, 0, half, 0, 0, lmap);
        var rmap = new Dictionary<int, SortedSet<int>>();
        Process(arr, half, size, 0, 0, rmap);
        var sum = 0;
        foreach (var num in arr) sum += num;

        var ans = int.MaxValue;
        foreach (var leftNum in lmap.Keys)
        foreach (var leftSum in lmap[leftNum])
        {
            int? rightSum = rmap[half - leftNum].LastOrDefault(x => x <= (sum >> 1) - leftSum);
            if (rightSum != null)
            {
                var pickSum = leftSum + rightSum ?? throw new InvalidOperationException();
                var restSum = sum - pickSum;
                ans = Math.Min(ans, restSum - pickSum);
            }
        }

        return ans;
    }


    // arr -> 8   0 1 2 3      4 5 6 7
    // Process(arr, 0, 4)  [0,4)
    // Process(arr, 4, 8)  [4,8)
    // arr[index....end-1]这个范围上，去做选择
    // pick挑了几个数！
    // sum挑的这些数，累加和是多少！
    // map记录结果
    // HashMap<Integer, TreeSet<Integer>> map
    // key -> 挑了几个数，比如挑了3个数，但是形成累加和可能多个！
    // value -> 有序表，都记下来！
    // 整个过程，纯暴力！2^15 -> 3万多，纯暴力跑完，依然很快！
    private static void Process(int[] arr, int index, int end, int pick, int sum, Dictionary<int, SortedSet<int>> map)
    {
        if (index == end)
        {
            if (!map.ContainsKey(pick)) map[pick] = new SortedSet<int>();

            map[pick].Add(sum);
        }
        else
        {
            Process(arr, index + 1, end, pick, sum, map);
            Process(arr, index + 1, end, pick + 1, sum + arr[index], map);
        }
    }

    //todo:输出结果有问题
    public static void Run()
    {
        Console.WriteLine(MinimumDifference([-36, 36])); //输出72
    }
}