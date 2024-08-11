namespace AdvancedTraining.Lesson19;

//todo:待整理
// 本题测试链接 : https://leetcode.cn/problems/lru-cache/
// 提交时把类名和构造方法名改成 : LRUCache
public class LRUCache
{
    private readonly MyCache<int, int> cache;

    public LRUCache(int capacity)
    {
        cache = new MyCache<int, int>(capacity);
    }

    public virtual int Get(int key)
    {
        int? ans = cache.Get(key);
        return ans == null ? -1 : ans.Value;
    }

    public virtual void Put(int key, int value)
    {
        cache.Set(key, value);
    }

    public class Node<K, V>
    {
        public K key;
        public Node<K, V> last;
        public Node<K, V> next;
        public V value;

        public Node(K key, V value)
        {
            this.key = key;
            this.value = value;
        }
    }

    public class NodeDoubleLinkedList<K, V>
    {
        internal Node<K, V> head;
        internal Node<K, V> tail;

        public NodeDoubleLinkedList()
        {
            head = null;
            tail = null;
        }

        // 现在来了一个新的node，请挂到尾巴上去
        public virtual void AddNode(Node<K, V> newNode)
        {
            if (newNode == null) return;
            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.next = newNode;
                newNode.last = tail;
                tail = newNode;
            }
        }

        // node 入参，一定保证！node在双向链表里！
        // node原始的位置，左右重新连好，然后把node分离出来
        // 挂到整个链表的尾巴上
        public virtual void MoveNodeToTail(Node<K, V> node)
        {
            if (tail == node) return;
            if (head == node)
            {
                head = node.next;
                head.last = null;
            }
            else
            {
                node.last.next = node.next;
                node.next.last = node.last;
            }

            node.last = tail;
            node.next = null;
            tail.next = node;
            tail = node;
        }

        public virtual Node<K, V> removeHead()
        {
            if (head == null) return null;
            var res = head;
            if (head == tail)
            {
                head = null;
                tail = null;
            }
            else
            {
                head = res.next;
                res.next = null;
                head.last = null;
            }

            return res;
        }
    }

    public class MyCache<K, V>
    {
        internal readonly int capacity;
        internal Dictionary<K, Node<K, V>> keyNodeMap;
        internal NodeDoubleLinkedList<K, V> nodeList;

        public MyCache(int cap)
        {
            keyNodeMap = new Dictionary<K, Node<K, V>>();
            nodeList = new NodeDoubleLinkedList<K, V>();
            capacity = cap;
        }

        public virtual V Get(K key)
        {
            if (keyNodeMap.ContainsKey(key))
            {
                var res = keyNodeMap[key];
                nodeList.MoveNodeToTail(res);
                return res.value;
            }

            return default;
        }

        // Set(Key, Value)
        // 新增  更新value的操作
        public virtual void Set(K key, V value)
        {
            if (keyNodeMap.ContainsKey(key))
            {
                var node = keyNodeMap[key];
                node.value = value;
                nodeList.MoveNodeToTail(node);
            }
            else
            {
                // 新增！
                var newNode = new Node<K, V>(key, value);
                keyNodeMap[key] = newNode;
                nodeList.AddNode(newNode);
                if (keyNodeMap.Count == capacity + 1) RemoveMostUnusedCache();
            }
        }

        internal virtual void RemoveMostUnusedCache()
        {
            var removeNode = nodeList.removeHead();
            keyNodeMap.Remove(removeNode.key);
        }
    }
}