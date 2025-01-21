//pass
namespace AdvancedTraining.Lesson16;

public class MinPatches
{
    // arr请保证有序，且正数  1~aim
    private static int MinPatches1(int[] arr, int aim)
    {
        var patches = 0; // 缺多少个数字
        long range = 0; // 已经完成了1 ~ range的目标
        Array.Sort(arr);
        for (var i = 0; i != arr.Length; i++)
        {
            // arr[i]
            // 要求：1 ~ arr[i]-1 范围被搞定！
            while (arr[i] - 1 > range)
            {
                // arr[i] 1 ~ arr[i]-1
                range += range + 1; // range + 1 是缺的数字
                patches++;
                if (range >= aim) return patches;
            }

            // 要求被满足了！
            range += arr[i];
            if (range >= aim) return patches;
        }

        while (aim >= range + 1)
        {
            range += range + 1;
            patches++;
        }

        return patches;
    }

    // 嘚瑟
    private static int MinPatches2(int[] arr, int K)
    {
        var patches = 0; // 缺多少个数字
        var range = 0; // 已经完成了1 ~ range的目标
        for (var i = 0; i != arr.Length; i++)
        {
            // 1~range
            // 1 ~ arr[i]-1
            while (arr[i] > range + 1)
            {
                // arr[i] 1 ~ arr[i]-1

                if (range > int.MaxValue - range - 1) return patches + 1;

                range += range + 1; // range + 1 是缺的数字
                patches++;
                if (range >= K) return patches;
            }

            if (range > int.MaxValue - arr[i]) return patches;
            range += arr[i];
            if (range >= K) return patches;
        }

        while (K >= range + 1)
        {
            if (K == range && K == int.MaxValue) return patches;
            if (range > int.MaxValue - range - 1) return patches + 1;
            range += range + 1;
            patches++;
        }

        return patches;
    }

    public static void Run()
    {
        Console.WriteLine(MinPatches1([1, 2, 31, 33], 2147483647));
        Console.WriteLine(MinPatches2([1, 2, 31, 33], 2147483647));
    }
}