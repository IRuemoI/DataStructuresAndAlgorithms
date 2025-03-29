namespace CustomTraining;

public class InventoryManagement
{
    public int InventoryManagementCode(int[] stock)
    {
        var i = 0;
        var j = stock.Length - 1;
        while (i < j)
        {
            var m = (i + j) / 2;
            if (stock[m] > stock[j])
                i = m + 1;
            else if (stock[m] < stock[j])
                j = m;
            else
                j--;
        }

        return stock[i];
    }
}