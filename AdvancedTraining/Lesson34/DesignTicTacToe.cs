namespace AdvancedTraining.Lesson34;

//https://www.cnblogs.com/lautsie/p/12271195.html
//todo:待整理
public class DesignTicTacToe //leetcode_0348
{
    internal class TicTacToe(int n)
    {
        private readonly int[,] _cols = new int[n, 3];
        private readonly int[] _leftUp = new int[3];
        private readonly bool[,] _matrix = new bool[n, n];
        private readonly int[] _rightUp = new int[3];

        private readonly int[,] _rows = new int[n, 3]; //0 1 2

        // rows[a,1] : 1这个人，在a行上，下了几个
        // rows[b,2] : 2这个人，在b行上，下了几个
        // leftUp[2] = 7 : 2这个人，在左对角线上，下了7个
        // rightUp[1] = 9 : 1这个人，在右对角线上，下了9个

        public virtual int Move(int row, int col, int player)
        {
            if (_matrix[row, col]) return 0;
            _matrix[row, col] = true;
            _rows[row, player]++;
            _cols[col, player]++;
            if (row == col) _leftUp[player]++;
            if (row + col == n - 1) _rightUp[player]++;
            if (_rows[row, player] == n || _cols[col, player] == n || _leftUp[player] == n ||
                _rightUp[player] == n) return player;
            return 0;
        }
    }
}