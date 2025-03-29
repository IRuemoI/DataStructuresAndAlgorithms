namespace Algorithms.Lesson29;

public class ReservoirSampling
{
    // 请等概率返回1~i中的一个数字
    private static int Random(int i)
    {
        return (int)(new Random().NextDouble() * i) + 1;
    }

    public static void Run()
    {
        var test = 10000;
        var ballNum = 17;
        var count = new int[ballNum + 1];
        for (var i = 0; i < test; i++)
        {
            var bag = new int[10];
            var bagI = 0;
            for (var num = 1; num <= ballNum; num++)
                if (num <= 10)
                {
                    bag[bagI++] = num;
                }
                else
                {
                    // num > 10
                    if (Random(num) <= 10)
                    {
                        // 一定要把num球入袋子
                        bagI = (int)(new Random().NextDouble() * 10);
                        bag[bagI] = num;
                    }
                }

            foreach (var num in bag) count[num]++;
        }

        for (var i = 0; i <= ballNum; i++) Console.WriteLine(count[i]);

        Console.WriteLine("hello");
        var all = 100;
        var choose = 10;
        var testTimes = 50000;
        var counts = new int[all + 1];
        for (var i = 0; i < testTimes; i++)
        {
            var box = new RandomBox(choose);
            for (var num = 1; num <= all; num++) box.Add(num);

            var ans = box.Choices();
            foreach (var element in ans) counts[element]++;
        }

        for (var i = 0; i < counts.Length; i++) Console.WriteLine(i + " times : " + counts[i]);
    }

    private class RandomBox(int capacity)
    {
        private readonly int[] _bag = new int[capacity];
        private int _count;

        private static int Rand(int max)
        {
            return (int)(new Random().NextDouble() * max) + 1;
        }

        public void Add(int num)
        {
            _count++;
            if (_count <= capacity)
            {
                _bag[_count - 1] = num;
            }
            else
            {
                if (Rand(_count) <= capacity) _bag[Rand(capacity) - 1] = num;
            }
        }

        public int[] Choices()
        {
            var ans = new int[capacity];
            for (var i = 0; i < capacity; i++) ans[i] = _bag[i];

            return ans;
        }
    }
}