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
        var result = new List<List<int>>();
        var whoIsAwardees = new WhoIsAwardee(k);
        //对于每一条顾客的操作
        for (var i = 0; i < arr.Length; i++)
        {
            //执行操作
            whoIsAwardees.Operate(i, arr[i], op[i]);
            //将当前获奖区的内容添加到历史中
            result.Add(whoIsAwardees.GetAwardees());
        }

        return result;
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
            var k = (int)(Utility.getRandomDouble * maxK) + 1;
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

    private class Customer(int v, int b, int o = 0)
    {
        public readonly int Id = v;
        public int Buy = b;
        public int EnterTime = o;
    }

    private class WhoIsAwardee
    {
        private readonly HeapGreater<Customer> _awardeeHeap; //获奖区
        private readonly int _awardeeLimit; //获奖区人数上限
        private readonly HeapGreater<Customer> _candidateHeap; //候选区
        private readonly Dictionary<int, Customer> _customerDict; //定义一个顾客编号和实例的字典

        public WhoIsAwardee(int limit)
        {
            _customerDict = new Dictionary<int, Customer>();
            //候选区根据购买数降序、操作时间升序排序
            _candidateHeap =
                new HeapGreater<Customer>((x, y) => x.Buy != y.Buy ? y.Buy - x.Buy : x.EnterTime - y.EnterTime);
            //获奖区根据购买数升序、操作时间升序排序(以此可以快速判断获奖区的顾客是否被候选区的顾客取代)
            _awardeeHeap =
                new HeapGreater<Customer>((x, y) => x.Buy != y.Buy ? x.Buy - y.Buy : x.EnterTime - y.EnterTime);
            _awardeeLimit = limit;
        }

        // 处理当前时间的操作
        public void Operate(int time, int id, bool buyOrRefund)
        {
            //如果当前操作是退货且顾客字典中并不包含这位顾客，直接退出
            if (!buyOrRefund && !_customerDict.ContainsKey(id)) return;

            //直行道这里就意味着这是一个新来的顾客，把他添加到顾客字典中
            if (!_customerDict.ContainsKey(id))
                _customerDict[id] = new Customer(id, 0);

            //在这里处理顾客的买、卖两种操作
            var customer = _customerDict[id]; //取出这条操作所属的顾客
            if (buyOrRefund)
                customer.Buy++; //如果是购买操作，购买数增加
            else
                customer.Buy--; //如果是退货操作，购买数减少

            //如果当前顾客的购买数为0,将这名顾客从字典中移除
            if (customer.Buy == 0) _customerDict.Remove(id);

            //如果这个顾客是第一次购买(并不在两个区中)
            if (!_candidateHeap.Contains(customer) && !_awardeeHeap.Contains(customer))
            {
                if (_awardeeHeap.count < _awardeeLimit) //如果测试获奖区还没满
                {
                    customer.EnterTime = time; //设置顾客的时间为本次处理的时间
                    _awardeeHeap.Push(customer); //将他放到获奖区中
                }
                else
                {
                    customer.EnterTime = time; //设置顾客的时间为本次处理的时间
                    _candidateHeap.Push(customer); //将他放到候选区中
                }
            }
            else if (_candidateHeap.Contains(customer)) //如果这名顾客在候选区中
            {
                if (customer.Buy == 0) //且这名顾客的购买数为零
                    _candidateHeap.Remove(customer); //将这名顾客从候选区中移除
                else
                    _candidateHeap.Resign(customer); //否则，更新这名顾客在堆中位置
            }
            else //如果这名顾客在获奖区中
            {
                if (customer.Buy == 0) //且这名顾客的购买数为零
                    _awardeeHeap.Remove(customer); //将这名顾客从获奖区中移除
                else
                    _awardeeHeap.Resign(customer); //否则，更新这名顾客在堆中位置
            }

            AwardeeMove(time); //执行获奖区和候选区间的顾客移动
        }

        public List<int> GetAwardees()
        {
            var customers = _awardeeHeap.GetAllElements();
            List<int> result = new();
            foreach (var customer in customers) result.Add(customer.Id);

            return result;
        }

        private void AwardeeMove(int time)
        {
            if (_candidateHeap.isEmpty) return; //如果候选区为空，直接退出

            if (_awardeeHeap.count < _awardeeLimit) //如果获奖区还没满
            {
                var customer = _candidateHeap.Pop(); //取出候选区中购买数最多的顾客
                customer.EnterTime = time; //设置顾客的时间为本次处理的时间
                _awardeeHeap.Push(customer); //将他放到获奖区中
            }
            else //如果获奖区已满
            {
                //如果候选区中购买数最多的顾客购买数大于获奖区中购买数最少的顾客购买数
                if (_candidateHeap.Peek().Buy > _awardeeHeap.Peek().Buy)
                {
                    var oldAwardee = _awardeeHeap.Pop(); //获取获奖区购买数最少的顾客作为被移出的获奖者
                    var newAwardee = _candidateHeap.Pop(); //获取候选区购买数最多的顾客作为新的获奖者
                    oldAwardee.EnterTime = time; //更新新添加的获奖者的时间
                    newAwardee.EnterTime = time; //更新被移除的获奖者的时间
                    _awardeeHeap.Push(newAwardee); //把新的获奖者添加到获奖区
                    _candidateHeap.Push(oldAwardee); //把被移除的获奖者添加到候选区
                }
            }
        }
    }

    #region 用于测试

    private static List<List<int>> Compare(int[] arr, bool[] op, int k)
    {
        Dictionary<int, Customer> customerDict = new(); //定义一个顾客编号和实例的字典
        List<Customer> candidate = new(); //候选区
        List<Customer> awardee = new(); //获奖区
        List<List<int>> historyList = new(); //用于返回获奖区中的历史内容
        //对于每一条顾客的操作
        for (var i = 0; i < arr.Length; i++)
        {
            var id = arr[i]; //获取本条操作的顾客ID
            var buyOrRefund = op[i]; //获取操作中的内容
            //如果当前操作是退货且顾客字典中并不包含这位顾客
            if (!buyOrRefund && !customerDict.ContainsKey(id))
            {
                historyList.Add(GetCurrentAwardee(awardee)); //将当前获奖区的内容添加到历史中
                continue; //跳过后续的步骤
            }

            /*
            执行到这里之后可能出现的情况：
            用户之前购买数是0,此时买货事件
            用户之前购买数>0,此时买货
            用户之前购买数>0,此时退货
            */

            //这是一个新来的顾客，把他添加到顾客字典中
            if (!customerDict.ContainsKey(id)) customerDict.Add(id, new Customer(id, 0));

            //在这里处理顾客的买、卖两种操作
            var customer = customerDict[id]; //取出这条操作所属的顾客
            if (buyOrRefund)
                customer.Buy++; //如果是购买操作，购买数增加
            else
                customer.Buy--; //如果是退货操作，购买数减少

            //如果当前顾客的购买数为0
            if (customer.Buy == 0)
                customerDict.Remove(id); //将这名顾客从字典中移除

            //如果这个顾客是第一次购买(并不在两个区中)
            if (!candidate.Contains(customer) && !awardee.Contains(customer))
            {
                if (awardee.Count < k) //如果测试获奖区还没满
                {
                    customer.EnterTime = i; //设置顾客的时间为本次处理的序号
                    awardee.Add(customer); //将他放到获奖区中
                }
                else
                {
                    customer.EnterTime = i; //设置顾客的时间为本次处理的序号
                    candidate.Add(customer); //将他放到候选区中
                }
            }

            CleanZeroBuy(awardee); //清理中奖区中购买数为0的顾客
            CleanZeroBuy(candidate); //清理候选区中购买数为0的顾客
            //候选区根据购买数降序、操作时间升序排序
            candidate.Sort((x, y) => x.Buy != y.Buy ? y.Buy - x.Buy : x.EnterTime - y.EnterTime);
            //获奖区根据购买数升序、操作时间升序排序(以此可以快速判断获奖区的顾客是否被候选区的顾客取代)
            awardee.Sort((x, y) => x.Buy != y.Buy ? x.Buy - y.Buy : x.EnterTime - y.EnterTime);
            Move(awardee, candidate, k, i);
            historyList.Add(GetCurrentAwardee(awardee));
        }

        return historyList;
    }

    /// <summary>
    ///     将候选区的顾客移动到获奖区中
    /// </summary>
    /// <param name="awardee">获奖区</param>
    /// <param name="candidate">候选区</param>
    /// <param name="k">获奖区大小</param>
    /// <param name="time">执行移动时的时间</param>
    private static void Move(List<Customer> awardee, List<Customer> candidate, int k, int time)
    {
        if (candidate.Count == 0) return; //如果候选区为空直接返回

        if (awardee.Count < k) //候选区不为空且获奖区没满
        {
            var customer = candidate[0]; //获取候选区购买数最多且最早的顾客
            customer.EnterTime = time; //更新这位顾客进入获奖区的时间
            awardee.Add(customer); //把这个顾客放入到获奖区中
            candidate.RemoveAt(0); //把这个顾客从候选区移除
        }
        else // 获奖区满了，候选区有东西
        {
            if (candidate[0].Buy > awardee[0].Buy) //如果候选区的顾客可以替换获奖区的顾客
            {
                var oldAwardee = awardee[0]; //获取获奖区第一个也就是获奖区购买数最少的顾客作为被移出的获奖者
                awardee.RemoveAt(0); //把他从获奖区移除
                var newAwardee = candidate[0]; //获取候选区第一个也就是候选区购买数最多的顾客作为新的获奖者
                candidate.RemoveAt(0); //把他从候选区移除
                newAwardee.EnterTime = time; //更新新添加的获奖者的时间
                oldAwardee.EnterTime = time; //更新被移除的获奖者的时间
                awardee.Add(newAwardee); //把新的获奖者添加到获奖区
                candidate.Add(oldAwardee); //把被移除的获奖者添加到候选区
            }
        }
    }


    /// <summary>
    ///     删除列表中购买数为零的顾客
    /// </summary>
    /// <param name="customerList">顾客列表</param>
    private static void CleanZeroBuy(List<Customer> customerList)
    {
        var noZero = new List<Customer>(); //定义一个非零购买数的顾客列表
        //将本列表所有非零购买数的顾客放入其中
        foreach (var customer in customerList)
            if (customer.Buy != 0)
                noZero.Add(customer);

        customerList.Clear(); //将原来的顾客列表清空
        foreach (var customer in noZero) customerList.Add(customer); //把这些购买数非零的顾客放入原来的列表中
    }

    private static List<int> GetCurrentAwardee(List<Customer> awardee)
    {
        List<int> result = new(); //定义一个返回获奖区顾客的列表
        foreach (var customer in awardee) result.Add(customer.Id); //把获奖区的顾客放入其中

        return result; //返回这个列表
    }

    // 为了测试
    private class Data(int[] a, bool[] o)
    {
        public readonly int[] Arr = a;
        public readonly bool[] Op = o;
    }

    // 为了测试
    private static Data RandomData(int maxValue, int maxLen)
    {
        var len = (int)(Utility.getRandomDouble * maxLen) + 1;
        var arr = new int[len];
        var op = new bool[len];
        for (var i = 0; i < len; i++)
        {
            arr[i] = (int)(Utility.getRandomDouble * maxValue);
            op[i] = Utility.getRandomDouble < 0.5;
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

    #endregion
}