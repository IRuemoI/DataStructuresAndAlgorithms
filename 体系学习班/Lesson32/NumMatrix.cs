namespace Algorithms.Lesson32;

// 测试链接：https://leetcode.cn/problems/range-sum-query-2d-mutable
// 但这个题是付费题目
public class NumMatrix //IndexTree2D
{
    private readonly int _m;
    private readonly int _n;
    private readonly int[,]? _nums;
    private readonly int[,]? _tree;

    public NumMatrix(int[,] matrix)
    {
        if (matrix.Length == 0 || matrix.GetLength(0) == 0) return;

        _n = matrix.GetLength(0);
        _m = matrix.GetLength(1);
        _tree = new int[_n + 1, _m + 1];
        _nums = new int[_n, _m];
        for (var i = 0; i < _n; i++)
        for (var j = 0; j < _m; j++)
            Update(i, j, matrix[i, j]);
    }

    private int Sum(int row, int col)
    {
        var sum = 0;
        for (var i = row + 1; i > 0; i -= i & -i)
        for (var j = col + 1; j > 0; j -= j & -j)
            if (_tree != null)
                sum += _tree[i, j];

        return sum;
    }

    public void Update(int row, int col, int val)
    {
        if (_n == 0 || _m == 0) return;

        if (_nums != null)
        {
            var add = val - _nums[row, col];
            _nums[row, col] = val;
            for (var i = row + 1; i <= _n; i += i & -i)
            for (var j = col + 1; j <= _m; j += j & -j)
                if (_tree != null)
                    _tree[i, j] += add;
        }
    }

    public int SumRegion(int row1, int col1, int row2, int col2)
    {
        if (_n == 0 || _m == 0) return 0;

        return Sum(row2, col2) + Sum(row1 - 1, col1 - 1) - Sum(row1 - 1, col2) - Sum(row2, col1 - 1);
    }
}

public class IndexTree2DTest
{
    public static void Run()
    {
        int[,] matrix =
        {
            { 3, 0, 1, 4, 2 },
            { 5, 6, 3, 2, 1 },
            { 1, 2, 0, 1, 5 },
            { 4, 1, 0, 1, 7 },
            { 1, 0, 3, 0, 5 }
        };
        var a = new NumMatrix(matrix);
        Console.WriteLine(a.SumRegion(2, 1, 4, 3)); //8
        a.Update(3, 2, 2);
        Console.WriteLine(a.SumRegion(2, 1, 4, 3)); //10
    }
}