namespace AdvancedTraining.Lesson07;

// 本题测试链接 : https://leetcode.cn/problems/binary-tree-cameras/
public class MinCameraCover
{
    private static int MinCameraCover1(TreeNode root)
    {
        var data = Process1(root);
        return (int)Math.Min(data.Uncovered + 1, Math.Min(data.CoveredNoCamera, data.CoveredHasCamera));
    }

    // 所有可能性都穷尽了
    private static Info Process1(TreeNode? x)
    {
        if (x == null)
            // base case
            return new Info(int.MaxValue, 0, int.MaxValue);

        var left = Process1(x.Left);
        var right = Process1(x.Right);
        // x uncovered x自己不被覆盖，x下方所有节点，都被覆盖
        // 左孩子： 左孩子没被覆盖，左孩子以下的点都被覆盖
        // 左孩子被覆盖但没相机，左孩子以下的点都被覆盖
        // 左孩子被覆盖也有相机，左孩子以下的点都被覆盖
        var uncovered = left.CoveredNoCamera + right.CoveredNoCamera;

        // x下方的点都被covered，x也被cover，但x上没相机
        var coveredNoCamera = Math.Min(left.CoveredHasCamera + right.CoveredHasCamera,
            Math.Min(left.CoveredHasCamera + right.CoveredNoCamera, left.CoveredNoCamera + right.CoveredHasCamera));


        // x下方的点都被covered，x也被cover，且x上有相机
        var coveredHasCamera = Math.Min(left.Uncovered, Math.Min(left.CoveredNoCamera, left.CoveredHasCamera)) +
                               Math.Min(right.Uncovered, Math.Min(right.CoveredNoCamera, right.CoveredHasCamera)) + 1;

        return new Info(uncovered, coveredNoCamera, coveredHasCamera);
    }

    private static int MinCameraCover2(TreeNode root)
    {
        var data = Process2(root);
        return data.Cameras + (data.Status == Status.Uncovered ? 1 : 0);
    }

    private static Data Process2(TreeNode? x)
    {
        if (x == null) return new Data(Status.CoveredNoCamera, 0);
        var left = Process2(x.Left);
        var right = Process2(x.Right);
        var cameras = left.Cameras + right.Cameras;

        // 左、或右，哪怕有一个没覆盖
        if (left.Status == Status.Uncovered || right.Status == Status.Uncovered)
            return new Data(Status.CoveredHasCamera, cameras + 1);

        // 左右孩子，不存在没被覆盖的情况
        if (left.Status == Status.CoveredHasCamera || right.Status == Status.CoveredHasCamera)
            return new Data(Status.CoveredNoCamera, cameras);
        // 左右孩子，不存在没被覆盖的情况，也都没有相机
        return new Data(Status.Uncovered, cameras);
    }

    public static void Run()
    {
        //todo:需要重新设置测试用例
        Console.WriteLine(MinCameraCover1(new TreeNode(null, null)));
        Console.WriteLine(MinCameraCover2(new TreeNode(null, null)));
    }

    // 以x为头，x下方的节点都是被covered，x自己的状况，分三种
    private enum Status
    {
        Uncovered,
        CoveredNoCamera,
        CoveredHasCamera
    }

    public class TreeNode(TreeNode? right, TreeNode? left)
    {
        public readonly TreeNode? Left = left;
        public readonly TreeNode? Right = right;
        public int Value;
    }

    // 潜台词：x是头节点，x下方的点都被覆盖的情况下
    private class Info(long un, long no, long has)
    {
        public readonly long CoveredHasCamera = has; // x被相机覆盖了，并且x上放了相机，x为头的树至少需要几个相机
        public readonly long CoveredNoCamera = no; // x被相机覆盖，但是x没相机，x为头的树至少需要几个相机
        public readonly long Uncovered = un; // x没有被覆盖，x为头的树至少需要几个相机
    }

    // 以x为头，x下方的节点都是被covered，得到的最优解中：
    // x是什么状态，在这种状态下，需要至少几个相机
    private class Data(Status status, int cameras)
    {
        public readonly int Cameras = cameras;
        public readonly Status Status = status;
    }
}