namespace AdvancedTraining.Lesson14;
// 本题测试链接 : https://www.nowcoder.com/practice/e13bceaca5b14860b83cbcc4912c5d4a
// 提交以下的代码，并把主类名改成Main
// 可以直接通过

public class BiggestBstTopologyInTree
{
    public static void Run()
    {
        Console.WriteLine("测试BiggestBSTTopology算法：");

        // 测试1：完整的BST树
        var tree1 = new[,]
        {
            { 2, 1, 3 },
            { 1, 0, 0 },
            { 3, 0, 0 }
        };
        var result1 = MaxBstTopology(2, tree1, new int[4]);
        Console.WriteLine($"测试1 - BST树(2为根，左1右3): {result1} (期望: 3)");

        // 测试2：非BST树
        var tree2 = new[,]
        {
            { 2, 1, 3 },
            { 1, 0, 0 },
            { 3, 0, 0 }
        };
        // 修改为非BST：让左孩子大于根节点
        tree2[2, 1] = 4; // 3的左孩子是4
        tree2[4, 0] = 3; // 4的父节点是3
        var result2 = MaxBstTopology(2, tree2, new int[5]);
        Console.WriteLine($"测试2 - 非BST树(2为根，左1右3，3的左孩子4): {result2} (期望: 2, 只包含2和1)");

        // 测试3：更大的BST树
        var tree3 = new[,]
        {
            { 5, 2, 8 },
            { 2, 1, 3 },
            { 8, 0, 0 },
            { 1, 0, 0 },
            { 3, 0, 0 }
        };
        var result3 = MaxBstTopology(5, tree3, new int[6]);
        Console.WriteLine($"测试3 - BST树(5为根，左2右8): {result3} (期望: 5, 包含所有节点)");

        Console.WriteLine("所有测试完成！");
    }

    // h: 代表当前的头节点
    // t: 代表树 t[i,0]是i节点的父节点、t[i,1]是i节点的左孩子、t[i,2]是i节点的右孩子
    // m: i节点为头的最大bst拓扑结构大小 -> m[i]
    // 返回: 以h为头的整棵树上，最大bst拓扑结构的大小
    private static int MaxBstTopology(int h, int[,] t, int[] m)
    {
        if (h == 0) return 0;
        var l = t[h, 1];
        var r = t[h, 2];
        int c;
        var p1 = MaxBstTopology(l, t, m);
        var p2 = MaxBstTopology(r, t, m);
        while (l < h && m[l] != 0) l = t[l, 2];
        if (m[l] != 0)
        {
            c = m[l];
            while (l != h)
            {
                m[l] -= c;
                l = t[l, 0];
            }
        }

        while (r > h && m[r] != 0) r = t[r, 1];
        if (m[r] != 0)
        {
            c = m[r];
            while (r != h)
            {
                m[r] -= c;
                r = t[r, 0];
            }
        }

        // 直接使用Java版本的逻辑
        m[h] = m[t[h, 1]] + m[t[h, 2]] + 1;
        return Math.Max(Math.Max(p1, p2), m[h]);
    }
}