//测试通过

namespace Algorithms.Lesson02;

public class Km
{
    //验证函数
    private static int Test(int[] array, int k)
    {
        var forTestDict = new Dictionary<int, int>();
        foreach (var num in array)
            if (!forTestDict.TryAdd(num, 1))
                forTestDict[num] += 1;

        foreach (var num in forTestDict.Keys)
            if (forTestDict[num] == k)
                return num;

        return -1;
    }

    // 请保证arr中，只有一种数出现了K次，其他数都出现了M次
    private static int OnlyKTimes(int[] arr, int k, int m)
    {
        if (!(1 < k && k < m)) throw new Exception($"K={k}和M={m}不合法！");

        var bitContainer = new int[32]; //声明一个可以容纳整型二进制位状态的数组
        foreach (var number in arr)
            for (var i = 0; i < 32; i++) //将每个数字中的二进制1分别放到位容器对应的空间中
                if (((number >> i) & 1) == 1)
                    bitContainer[i]++;

        //执行到此时，位容器中的每个元素内容是K个组成出现K次数字的二进制位和M个出现M次的数字的次数之和
        var result = 0;
        for (var i = 0; i < 32; i++)
        {
            //位容器中的每个元素对M求余之后正好剩下K个0时，说明这个二进制位上的0是组成出现K次数字的一部分
            if (bitContainer[i] % m == 0) continue;
            //位容器中的每个元素对M求余之后正好剩下K个1时，说明这个二进制位上的1是组成出现K次数字的一部分
            if (bitContainer[i] % m == k) result |= 1 << i;
            else
                throw new Exception("非预期的求余结果！");
        }

        var count = 0;
        if (result == 0)
        {
            //检查一下结果是否为0
            foreach (var num in arr)
                if (num == 0)
                    count++;

            //如果数组中正好出现了K个0，说明结果是0；否则抛出异常
            if (count != k) throw new Exception("预期为0,但是结果不为零!");
        }

        return result;
    }


    private static int[] RandomArray(int maxKinds, int range, int k, int m)
    {
        var kTimeNum = RandomNumber(range); // 生成正确答案数字
        var arrayNumKinds = (int)(new Random().NextDouble() * maxKinds) + 2;
        // k * 1 + (numKinds - 1) * m
        var arr = new int[k + (arrayNumKinds - 1) * m]; //计算生成数组的长度
        var index = 0;
        for (; index < k; index++) arr[index] = kTimeNum; //将答案填入数组

        arrayNumKinds--;
        var set = new HashSet<int> { kTimeNum }; //将答案写入将要填入数组的每个不同的数字
        while (arrayNumKinds != 0)
        {
            int curNum;
            do
            {
                curNum = RandomNumber(range);
            } while (set.Contains(curNum));

            set.Add(curNum);


            arrayNumKinds--;
            for (var i = 0; i < m; i++) arr[index++] = curNum;
        }

        // arr 填好了
        for (var i = 0; i < arr.Length; i++)
        {
            // i 位置的数，我想随机和j位置的数做交换
            var j = (int)(new Random().NextDouble() * arr.Length); // 0 ~ N-1
            (arr[j], arr[i]) = (arr[i], arr[j]);
        }

        return arr;
    }

    // [-range, +range]
    private static int RandomNumber(int range)
    {
        return (int)(new Random().NextDouble() * range) + 1 - ((int)(new Random().NextDouble() * range) + 1);
    }

    public static void Run()
    {
        var kinds = 5; //数字的最大种类数
        var range = 30; //产生数字范围(-range,range)
        var testTime = 10000;
        var max = 9;
        Console.WriteLine("测试开始");

        for (var i = 0; i < testTime; i++)
        {
            //生成两个一到九之间的数字作为K和M。
            var a = (int)(new Random().NextDouble() * max) + 2; // a:2 ~ 11
            var b = (int)(new Random().NextDouble() * max) + 2; // b:2 ~ 11
            var k = Math.Min(a, b);
            var m = Math.Max(a, b);
            // k < m
            if (k == m) m++;

            var arr = RandomArray(kinds, range, k, m);
            var ans1 = OnlyKTimes(arr, k, m);
            var ans2 = Test(arr, k);
            if (ans1 != ans2)
            {
                Console.WriteLine("出错了！");
                Console.WriteLine($"生成的参数 数组:{string.Join(",", arr)},K:{k},M:{m}");
                Console.WriteLine($"输出答案:{ans1},期望答案:{ans2}\n");
            }
        }

        Console.WriteLine("测试结束");
    }
}