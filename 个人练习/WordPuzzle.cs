namespace CustomTraining;

public class WordPuzzle
{
    public bool WordPuzzleCode(char[][] grid, string target)
    {
        var words = target.ToCharArray();
        for (var i = 0; i < grid.Length; i++)
        for (var j = 0; j < grid[0].Length; j++)
            if (DFS(grid, words, i, j, 0))
                return true;

        return false;
    }

    private bool DFS(char[][] grid, char[] target, int i, int j, int k)
    {
        if (i >= grid.Length || i < 0 || j >= grid[0].Length || j < 0 || grid[i][j] != target[k]) return false;

        if (k == target.Length - 1) return true;

        grid[i][j] = '\0';
        var res = DFS(grid, target, i + 1, j, k + 1) || DFS(grid, target, i - 1, j, k + 1) || DFS(grid, target, i, j + 1, k + 1) || DFS(grid, target, i, j - 1, k + 1);
        grid[i][j] = target[k];

        return res;
    }
}