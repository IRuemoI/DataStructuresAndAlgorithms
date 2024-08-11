#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson34;

//todo:待整理
public class InsertDeleteGetRandom //Problem_0380
{
    public class RandomizedSet
    {
        private readonly InsertDeleteGetRandom outerInstance;
        internal Dictionary<int, int> indexKeyMap;


        internal Dictionary<int, int> keyIndexMap;
        internal int size;

        public RandomizedSet(InsertDeleteGetRandom outerInstance)
        {
            this.outerInstance = outerInstance;
            keyIndexMap = new Dictionary<int, int>();
            indexKeyMap = new Dictionary<int, int>();
            size = 0;
        }

        public virtual int Random
        {
            get
            {
                if (size == 0) return -1;
                var randomIndex = (int)(Utility.GetRandomDouble * size);
                return indexKeyMap[randomIndex];
            }
        }

        public virtual bool Insert(int val)
        {
            if (!keyIndexMap.ContainsKey(val))
            {
                keyIndexMap[val] = size;
                indexKeyMap[size++] = val;
                return true;
            }

            return false;
        }

        public virtual bool Remove(int val)
        {
            if (keyIndexMap.ContainsKey(val))
            {
                var deleteIndex = keyIndexMap[val];
                var lastIndex = --size;
                var lastKey = indexKeyMap[lastIndex];
                keyIndexMap[lastKey] = deleteIndex;
                indexKeyMap[deleteIndex] = lastKey;
                keyIndexMap.Remove(val);
                indexKeyMap.Remove(lastIndex);
                return true;
            }

            return false;
        }
    }
}