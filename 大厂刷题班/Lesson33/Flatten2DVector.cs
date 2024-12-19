namespace AdvancedTraining.Lesson33;
//pass
//https://www.cnblogs.com/fenshen371/p/5806352.html
public class Flatten2DVector //leetcode_0251
{
    public static void Run()
    {
        int[][] arr =
        [
            [1, 2],
            [3],
            [4, 5, 6]
        ];
        var result = new Vector2D(arr);

        while (result.HasNext()) Console.Write(result.Next() + ",");
    }

    private class Vector2D
    {
        private readonly int[][] _matrix;
        private int _col;
        private bool _curUse;
        private int _row;

        public Vector2D(int[][] v)
        {
            _matrix = v;
            _row = 0;
            _col = -1;
            _curUse = true;
            HasNext();
        }

        public int Next()
        {
            var ans = _matrix[_row][_col];
            _curUse = true;
            HasNext();
            return ans;
        }

        public bool HasNext()
        {
            if (_row == _matrix.Length) return false;
            if (!_curUse) return true;
            // (row，col)用过了
            if (_col < _matrix[_row].Length - 1)
            {
                _col++;
            }
            else
            {
                _col = 0;
                do
                {
                    _row++;
                } while (_row < _matrix.Length && _matrix[_row].Length == 0);
            }

            // 新的(row，col)
            if (_row != _matrix.Length)
            {
                _curUse = false;
                return true;
            }

            return false;
        }
    }
}