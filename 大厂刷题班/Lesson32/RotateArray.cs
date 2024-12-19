namespace AdvancedTraining.Lesson32;
//pass
public class RotateArray //leetcode_0189
{
    private static void Rotate1(int[] numbers, int k)
    {
        var n = numbers.Length;
        k %= n;
        Reverse(numbers, 0, n - k - 1);
        Reverse(numbers, n - k, n - 1);
        Reverse(numbers, 0, n - 1);
    }

    private static void Reverse(int[] numbers, int l, int r)
    {
        while (l < r)
        {
            var tmp = numbers[l];
            numbers[l++] = numbers[r];
            numbers[r--] = tmp;
        }
    }

    private static void Rotate2(int[] numbers, int k)
    {
        var n = numbers.Length;
        k %= n;
        if (k == 0) return;
        var l = 0;
        var r = n - 1;
        var leftPart = n - k;
        var rightPart = k;
        var same = Math.Min(leftPart, rightPart);
        var diff = leftPart - rightPart;
        Exchange(numbers, l, r, same);
        while (diff != 0)
        {
            if (diff > 0)
            {
                l += same;
                leftPart = diff;
            }
            else
            {
                r -= same;
                rightPart = -diff;
            }

            same = Math.Min(leftPart, rightPart);
            diff = leftPart - rightPart;
            Exchange(numbers, l, r, same);
        }
    }

    private static void Exchange(int[] numbers, int start, int end, int size)
    {
        var i = end - size + 1;
        while (size-- != 0)
        {
            (numbers[start], numbers[i]) = (numbers[i], numbers[start]);
            start++;
            i++;
        }
    }

    public static void Run()
    {
        int[] arr1 = [1, 2, 3, 4, 5, 6, 7];
        int[] arr2 = [1, 2, 3, 4, 5, 6, 7];
        Rotate1(arr1, 3);
        Rotate2(arr2, 3);
        Console.WriteLine(string.Join(",", arr1)); //输出5,6,7,1,2,3,4
        Console.WriteLine(string.Join(",", arr2)); //输出5,6,7,1,2,3,4
    }
}