#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson15;

// 本题为leetcode原题
// 测试链接：https://leetcode.cn/problems/number-of-islands/
// 所有方法都可以直接通过
public class NumberOfIslands
{
    private static int NumIslands3(char[,] board)
    {
        var islands = 0;
        for (var i = 0; i < board.GetLength(0); i++)
        for (var j = 0; j < board.GetLength(1); j++)
            if (board[i, j] == '1')
            {
                islands++;
                Infect(board, i, j);
            }

        return islands;
    }

    // 从(i,j)这个位置出发，把所有练成一片的'1'字符，变成0
    private static void Infect(char[,] board, int i, int j)
    {
        if (i < 0 || i == board.GetLength(0) || j < 0 || j == board.GetLength(1) || board[i, j] != '1') return;

        board[i, j] = '0';
        Infect(board, i - 1, j);
        Infect(board, i + 1, j);
        Infect(board, i, j - 1);
        Infect(board, i, j + 1);
    }

    private static int NumIslands1(char[,] board)
    {
        var row = board.GetLength(0);
        var col = board.GetLength(1);
        var dots = new Dot[row, col];
        var dotList = new List<Dot>();
        for (var i = 0; i < row; i++)
        for (var j = 0; j < col; j++)
            if (board[i, j] == '1')
            {
                dots[i, j] = new Dot();
                dotList.Add(dots[i, j]);
            }

        var uf = new UnionFind1<Dot>(dotList);
        for (var j = 1; j < col; j++)
            // (0,j)  (0,0)跳过了  (0,1) (0,2) (0,3)
            if (board[0, j - 1] == '1' && board[0, j] == '1')
                uf.Union(dots[0, j - 1], dots[0, j]);

        for (var i = 1; i < row; i++)
            if (board[i - 1, 0] == '1' && board[i, 0] == '1')
                uf.Union(dots[i - 1, 0], dots[i, 0]);

        for (var i = 1; i < row; i++)
        for (var j = 1; j < col; j++)
            if (board[i, j] == '1')
            {
                if (board[i, j - 1] == '1') uf.Union(dots[i, j - 1], dots[i, j]);

                if (board[i - 1, j] == '1') uf.Union(dots[i - 1, j], dots[i, j]);
            }

        return uf.Sets();
    }

    private static int NumIslands2(char[,] board)
    {
        var row = board.GetLength(0);
        var col = board.GetLength(1);
        var uf = new UnionFind2(board);
        for (var j = 1; j < col; j++)
            if (board[0, j - 1] == '1' && board[0, j] == '1')
                uf.Union(0, j - 1, 0, j);

        for (var i = 1; i < row; i++)
            if (board[i - 1, 0] == '1' && board[i, 0] == '1')
                uf.Union(i - 1, 0, i, 0);

        for (var i = 1; i < row; i++)
        for (var j = 1; j < col; j++)
            if (board[i, j] == '1')
            {
                if (board[i, j - 1] == '1') uf.Union(i, j - 1, i, j);

                if (board[i - 1, j] == '1') uf.Union(i - 1, j, i, j);
            }

        return uf.Sets();
    }

    // 为了测试
    private static char[,] GenerateRandomMatrix(int row, int col)
    {
        var board = new char[row, col];
        for (var i = 0; i < row; i++)
        for (var j = 0; j < col; j++)
            board[i, j] = Utility.GetRandomDouble < 0.5 ? '1' : '0';

        return board;
    }

    // 为了测试
    private static char[,] Copy(char[,] board)
    {
        var row = board.GetLength(0);
        var col = board.GetLength(1);
        var ans = new char[row, col];
        for (var i = 0; i < row; i++)
        for (var j = 0; j < col; j++)
            ans[i, j] = board[i, j];

        return ans;
    }

    // 为了测试
    public static void Run()
    {
        var row = 1000;
        var col = 1001;
        var board1 = GenerateRandomMatrix(row, col);
        var board2 = Copy(board1);
        var board3 = Copy(board1);
        Console.WriteLine("感染方法、并查集(map实现)、并查集(数组实现)的运行结果和运行时间");
        Console.WriteLine("随机生成的二维矩阵规模 : " + row + " * " + col);

        Utility.RestartStopwatch();
        Console.WriteLine("感染方法的运行结果: " + NumIslands3(board1));

        Console.WriteLine("感染方法的运行时间: " + Utility.GetStopwatchElapsedMilliseconds() + " ms");
        Console.WriteLine("-------------------------");

        Utility.RestartStopwatch();
        Console.WriteLine("并查集(map实现)的运行结果: " + NumIslands1(board2));

        Console.WriteLine("并查集(map实现)的运行时间: " + Utility.GetStopwatchElapsedMilliseconds() + " ms");

        Utility.RestartStopwatch();
        Console.WriteLine("并查集(数组实现)的运行结果: " + NumIslands2(board3));

        Console.WriteLine("并查集(数组实现)的运行时间: " + Utility.GetStopwatchElapsedMilliseconds() + " ms");

        Console.WriteLine("-------------------------");

        row = 10000;
        col = 10001;
        board1 = GenerateRandomMatrix(row, col);
        board3 = Copy(board1);
        Console.WriteLine("感染方法、并查集(数组实现)的运行结果和运行时间");
        Console.WriteLine("随机生成的二维矩阵规模 : " + row + " * " + col);

        Utility.RestartStopwatch();
        Console.WriteLine("感染方法的运行结果: " + NumIslands3(board1));

        Console.WriteLine("感染方法的运行时间: " + Utility.GetStopwatchElapsedMilliseconds() + " ms");

        Utility.RestartStopwatch();
        Console.WriteLine("并查集(数组实现)的运行结果: " + NumIslands2(board3));

        Console.WriteLine("并查集(数组实现)的运行时间: " + Utility.GetStopwatchElapsedMilliseconds() + " ms");
    }

    private class Dot
    {
    }

    private class Node<T>
    {
        private T _value;

        public Node(T v)
        {
            _value = v;
        }
    }

    private class UnionFind1<T> where T : notnull
    {
        private readonly Dictionary<T, Node<T>> _nodes;
        private readonly Dictionary<Node<T>, Node<T>> _parents;
        private readonly Dictionary<Node<T>, int> _sizeMap;

        public UnionFind1(List<T> values)
        {
            _nodes = new Dictionary<T, Node<T>>();
            _parents = new Dictionary<Node<T>, Node<T>>();
            _sizeMap = new Dictionary<Node<T>, int>();
            foreach (var cur in values)
            {
                Node<T> node = new(cur);
                _nodes.Add(cur, node);
                _parents.Add(node, node);
                _sizeMap.Add(node, 1);
            }
        }

        private Node<T> FindFather(Node<T> cur)
        {
            Stack<Node<T>> path = new();
            while (cur != _parents[cur])
            {
                path.Push(cur);
                cur = _parents[cur];
            }

            while (path.Count != 0) _parents[path.Pop()] = cur;

            return cur;
        }

        public void Union(T a, T b)
        {
            var aHead = FindFather(_nodes[a]);
            var bHead = FindFather(_nodes[b]);
            if (aHead != bHead)
            {
                var aSetSize = _sizeMap[aHead];
                var bSetSize = _sizeMap[bHead];
                var big = aSetSize >= bSetSize ? aHead : bHead;
                var small = big == aHead ? bHead : aHead;
                _parents[small] = big;
                _sizeMap[big] = aSetSize + bSetSize;
                _sizeMap.Remove(small);
            }
        }

        public int Sets()
        {
            return _sizeMap.Count;
        }
    }

    private class UnionFind2
    {
        private readonly int _col;
        private readonly int[] _help;
        private readonly int[] _parent;
        private readonly int[] _size;
        private int _sets;

        public UnionFind2(char[,] board)
        {
            _col = board.GetLength(1);
            _sets = 0;
            var row = board.GetLength(0);
            var len = row * _col;
            _parent = new int[len];
            _size = new int[len];
            _help = new int[len];
            for (var r = 0; r < row; r++)
            for (var c = 0; c < _col; c++)
                if (board[r, c] == '1')
                {
                    var i = Index(r, c);
                    _parent[i] = i;
                    _size[i] = 1;
                    _sets++;
                }
        }

        // (r,c) -> i
        private int Index(int r, int c)
        {
            return r * _col + c;
        }

        // 原始位置 -> 下标
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

        public void Union(int r1, int c1, int r2, int c2)
        {
            var i1 = Index(r1, c1);
            var i2 = Index(r2, c2);
            var f1 = Find(i1);
            var f2 = Find(i2);
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