namespace Algorithms.Lesson05;

public static class DutchNationalFlagProblem
{
    //小于等于、大于的情况
    private static void LessEqualAndGreater(int[] arr, int target)
    {
        int lessEqualEdge = 0;
        int currentIndex = 0;
        while (currentIndex < arr.Length)
        {
            if (arr[currentIndex] <= target)
            {
                (arr[currentIndex], arr[lessEqualEdge]) = (arr[lessEqualEdge], arr[currentIndex]);
                lessEqualEdge++;
            }

            currentIndex++;
        }
    }

    //小于、等于、大于的情况
    private static void LessAndEqualAndGreater(int[] arr, int target)
    {
        int lessEdge = 0;
        int greaterEdge = arr.Length - 1;
        int currentIndex = 0;
        while (currentIndex <= greaterEdge)
        {
            if (arr[currentIndex] < target)
            {
                (arr[currentIndex], arr[lessEdge]) = (arr[lessEdge], arr[currentIndex]);
                lessEdge++;
                currentIndex++;
            }
            else if (arr[currentIndex] > target)
            {
                (arr[currentIndex], arr[greaterEdge]) = (arr[greaterEdge], arr[currentIndex]);
                greaterEdge--;
                //从后面换过来的数还不知道是什么情况，所以currentIndex不变，下一轮继续处理
            }
            else //arr[currentIndex] == target的情况
            {
                currentIndex++;
            }
        }
    }

    //验证小于、等于、大于的情况
    private static bool ValidLorEorG(int[] arr, int target)
    {
        int equalLeftEdge = 0;
        int equalRightEdge = arr.Length - 1;
        //设置一个循环，当equalLeftEdge < target时，equalLeftEdge向右移动
        while (equalLeftEdge < arr.Length && arr[equalLeftEdge] < target)
        {
            equalLeftEdge++;
        }

        //设置一个循环，当equalRightEdge > target时，equalRightEdge向左移动
        while (equalRightEdge > -1 && arr[equalRightEdge] > target)
        {
            equalRightEdge--;
        }

        //没有等于target的数直接返回false
        if (equalLeftEdge > equalRightEdge)
        {
            return false;
        }

        //设置一个循环，验证equalLeftEdge到equalRightEdge的所有数是否都等于target
        while (equalLeftEdge <= equalRightEdge)
        {
            if (arr[equalLeftEdge] != target)
            {
                return false;
            }
            else
            {
                equalLeftEdge++;
            }
        }

        return true;
    }

    //验证小于等于、大于的情况
    private static bool ValidLEorG(int[] arr, int target)
    {
        int equalLeftEdge = 0;
        //设置一个循环，当equalLeftEdge <= target时，equalLeftEdge向右移动
        while (equalLeftEdge < arr.Length && arr[equalLeftEdge] <= target)
        {
            equalLeftEdge++;
        }

        //设置一个循环，验证equalLeftEdge到arr.Length-1的所有数是否都大于target
        while (equalLeftEdge < arr.Length)
        {
            if (arr[equalLeftEdge] > target)
            {
                equalLeftEdge++;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public static void Run()
    {
        #region 验证小于、等于、大于

        for (int i = 0; i < 100000; i++)
        {
            // 创建一个随机长度和随机值的数组
            int maxSize = 30;
            int maxValue = 100;
            List<int> numbers = [];
            Random random = new();
            var length = random.Next(10, maxSize);
            for (int j = 0; j < length; j++) numbers.Add(random.Next(0, maxValue));
            int[] arr = numbers.ToArray();
            int target = arr[random.Next(0, arr.Length)];

            int[] original = (int[])arr.Clone();

            LessAndEqualAndGreater(arr, target);
            if (!ValidLorEorG(arr, target))
            {
                Console.WriteLine(
                    $"这是第{i + 1}次测试 出现错误 原始数组为{string.Join(",", original.Select(element => element.ToString()))} 目标值为{target}");
                foreach (var element in arr) Console.Write(element + ",");
                Console.WriteLine("\n");
            }
        }

        #endregion

        #region 验证小于等于、大于

        for (int i = 0; i < 100000; i++)
        {
            // 创建一个随机长度和随机值的数组

            int maxSize = 30;
            int maxValue = 100;
            List<int> numbers = [];
            Random random = new();
            var length = random.Next(10, maxSize);
            for (int j = 0; j < length; j++) numbers.Add(random.Next(0, maxValue));
            int[] arr = numbers.ToArray();
            int target = arr[random.Next(0, arr.Length)];


            int[] original = (int[])arr.Clone();

            LessEqualAndGreater(arr, target);
            if (!ValidLEorG(arr, target))
            {
                Console.WriteLine(
                    $"这是第{i + 1}次测试 出现错误 原始数组为{string.Join(",", original.Select(element => element.ToString()))} 目标值为{target}");
                foreach (var element in arr) Console.Write(element + ",");
                Console.WriteLine("\n");
            }
        }

        #endregion

        Console.WriteLine("测试完成");
    }
}