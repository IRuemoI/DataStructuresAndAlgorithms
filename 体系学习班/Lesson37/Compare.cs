#region

using Algorithms.Lesson35;
using Algorithms.Lesson36;
using Common.Utilities;

#endregion

namespace Algorithms.Lesson37;

// 本文件为avl、sbt、skiplist三种结构的测试文件
public static class Compare
{
    public static void Run()
    {
        FunctionTest();
        Console.WriteLine("======");
        PerformanceTest();
    }

    private static void FunctionTest()
    {
        Console.WriteLine("功能测试开始");
        var sortedDictionary = new SortedDictionary<int, int>(); //红黑树
        var avl = new AvlTreeMap<int, int>();
        var sbt = new SizeBalancedTreeMap<int, int>();
        var skip = new SkipListMap<int, int>();
        const int maxK = 500;
        const int maxV = 50000;
        var testTime = 10000;
        for (var i = 0; i < testTime; i++)
        {
            var newKey = (int)(Utility.getRandomDouble * maxK) + 1;
            var newValue = (int)(Utility.getRandomDouble * maxV) + 1;
            sortedDictionary[newKey] = newValue;
            avl.Put(newKey, newValue);
            sbt.Put(newKey, newValue);
            skip.Put(newKey, newValue);

            var removeKey = (int)(Utility.getRandomDouble * maxK) + 1;
            sortedDictionary.Remove(removeKey);
            avl.Remove(removeKey);
            sbt.Remove(removeKey);
            skip.Remove(removeKey);

            var queryKey = (int)(Utility.getRandomDouble * maxK) + 1;
            if (sortedDictionary.ContainsKey(queryKey) != avl.ContainsKey(queryKey) ||
                avl.ContainsKey(queryKey) != sbt.ContainsKey(queryKey) ||
                sbt.ContainsKey(queryKey) != skip.ContainsKey(queryKey))
            {
                Console.WriteLine("containsKey Oops");
                Console.WriteLine(sortedDictionary.ContainsKey(queryKey));
                Console.WriteLine(avl.ContainsKey(queryKey));
                Console.WriteLine(sbt.ContainsKey(queryKey));
                Console.WriteLine(skip.ContainsKey(queryKey));
                break;
            }

            if (sortedDictionary.ContainsKey(queryKey))
            {
                var v1 = sortedDictionary[queryKey];
                int? v2 = avl.Get(queryKey);
                int? v3 = sbt.Get(queryKey);
                int? v4 = skip.Get(queryKey);
                if (v1 != v2 || v2 != v3 || v3 != v4)
                {
                    Console.WriteLine("get Oops");
                    Console.WriteLine(sortedDictionary[queryKey]);
                    Console.WriteLine(avl.Get(queryKey));
                    Console.WriteLine(sbt.Get(queryKey));
                    Console.WriteLine(skip.Get(queryKey));
                    break;
                }

                int? f1 = sortedDictionary.LastOrDefault(x => x.Key <= queryKey).Key; //FloorKey
                int? f2 = avl.FloorKey(queryKey);
                int? f3 = sbt.FloorKey(queryKey);
                int? f4 = skip.FloorKey(queryKey);
                if (f1 == null && (f2 != null || f3 != null || f4 != null))
                {
                    Console.WriteLine("floorKey Oops");
                    Console.WriteLine(sortedDictionary.LastOrDefault(x => x.Key <= queryKey).Key);
                    Console.WriteLine(avl.FloorKey(queryKey));
                    Console.WriteLine(sbt.FloorKey(queryKey));
                    Console.WriteLine(skip.FloorKey(queryKey));
                    break;
                }

                if (f1 != null && (f2 == null || f3 == null || f4 == null))
                {
                    Console.WriteLine("floorKey Oops");
                    Console.WriteLine(sortedDictionary.LastOrDefault(x => x.Key <= queryKey).Key);
                    Console.WriteLine(avl.FloorKey(queryKey));
                    Console.WriteLine(sbt.FloorKey(queryKey));
                    Console.WriteLine(skip.FloorKey(queryKey));
                    break;
                }

                if (f1 != null)
                {
                    var ans1 = f1.Value;
                    var ans2 = f2!.Value;
                    var ans3 = f3!.Value;
                    var ans4 = f4!.Value;
                    if (ans1 != ans2 || ans2 != ans3 || ans3 != ans4)
                    {
                        Console.WriteLine("floorKey Oops");
                        Console.WriteLine(sortedDictionary.LastOrDefault(x => x.Key <= queryKey).Key);
                        Console.WriteLine(avl.FloorKey(queryKey));
                        Console.WriteLine(sbt.FloorKey(queryKey));
                        Console.WriteLine(skip.FloorKey(queryKey));
                        break;
                    }
                }

                f1 = sortedDictionary.FirstOrDefault(x => x.Key >= queryKey).Key; //CeilingKey
                f2 = avl.CeilingKey(queryKey);
                f3 = sbt.CeilingKey(queryKey);
                f4 = skip.CeilingKey(queryKey);
                if (f1 == null && (f2 != null || f3 != null || f4 != null))
                {
                    Console.WriteLine("ceilingKey Oops");
                    Console.WriteLine(sortedDictionary.FirstOrDefault(x => x.Key >= queryKey).Key);
                    Console.WriteLine(avl.CeilingKey(queryKey));
                    Console.WriteLine(sbt.CeilingKey(queryKey));
                    Console.WriteLine(skip.CeilingKey(queryKey));
                    break;
                }

                if (f1 != null && (f2 == null || f3 == null || f4 == null))
                {
                    Console.WriteLine("ceilingKey Oops");
                    Console.WriteLine(sortedDictionary.First(x => x.Key >= queryKey).Key);
                    Console.WriteLine(avl.CeilingKey(queryKey));
                    Console.WriteLine(sbt.CeilingKey(queryKey));
                    Console.WriteLine(skip.CeilingKey(queryKey));
                    break;
                }

                if (f1 != null)
                {
                    var ans1 = f1.Value;
                    var ans2 = f2!.Value;
                    var ans3 = f3!.Value;
                    var ans4 = f4!.Value;
                    if (ans1 != ans2 || ans2 != ans3 || ans3 != ans4)
                    {
                        Console.WriteLine("ceilingKey Oops");
                        Console.WriteLine(sortedDictionary.FirstOrDefault(x => x.Key >= queryKey).Key);
                        Console.WriteLine(avl.CeilingKey(queryKey));
                        Console.WriteLine(sbt.CeilingKey(queryKey));
                        Console.WriteLine(skip.CeilingKey(queryKey));
                        break;
                    }
                }
            }

            int? g1 = sortedDictionary.FirstOrDefault().Key;
            int? g2 = avl.FirstKey();
            int? g3 = sbt.FirstKey();
            int? g4 = skip.FirstKey();
            if (g1 == null && (g2 != null || g3 != null || g4 != null))
            {
                Console.WriteLine("firstKey Oops1");
                Console.WriteLine(sortedDictionary.FirstOrDefault().Key);
                Console.WriteLine(avl.FirstKey());
                Console.WriteLine(sbt.FirstKey());
                Console.WriteLine(skip.FirstKey());
                break;
            }

            if (g1 != null && (g2 == null || g3 == null || g4 == null))
            {
                Console.WriteLine("firstKey Oops2");
                Console.WriteLine(sortedDictionary.FirstOrDefault().Key);
                Console.WriteLine(avl.FirstKey());
                Console.WriteLine(sbt.FirstKey());
                Console.WriteLine(skip.FirstKey());
                break;
            }

            if (g1 != null)
            {
                var ans1 = g1.Value;
                var ans2 = g2!.Value;
                var ans3 = g3!.Value;
                var ans4 = g4!.Value;
                if (ans1 != ans2 || ans2 != ans3 || ans1 != ans4)
                {
                    Console.WriteLine("firstKey Oops3");
                    Console.WriteLine(sortedDictionary[0]);
                    Console.WriteLine(avl.FirstKey());
                    Console.WriteLine(sbt.FirstKey());
                    Console.WriteLine(skip.FirstKey());
                    break;
                }
            }

            g1 = sortedDictionary.LastOrDefault().Key;
            g2 = avl.LastKey();
            g3 = sbt.LastKey();
            g4 = skip.LastKey();
            if (g1 == null && (g2 != null || g3 != null || g4 != null))
            {
                Console.WriteLine("lastKey Oops4");
                Console.WriteLine(sortedDictionary.LastOrDefault().Key);
                Console.WriteLine(avl.LastKey());
                Console.WriteLine(sbt.LastKey());
                Console.WriteLine(skip.LastKey());
                break;
            }

            if (g1 != null && (g2 == null || g3 == null || g4 == null))
            {
                Console.WriteLine("firstKey Oops");
                Console.WriteLine(sortedDictionary.LastOrDefault().Key);
                Console.WriteLine(avl.LastKey());
                Console.WriteLine(sbt.LastKey());
                Console.WriteLine(skip.LastKey());
                break;
            }

            if (g1 != null)
            {
                var ans1 = g1.Value;
                var ans2 = g2!.Value;
                var ans3 = g3!.Value;
                var ans4 = g4!.Value;
                if (ans1 != ans2 || ans2 != ans3 || ans3 != ans4)
                {
                    Console.WriteLine("lastKey Oops");
                    Console.WriteLine(sortedDictionary.LastOrDefault().Key);
                    Console.WriteLine(avl.LastKey());
                    Console.WriteLine(sbt.LastKey());
                    Console.WriteLine(skip.LastKey());
                    break;
                }
            }

            if (sortedDictionary.Count != avl.size ||
                avl.size != sbt.size ||
                sbt.size != skip.size)
            {
                Console.WriteLine("size Oops");
                Console.WriteLine(sortedDictionary.Count);
                Console.WriteLine(avl.size);
                Console.WriteLine(sbt.size);
                Console.WriteLine(skip.size);
                break;
            }
        }

        Console.WriteLine("功能测试结束");
    }

    private static void PerformanceTest()
    {
        Console.WriteLine("性能测试开始");


        var max = 10000;
        var sortedDictionary = new SortedDictionary<int, int>();
        var avl = new AvlTreeMap<int, int>();
        var sbt = new SizeBalancedTreeMap<int, int>();
        var skip = new SkipListMap<int, int>();
        Console.WriteLine("顺序递增加入测试，数据规模 : " + max);

        Utility.RestartStopwatch();
        for (var i = 0; i < max; i++) sortedDictionary.Add(i, i);

        Console.WriteLine($"treeMap 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = 0; i < max; i++) avl.Put(i, i);

        Console.WriteLine($"avl 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = 0; i < max; i++) sbt.Put(i, i);

        Console.WriteLine($"sbt 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = 0; i < max; i++) skip.Put(i, i);


        Console.WriteLine($"skip 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Console.WriteLine("顺序递增删除测试，数据规模 : " + max);
        Utility.RestartStopwatch();
        for (var i = 0; i < max; i++) sortedDictionary.Remove(i);


        Console.WriteLine($"sortedDictionary 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");


        Utility.RestartStopwatch();
        for (var i = 0; i < max; i++) avl.Remove(i);

        Console.WriteLine($"avl 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = 0; i < max; i++) sbt.Remove(i);

        Console.WriteLine($"sbt 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();

        for (var i = 0; i < max; i++) skip.Remove(i);

        Console.WriteLine($"skip 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Console.WriteLine("顺序递减加入测试，数据规模 : " + max);
        Utility.RestartStopwatch();
        for (var i = max; i >= 0; i--) sortedDictionary[i] = i;


        Console.WriteLine($"sortedDictionary 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = max; i >= 0; i--) avl.Put(i, i);

        Console.WriteLine($"avl 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = max; i >= 0; i--) sbt.Put(i, i);

        Console.WriteLine($"sbt 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = max; i >= 0; i--) skip.Put(i, i);

        Console.WriteLine($"skip 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Console.WriteLine("顺序递减删除测试，数据规模 : " + max);
        Utility.RestartStopwatch();
        for (var i = max; i >= 0; i--) sortedDictionary.Remove(i);

        Console.WriteLine($"sortedDictionary 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = max; i >= 0; i--) avl.Remove(i);

        Console.WriteLine($"avl 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = max; i >= 0; i--) sbt.Remove(i);

        Console.WriteLine($"sbt 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = max; i >= 0; i--) skip.Remove(i);

        Console.WriteLine($"skip 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Console.WriteLine("随机加入测试，数据规模 : " + max);
        Utility.RestartStopwatch();
        for (var i = 0; i < max; i++) sortedDictionary[(int)(Utility.getRandomDouble * i)] = i;

        Console.WriteLine($"sortedDictionary 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = max; i >= 0; i--) avl.Put((int)(Utility.getRandomDouble * i), i);

        Console.WriteLine($"avl 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = max; i >= 0; i--) sbt.Put((int)(Utility.getRandomDouble * i), i);

        Console.WriteLine($"sbt 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = max; i >= 0; i--) skip.Put((int)(Utility.getRandomDouble * i), i);

        Console.WriteLine($"skip 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Console.WriteLine("随机删除测试，数据规模 : " + max);
        Utility.RestartStopwatch();
        for (var i = 0; i < max; i++) sortedDictionary.Remove((int)(Utility.getRandomDouble * i));

        Console.WriteLine($"sortedDictionary 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = max; i >= 0; i--) avl.Remove((int)(Utility.getRandomDouble * i));

        Console.WriteLine($"avl 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = max; i >= 0; i--) sbt.Remove((int)(Utility.getRandomDouble * i));

        Console.WriteLine($"sbt 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = max; i >= 0; i--) skip.Remove((int)(Utility.getRandomDouble * i));

        Console.WriteLine($"skip 运行时间{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Console.WriteLine("性能测试结束");
    }
}