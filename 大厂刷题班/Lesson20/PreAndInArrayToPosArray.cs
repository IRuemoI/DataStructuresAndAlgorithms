//pass

#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson20;

public class PreAndInArrayToPosArray
{
    private static int[]? zuo(int[]? pre, int[]? @in)
    {
        if (pre == null || @in == null || pre.Length != @in.Length) return null;
        var n = pre.Length;
        var inMap = new Dictionary<int, int>();
        for (var i = 0; i < n; i++) inMap[@in[i]] = i;
        var pos = new int[n];
        Func(pre, 0, n - 1, @in, 0, n - 1, pos, 0, n - 1, inMap);
        return pos;
    }

    private static void Func(int[] pre, int l1, int r1, int[] @in, int l2, int r2, int[] pos, int l3, int r3,
        Dictionary<int, int> inMap)
    {
        if (l1 > r1) return;
        if (l1 == r1)
        {
            pos[l3] = pre[l1];
        }
        else
        {
            pos[r3] = pre[l1];
            var index = inMap[pre[l1]];
            Func(pre, l1 + 1, l1 + index - l2, @in, l2, index - 1, pos, l3, l3 + index - l2 - 1, inMap);
            Func(pre, l1 + index - l2 + 1, r1, @in, index + 1, r2, pos, l3 + index - l2, r3 - 1, inMap);
        }
    }

    private static int[]? preInToPos1(int[]? pre, int[]? @in)
    {
        if (pre == null || @in == null || pre.Length != @in.Length) return null;
        var n = pre.Length;
        var pos = new int[n];
        Process1(pre, 0, n - 1, @in, 0, n - 1, pos, 0, n - 1);
        return pos;
    }

    // L1...R1 L2...R2 L3...R3
    private static void Process1(int[] pre, int l1, int r1, int[] @in, int l2, int r2, int[] pos, int l3, int r3)
    {
        if (l1 > r1) return;
        if (l1 == r1)
        {
            pos[l3] = pre[l1];
            return;
        }

        pos[r3] = pre[l1];
        var mid = l2;
        for (; mid <= r2; mid++)
            if (@in[mid] == pre[l1])
                break;
        var leftSize = mid - l2;
        Process1(pre, l1 + 1, l1 + leftSize, @in, l2, mid - 1, pos, l3, l3 + leftSize - 1);
        Process1(pre, l1 + leftSize + 1, r1, @in, mid + 1, r2, pos, l3 + leftSize, r3 - 1);
    }

    private static int[]? preInToPos2(int[]? pre, int[]? @in)
    {
        if (pre == null || @in == null || pre.Length != @in.Length) return null;
        var n = pre.Length;
        var inMap = new Dictionary<int, int>();
        for (var i = 0; i < n; i++) inMap[@in[i]] = i;
        var pos = new int[n];
        Process2(pre, 0, n - 1, @in, 0, n - 1, pos, 0, n - 1, inMap);
        return pos;
    }

    private static void Process2(int[] pre, int l1, int r1, int[] @in, int l2, int r2, int[] pos, int l3, int r3,
        Dictionary<int, int> inMap)
    {
        if (l1 > r1) return;
        if (l1 == r1)
        {
            pos[l3] = pre[l1];
            return;
        }

        pos[r3] = pre[l1];
        var mid = inMap[pre[l1]];
        var leftSize = mid - l2;
        Process2(pre, l1 + 1, l1 + leftSize, @in, l2, mid - 1, pos, l3, l3 + leftSize - 1, inMap);
        Process2(pre, l1 + leftSize + 1, r1, @in, mid + 1, r2, pos, l3 + leftSize, r3 - 1, inMap);
    }

    //用于测试
    private static int[] getPreArray(Node head)
    {
        var arr = new List<int>();
        FillPreArray(head, arr);
        var ans = new int[arr.Count];
        for (var i = 0; i < ans.Length; i++) ans[i] = arr[i];
        return ans;
    }

    //用于测试
    private static void FillPreArray(Node? head, List<int> arr)
    {
        if (head == null) return;
        arr.Add(head.Value);
        FillPreArray(head.Left, arr);
        FillPreArray(head.Right, arr);
    }

    //用于测试
    private static int[] getInArray(Node head)
    {
        var arr = new List<int>();
        FillInArray(head, arr);
        var ans = new int[arr.Count];
        for (var i = 0; i < ans.Length; i++) ans[i] = arr[i];
        return ans;
    }

    //用于测试
    private static void FillInArray(Node? head, List<int> arr)
    {
        if (head == null) return;
        FillInArray(head.Left, arr);
        arr.Add(head.Value);
        FillInArray(head.Right, arr);
    }

    //用于测试
    private static int[] getPosArray(Node head)
    {
        var arr = new List<int>();
        FillPostArray(head, arr);
        var ans = new int[arr.Count];
        for (var i = 0; i < ans.Length; i++) ans[i] = arr[i];
        return ans;
    }

    //用于测试
    private static void FillPostArray(Node? head, List<int> arr)
    {
        if (head == null) return;
        FillPostArray(head.Left, arr);
        FillPostArray(head.Right, arr);
        arr.Add(head.Value);
    }

    //用于测试
    private static Node? GenerateRandomTree(int value, int maxLevel)
    {
        var hasValue = new HashSet<int>();
        return CreateTree(value, 1, maxLevel, hasValue);
    }

    //用于测试
    private static Node? CreateTree(int value, int level, int maxLevel, HashSet<int> hasValue)
    {
        if (level > maxLevel) return null;
        int cur;
        do
        {
            cur = (int)(Utility.getRandomDouble * value) + 1;
        } while (hasValue.Contains(cur));

        hasValue.Add(cur);
        var head = new Node(cur)
        {
            Left = CreateTree(value, level + 1, maxLevel, hasValue),
            Right = CreateTree(value, level + 1, maxLevel, hasValue)
        };
        return head;
    }

    //用于测试
    private static bool IsEqual(int[]? arr1, int[]? arr2)
    {
        if ((arr1 == null && arr2 != null) || (arr1 != null && arr2 == null)) return false;
        if (arr1 == null && arr2 == null) return true;
        if (arr1?.Length != arr2?.Length) return false;
        for (var i = 0; i < arr1?.Length; i++)
            if (arr1[i] != arr2?[i])
                return false;
        return true;
    }

    public static void Run()
    {
        Console.WriteLine("测试开始");
        var maxLevel = 5;
        var value = 1000;
        var testTime = 5000;
        for (var i = 0; i < testTime; i++)
        {
            var head = GenerateRandomTree(value, maxLevel);

            if (head != null)
            {
                var pre = getPreArray(head);
                var @in = getInArray(head);
                var pos = getPosArray(head);
                var ans1 = preInToPos1(pre, @in);
                var ans2 = preInToPos2(pre, @in);
                var classAns = zuo(pre, @in);
                if (ans1 == null || ans2 == null || classAns == null) throw new Exception();
                if (!IsEqual(pos, ans1) || !IsEqual(ans1, ans2) || !IsEqual(pos, classAns)) Console.WriteLine("出错啦！");
            }
        }

        Console.WriteLine("测试结束");
    }

    private class Node(int v)
    {
        public readonly int Value = v;
        public Node? Left;
        public Node? Right;
    }
}