//测试通过

#region

using Common.DataStructures.Heap;
using Common.Utilities;

#endregion

namespace Algorithms.Lesson07;

public class EveryStepShowBoss
{
    private static List<List<int>> TopK(int[] arr, bool[] op, int k)
    {
        var ans = new List<List<int>>();
        var whoIsAwardees = new WhoIsAwardee(k);
        for (var i = 0; i < arr.Length; i++)
        {
            whoIsAwardees.Operate(i, arr[i], op[i]);
            ans.Add(whoIsAwardees.GetAwardees());
        }

        return ans;
    }

    // 干完所有的事，模拟，不优化
    private static List<List<int>> Compare(int[] arr, bool[] op, int k)
    {
        Dictionary<int, Customer> map = new();
        List<Customer> candidate = new();
        List<Customer> awardee = new();
        List<List<int>> ans = new();
        for (var i = 0; i < arr.Length; i++)
        {
            var id = arr[i];
            var buyOrRefund = op[i];
            if (!buyOrRefund && !map.ContainsKey(id))
            {
                ans.Add(GetCurAns(awardee));
                continue;
            }

            // 没有发生：用户购买数为0并且又退货了
            // 用户之前购买数是0，此时买货事件
            // 用户之前购买数>0， 此时买货
            // 用户之前购买数>0, 此时退货
            if (!map.ContainsKey(id)) map.Add(id, new Customer(id, 0));

            // 买、卖
            var c = map[id];
            if (buyOrRefund)
                c.Buy++;
            else
                c.Buy--;

            if (c.Buy == 0)
                map.Remove(id);

            // c
            // 下面做
            if (!candidate.Contains(c) && !awardee.Contains(c))
            {
                if (awardee.Count < k)
                {
                    c.EnterTime = i;
                    awardee.Add(c);
                }
                else
                {
                    c.EnterTime = i;
                    candidate.Add(c);
                }
            }

            CleanZeroBuy(candidate);
            CleanZeroBuy(awardee);
            candidate.Sort((x, y) => x.Buy != y.Buy ? y.Buy - x.Buy : x.EnterTime - y.EnterTime);
            awardee.Sort((x, y) => x.Buy != y.Buy ? x.Buy - y.Buy : x.EnterTime - y.EnterTime);
            Move(candidate, awardee, k, i);
            ans.Add(GetCurAns(awardee));
        }

        return ans;
    }

    private static void Move(List<Customer> candidate, List<Customer> awardee, int k, int time)
    {
        if (candidate.Count == 0) return;

        // 候选区不为空
        if (awardee.Count < k)
        {
            var c = candidate[0];
            c.EnterTime = time;
            awardee.Add(c);
            candidate.RemoveAt(0);
        }
        else
        {
            // 等奖区满了，候选区有东西
            if (candidate[0].Buy > awardee[0].Buy)
            {
                var oldAwardee = awardee[0];
                awardee.RemoveAt(0);
                var newAwardee = candidate[0];
                candidate.RemoveAt(0);
                newAwardee.EnterTime = time;
                oldAwardee.EnterTime = time;
                awardee.Add(newAwardee);
                candidate.Add(oldAwardee);
            }
        }
    }

    private static void CleanZeroBuy(List<Customer> arr)
    {
        var noZero = new List<Customer>();
        foreach (var c in arr)
            if (c.Buy != 0)
                noZero.Add(c);

        arr.Clear();
        foreach (var c in noZero) arr.Add(c);
    }

    private static List<int> GetCurAns(List<Customer> awardee)
    {
        List<int> ans = new();
        foreach (var c in awardee) ans.Add(c.Id);

        return ans;
    }

    // 为了测试
    private static Data RandomData(int maxValue, int maxLen)
    {
        var len = (int)(Utility.GetRandomDouble * maxLen) + 1;
        var arr = new int[len];
        var op = new bool[len];
        for (var i = 0; i < len; i++)
        {
            arr[i] = (int)(Utility.GetRandomDouble * maxValue);
            op[i] = Utility.GetRandomDouble < 0.5;
        }

        return new Data(arr, op);
    }

    // 为了测试
    private static bool SameAnswer(List<List<int>> ans1, List<List<int>> ans2)
    {
        if (ans1.Count != ans2.Count) return false;

        for (var i = 0; i < ans1.Count; i++)
        {
            var cur1 = ans1[i];
            var cur2 = ans2[i];
            if (cur1.Count != cur2.Count) return false;

            cur1.Sort((a, b) => a - b);
            cur2.Sort((a, b) => a - b);
            for (var j = 0; j < cur1.Count; j++)
                if (!cur1[j].Equals(cur2[j]))
                    return false;
        }

        return true;
    }

    public static void Run()
    {
        const int maxValue = 10;
        const int maxLen = 100;
        const int maxK = 6;
        const int testTimes = 1000;

        Console.WriteLine("测试开始");

        Utility.RestartStopwatch();
        for (var i = 0; i < testTimes; i++)
        {
            var testData = RandomData(maxValue, maxLen);
            var k = (int)(Utility.GetRandomDouble * maxK) + 1;
            var arr = testData.Arr;
            var op = testData.Op;
            var ans1 = TopK(arr, op, k);
            var ans2 = Compare(arr, op, k);
            if (!SameAnswer(ans1, ans2))
            {
                for (var j = 0; j < arr.Length; j++) Console.WriteLine(arr[j] + " , " + op[j]);

                Console.WriteLine(k);
                Console.WriteLine("答案1：");
                foreach (var item in ans1)
                {
                    Console.Write("[");
                    foreach (var element in item) Console.Write(element + ",");
                    Console.Write("],");
                    Console.WriteLine();
                }

                Console.WriteLine("答案2：");
                foreach (var item in ans2)
                {
                    Console.Write("[");
                    foreach (var element in item) Console.Write(element + ",");
                    Console.Write("],");
                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine("出错了！当前测试次数：{0}", i);
                break;
            }
        }

        Console.WriteLine($"测试结束，总耗时:{Utility.GetStopwatchElapsedMilliseconds()}ms");
    }

    private class Customer
    {
        public readonly int Id;
        public int Buy;
        public int EnterTime;

        public Customer(int v, int b, int o = 0)
        {
            Id = v;
            Buy = b;
            EnterTime = o;
        }
    }

    private class WhoIsAwardee
    {
        private readonly HeapGreater<Customer> _awardeeHeap;
        private readonly int _awardeeLimit;
        private readonly HeapGreater<Customer> _candidateHeap;
        private readonly Dictionary<int, Customer> _customers;

        public WhoIsAwardee(int limit)
        {
            _customers = new Dictionary<int, Customer>();
            _candidateHeap =
                new HeapGreater<Customer>((x, y) => x.Buy != y.Buy ? y.Buy - x.Buy : x.EnterTime - y.EnterTime);
            _awardeeHeap =
                new HeapGreater<Customer>((x, y) => x.Buy != y.Buy ? x.Buy - y.Buy : x.EnterTime - y.EnterTime);
            _awardeeLimit = limit;
        }

        // 当前处理i号事件，arr[i] -> id,  buyOrRefund
        public void Operate(int time, int id, bool buyOrRefund)
        {
            if (!buyOrRefund && !_customers.ContainsKey(id)) return;

            if (!_customers.ContainsKey(id))
                _customers[id] = new Customer(id, 0);

            var c = _customers[id];
            if (buyOrRefund)
                c.Buy++;
            else
                c.Buy--;

            if (c.Buy == 0) _customers.Remove(id);

            if (!_candidateHeap.Contains(c) && !_awardeeHeap.Contains(c))
            {
                if (_awardeeHeap.Size() < _awardeeLimit)
                {
                    c.EnterTime = time;
                    _awardeeHeap.Push(c);
                }
                else
                {
                    c.EnterTime = time;
                    _candidateHeap.Push(c);
                }
            }
            else if (_candidateHeap.Contains(c))
            {
                if (c.Buy == 0)
                    _candidateHeap.Remove(c);
                else
                    _candidateHeap.Resign(c);
            }
            else
            {
                if (c.Buy == 0)
                    _awardeeHeap.Remove(c);
                else
                    _awardeeHeap.Resign(c);
            }

            AwardeeMove(time);
        }

        public List<int> GetAwardees()
        {
            var customers = _awardeeHeap.GetAllElements();
            List<int> ans = new();
            foreach (var c in customers) ans.Add(c.Id);

            return ans;
        }

        private void AwardeeMove(int time)
        {
            if (_candidateHeap.IsEmpty()) return;

            if (_awardeeHeap.Size() < _awardeeLimit)
            {
                var p = _candidateHeap.Pop();
                p.EnterTime = time;
                _awardeeHeap.Push(p);
            }
            else
            {
                if (_candidateHeap.Peek().Buy > _awardeeHeap.Peek().Buy)
                {
                    var oldAwardee = _awardeeHeap.Pop();
                    var newAwardee = _candidateHeap.Pop();
                    oldAwardee.EnterTime = time;
                    newAwardee.EnterTime = time;
                    _awardeeHeap.Push(newAwardee);
                    _candidateHeap.Push(oldAwardee);
                }
            }
        }
    }

    // 为了测试
    private class Data
    {
        public readonly int[] Arr;
        public readonly bool[] Op;

        public Data(int[] a, bool[] o)
        {
            Arr = a;
            Op = o;
        }
    }
}