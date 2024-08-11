//测试通过

namespace Algorithms.Lesson18;

public class CardsInLine
{
    // 根据规则，返回获胜者的分数
    private static int Win1(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;

        var first = OnFirst1(arr, 0, arr.Length - 1);
        var second = OnSecond2(arr, 0, arr.Length - 1);
        return Math.Max(first, second);
    }

    // arr[L..R]，先手获得的最好分数返回
    private static int OnFirst1(int[] arr, int l, int r)
    {
        if (l == r) return arr[l];

        var p1 = arr[l] + OnSecond2(arr, l + 1, r);
        var p2 = arr[r] + OnSecond2(arr, l, r - 1);
        return Math.Max(p1, p2);
    }

    // // arr[L..R]，后手获得的最好分数返回
    private static int OnSecond2(int[] arr, int l, int r)
    {
        if (l == r) return 0;

        var p1 = OnFirst1(arr, l + 1, r); // 对手拿走了L位置的数
        var p2 = OnFirst1(arr, l, r - 1); // 对手拿走了R位置的数
        return Math.Min(p1, p2);
    }

    private static int Win2(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;

        var n = arr.Length;
        var fMap = new int[n, n];
        var gMap = new int[n, n];
        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
        {
            fMap[i, j] = -1;
            gMap[i, j] = -1;
        }

        var first = OnFirst2(arr, 0, arr.Length - 1, fMap, gMap);
        var second = OnSecond2(arr, 0, arr.Length - 1, fMap, gMap);
        return Math.Max(first, second);
    }

    // arr[L..R]，先手获得的最好分数返回
    private static int OnFirst2(int[] arr, int l, int r, int[,] fMap, int[,] gMap)
    {
        if (fMap[l, r] != -1) return fMap[l, r];

        int ans;
        if (l == r)
        {
            ans = arr[l];
        }
        else
        {
            var p1 = arr[l] + OnSecond2(arr, l + 1, r, fMap, gMap);
            var p2 = arr[r] + OnSecond2(arr, l, r - 1, fMap, gMap);
            ans = Math.Max(p1, p2);
        }

        fMap[l, r] = ans;
        return ans;
    }

    // // arr[L..R]，后手获得的最好分数返回
    private static int OnSecond2(int[] arr, int l, int r, int[,] fMap, int[,] gMap)
    {
        if (gMap[l, r] != -1) return gMap[l, r];

        var ans = 0;
        if (l != r)
        {
            var p1 = OnFirst2(arr, l + 1, r, fMap, gMap); // 对手拿走了L位置的数
            var p2 = OnFirst2(arr, l, r - 1, fMap, gMap); // 对手拿走了R位置的数
            ans = Math.Min(p1, p2);
        }

        gMap[l, r] = ans;
        return ans;
    }

    private static int Win3(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;

        var n = arr.Length;
        var firstTable = new int[n, n];
        var secondTable = new int[n, n];
        for (var i = 0; i < n; i++) firstTable[i, i] = arr[i];

        for (var startCol = 1; startCol < n; startCol++)
        {
            var l = 0;
            var r = startCol;
            while (r < n)
            {
                firstTable[l, r] = Math.Max(arr[l] + secondTable[l + 1, r], arr[r] + secondTable[l, r - 1]);
                secondTable[l, r] = Math.Min(firstTable[l + 1, r], firstTable[l, r - 1]);
                l++;
                r++;
            }
        }

        return Math.Max(firstTable[0, n - 1], secondTable[0, n - 1]);
    }

    public static void Run()
    {
        int[] arr = [5, 7, 4, 5, 8, 1, 6, 0, 3, 4, 6, 1, 7];
        Console.WriteLine(Win1(arr));
        Console.WriteLine(Win2(arr));
        Console.WriteLine(Win3(arr));
    }
}