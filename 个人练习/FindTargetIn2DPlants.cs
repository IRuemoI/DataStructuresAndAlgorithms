namespace CustomTraining;

public class FindTargetIn2DPlants
{
    public bool FindTargetIn2DPlantsCode(int[][] plants, int target)
    {
        var i = plants.Length - 1;
        var j = 0;
        while (i >= 0 && j < plants[0].Length)
            if (plants[i][j] > target) i--;
            else if (plants[i][j] < target) j++;
            else return true;

        return false;
    }
}