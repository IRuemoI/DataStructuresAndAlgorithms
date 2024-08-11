namespace AdvancedTraining.Lesson42;

//https://www.cnblogs.com/songchengyu/p/15091107.html
public class BestMeetingPoint //Problem_0296
{
    private static int MinTotalDistance(int[][] grid)
    {
        var n = grid.Length;
        var m = grid[0].Length;
        var iOnes = new int[n];
        var jOnes = new int[m];
        for (var i = 0; i < n; i++)
        for (var j = 0; j < m; j++)
            if (grid[i][j] == 1)
            {
                iOnes[i]++;
                jOnes[j]++;
            }

        var total = 0;
        var i1 = 0;
        var j1 = n - 1;
        var iRest = 0;
        var jRest = 0;
        while (i1 < j1)
            if (iOnes[i1] + iRest <= iOnes[j1] + jRest)
            {
                total += iOnes[i1] + iRest;
                iRest += iOnes[i1++];
            }
            else
            {
                total += iOnes[j1] + jRest;
                jRest += iOnes[j1--];
            }

        i1 = 0;
        j1 = m - 1;
        iRest = 0;
        jRest = 0;
        while (i1 < j1)
            if (jOnes[i1] + iRest <= jOnes[j1] + jRest)
            {
                total += jOnes[i1] + iRest;
                iRest += jOnes[i1++];
            }
            else
            {
                total += jOnes[j1] + jRest;
                jRest += jOnes[j1--];
            }

        return total;
    }
}