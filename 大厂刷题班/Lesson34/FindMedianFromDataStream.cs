#region

using Common.DataStructures.Heap;

#endregion

namespace AdvancedTraining.Lesson34;

//https://leetcode.cn/problems/find-median-from-data-stream/description/
//pass
public class FindMedianFromDataStream //leetcode_0295
{
    public static void Run()
    {
        var medianFinder = new MedianFinder();
        medianFinder.AddNum(1); // arr = [1]
        medianFinder.AddNum(2); // arr = [1, 2]
        medianFinder.FindMedian(); // 返回 1.5 ((1 + 2) / 2)
        medianFinder.AddNum(3); // arr[1, 2, 3]
        medianFinder.FindMedian(); // return 2.0
    }

    private class MedianFinder
    {
        private readonly Heap<int> _maxHeap;
        private readonly Heap<int> _minHeap;

        public MedianFinder()
        {
            _maxHeap = new Heap<int>((a, b) => b - a);
            _minHeap = new Heap<int>((a, b) => a - b);
        }

        public void AddNum(int num)
        {
            if (_maxHeap.isEmpty || _maxHeap.Peek() >= num)
                _maxHeap.Push(num);
            else
                _minHeap.Push(num);
            Balance();
        }

        public double FindMedian()
        {
            if (_maxHeap.count == _minHeap.count)
                return (double)(_maxHeap.Peek() + _minHeap.Peek()) / 2;
            return _maxHeap.count > _minHeap.count ? _maxHeap.Peek() : _minHeap.Peek();
        }

        protected void Balance()
        {
            if (Math.Abs(_maxHeap.count - _minHeap.count) == 2)
            {
                if (_maxHeap.count > _minHeap.count)
                    _minHeap.Push(_maxHeap.Pop());
                else
                    _maxHeap.Push(_minHeap.Pop());
            }
        }
    }
}