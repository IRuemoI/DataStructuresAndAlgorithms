namespace CustomTraining;

public class InventoryManagement
{
    public int InventoryManagementCode(int[] stock)
    {
        int i = 0;
        int j = stock.Length - 1;
        while (i < j)
        {
            int m = (i + j) / 2;
            if (stock[m] > stock[j])
            {
                i = m + 1;
            }
            else if (stock[m] < stock[j])
            {
                j = m;
            }
            else
            {
                j--;
            }
        }

        return stock[i];
    }
}