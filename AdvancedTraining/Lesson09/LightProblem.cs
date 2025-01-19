//pass
#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson09;

/*
 * 给定一个数组arr，长度为N，arr中的值不是0就是1
 * arr[i]表示第i栈灯的状态，0代表灭灯，1代表亮灯
 * 每一栈灯都有开关，但是按下i号灯的开关，会同时改变i-1、i、i+2栈灯的状态
 * 问题一：
 * 如果N栈灯排成一条直线,请问最少按下多少次开关,能让灯都亮起来
 * 排成一条直线说明：
 * i为中间位置时，i号灯的开关能影响i-1、i和i+1
 * 0号灯的开关只能影响0和1位置的灯
 * N-1号灯的开关只能影响N-2和N-1位置的灯
 *
 * 问题二：
 * 如果N栈灯排成一个圈,请问最少按下多少次开关,能让灯都亮起来
 * 排成一个圈说明：
 * i为中间位置时，i号灯的开关能影响i-1、i和i+1
 * 0号灯的开关能影响N-1、0和1位置的灯
 * N-1号灯的开关能影响N-2、N-1和0位置的灯
 *
 * */
public class LightProblem
{
    // 无环改灯问题的暴力版本
    private static int NoLoopRight(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        if (arr.Length == 1) return arr[0] == 1 ? 0 : 1;
        if (arr.Length == 2) return arr[0] != arr[1] ? int.MaxValue : arr[0] ^ 1;
        return F1(arr, 0);
    }

    private static int F1(int[] arr, int i)
    {
        if (i == arr.Length) return Valid(arr) ? 0 : int.MaxValue;
        var p1 = F1(arr, i + 1);
        Change1(arr, i);
        var p2 = F1(arr, i + 1);
        Change1(arr, i);
        p2 = p2 == int.MaxValue ? p2 : p2 + 1;
        return Math.Min(p1, p2);
    }

    private static void Change1(int[] arr, int i)
    {
        if (i == 0)
        {
            arr[0] ^= 1;
            arr[1] ^= 1;
        }
        else if (i == arr.Length - 1)
        {
            arr[i - 1] ^= 1;
            arr[i] ^= 1;
        }
        else
        {
            arr[i - 1] ^= 1;
            arr[i] ^= 1;
            arr[i + 1] ^= 1;
        }
    }

    private static bool Valid(int[] arr)
    {
        foreach (var item in arr)
            if (item == 0)
                return false;

        return true;
    }

    // 无环改灯问题的递归版本
    private static int NoLoopMinStep1(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        if (arr.Length == 1) return arr[0] ^ 1;
        if (arr.Length == 2) return arr[0] != arr[1] ? int.MaxValue : arr[0] ^ 1;
        // 不变0位置的状态
        var p1 = Process1(arr, 2, arr[0], arr[1]);
        // 改变0位置的状态
        var p2 = Process1(arr, 2, arr[0] ^ 1, arr[1] ^ 1);
        if (p2 != int.MaxValue) p2++;
        return Math.Min(p1, p2);
    }

    // 当前在哪个位置上，做选择，NextIndex - 1 = cur ，当前！
    // cur - 1 preStatus
    // cur  curStatus
    // 0....cur-2  全亮的！
    private static int Process1(int[] arr, int nextIndex, int preStatus, int curStatus)
    {
        if (nextIndex == arr.Length)
            // 当前来到最后一个开关的位置
            return preStatus != curStatus ? int.MaxValue : curStatus ^ 1;
        // 没到最后一个按钮呢！
        // i < arr.length
        if (preStatus == 0)
        {
            // 一定要改变
            curStatus ^= 1;
            var cur = arr[nextIndex] ^ 1;
            var next = Process1(arr, nextIndex + 1, curStatus, cur);
            return next == int.MaxValue ? next : next + 1;
        } // 一定不能改变

        return Process1(arr, nextIndex + 1, curStatus, arr[nextIndex]);
    }

    // 无环改灯问题的迭代版本
    private static int NoLoopMinStep2(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        if (arr.Length == 1) return arr[0] == 1 ? 0 : 1;
        if (arr.Length == 2) return arr[0] != arr[1] ? int.MaxValue : arr[0] ^ 1;
        var p1 = TraceNoLoop(arr, arr[0], arr[1]);
        var p2 = TraceNoLoop(arr, arr[0] ^ 1, arr[1] ^ 1);
        p2 = p2 == int.MaxValue ? p2 : p2 + 1;
        return Math.Min(p1, p2);
    }

    private static int TraceNoLoop(int[] arr, int preStatus, int curStatus)
    {
        var i = 2;
        var op = 0;
        while (i != arr.Length)
            if (preStatus == 0)
            {
                op++;
                preStatus = curStatus ^ 1;
                curStatus = arr[i++] ^ 1;
            }
            else
            {
                preStatus = curStatus;
                curStatus = arr[i++];
            }

        return preStatus != curStatus ? int.MaxValue : op + (curStatus ^ 1);
    }

    // 有环改灯问题的暴力版本
    private static int LoopRight(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        if (arr.Length == 1) return arr[0] == 1 ? 0 : 1;
        if (arr.Length == 2) return arr[0] != arr[1] ? int.MaxValue : arr[0] ^ 1;
        return F2(arr, 0);
    }

    private static int F2(int[] arr, int i)
    {
        if (i == arr.Length) return Valid(arr) ? 0 : int.MaxValue;
        var p1 = F2(arr, i + 1);
        Change2(arr, i);
        var p2 = F2(arr, i + 1);
        Change2(arr, i);
        p2 = p2 == int.MaxValue ? p2 : p2 + 1;
        return Math.Min(p1, p2);
    }

    private static void Change2(int[] arr, int i)
    {
        arr[LastIndex(i, arr.Length)] ^= 1;
        arr[i] ^= 1;
        arr[NextIndex(i, arr.Length)] ^= 1;
    }

    private static int LastIndex(int i, int n)
    {
        return i == 0 ? n - 1 : i - 1;
    }

    private static int NextIndex(int i, int n)
    {
        return i == n - 1 ? 0 : i + 1;
    }

    // 有环改灯问题的递归版本
    private static int LoopMinStep1(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        if (arr.Length == 1) return arr[0] == 1 ? 0 : 1;
        if (arr.Length == 2) return arr[0] != arr[1] ? int.MaxValue : arr[0] ^ 1;
        if (arr.Length == 3) return arr[0] != arr[1] || arr[0] != arr[2] ? int.MaxValue : arr[0] ^ 1;
        // 0不变，1不变
        var p1 = Process2(arr, 3, arr[1], arr[2], arr[^1], arr[0]);
        // 0改变，1不变
        var p2 = Process2(arr, 3, arr[1] ^ 1, arr[2], arr[^1] ^ 1, arr[0] ^ 1);
        // 0不变，1改变
        var p3 = Process2(arr, 3, arr[1] ^ 1, arr[2] ^ 1, arr[^1], arr[0] ^ 1);
        // 0改变，1改变
        var p4 = Process2(arr, 3, arr[1], arr[2] ^ 1, arr[^1] ^ 1, arr[0]);
        p2 = p2 != int.MaxValue ? p2 + 1 : p2;
        p3 = p3 != int.MaxValue ? p3 + 1 : p3;
        p4 = p4 != int.MaxValue ? p4 + 2 : p4;
        return Math.Min(Math.Min(p1, p2), Math.Min(p3, p4));
    }


    // 下一个位置是，NextIndex
    // 当前位置是，NextIndex - 1 -> curIndex
    // 上一个位置是, NextIndex - 2 -> preIndex   preStatus
    // 当前位置是，NextIndex - 1, curStatus
    // endStatus, N-1位置的状态
    // firstStatus, 0位置的状态
    // 返回，让所有灯都亮，至少按下几次按钮

    // 当前来到的位置(NextIndex - 1)，一定不能是1！至少从2开始
    // NextIndex >= 3
    private static int Process2(int[] arr, int nextIndex, int preStatus, int curStatus, int endStatus, int firstStatus)
    {
        if (nextIndex == arr.Length)
            // 最后一按钮！
            return endStatus != firstStatus || endStatus != preStatus ? int.MaxValue : endStatus ^ 1;
        // 当前位置，NextIndex - 1
        // 当前的状态，叫curStatus
        // 如果不按下按钮，下一步的preStatus, curStatus
        // 如果按下按钮，下一步的preStatus, curStatus ^ 1
        // 如果不按下按钮，下一步的curStatus, arr[NextIndex]
        // 如果按下按钮，下一步的curStatus, arr[NextIndex] ^ 1
        var noNextPreStatus = 0;
        var yesNextPreStatus = 0;
        var noNextCurStatus = 0;
        var yesNextCurStatus = 0;
        var noEndStatus = 0;
        var yesEndStatus = 0;
        if (nextIndex < arr.Length - 1)
        {
            // 当前没来到N-2位置
            noNextPreStatus = curStatus;
            yesNextPreStatus = curStatus ^ 1;
            noNextCurStatus = arr[nextIndex];
            yesNextCurStatus = arr[nextIndex] ^ 1;
        }
        else if (nextIndex == arr.Length - 1)
        {
            // 当前来到的就是N-2位置
            noNextPreStatus = curStatus;
            yesNextPreStatus = curStatus ^ 1;
            noNextCurStatus = endStatus;
            yesNextCurStatus = endStatus ^ 1;
            noEndStatus = endStatus;
            yesEndStatus = endStatus ^ 1;
        }

        if (preStatus == 0)
        {
            var next = Process2(arr, nextIndex + 1, yesNextPreStatus, yesNextCurStatus,
                nextIndex == arr.Length - 1 ? yesEndStatus : endStatus, firstStatus);
            return next == int.MaxValue ? next : next + 1;
        }

        return Process2(arr, nextIndex + 1, noNextPreStatus, noNextCurStatus,
            nextIndex == arr.Length - 1 ? noEndStatus : endStatus, firstStatus);
        //		int curStay = (NextIndex == arr.length - 1) ? endStatus : arr[NextIndex];
        //		int curChange = (NextIndex == arr.length - 1) ? (endStatus ^ 1) : (arr[NextIndex] ^ 1);
        //		int endChange = (NextIndex == arr.length - 1) ? curChange : endStatus;
        //		if (preStatus == 0) {
        //			int next = Process2(arr, NextIndex + 1, curStatus ^ 1, curChange, endChange, firstStatus);
        //			return next == Integer.MAX_VALUE ? next : (next + 1);
        //		} else {
        //			return Process2(arr, NextIndex + 1, curStatus, curStay, endStatus, firstStatus);
        //		}
    }

    // 有环改灯问题的迭代版本
    private static int LoopMinStep2(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        if (arr.Length == 1) return arr[0] == 1 ? 0 : 1;
        if (arr.Length == 2) return arr[0] != arr[1] ? int.MaxValue : arr[0] ^ 1;
        if (arr.Length == 3) return arr[0] != arr[1] || arr[0] != arr[2] ? int.MaxValue : arr[0] ^ 1;
        // 0不变，1不变
        var p1 = TraceLoop(arr, arr[1], arr[2], arr[^1], arr[0]);
        // 0改变，1不变
        var p2 = TraceLoop(arr, arr[1] ^ 1, arr[2], arr[^1] ^ 1, arr[0] ^ 1);
        // 0不变，1改变
        var p3 = TraceLoop(arr, arr[1] ^ 1, arr[2] ^ 1, arr[^1], arr[0] ^ 1);
        // 0改变，1改变
        var p4 = TraceLoop(arr, arr[1], arr[2] ^ 1, arr[^1] ^ 1, arr[0]);
        p2 = p2 != int.MaxValue ? p2 + 1 : p2;
        p3 = p3 != int.MaxValue ? p3 + 1 : p3;
        p4 = p4 != int.MaxValue ? p4 + 2 : p4;
        return Math.Min(Math.Min(p1, p2), Math.Min(p3, p4));
    }

    private static int TraceLoop(int[] arr, int preStatus, int curStatus, int endStatus, int firstStatus)
    {
        var i = 3;
        var op = 0;
        while (i < arr.Length - 1)
            if (preStatus == 0)
            {
                op++;
                preStatus = curStatus ^ 1;
                curStatus = arr[i++] ^ 1;
            }
            else
            {
                preStatus = curStatus;
                curStatus = arr[i++];
            }

        if (preStatus == 0)
        {
            op++;
            preStatus = curStatus ^ 1;
            endStatus ^= 1;
            //curStatus = endStatus;
        }
        else
        {
            preStatus = curStatus;
            //curStatus = endStatus;
        }

        return endStatus != firstStatus || endStatus != preStatus ? int.MaxValue : op + (endStatus ^ 1);
    }

    // 生成长度为len的随机数组，值只有0和1两种值
    private static int[] randomArray(int len)
    {
        var arr = new int[len];
        for (var i = 0; i < arr.Length; i++) arr[i] = (int)(Utility.getRandomDouble * 2);
        return arr;
    }

    public static void Run()
    {
        Console.WriteLine("如果没有任何Oops打印，说明所有方法都正确");
        Console.WriteLine("测试开始");
        var testTime = 20000;
        var lenMax = 12;
        for (var i = 0; i < testTime; i++)
        {
            var len = (int)(Utility.getRandomDouble * lenMax);
            var arr = randomArray(len);
            var ans1 = NoLoopRight(arr);
            var ans2 = NoLoopMinStep1(arr);
            var ans3 = NoLoopMinStep2(arr);
            if (ans1 != ans2 || ans1 != ans3) Console.WriteLine("1 出错");
        }

        for (var i = 0; i < testTime; i++)
        {
            var len1 = (int)(Utility.getRandomDouble * lenMax);
            var arr1 = randomArray(len1);
            var ans1 = LoopRight(arr1);
            var ans2 = LoopMinStep1(arr1);
            var ans3 = LoopMinStep2(arr1);
            if (ans1 != ans2 || ans1 != ans3) Console.WriteLine("2 出错");
        }

        Console.WriteLine("测试结束");


        var len2 = 100000000;
        Console.WriteLine("性能测试");
        Console.WriteLine("数组大小：" + len2);
        var arr2 = randomArray(len2);


        NoLoopMinStep2(arr2);
        Utility.RestartStopwatch();
        Console.WriteLine("NoLoopMinStep2 run time: " + Utility.GetStopwatchElapsedMilliseconds() + "(ms)");

        Utility.RestartStopwatch();
        LoopMinStep2(arr2);
        Console.WriteLine("LoopMinStep2 run time: " + Utility.GetStopwatchElapsedMilliseconds() + "(ms)");
    }
}