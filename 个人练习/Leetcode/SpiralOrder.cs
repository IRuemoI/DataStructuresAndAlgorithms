namespace CustomTraining.Leetcode;

//leetcode:https://leetcode.cn/problems/spiral-matrix/
public class SpiralOrder
{
    private IList<int> SpiralOrderCode(int[][] matrix)
    {
        if (matrix.Length == 0) return [];

        var left = 0;
        var right = matrix[0].Length - 1;
        var top = 0;
        var bottom = matrix.Length - 1;

        var index = 0;
        IList<int> result = new int[(right + 1) * (bottom + 1)];
        while (true)
        {
            for (var i = left; i <= right; i++) result[index++] = matrix[top][i];

            top++;
            if (top > bottom) break;

            for (var i = top; i <= bottom; i++) result[index++] = matrix[i][right];

            right--;
            if (left > right) break;

            for (var i = right; i >= left; i--) result[index++] = matrix[bottom][i];

            bottom--;
            if (top > bottom) break;

            for (var i = bottom; i >= top; i--) result[index++] = matrix[i][left];

            left++;
            if (left > right) break;
        }

        return result;
    }
}