namespace AdvancedTraining.Lesson14;
// 本题测试链接 : https://www.nowcoder.com/practice/e13bceaca5b14860b83cbcc4912c5d4a
// 提交以下的代码，并把主类名改成Main
// 可以直接通过

public class BiggestBstTopologyInTree
{
    public static void Run()
    {
        var n = Console.Read();
        var h = Console.Read();
        var tree = new int[n + 1, 3];
        for (var i = 1; i <= n; i++)
        {
            var c = Console.Read();
            var l = Console.Read();
            var r = Console.Read();
            tree[l, 0] = c;
            tree[r, 0] = c;
            tree[c, 1] = l;
            tree[c, 2] = r;
        }

        Console.WriteLine(MaxBstTopology(h, tree, new int[n + 1]));
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

        m[h] = m[t[h, 1]] + m[t[h, 2]] + 1;
        return Math.Max(Math.Max(p1, p2), m[h]);
    }
}