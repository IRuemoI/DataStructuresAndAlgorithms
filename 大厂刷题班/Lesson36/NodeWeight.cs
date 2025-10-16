namespace AdvancedTraining.Lesson36;

// 来自美团
// 有一棵树，给定头节点h，和结构数组m，下标0弃而不用
// 比如h = 1, m = [ [] , [2,3], [4], [5,6], [], [], []]
// 表示1的孩子是2、3; 2的孩子是4; 3的孩子是5、6; 4、5和6是叶节点，都不再有孩子
// 每一个节点都有颜色，记录在c数组里，比如c[i] = 4, 表示节点i的颜色为4
// 一开始只有叶节点是有权值的，记录在w数组里，
// 比如，如果一开始就有w[i] = 3, 表示节点i是叶节点、且权值是3
// 现在规定非叶节点i的权值计算方式：
// 根据i的所有直接孩子来计算，假设i的所有直接孩子，颜色只有a,b,k
// w[i] = Max {
//              (颜色为a的所有孩子个数 + 颜色为a的孩子权值之和), 
//              (颜色为b的所有孩子个数 + 颜色为b的孩子权值之和),
//              (颜色为k的所有孩子个数 + 颜色k的孩子权值之和)
//            }
// 请计算所有孩子的权值并返回
// https://www.cnblogs.com/moonfdd/p/17395296.html
public class NodeWeight
{
    // 当前来到h节点，
    // h的直接孩子，在哪呢？m[h] = {a,b,c,d,e}
    // 每个节点的颜色在哪？比如i号节点，c[i]就是i号节点的颜色
    // 每个节点的权值在哪？比如i号节点，w[i]就是i号节点的权值
    // void : 把w数组填满就是这个函数的目标
    private static void W(int h, int[][] m, int[] w, int[] c)
    {
        if (m[h].Length == 0)
            // 叶节点
            return;

        // 有若干个直接孩子
        // 1 7个
        // 3 10个
        var colors = new Dictionary<int, int>();
        // 1 20
        // 3 45
        var weights = new Dictionary<int, int>();
        foreach (var child in m[h])
        {
            W(child, m, w, c);
            colors[c[child]] = colors.GetValueOrDefault(c[child], 0) + 1;
            weights[c[child]] = weights.GetValueOrDefault(c[child], 0) + w[child];
        }

        foreach (var color in colors.Keys) w[h] = Math.Max(w[h], colors[color] + weights[color]);
    }

    //todo:待修复
    public static void Run()
    {
        const int h = 1;
        int[][] m = [[], [2, 3], [4], [5, 6], [], [], []];
        int[] w = [0, 0, 0, 4, 5, 6, 0];
        int[] c = [0, 0, 0, 4, 3, 2, 0];
        W(h, m, w, c);

        Console.WriteLine(string.Join(",", w));
        Console.WriteLine(string.Join(",", c));
    }
}