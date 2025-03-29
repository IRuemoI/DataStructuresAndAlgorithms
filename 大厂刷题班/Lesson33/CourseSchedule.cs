namespace AdvancedTraining.Lesson33;

//pass
public class CourseSchedule //leetcode_0207
{
    private static bool CanFinish1(int numCourses, int[][]? prerequisites)
    {
        if (prerequisites == null || prerequisites.Length == 0) return true;
        // 一个编号 对应 一个课的实例
        var nodes = new Dictionary<int, Course>();
        foreach (var arr in prerequisites)
        {
            var to = arr[0];
            var from = arr[1]; // from -> to
            if (!nodes.ContainsKey(to)) nodes[to] = new Course(to);
            if (!nodes.ContainsKey(from)) nodes[from] = new Course(from);
            var t = nodes[to];
            var f = nodes[from];
            f.NextList.Add(t);
            t.In++;
        }

        var needPrerequisiteNumbers = nodes.Count;
        var zeroInQueue = new LinkedList<Course>();
        foreach (var node in nodes.Values)
            if (node.In == 0)
                zeroInQueue.AddLast(node);
        var count = 0;
        while (zeroInQueue.Count > 0)
        {
            var cur = zeroInQueue.First();
            zeroInQueue.RemoveFirst();
            count++;
            foreach (var next in cur.NextList)
                if (--next.In == 0)
                    zeroInQueue.AddLast(next);
        }

        return count == needPrerequisiteNumbers;
    }

    // 和方法1算法过程一样
    // 但是写法优化了
    private static bool CanFinish2(int courses, int[][]? relation)
    {
        if (relation == null || relation.Length == 0) return true;
        // 3 :  0 1 2
        // nexts :   0   {}
        //           1   {}
        //           2   {}
        //           3   {0,1,2}
        var nextList = new List<List<int>>();
        for (var i = 0; i < courses; i++) nextList.Add(new List<int>());
        // 3 入度 1  in[3] == 1
        var @in = new int[courses];
        foreach (var arr in relation)
        {
            // arr[1] from   arr[0] to
            nextList[arr[1]].Add(arr[0]);
            @in[arr[0]]++;
        }

        // 队列
        var zero = new int[courses];
        // 该队列有效范围是[l,r)
        // 新来的数，放哪？r位置，r++
        // 出队列的数，从哪拿？l位置，l++
        // l == r 队列无元素  l < r 队列有元素
        var l = 0;
        var r = 0;
        for (var i = 0; i < courses; i++)
            if (@in[i] == 0)
                zero[r++] = i;
        var count = 0;
        while (l != r)
        {
            count++; // zero[l] 出队列   l++
            foreach (var next in nextList[zero[l++]])
                if (--@in[next] == 0)
                    zero[r++] = next;
        }

        return count == nextList.Count;
    }

    public static void Run()
    {
        var numCourses = 2;
        int[][] prerequisites = [[1, 0]];
        Console.WriteLine(CanFinish1(numCourses, prerequisites)); //输出True
        Console.WriteLine(CanFinish2(numCourses, prerequisites)); //输出True
        numCourses = 2;
        prerequisites = [[1, 0], [0, 1]];
        Console.WriteLine(CanFinish1(numCourses, prerequisites)); //输出False
        Console.WriteLine(CanFinish2(numCourses, prerequisites)); //输出False
    }

    // 一个node，就是一个课程
    // name是课程的编号
    // in是课程的入度
    public class Course(int n)
    {
        public readonly List<Course> NextList = [];
        public int In;
        public int Name = n;
    }
}