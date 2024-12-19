//测试通过

namespace Algorithms.Lesson03;

public class HashMapAndSortedMap
{
    public static void Run()
    {
        Dictionary<int, string> test = new();
        var a = 19000000;
        var b = 19000000;
        Console.WriteLine(a == b);

        test.Add(a, "我是3");
        Console.WriteLine(test.ContainsKey(b));

        var z1 = new Zuo(1);
        var z2 = new Zuo(1);
        Dictionary<Zuo, string> test2 = new() { { z1, "我是z1" } };
        Console.WriteLine(test2.ContainsKey(z2));

        // UnSortedMap
        Dictionary<int, string?> map = new()
        {
            { 1000000, "我是1000000" },
            { 2, "我是2" },
            { 3, "我是3" },
            { 4, "我是4" },
            { 5, "我是5" },
            { 6, "我是6" }
        };
        //map.Add(1000000, "我是1000001");

        Console.WriteLine(map.ContainsKey(1));
        Console.WriteLine(map.ContainsKey(10));

        Console.WriteLine(map[4]);
        map.TryGetValue(10, out var value1);
        Console.WriteLine(value1);

        map[4] = "他是4";
        Console.WriteLine(map[4]);

        map.Remove(4);
        map.TryGetValue(4, out var value2);
        Console.WriteLine(value2);

        // key
        HashSet<string> set = ["abc"];
        Console.WriteLine(set.Contains("abc") ? "yes" : "No");
        set.Remove("abc");

        // 哈希表，增、删、改、查，在使用时，O（1）

        Console.WriteLine("=====================");

        var c = 100000;
        var d = 100000;
        Console.WriteLine(c.Equals(d));

        var e = 127; // - 128 ~ 127
        var f = 127;
        Console.WriteLine(e == f);

        Dictionary<Node, string> map2 = new();
        var node1 = new Node(1);
        var node2 = node1;
        map2[node1] = "我是node1";
        map2[node2] = "我是node1";
        Console.WriteLine(map2.Count);

        Console.WriteLine("======================");

        // SortedDictionary 有序表：接口名
        // 红黑树、avl、sb树、跳表
        // O(logN)
        Console.WriteLine("有序表测试开始");
        SortedDictionary<int, string> sortedDictionary = new()
        {
            { 3, "我是3" },
            { 4, "我是4" },
            { 8, "我是8" },
            { 5, "我是5" },
            { 7, "我是7" },
            { 1, "我是1" },
            { 2, "我是2" }
        };

        Console.WriteLine(sortedDictionary.ContainsKey(1));
        Console.WriteLine(sortedDictionary.ContainsKey(10));

        Console.WriteLine(sortedDictionary[4]);
        sortedDictionary.TryGetValue(10, out var value3);
        Console.WriteLine(value3);

        sortedDictionary[4] = "他是4";
        Console.WriteLine(sortedDictionary[4]);

        // treeMap.remove(4);
        Console.WriteLine(sortedDictionary[4]);

        Console.WriteLine("新鲜：");

        Console.WriteLine(sortedDictionary.First());
        Console.WriteLine(sortedDictionary.Last());
        // <= 4
        //Console.WriteLine(treeMap.floorKey(4));
        // >= 4
        //Console.WriteLine(treeMap.ceilingKey(4));
        // O(logN)
    }

    private class Node(int v)
    {
        public int Value = v;
    }

    private class Zuo(int v)
    {
        public int Value = v;
    }
}