//测试通过

namespace Algorithms.Lesson18;

public class CardsInLine
{
    // 根据规则，返回获胜者的分数
    private static int Win1(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;

        var first = OnFirst1(arr, 0, arr.Length - 1);//先手拿
        var second = OnSecond1(arr, 0, arr.Length - 1);//后手拿
        return Math.Max(first, second);
    }

    // arr[L..R]，先手获得的最好分数返回
    private static int OnFirst1(int[] arr, int l, int r)
    {
        if (l == r) return arr[l];

        var p1 = arr[l] + OnSecond1(arr, l + 1, r);//先手拿走了最左位置的牌加上后续的牌
        var p2 = arr[r] + OnSecond1(arr, l, r - 1);//先手拿走了最右位置的牌加上后续的牌
        return Math.Max(p1, p2);//返回先手拿牌的最大值
    }

    // // arr[L..R]，后手获得的最好分数返回
    private static int OnSecond1(int[] arr, int l, int r)
    {
        if (l == r) return 0;

        var p1 = OnFirst1(arr, l + 1, r); // 对手拿走了L位置的数
        var p2 = OnFirst1(arr, l, r - 1); // 对手拿走了R位置的数
        return Math.Min(p1, p2);//对手会拿走大的牌会留给你小的
    }

    private static int Win2(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;

        var n = arr.Length;
        //这种相互依赖的情况准备两张表
        var firstMap = new int[n, n];//变换的维度有两个，l和r，他们的的取值为[0,n-1]
        var secondMap = new int[n, n];
        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
        {
            firstMap[i, j] = -1;
            secondMap[i, j] = -1;
        }

        var first = OnFirst2(arr, 0, arr.Length - 1, firstMap, secondMap);
        var second = OnSecond2(arr, 0, arr.Length - 1, firstMap, secondMap);
        return Math.Max(first, second);
    }

    // arr[L..R]，先手获得的最好分数返回
    private static int OnFirst2(int[] arr, int l, int r, int[,] firstMap, int[,] secondMap)
    {
        if (firstMap[l, r] != -1) return firstMap[l, r];

        int ans;
        if (l == r)
        {
            ans = arr[l];
        }
        else
        {
            var p1 = arr[l] + OnSecond2(arr, l + 1, r, firstMap, secondMap);
            var p2 = arr[r] + OnSecond2(arr, l, r - 1, firstMap, secondMap);
            ans = Math.Max(p1, p2);
        }

        firstMap[l, r] = ans;
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