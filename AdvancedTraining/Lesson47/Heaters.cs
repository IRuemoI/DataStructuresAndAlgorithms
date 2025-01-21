#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson47;

public class Heaters //leetcode_0475
{
    // 比如地点是7, 9, 14
    // 供暖点的位置: 1 3 4 5 13 15 17
    // 先看地点7
    // 由1供暖，半径是6
    // 由3供暖，半径是4
    // 由4供暖，半径是3
    // 由5供暖，半径是2
    // 由13供暖，半径是6
    // 由此可知，地点7应该由供暖点5来供暖，半径是2
    // 再看地点9
    // 供暖点不回退
    // 由5供暖，半径是4
    // 由13供暖，半径是4
    // 由15供暖，半径是6
    // 由此可知，地点9应该由供暖点13来供暖，半径是4
    // 为什么是13而不是5？因为接下来的地点都会更靠右，所以半径一样的时候，就应该选更右的供暖点
    // 再看地点14
    // 供暖点不回退
    // 由13供暖，半径是1
    // 由15供暖，半径是1
    // 由17供暖，半径是3
    // 由此可知，地点14应该由供暖点15来供暖，半径是1
    // 以此类推
    private static int FindRadius(int[] houses, int[] heaters)
    {
        Array.Sort(houses);
        Array.Sort(heaters);
        var ans = 0;
        // 时间复杂度O(N)
        // i是地点，j是供暖点
        for (int i = 0, j = 0; i < houses.Length; i++)
        {
            while (!Best(houses, heaters, i, j)) j++;
            ans = Math.Max(ans, Math.Abs(heaters[j] - houses[i]));
        }

        return ans;
    }

    // 这个函数含义：
    // 当前的地点houses[i]由heaters[j]来供暖是最优的吗？
    // 当前的地点houses[i]由heaters[j]来供暖，产生的半径是a
    // 当前的地点houses[i]由heaters[j + 1]来供暖，产生的半径是b
    // 如果a < b, 说明是最优，供暖不应该跳下一个位置
    // 如果a >= b, 说明不是最优，应该跳下一个位置
    private static bool Best(int[] houses, int[] heaters, int i, int j)
    {
        return j == heaters.Length - 1 || Math.Abs(heaters[j] - houses[i]) < Math.Abs(heaters[j + 1] - houses[i]);
    }

    // 下面这个方法不对，你能找出原因嘛？^_^
    private static int FindRadius2(int[] houses, int[] heaters)
    {
        Array.Sort(houses);
        Array.Sort(heaters);
        var ans = 0;
        // 时间复杂度O(N)
        // i是地点，j是供暖点
        for (int i = 0, j = 0; i < houses.Length; i++)
        {
            while (!Best2(houses, heaters, i, j)) j++;
            ans = Math.Max(ans, Math.Abs(heaters[j] - houses[i]));
        }

        return ans;
    }

    private static bool Best2(int[] houses, int[] heaters, int i, int j)
    {
        return j == heaters.Length - 1 || Math.Abs(heaters[j] - houses[i]) <= Math.Abs(heaters[j + 1] - houses[i]);
    }

    // 为了测试
    private static int[] randomArray(int len, int v)
    {
        var arr = new int[len];
        for (var i = 0; i < len; i++) arr[i] = (int)(Utility.getRandomDouble * v) + 1;
        return arr;
    }

    // 为了测试
    public static void Run()
    {
        var len = 5;
        var v = 10;
        var testTime = 10000;
        for (var i = 0; i < testTime; i++)
        {
            var a = randomArray(len, v);
            var b = randomArray(len, v);
            var ans1 = FindRadius(a, b);
            var ans2 = FindRadius2(a, b);
            if (ans1 != ans2)
            {
                Console.WriteLine("A : ");
                foreach (var num in a) Console.Write(num + " ");
                Console.WriteLine();
                Console.WriteLine("B : ");
                foreach (var num in b) Console.Write(num + " ");
                Console.WriteLine();
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                break;
            }
        }
    }
}