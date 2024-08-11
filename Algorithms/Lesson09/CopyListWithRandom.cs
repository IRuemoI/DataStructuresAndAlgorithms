//passed

namespace Algorithms.Lesson09;

public class CopyListWithRandom
{
    private static Node? CopyListWithRand1(Node? head)
    {
        if (head == null) return null;

        // key 老节点
        // value 新节点
        var map = new Dictionary<Node, Node>();
        var cur = head;
        while (cur != null)
        {
            map.Add(cur, new Node(cur.Value));
            cur = cur.Next;
        }

        cur = head;

        while (cur != null)
        {
            // cur 老
            // map.get(cur) 新
            // 新.next ->  cur.next克隆节点找到
            map[cur].Rand = map[cur.Rand!];
            if (cur.Next != null)
                map[cur].Next = map[cur.Next];
            else
                break;


            cur = cur.Next;
        }

        return map[head];
    }

    private static Node? CopyListWithRand2(Node? head)
    {
        if (head == null) return null;

        var cur = head;
        Node? next;
        // copy node and link to every node
        // 1 -> 2
        // 1 -> 1' -> 2
        while (cur != null)
        {
            // cur 老 next 老的下一个
            next = cur.Next;
            cur.Next = new Node(cur.Value)
            {
                Next = next
            };
            cur = next;
        }

        cur = head;
        Node? curCopy;
        // set copy node rand
        // 1 -> 1' -> 2 -> 2'
        while (cur != null)
        {
            // cur 老
            // cur.next 新 copy
            next = cur.Next?.Next;
            curCopy = cur.Next;
            if (curCopy != null) curCopy.Rand = cur.Rand?.Next;
            cur = next;
        }

        // head head.next
        var res = head.Next;
        cur = head;
        // split
        while (cur != null)
        {
            next = cur.Next?.Next;
            curCopy = cur.Next;
            cur.Next = next;
            if (curCopy != null) curCopy.Next = next?.Next;
            cur = next;
        }

        return res;
    }

    private static void PrintRandLinkedList(Node? head)
    {
        var cur = head;
        Console.Write("order: ");
        while (cur != null)
        {
            Console.Write(cur.Value + " ");
            cur = cur.Next;
        }

        Console.WriteLine();
        cur = head;
        Console.Write("rand:  ");
        while (cur != null)
        {
            Console.Write(cur.Rand == null ? "null" : cur.Rand.Value + " ");
            cur = cur.Next;
        }

        Console.WriteLine();
    }

    private static bool IsEquals(Node? linkedList1, Node? linkedList2)
    {
        var list1Value = new List<int>();
        var list1Next = new List<string>();
        var list1Rand = new List<string>();
        var list2Value = new List<int>();
        var list2Next = new List<string>();
        var list2Rand = new List<string>();

        while (linkedList1 != null)
        {
            list1Value.Add(linkedList1.Value);
            list1Next.Add(linkedList1.Next == null ? "null" : linkedList1.Value.ToString());
            list1Rand.Add(linkedList1.Rand == null ? "null" : linkedList1.Value.ToString());
            linkedList1 = linkedList1.Next;
        }

        while (linkedList2 != null)
        {
            list2Value.Add(linkedList2.Value);
            list2Next.Add(linkedList2.Next == null ? "null" : linkedList2.Value.ToString());
            list2Rand.Add(linkedList2.Rand == null ? "null" : linkedList2.Value.ToString());
            linkedList2 = linkedList2.Next;
        }

        var length1 = list1Value.Count;
        var length2 = list2Value.Count;

        if (length1 != length2) return false;


        for (var i = 0; i < Math.Max(length1, length2); i++)
            if (list1Next[i] != list2Next[i] || list1Rand[i] != list2Rand[i] || list1Value[i] != list2Value[i])
                return false;

        return true;
    }

    public static void Run()
    {
        const int maxLength = 30;
        const int testTimes = 3;
        var randomGen = new Random();
        for (var i = 0; i < testTimes; i++)
        {
            var listLength = randomGen.Next(maxLength) + 1;
            Console.WriteLine("本轮生成的链表长度为{0}", listLength);
            var origin = RandomizedList.GenerateRandomizedList(listLength);
            PrintRandLinkedList(origin);

            var copy1 = CopyListWithRand1(origin);
            PrintRandLinkedList(copy1);

            var copy2 = CopyListWithRand2(origin);
            PrintRandLinkedList(copy2);

            if (!(IsEquals(origin, copy1) && IsEquals(copy1, copy2))) Console.WriteLine("oops");

            Console.WriteLine("========================");
        }

        Console.WriteLine("pass");
    }

    public class Node(int data)
    {
        public readonly int Value = data;
        public Node? Next;
        public Node? Rand;
    }

    public static class RandomizedList
    {
        private static Node? _head;
        private static readonly Random Rand = new();

        // 用于复制链表的方法将在之后实现  

        // 生成带有随机指针的链表  
        public static Node? GenerateRandomizedList(int n)
        {
            if (n <= 0) return null;

            _head = new Node(1);
            var current = _head;

            // 生成 n-1 个节点并添加到链表  
            for (var i = 2; i <= n; i++)
            {
                var newNode = new Node(i);
                current.Next = newNode;
                current = newNode;
            }

            // 随机设置每个节点的 random 指针  
            current = _head;

            for (var i = 0; i < n; i++)
            {
                var randomIndex = Rand.Next(n); // 随机选择索引（包括当前节点）  
                var temp = _head;
                var index = 0;

                // 遍历链表找到对应索引的节点  
                while (index < randomIndex)
                {
                    temp = temp.Next!;
                    index++;
                }

                current.Rand = temp;
                current = current.Next!;
            }


            return _head;
        }
    }
}