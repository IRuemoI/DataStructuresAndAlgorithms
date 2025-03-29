#region

using System.Collections;

#endregion

namespace AdvancedTraining.Lesson34;

//https://leetcode.cn/problems/flatten-nested-list-iterator/description/
//todo:待整理
public class FlattenNestedListIterator //leetcode_0341
{
    // 不要提交这个接口类
    public interface INestedInteger
    {
        // @return true if this NestedInteger holds a single integer, rather than a
        // nested list.
        bool integer1 { get; }

        // @return the single integer that this NestedInteger holds, if it holds a
        // single integer
        // Return null if this NestedInteger holds a nested list
        int? integer2 { get; }

        // @return the nested list that this NestedInteger holds, if it holds a nested
        // list
        // Return null if this NestedInteger holds a single integer
        IList<INestedInteger> list { get; }
    }

    public class NestedIterator : IEnumerator<int>
    {
        private readonly IList<INestedInteger> _list;
        private readonly Stack<int> _stack;
        private readonly FlattenNestedListIterator outerInstance;
        private bool _used;

        public NestedIterator(FlattenNestedListIterator outerInstance, IList<INestedInteger> nestedList)
        {
            this.outerInstance = outerInstance;
            _list = nestedList;
            _stack = new Stack<int>();
            _stack.Push(-1);
            _used = true;
            HasNext();
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public int Current { get; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int? next()
        {
            int? ans = null;
            if (!_used)
            {
                ans = get(_list, _stack);
                _used = true;
                HasNext();
            }

            return ans;
        }

        public bool HasNext()
        {
            if (_stack.Count == 0) return false;
            if (!_used) return true;
            if (FindNext(_list, _stack)) _used = false;
            return !_used;
        }

        internal virtual int? get(IList<INestedInteger> nestedList, Stack<int> stack)
        {
            var index = stack.Pop();
            int? ans = null;
            if (stack.Count > 0)
                ans = get(nestedList[index].list, stack);
            else
                ans = nestedList[index].integer2;
            stack.Push(index);
            return ans;
        }

        internal virtual bool FindNext(IList<INestedInteger> nestedList, Stack<int> stack)
        {
            var index = stack.Pop();
            if (stack.Count > 0 && FindNext(nestedList[index].list, stack))
            {
                stack.Push(index);
                return true;
            }

            for (var i = index + 1; i < nestedList.Count; i++)
                if (PickFirst(nestedList[i], i, stack))
                    return true;
            return false;
        }

        internal virtual bool PickFirst(INestedInteger nested, int position, Stack<int> stack)
        {
            if (nested.integer1)
            {
                stack.Push(position);
                return true;
            }

            var actualList = nested.list;
            for (var i = 0; i < actualList.Count; i++)
                if (PickFirst(actualList[i], i, stack))
                {
                    stack.Push(position);
                    return true;
                }

            return false;
        }
    }
}