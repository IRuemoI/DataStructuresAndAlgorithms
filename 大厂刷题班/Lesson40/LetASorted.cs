namespace AdvancedTraining.Lesson40;

// 给定两个数组A和B，长度都是N
// A[i]不可以在A中和其他数交换，只可以选择和B[i]交换(0<=i<n)
// 你的目的是让A有序，返回你能不能做到
public class LetASorted
{
    private static bool LetASortedCode1(int[] a, int[] b)
    {
        return Process(a, b, 0, int.MinValue);
    }

    private static bool LetASortedCode2(int[] a, int[] b)
    {
        return Process2(a, b, 0, int.MinValue);
    }

    // 当前推进到了i位置，对于A和B都是i位置
    // A[i]前一个数字，lastA
    // 能否通过题意中的操作，A[i] B[i] 让A整体有序
    private static bool Process(int[] a, int[] b, int i, int lastA)
    {
        if (i == a.Length) return true;
        // 第一种选择 : A[i]不和B[i]交换
        if (a[i] >= lastA && Process(a, b, i + 1, a[i])) return true;
        // 第一种选择 : A[i]和B[i]交换
        if (b[i] >= lastA && Process(a, b, i + 1, b[i])) return true;
        return false;
    }

    private static bool Process2(int[] a, int[] b, int i, int lastA)
    {
        if (i == a.Length) return true;
        // 第一种选择 : A[i]不和B[i]交换
        if (a[i] <= lastA && Process2(a, b, i + 1, a[i])) return true;
        // 第一种选择 : A[i]和B[i]交换
        if (b[i] <= lastA && Process2(a, b, i + 1, b[i])) return true;
        return false;
    }

    // A B 操作 : A[i] 与 B[i] 交换！
    // 目的 : 让A和B都有序，能不能做到
    //	private static boolean Process3(int[] A, int[] B, int i, int lastA, int lastB) {
    //
    //	}

    public static void Run()
    {
        Console.WriteLine(LetASortedCode1([3, 2, 2], [1, 2, 3])); //输出True
        Console.WriteLine(LetASortedCode2([3, 2, 2], [1, 2, 3])); //输出True
    }
}