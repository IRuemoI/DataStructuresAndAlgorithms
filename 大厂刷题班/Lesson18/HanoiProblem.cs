//pass

namespace AdvancedTraining.Lesson18;

public class HanoiProblem
{
    private static int Step1(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return -1;
        return Process(arr, arr.Length - 1, 1, 2, 3);
    }

    // 目标是: 把0~i的圆盘，从from全部挪到to上
    // 返回，根据arr中的状态arr[0..i]，它是最优解的第几步？
    // f(i, 3 , 2, 1) f(i, 1, 3, 2) f(i, 3, 1, 2)
    private static int Process(int[] arr, int i, int from, int other, int to)
    {
        if (i == -1) return 0;
        if (arr[i] != from && arr[i] != to) return -1;
        if (arr[i] == from)
            // 第一大步没走完
            return Process(arr, i - 1, from, to, other); // arr[i] == to
        // 已经走完1，2两步了，
        var rest = Process(arr, i - 1, other, from, to); // 第三大步完成的程度
        if (rest == -1) return -1;
        return (1 << i) + rest;
    }

    private static int Step2(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return -1;
        var from = 1;
        var mid = 2;
        var to = 3;
        var i = arr.Length - 1;
        var res = 0;
        while (i >= 0)
        {
            if (arr[i] != from && arr[i] != to) return -1;
            int tmp;
            if (arr[i] == to)
            {
                res += 1 << i;
                tmp = from;
                from = mid;
            }
            else
            {
                tmp = to;
                to = mid;
            }

            mid = tmp;
            i--;
        }

        return res;
    }

    private static int Kth(int[] arr)
    {
        var n = arr.Length;
        return Step(arr, n - 1, 1, 3, 2);
    }

    // 0...index这些圆盘，arr[0..index] index+1层塔
    // 在哪？from 去哪？to 另一个是啥？other
    // arr[0..index]这些状态，是index+1层汉诺塔问题的，最优解第几步
    private static int Step(int[] arr, int index, int from, int to, int other)
    {
        if (index == -1) return 0;
        if (arr[index] == other) return -1;
        // arr[index] == from arr[index] == to;
        if (arr[index] == from) return Step(arr, index - 1, from, other, to);

        var p1 = (1 << index) - 1;
        var p2 = 1;
        var p3 = Step(arr, index - 1, other, to, from);
        if (p3 == -1) return -1;
        return p1 + p2 + p3;
    }

    public static void Run()
    {
        int[] arr = [3, 3, 2, 1];
        Console.WriteLine(Step1(arr));
        Console.WriteLine(Step2(arr));
        Console.WriteLine(Kth(arr));
    }
}