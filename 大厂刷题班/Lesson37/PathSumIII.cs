namespace AdvancedTraining.Lesson37;

//https://leetcode.cn/problems/path-sum-iii/description/
public class PathSumIii //leetcode_0437
{
    private static int PathSum(TreeNode root, int sum)
    {
        var preSumMap = new Dictionary<int, int>
        {
            [0] = 1
        };
        return Process(root, sum, 0, preSumMap);
    }

    // 返回方法数
    private static int Process(TreeNode? x, int sum, int preAll, Dictionary<int, int> preSumMap)
    {
        if (x == null) return 0;
        var all = preAll + x.Val;
        var ans = 0;
        if (preSumMap.ContainsKey(all - sum)) ans = preSumMap[all - sum];
        if (!preSumMap.TryAdd(all, 1))
            preSumMap[all] = preSumMap[all] + 1;
        ans += Process(x.Left, sum, all, preSumMap);
        ans += Process(x.Right, sum, all, preSumMap);
        if (preSumMap[all] == 1)
            preSumMap.Remove(all);
        else
            preSumMap[all] -= 1;
        return ans;
    }

    //todo:待整理
    public static void Run()
    {
    }

    public class TreeNode
    {
        public TreeNode? Left;
        public TreeNode? Right;
        public int Val;
    }
}