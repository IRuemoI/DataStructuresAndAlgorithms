namespace Algorithms.Lesson15;

// 本题为leetcode原题
// 测试链接：https://leetcode.cn/problems/friend-circles/
// 可以直接通过
public abstract class FriendCircles
{
    public static int FindCircleNum(int[,] m)
    {
        var n = m.GetLength(0);
        // {0} {1} {2} {N-1}
        var unionFind = new UnionFind(n);
        for (var i = 0; i < n; i++)
        for (var j = i + 1; j < n; j++)
            if (m[i, j] == 1)
                // i和j互相认识
                unionFind.Union(i, j);

        return unionFind.Sets();
    }

    private class UnionFind
    {
        // 辅助结构
        private readonly int[] _help;

        // parent[i] = k ： i的父亲是k
        private readonly int[] _parent;

        // size[i] = k ： 如果i是代表节点，size[i]才有意义，否则无意义
        // i所在的集合大小是多少
        private readonly int[] _size;

        // 一共有多少个集合
        private int _sets;

        public UnionFind(int n)
        {
            _parent = new int[n];
            _size = new int[n];
            _help = new int[n];
            _sets = n;
            for (var i = 0; i < n; i++)
            {
                _parent[i] = i;
                _size[i] = 1;
            }
        }

        // 从i开始一直往上，往上到不能再往上，代表节点，返回
        // 这个过程要做路径压缩
        private int Find(int i)
        {
            var hi = 0;
            while (i != _parent[i])
            {
                _help[hi++] = i;
                i = _parent[i];
            }

            for (hi--; hi >= 0; hi--) _parent[_help[hi]] = i;

            return i;
        }

        public void Union(int i, int j)
        {
            var f1 = Find(i);
            var f2 = Find(j);
            if (f1 != f2)
            {
                if (_size[f1] >= _size[f2])
                {
                    _size[f1] += _size[f2];
                    _parent[f2] = f1;
                }
                else
                {
                    _size[f2] += _size[f1];
                    _parent[f1] = f2;
                }

                _sets--;
            }
        }

        public int Sets()
        {
            return _sets;
        }
    }
}

public class FriendCircleTest
{
    public static void Run()
    {
        int[,] a =
        {
            { 1, 1, 0 },
            { 1, 1, 0 },
            { 0, 0, 1 }
        };

        int[,] b =
        {
            { 1, 1, 0 },
            { 1, 1, 1 },
            { 0, 1, 1 }
        };
        Console.WriteLine(FriendCircles.FindCircleNum(a)); //输出2 
        Console.WriteLine(FriendCircles.FindCircleNum(b)); //输出1

        #region 关于二维数组的各个维度长度如何获取

        int[,,] c =
        {
            {
                { 1, 3, 4 },
                { 2, 4, 1 }
            },
            {
                { 1, 3, 4 },
                { 2, 4, 1 }
            },
            {
                { 1, 3, 4 },
                { 2, 4, 1 }
            },
            {
                { 1, 3, 4 },
                { 2, 4, 1 }
            }
        };
        Console.WriteLine("矩形数组获取各个维度的长度:"); //[0,1,2]看上面数组的构造方式，在外层先被构造是第0维，然后是第1维，然后是第2维
        Console.WriteLine(c.GetLength(0)); //输出4
        Console.WriteLine(c.GetLength(1)); //输出2
        Console.WriteLine(c.GetLength(2)); //输出3

        #endregion
    }
}