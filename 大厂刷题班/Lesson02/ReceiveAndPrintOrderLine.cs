//pass

namespace AdvancedTraining.Lesson02;

public class ReceiveAndPrintOrderLine
{
    public static void Run()
    {
        // MessageBox only Receive 1~N
        var box = new MessageBox();
        // 1....
        Console.WriteLine("这是2来到的时候");
        box.Receive(2, "B"); // - 2"
        Console.WriteLine("这是1来到的时候");
        box.Receive(1, "A"); // 1 2 -> Print, trigger is 1
        box.Receive(4, "D"); // - 4
        box.Receive(5, "E"); // - 4 5
        box.Receive(7, "G"); // - 4 5 - 7
        box.Receive(8, "H"); // - 4 5 - 7 8
        box.Receive(6, "F"); // - 4 5 6 7 8
        box.Receive(3, "C"); // 3 4 5 6 7 8 -> Print, trigger is 3
        box.Receive(9, "I"); // 9 -> Print, trigger is 9
        box.Receive(10, "J"); // 10 -> Print, trigger is 10
        box.Receive(12, "L"); // - 12
        box.Receive(13, "M"); // - 12 13
        box.Receive(11, "K"); // 11 12 13 -> Print, trigger is 11
    }

    private class Node(string str)
    {
        public readonly string Info = str;
        public Node? Next;
    }

    private class MessageBox
    {
        private readonly Dictionary<int, Node> _headMap = new();
        private readonly Dictionary<int, Node> _tailMap = new();
        private int _waitPoint = 1;

        // 消息的编号，info消息的内容, 消息一定从1开始
        public virtual void Receive(int num, string info)
        {
            if (num < 1) return;
            var cur = new Node(info);
            // num~num
            _headMap[num] = cur;
            _tailMap[num] = cur;
            // 建立了num~num这个连续区间的头和尾
            // 查询有没有某个连续区间以num-1结尾
            if (_tailMap.ContainsKey(num - 1))
            {
                _tailMap[num - 1].Next = cur;
                _tailMap.Remove(num - 1);
                _headMap.Remove(num);
            }

            // 查询有没有某个连续区间以num+1开头的
            if (_headMap.ContainsKey(num + 1))
            {
                cur.Next = _headMap[num + 1];
                _tailMap.Remove(num);
                _headMap.Remove(num + 1);
            }

            if (num == _waitPoint) Print();
        }

        protected virtual void Print()
        {
            var node = _headMap[_waitPoint];
            _headMap.Remove(_waitPoint);
            while (node != null)
            {
                Console.Write(node.Info + " ");
                node = node.Next;
                _waitPoint++;
            }

            _tailMap.Remove(_waitPoint - 1);
            Console.WriteLine();
        }
    }
}