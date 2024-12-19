//passed

namespace Algorithms.Lesson09;

public class CopyListWithRandom
{
    private static Node? CopyListWithRand1(Node? head)
    {
        if (head == null) return null; //空链表直接返回

        // 复制各节点，并建立 “原节点 -> 新节点” 的映射字典
        var map = new Dictionary<Node, Node>();
        var current = head;
        while (current != null)
        {
            map.Add(current, new Node(current.Value));
            current = current.Next;
        }

        // 按照旧链表复制新链表的next和random指向
        current = head;
        while (current != null)
        {
            map[current].Rand = map[current.Rand!];
            if (current.Next != null)
                map[current].Next = map[current.Next];
            else
                break;

            current = current.Next;
        }

        //返回复制链表的头节点
        return map[head];
    }

    private static Node? CopyListWithRand2(Node? head)
    {
        if (head == null) return null; //空链表直接返回

        var current = head;
        Node? originNext;

        // 将原链表的每个节点复制并发到本节点的后方
        while (current != null)
        {
            originNext = current.Next;
            current.Next = new Node(current.Value)
            {
                Next = originNext
            };
            current = originNext;
        }

        // 复制各节点的random指向
        current = head;
        Node? currentCopy;
        while (current != null)
        {
            originNext = current.Next?.Next;
            currentCopy = current.Next;
            if (currentCopy != null) currentCopy.Rand = current.Rand?.Next;
            current = originNext;
        }

        // 新链表的头节点是原链表头节点的下个节点
        var newHead = head.Next;

        // 分离原链表和复制链表
        current = head;
        while (current != null)
        {
            originNext = current.Next?.Next;
            currentCopy = current.Next;
            current.Next = originNext;
            if (currentCopy != null) currentCopy.Next = originNext?.Next;
            current = originNext;
        }

        return newHead;
    }

    # region 用于测试

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
    
    private class Node(int data)
    {
        public readonly int Value = data;
        public Node? Next;
        public Node? Rand;
    }

    private static class RandomizedList
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

    #endregion

    public static void Run()
    {
        const int maxLength = 30;
        const int testTimes = 10;
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

        Console.WriteLine("测试完成");
    }
}