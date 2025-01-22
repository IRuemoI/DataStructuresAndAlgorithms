#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson34;

//pass

public class RandomizedSet //leetcode_0380
{
    private readonly Dictionary<int, int> _indexKeyMap = new();
    private readonly Dictionary<int, int> _keyIndexMap = new();
    private int _size;
    private readonly Random _random = new();

    public int GetRandom()
    {
        if (_size == 0) return -1;
        var randomIndex = (int)(_random.NextDouble() * _size);
        return _indexKeyMap[randomIndex];
    }

    public bool Insert(int val)
    {
        if (_keyIndexMap.TryAdd(val, _size))
        {
            _indexKeyMap[_size++] = val;
            return true;
        }

        return false;
    }

    public bool Remove(int val)
    {
        if (_keyIndexMap.ContainsKey(val))
        {
            var deleteIndex = _keyIndexMap[val];
            var lastIndex = --_size;
            var lastKey = _indexKeyMap[lastIndex];
            _keyIndexMap[lastKey] = deleteIndex;
            _indexKeyMap[deleteIndex] = lastKey;
            _keyIndexMap.Remove(val);
            _indexKeyMap.Remove(lastIndex);
            return true;
        }

        return false;
    }
}