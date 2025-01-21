//pass
namespace AdvancedTraining.Lesson19;
// 本题测试链接 : https://leetcode.cn/problems/smallest-range-covering-elements-from-k-lists/
public class SmallestRangeCoveringElementsFromKLists
{
    private static int[] SmallestRange(List<List<int>> numbers)
    {
        var n = numbers.Count;
        var orderSet = new SortedSet<Node>(new NodeComparator());
        for (var i = 0; i < n; i++) orderSet.Add(new Node(numbers[i][0], i, 0));
        var set = false;
        var a = 0;
        var b = 0;
        while (orderSet.Count == n)
        {
            var min = orderSet.Min;
            var max = orderSet.Max;
            if (!set || max?.Value - min?.Value < b - a)
            {
                set = true;
                a = min!.Value;
                b = max!.Value;
            }

            min = orderSet.First();
            orderSet.Remove(orderSet.First());
            var arrId = min.ArrId;
            var index = min.Index + 1;
            if (index != numbers[arrId].Count) orderSet.Add(new Node(numbers[arrId][index], arrId, index));
        }

        return [a, b];
    }

    public static void Run()
    {
        List<List<int>> numbers = [[4, 10, 15, 24, 26], [0, 9, 12, 20], [5, 18, 22, 30]];
        var result = SmallestRange(numbers);
        foreach (var item in result) Console.Write(item + ","); //输出20，24
    }

    private class Node(int v, int ai, int i)
    {
        public readonly int ArrId = ai;
        public readonly int Index = i;
        public readonly int Value = v;
    }

    private class NodeComparator : IComparer<Node>
    {
        public int Compare(Node? o1, Node? o2)
        {
            if (o1 == null || o2 == null) throw new Exception();
            return o1.Value != o2.Value ? o1.Value - o2.Value : o1.ArrId - o2.ArrId;
        }
    }
}