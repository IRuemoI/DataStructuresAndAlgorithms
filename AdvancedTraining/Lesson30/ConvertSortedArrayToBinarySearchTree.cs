namespace AdvancedTraining.Lesson30;

public class ConvertSortedArrayToBinarySearchTree //Problem_0108
{
    public virtual TreeNode? SortedArrayToBst(int[] numbers)
    {
        return Process(numbers, 0, numbers.Length - 1);
    }

    private static TreeNode? Process(int[] nums, int l, int r)
    {
        if (l > r) return null;
        if (l == r) return new TreeNode(nums[l]);
        var m = (l + r) / 2;
        var head = new TreeNode(nums[m])
        {
            Left = Process(nums, l, m - 1),
            Right = Process(nums, m + 1, r)
        };
        return head;
    }


    //todo:待整理
    public static void Run()
    {
    }

    public class TreeNode(int v)
    {
        public TreeNode? Left;
        public TreeNode? Right;
        public int Val = v;
    }
}