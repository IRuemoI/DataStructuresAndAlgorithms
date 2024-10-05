namespace AdvancedTraining.Lesson33;

public class CourseScheduleIi //Problem_0210
{
    private static int[] FindOrder(int numCourses, int[][]? prerequisites)
    {
        var ans = new int[numCourses];
        for (var i = 0; i < numCourses; i++) ans[i] = i;
        if (prerequisites == null || prerequisites.Length == 0) return ans;
        var nodes = new Dictionary<int, Node>();
        foreach (var arr in prerequisites)
        {
            var to = arr[0];
            var from = arr[1];
            if (!nodes.ContainsKey(to)) nodes[to] = new Node(to);
            if (!nodes.ContainsKey(from)) nodes[from] = new Node(from);
            var t = nodes[to];
            var f = nodes[from];
            f.NextList.Add(t);
            t.In++;
        }

        var index = 0;
        var zeroInQueue = new LinkedList<Node>();
        for (var i = 0; i < numCourses; i++)
            if (!nodes.ContainsKey(i))
            {
                ans[index++] = i;
            }
            else
            {
                if (nodes[i].In == 0) zeroInQueue.AddLast(nodes[i]);
            }

        var needPrerequisiteNums = nodes.Count;
        var count = 0;
        while (zeroInQueue.Count > 0)
        {
            var cur = zeroInQueue.First();
            zeroInQueue.RemoveFirst();
            ans[index++] = cur.Name;
            count++;
            foreach (var next in cur.NextList)
                if (--next.In == 0)
                    zeroInQueue.AddLast(next);
        }

        return count == needPrerequisiteNums ? ans : [];
    }

    public static void Run()
    {
        const int numCourses = 4;
        int[][] prerequisites = [[1, 0], [2, 0], [3, 1], [3, 2]];

        Console.WriteLine(string.Join(",", FindOrder(numCourses, prerequisites))); //输出：[0,2,1,3]
    }

    private class Node(int n)
    {
        public readonly int Name = n;
        public readonly List<Node> NextList = [];
        public int In;
    }
}