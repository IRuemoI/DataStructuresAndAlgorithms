namespace AdvancedTraining.Lesson02;

public class SetAll
{
    public class MyValue<TV>(TV v, long t)
    {
        public readonly long Time = t;
        public readonly TV Value = v;
    }

    public class MyHashMap<TK, TV> where TK : notnull
    {
        private readonly Dictionary<TK, MyValue<TV>> _map = new();
        private MyValue<TV> _setAll = new(default, -1);
        private long _time;

        public void Put(TK key, TV value)
        {
            _map[key] = new MyValue<TV>(value, _time++);
        }

        public void SetAll(TV value)
        {
            _setAll = new MyValue<TV>(value, _time++);
        }

        public TV Get(TK key)
        {
            if (!_map.ContainsKey(key)) return default;
            if (_map[key].Time > _setAll.Time)
                return _map[key].Value;
            return _setAll.Value;
        }
    }
}