namespace AdvancedTraining.Lesson34;
//pass
public class CountOfSmallerNumbersAfterSelf //leetcode_0315
{
    private static IList<int> CountSmaller(int[]? numbers)
    {
        IList<int> ans = new List<int>();
        if (numbers == null) return ans;
        for (var i = 0; i < numbers.Length; i++) ans.Add(0);
        if (numbers.Length < 2) return ans;
        var arr = new Node[numbers.Length];
        for (var i = 0; i < arr.Length; i++) arr[i] = new Node(numbers[i], i);
        Process(arr, 0, arr.Length - 1, ans);
        return ans;
    }

    private static void Process(Node[] arr, int l, int r, IList<int> ans)
    {
        if (l == r) return;
        var mid = l + ((r - l) >> 1);
        Process(arr, l, mid, ans);
        Process(arr, mid + 1, r, ans);
        Merge(arr, l, mid, r, ans);
    }

    private static void Merge(Node[] arr, int l, int m, int r, IList<int> ans)
    {
        var help = new Node[r - l + 1];
        var i = help.Length - 1;
        var p1 = m;
        var p2 = r;
        while (p1 >= l && p2 >= m + 1)
        {
            if (arr[p1].Value > arr[p2].Value) ans[arr[p1].Index] = ans[arr[p1].Index] + p2 - m;
            help[i--] = arr[p1].Value > arr[p2].Value ? arr[p1--] : arr[p2--];
        }

        while (p1 >= l) help[i--] = arr[p1--];
        while (p2 >= m + 1) help[i--] = arr[p2--];
        for (i = 0; i < help.Length; i++) arr[l + i] = help[i];
    }

    public static void Run()
    {
        Console.WriteLine(string.Join(",", CountSmaller([5, 2, 6, 1]))); //输出：[2,1,1,0] 
    }

    private class Node(int v, int i)
    {
        public readonly int Index = i;
        public readonly int Value = v;
    }
}