namespace AdvancedTraining.Lesson46;

//https://leetcode.cn/problems/max-sum-of-rectangle-no-larger-than-k/description/
public class MaxSumOfRectangleNoLargerThanK //Problem_0363
{
    private static int NearK(int[]? arr, int k)
    {
        if (arr == null || arr.Length == 0) return int.MinValue;

        var set = new SortedSet<int> { 0 };
        var ans = int.MinValue;
        var sum = 0;
        foreach (var item in arr)
        {
            // 讨论子数组必须以i位置结尾，最接近k的累加和是多少？
            sum += item;
            // 找之前哪个前缀和 >= sum - k  且最接近
            // 有序表中，ceiling(x) 返回>=x且最接近的！
            // 有序表中，floor(x) 返回<=x且最接近的！
            int? find = set.FirstOrDefault(x => x <= sum - k);
            if (find != null)
            {
                var curAns = sum - find ?? throw new InvalidOperationException();
                ans = Math.Max(ans, curAns);
            }

            set.Add(sum);
        }

        return ans;
    }

    private static int MaxSumSubMatrix(int[,]? matrix, int k)
    {
        //判断条件不正确
        //if (matrix == null || matrix[0] == null)
        if (matrix == null) return 0;

        if (matrix.Length > matrix.GetLength(1)) matrix = Rotate(matrix);

        var row = matrix.GetLength(0);
        var col = matrix.GetLength(1);
        var res = int.MinValue;
        var sumSet = new SortedSet<int>();
        for (var s = 0; s < row; s++)
        {
            var colSum = new int[col];
            for (var e = s; e < row; e++)
            {
                // s ~ e 这些行  选的子矩阵必须包含、且只包含s行~e行的数据
                // 0 ~ 0  0 ~ 1  0 ~ 2 。。。
                // 1 ~ 2  1 ~ 2  1 ~ 3 。。。
                sumSet.Add(0);
                var rowSum = 0;
                for (var c = 0; c < col; c++)
                {
                    colSum[c] += matrix[e, c];
                    rowSum += colSum[c];
                    int? it = sumSet.FirstOrDefault(x => x <= rowSum - k);
                    if (it != null) res = Math.Max(res, rowSum - it ?? throw new InvalidOperationException());

                    sumSet.Add(rowSum);
                }

                sumSet.Clear();
            }
        }

        return res;
    }

    private static int[,] Rotate(int[,] matrix)
    {
        var n = matrix.GetLength(0);
        var m = matrix.GetLength(1);
        var r = new int[m, n];
        for (var i = 0; i < n; i++)
        for (var j = 0; j < m; j++)
            r[j, i] = matrix[i, j];

        return r;
    }

    //todo:输出有问题
    public static void Run()
    {
        int[,] m =
        {
            { 2, 2, -1 }
        };
        Console.WriteLine(MaxSumSubMatrix(m, 3)); //输出3
    }
}