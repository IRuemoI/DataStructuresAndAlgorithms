//测试通过

#region

using Common.DataStructures.Heap;
using Common.Utilities;

#endregion

namespace Algorithms.Lesson06;

public class Heap
{
    public static void Run()
    {
        Console.WriteLine("测试大根堆");
        var value = 1000;
        var limit = 100;
        var testTimes = 100000;

        for (var i = 0; i < testTimes; i++)
        {
            var curLimit = (int)(Utility.GetRandomDouble * limit) + 1;
            //var my = new MyMaxHeap(curLimit);
            var my = new Heap<int>((x, y) => y.CompareTo(x), curLimit);
            var test = new RightMaxHeap(curLimit);
            var curOpTimes = (int)(Utility.GetRandomDouble * limit);
            for (var j = 0; j < curOpTimes; j++)
            {
                if (my.IsEmpty != test.IsEmpty())
                    Console.WriteLine("出错啦！");

                if (my.IsFull != test.IsFull())
                    Console.WriteLine("出错啦！");

                if (my.IsEmpty)
                {
                    var curValue = (int)(Utility.GetRandomDouble * value);
                    my.Push(curValue);
                    test.Push(curValue);
                }
                else if (my.IsFull)
                {
                    if (my.Pop() != test.Pop())
                        Console.WriteLine("出错啦！");
                }
                else
                {
                    if (Utility.GetRandomDouble < 0.5)
                    {
                        var curValue = (int)(Utility.GetRandomDouble * value);
                        my.Push(curValue);
                        test.Push(curValue);
                    }
                    else
                    {
                        if (my.Pop() != test.Pop())
                            Console.WriteLine("出错啦！");
                    }
                }
            }
        }

        Console.WriteLine("测试完成");
    }

    private class MyMaxHeap
    {
        private readonly int[] _heap;
        private readonly int _limit;
        private int _heapSize;
    
        public MyMaxHeap(int limit)
        {
            _heap = new int[limit];
            _limit = limit;
            _heapSize = 0;
        }
    
        public bool IsEmpty()
        {
            return _heapSize == 0;
        }
    
        public bool IsFull()
        {
            return _heapSize == _limit;
        }
    
        public void Push(int value)
        {
            if (_heapSize == _limit) throw new Exception("heap is full");
    
            _heap[_heapSize] = value;
            // value  heapSize
            HeapInsert(_heap, _heapSize++);
        }
    
        // 用户此时，让你返回最大值，并且在大根堆中，把最大值删掉
        // 剩下的数，依然保持大根堆组织
        public int Pop()
        {
            var ans = _heap[0];
            Swap(_heap, 0, --_heapSize);
            Heapify(_heap, 0, _heapSize);
            return ans;
        }
    
    
        // 新加进来的数，现在停在了index位置，请依次往上移动，
        // 移动到0位置，或者干不掉自己的父亲了，停！
        private void HeapInsert(int[] arr, int index)
        {
            while (arr[index] > arr[(index - 1) / 2])
            {
                Swap(arr, index, (index - 1) / 2);
                index = (index - 1) / 2;
            }
        }
    
        // 从index位置，往下看，不断的下沉
        // 停：较大的孩子都不再比index位置的数大；已经没孩子了
        private void Heapify(int[] arr, int index, int heapSize)
        {
            var left = index * 2 + 1;
            while (left < heapSize)
            {
                // 如果有左孩子，有没有右孩子，可能有可能没有！
                // 把较大孩子的下标，给largest
                var largest = left + 1 < heapSize && arr[left + 1] > arr[left] ? left + 1 : left;
                largest = arr[largest] > arr[index] ? largest : index;
                if (largest == index) break;
    
                // index和较大孩子，要互换
                Swap(arr, largest, index);
                index = largest;
                left = index * 2 + 1;
            }
        }
    
        private static void Swap(int[] arr, int i, int j)
        {
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }
    
        public int Peek()
        {
            return _heap[0];
        }
    }
    
    // private class MyMinHeap
    // {
    //     private readonly int[] _heap;
    //     private readonly int _limit;
    //     private int _heapSize;
    //
    //     public MyMinHeap(int limit)
    //     {
    //         _heap = new int[limit];
    //         _limit = limit;
    //         _heapSize = 0;
    //     }
    //
    //     public bool IsEmpty()
    //     {
    //         return _heapSize == 0;
    //     }
    //
    //     public bool IsFull()
    //     {
    //         return _heapSize == _limit;
    //     }
    //
    //     public void Push(int value)
    //     {
    //         if (_heapSize == _limit) throw new Exception("heap is full");
    //
    //         _heap[_heapSize] = value;
    //         HeapInsert(_heap, _heapSize++);
    //     }
    //
    //     // 用户此时，让你返回最小值，并且在小根堆中，把最小值删掉
    //     // 剩下的数，依然保持小根堆组织
    //     public int Pop()
    //     {
    //         var ans = _heap[0];
    //         Swap(_heap, 0, --_heapSize);
    //         Heapify(_heap, 0, _heapSize);
    //         return ans;
    //     }
    //
    //     // 新加进来的数，现在停在了index位置，请依次往上移动，
    //     // 移动到0位置，或者干不掉自己的父亲了，停！
    //     private void HeapInsert(int[] arr, int index)
    //     {
    //         while (arr[index] < arr[(index - 1) / 2])
    //         {
    //             Swap(arr, index, (index - 1) / 2);
    //             index = (index - 1) / 2;
    //         }
    //     }
    //
    //     // 从index位置，往下看，不断的下沉
    //     // 停：较小的孩子都不再比index位置的数小；已经没孩子了
    //     private void Heapify(int[] arr, int index, int heapSize)
    //     {
    //         var left = index * 2 + 1;
    //         while (left < heapSize)
    //         {
    //             // 如果有左孩子，有没有右孩子，可能有可能没有！
    //             // 把较小孩子的下标，给smallest
    //             var smallest = left + 1 < heapSize && arr[left + 1] < arr[left] ? left + 1 : left;
    //             smallest = arr[smallest] < arr[index] ? smallest : index;
    //             if (smallest == index) break;
    //
    //             // index和较小孩子，要互换
    //             Swap(arr, smallest, index);
    //             index = smallest;
    //             left = index * 2 + 1;
    //         }
    //     }
    //
    //     private static void Swap(int[] arr, int i, int j)
    //     {
    //         (arr[i], arr[j]) = (arr[j], arr[i]);
    //     }
    //
    //     public int Peek()
    //     {
    //         return _heap[0];
    //     }
    // }
    
    private class RightMaxHeap
    {
        private readonly int[] _arr;
        private readonly int _limit;
        private int _size;

        public RightMaxHeap(int limit)
        {
            _arr = new int[limit];
            _limit = limit;
            _size = 0;
        }

        public bool IsEmpty()
        {
            return _size == 0;
        }

        public bool IsFull()
        {
            return _size == _limit;
        }

        public void Push(int value)
        {
            if (_size == _limit) throw new Exception("heap is full");

            _arr[_size++] = value;
        }

        public int Pop()
        {
            var maxIndex = 0;
            for (var i = 1; i < _size; i++)
                if (_arr[i] > _arr[maxIndex])
                    maxIndex = i;

            var ans = _arr[maxIndex];
            _arr[maxIndex] = _arr[--_size];
            return ans;
        }
    }
}