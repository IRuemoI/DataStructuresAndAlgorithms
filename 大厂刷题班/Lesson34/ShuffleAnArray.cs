#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson34;

//https://leetcode.cn/problems/shuffle-an-array/description/
//pass
public class ShuffleAnArray //leetcode_0384
{
    public static void Run()
    {
        var solution = new Solution([1, 2, 3]);
        solution.Shuffle(); // 打乱数组 [1,2,3] 并返回结果。任何 [1,2,3]的排列返回的概率应该相同。例如，返回 [3, 1, 2]
        solution.Reset(); // 重设数组到它的初始状态 [1, 2, 3] 。返回 [1, 2, 3]
        solution.Shuffle(); // 随机返回数组 [1, 2, 3] 打乱后的结果。例如，返回 [1, 3, 2]
    }

    private class Solution
    {
        private readonly int _n;

        private readonly int[] _origin;

        private readonly int[] _shuffleArray;

        public Solution(int[] numbers)
        {
            _origin = numbers;
            _n = numbers.Length;
            _shuffleArray = new int[_n];
            for (var i = 0; i < _n; i++) _shuffleArray[i] = _origin[i];
        }

        public int[] Reset()
        {
            return _origin;
        }

        public int[] Shuffle()
        {
            for (var i = _n - 1; i >= 0; i--)
            {
                var r = (int)(Utility.getRandomDouble * (i + 1));
                (_shuffleArray[r], _shuffleArray[i]) = (_shuffleArray[i], _shuffleArray[r]);
            }

            return _shuffleArray;
        }
    }
}