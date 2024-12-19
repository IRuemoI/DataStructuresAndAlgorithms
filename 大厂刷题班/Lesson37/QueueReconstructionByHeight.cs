#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson37;

//https://leetcode.cn/problems/queue-reconstruction-by-height/description/
public class QueueReconstructionByHeight //leetcode_0406
{
    private static int[,] reconstructQueue1(int[,] people)
    {
        var N = people.Length;
        var units = new Unit[N];
        for (var i = 0; i < N; i++) units[i] = new Unit(people[i, 0], people[i, 1]);

        Array.Sort(units, new UnitComparator());
        var arrList = new List<Unit>();
        foreach (var unit in units) arrList.Insert(unit.k, unit);
        var ans = new int[N, 2];
        var index = 0;
        foreach (var unit in arrList)
        {
            ans[index, 0] = unit.h;
            ans[index++, 1] = unit.k;
        }

        return ans;
    }

    private static int[,] reconstructQueue2(int[,] people)
    {
        var N = people.Length;
        var units = new Unit[N];
        for (var i = 0; i < N; i++) units[i] = new Unit(people[i, 0], people[i, 1]);

        Array.Sort(units, new UnitComparator());
        var tree = new SBTree();
        for (var i = 0; i < N; i++) tree.Insert(units[i].k, i);

        var allIndexes = tree.allIndexes();
        var ans = new int[N, 2];
        var index = 0;
        foreach (var arri in allIndexes)
        {
            ans[index, 0] = units[arri].h;
            ans[index++, 1] = units[arri].k;
        }

        return ans;
    }

    // 通过以下这个测试，
    // 可以很明显的看到LinkedList的插入和get效率不如SBTree
    // LinkedList需要找到index所在的位置之后才能插入或者读取，时间复杂度O(N)
    // SBTree是平衡搜索二叉树，所以插入或者读取时间复杂度都是O(logN)
    // todo:待整理
    public static void Run()
    {
        // 功能测试
        var test = 10000;
        const int max = 1000000;
        var pass = true;
        var list = new List<int>();
        var sbtree = new SBTree();
        for (var i = 0; i < test; i++)
        {
            var randomIndex = (int)(Utility.getRandomDouble * (i + 1));
            var randomValue = (int)(Utility.getRandomDouble * (max + 1));
            list[randomIndex] = randomValue;
            sbtree.Insert(randomIndex, randomValue);
        }

        for (var i = 0; i < test; i++)
            if (list[i] != sbtree.Get(i))
            {
                pass = false;
                break;
            }

        Console.WriteLine("功能测试是否通过 : " + pass);

        // 性能测试
        test = 50000;
        list = new List<int>();
        sbtree = new SBTree();

        Utility.RestartStopwatch();
        for (var i = 0; i < test; i++)
        {
            var randomIndex = (int)(Utility.getRandomDouble * (i + 1));
            var randomValue = (int)(Utility.getRandomDouble * (max + 1));
            list[randomIndex] = randomValue;
        }


        Console.WriteLine("LinkedList插入总时长(毫秒) ： " + Utility.GetStopwatchElapsedMilliseconds());

        Utility.RestartStopwatch();
        for (var i = 0; i < test; i++)
        {
            var randomIndex = (int)(Utility.getRandomDouble * (i + 1));
            var temp = list[randomIndex];
        }


        Console.WriteLine("LinkedList读取总时长(毫秒) : " + Utility.GetStopwatchElapsedMilliseconds());

        Utility.RestartStopwatch();
        for (var i = 0; i < test; i++)
        {
            var randomIndex = (int)(Utility.getRandomDouble * (i + 1));
            var randomValue = (int)(Utility.getRandomDouble * (max + 1));
            sbtree.Insert(randomIndex, randomValue);
        }


        Console.WriteLine("SBTree插入总时长(毫秒) : " + Utility.GetStopwatchElapsedMilliseconds());

        Utility.RestartStopwatch();
        for (var i = 0; i < test; i++)
        {
            var randomIndex = (int)(Utility.getRandomDouble * (i + 1));
            sbtree.Get(randomIndex);
        }


        Console.WriteLine("SBTree读取总时长(毫秒) :  " + Utility.GetStopwatchElapsedMilliseconds());
    }

    public class Unit
    {
        public int h;
        public int k;

        public Unit(int height, int greater)
        {
            h = height;
            k = greater;
        }
    }

    public class UnitComparator : IComparer<Unit>
    {
        public virtual int Compare(Unit o1, Unit o2)
        {
            return o1.h != o2.h ? o2.h - o1.h : o1.k - o2.k;
        }
    }

    public class SBTNode
    {
        public SBTNode l;
        public SBTNode r;
        public int size;
        public int value;

        public SBTNode(int arrIndex)
        {
            value = arrIndex;
            size = 1;
        }
    }

    public class SBTree
    {
        internal SBTNode root;

        internal virtual SBTNode RightRotate(SBTNode cur)
        {
            var leftNode = cur.l;
            cur.l = leftNode.r;
            leftNode.r = cur;
            leftNode.size = cur.size;
            cur.size = (cur.l != null ? cur.l.size : 0) + (cur.r != null ? cur.r.size : 0) + 1;
            return leftNode;
        }

        internal virtual SBTNode LeftRotate(SBTNode cur)
        {
            var rightNode = cur.r;
            cur.r = rightNode.l;
            rightNode.l = cur;
            rightNode.size = cur.size;
            cur.size = (cur.l != null ? cur.l.size : 0) + (cur.r != null ? cur.r.size : 0) + 1;
            return rightNode;
        }

        internal virtual SBTNode Maintain(SBTNode cur)
        {
            if (cur == null) return null;

            var leftSize = cur.l != null ? cur.l.size : 0;
            var leftLeftSize = cur.l != null && cur.l.l != null ? cur.l.l.size : 0;
            var leftRightSize = cur.l != null && cur.l.r != null ? cur.l.r.size : 0;
            var rightSize = cur.r != null ? cur.r.size : 0;
            var rightLeftSize = cur.r != null && cur.r.l != null ? cur.r.l.size : 0;
            var rightRightSize = cur.r != null && cur.r.r != null ? cur.r.r.size : 0;
            if (leftLeftSize > rightSize)
            {
                cur = RightRotate(cur);
                cur.r = Maintain(cur.r);
                cur = Maintain(cur);
            }
            else if (leftRightSize > rightSize)
            {
                cur.l = LeftRotate(cur.l);
                cur = RightRotate(cur);
                cur.l = Maintain(cur.l);
                cur.r = Maintain(cur.r);
                cur = Maintain(cur);
            }
            else if (rightRightSize > leftSize)
            {
                cur = LeftRotate(cur);
                cur.l = Maintain(cur.l);
                cur = Maintain(cur);
            }
            else if (rightLeftSize > leftSize)
            {
                cur.r = RightRotate(cur.r);
                cur = LeftRotate(cur);
                cur.l = Maintain(cur.l);
                cur.r = Maintain(cur.r);
                cur = Maintain(cur);
            }

            return cur;
        }

        internal virtual SBTNode Insert(SBTNode root, int index, SBTNode cur)
        {
            if (root == null) return cur;

            root.size++;
            var leftAndHeadSize = (root.l != null ? root.l.size : 0) + 1;
            if (index < leftAndHeadSize)
                root.l = Insert(root.l, index, cur);
            else
                root.r = Insert(root.r, index - leftAndHeadSize, cur);

            root = Maintain(root);
            return root;
        }

        internal virtual SBTNode Get(SBTNode root, int index)
        {
            var leftSize = root.l != null ? root.l.size : 0;
            if (index < leftSize)
                return Get(root.l, index);
            if (index == leftSize)
                return root;
            return Get(root.r, index - leftSize - 1);
        }

        internal virtual void Process(SBTNode head, LinkedList<int> indexes)
        {
            if (head == null) return;

            Process(head.l, indexes);
            indexes.AddLast(head.value);
            Process(head.r, indexes);
        }

        public virtual void Insert(int index, int value)
        {
            var cur = new SBTNode(value);
            if (root == null)
            {
                root = cur;
            }
            else
            {
                if (index <= root.size) root = Insert(root, index, cur);
            }
        }

        public virtual int Get(int index)
        {
            var ans = Get(root, index);
            return ans.value;
        }

        public virtual LinkedList<int> allIndexes()
        {
            var indexes = new LinkedList<int>();
            Process(root, indexes);
            return indexes;
        }
    }
}