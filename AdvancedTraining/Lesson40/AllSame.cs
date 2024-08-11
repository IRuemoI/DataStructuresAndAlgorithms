#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson40;

// 来自腾讯
// 比如arr = {3,1,2,4}
// 下标对应是：0 1 2 3
// 你最开始选择一个下标进行操作，一旦最开始确定了是哪个下标，以后都只能在这个下标上进行操作
// 比如你选定1下标，1下标上面的数字是1，你可以选择变化这个数字，比如你让这个数字变成2
// 那么arr = {3,2,2,4}
// 下标对应是：0 1 2 3
// 因为你最开始确定了1这个下标，所以你以后都只能对这个下标进行操作，
// 但是，和你此时下标上的数字一样的、且位置连成一片的数字，会跟着一起变
// 比如你选择让此时下标1的数字2变成3，
// 那么arr = {3,3,3,4} 可以看到下标1和下标2的数字一起变成3，这是规则！一定会一起变
// 下标对应是：0 1 2 3
// 接下来，你还是只能对1下标进行操作，那么数字一样的、且位置连成一片的数字(arr[0~2]这个范围)都会一起变
// 决定变成4
// 那么arr = {4,4,4,4}
// 下标对应是：0 1 2 3
// 至此，所有数都成一样的了，你在下标1上做了3个决定(第一次变成2，第二次变成3，第三次变成4)，
// 因为联动规则，arr全刷成一种数字了
// 给定一个数组arr，最开始选择哪个下标，你随意
// 你的目的是一定要让arr都成为一种数字，注意联动效果会一直生效
// 返回最小的变化数
// arr长度 <= 200, arr中的值 <= 10^6
public class AllSame
{
    private static int AllSame1(int[] arr)
    {
        var ans = int.MaxValue;
        for (var i = 0; i < arr.Length; i++) ans = Math.Min(ans, Process1(arr, i - 1, arr[i], i + 1));
        return ans;
    }

    // 左边arr[0..left]，如果left == -1，说明没有左边了
    // 右边arr[right...n-1]，如果right == n，说明没有右边了
    // 中间的值是midV，中间的值代表中间一整个部分的值，中间部分有可能是一个数，也可能是一堆数，但是值都为midV
    // 返回arr都刷成一样的，最小代价是多少
    // left 可能性 : N
    // right 可能性 : N
    // midV 可能性 : arr中的最大值！
    private static int Process1(int[] arr, int left, int midV, int right)
    {
        for (; left >= 0 && arr[left] == midV;) left--;
        for (; right < arr.Length && arr[right] == midV;) right++;
        if (left == -1 && right == arr.Length) return 0;
        var p1 = int.MaxValue;
        if (left >= 0) p1 = Process1(arr, left - 1, arr[left], right) + 1;
        var p2 = int.MaxValue;
        if (right < arr.Length) p2 = Process1(arr, left, arr[right], right + 1) + 1;
        return Math.Min(p1, p2);
    }

    private static int AllSame2(int[] arr)
    {
        var ans = int.MaxValue;
        for (var i = 0; i < arr.Length; i++) ans = Math.Min(ans, Process2(arr, i - 1, true, i + 1));
        return ans;
    }

    // 左边arr[0..l]，如果left == -1，说明没有左边了
    // 右边arr[r...n-1]，如果right == n，说明没有右边了
    // 中间的值代表arr[l+1...r-1]这个部分的值已经刷成了一种
    // 中间的值，如果和arr[l+1]一样，isLeft就是true
    // 中间的值，如果和arr[r-1]一样，isLeft就是false
    // 返回arr都刷成一样的，最小代价是多少
    // left 可能性 : N
    // right 可能性 : N
    // isLeft 可能性 : true/false,两种
    private static int Process2(int[] arr, int l, bool isLeft, int r)
    {
        var left = l;
        for (; left >= 0 && arr[left] == (isLeft ? arr[l + 1] : arr[r - 1]);) left--;
        var right = r;
        for (; right < arr.Length && arr[right] == (isLeft ? arr[l + 1] : arr[r - 1]);) right++;
        if (left == -1 && right == arr.Length) return 0;
        var p1 = int.MaxValue;
        if (left >= 0) p1 = Process2(arr, left - 1, true, right) + 1;
        var p2 = int.MaxValue;
        if (right < arr.Length) p2 = Process2(arr, left, false, right + 1) + 1;
        return Math.Min(p1, p2);
    }
    // 如上的递归，请改动态规划，具体参考体系学习班，动态规划大章节！

    // 为了测试
    private static int[] RandomArray(int maxSize, int maxNum)
    {
        var size = 2 + (int)(Utility.GetRandomDouble * maxSize);
        var arr = new int[size];
        for (var i = 0; i < size; i++) arr[i] = 1 + (int)(Utility.GetRandomDouble * maxSize);
        return arr;
    }

    // 为了测试
    public static void Run()
    {
        Console.WriteLine("测试开始");
        for (var i = 0; i < 10000; i++)
        {
            var arr = RandomArray(20, 10);
            var ans1 = AllSame1(arr);
            var ans2 = AllSame2(arr);
            if (ans1 != ans2)
            {
                Console.WriteLine("出错了！！！");
                foreach (var i1 in arr) Console.Write(i1 + " ");
                Console.WriteLine();
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}