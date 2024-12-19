namespace AdvancedTraining.Lesson23;

public class LongestIntegratedLength
{
    private static int MaxLen(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        var n = arr.Length;
        var set = new HashSet<int>();
        var ans = 1;
        for (var l = 0; l < n; l++)
        {
            set.Clear();
            var min = arr[l];
            var max = arr[l];
            set.Add(arr[l]);
            // L..R
            for (var r = l + 1; r < n; r++)
            {
                // L....R
                if (!set.Add(arr[r])) break;
                min = Math.Min(min, arr[r]);
                max = Math.Max(max, arr[r]);
                if (max - min == r - l) ans = Math.Max(ans, r - l + 1);
            }
        }

        return ans;
    }

    private static int GetLil1(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        var len = 0;
        // O(N^3 * log N)
        for (var start = 0; start < arr.Length; start++)
            // 子数组所有可能的开头
        for (var end = start; end < arr.Length; end++)
            // 开头为start的情况下，所有可能的结尾
            // arr[s ... e]
            // O(N * logN)
            if (IsIntegrated(arr, start, end))
                len = Math.Max(len, end - start + 1);
        return len;
    }

    private static bool IsIntegrated(int[] arr, int left, int right)
    {
        //int[] newArra = Arrays.copyOfRange(arr, left, right + 1); // O(N)
        var newArr = new int[right + 1 - left];
        Array.Copy(arr, newArr, right + 1);
        Array.Sort(newArr); // O(N*logN)
        for (var i = 1; i < newArr.Length; i++)
            if (newArr[i - 1] != newArr[i] - 1)
                return false;
        return true;
    }

    private static int GetLil2(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        var len = 0;
        var set = new HashSet<int>();
        for (var l = 0; l < arr.Length; l++)
        {
            // L 左边界
            // L .......
            set.Clear();
            var max = int.MinValue;
            var min = int.MaxValue;
            for (var r = l; r < arr.Length; r++)
            {
                // R 右边界
                // arr[L..R]这个子数组在验证 l...R L...r+1 l...r+2
                if (!set.Add(arr[r]))
                    // arr[L..R]上开始 出现重复值了，arr[L..R往后]不需要验证了，
                    // 一定不是可整合的
                    break;
                // arr[L..R]上无重复值
                max = Math.Max(max, arr[r]);
                min = Math.Min(min, arr[r]);
                if (max - min == r - l)
                    // L..R 是可整合的
                    len = Math.Max(len, r - l + 1);
            }
        }

        return len;
    }

    //todo:待修复
    public static void Run()
    {
        int[] arr = [5, 5, 3, 2, 6, 4, 3];
        Console.WriteLine(GetLil1(arr));
        Console.WriteLine(GetLil2(arr));
    }
}