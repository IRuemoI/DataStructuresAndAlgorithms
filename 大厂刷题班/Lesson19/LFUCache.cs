namespace AdvancedTraining.Lesson19;

//todo:待整理
// 本题测试链接 : https://leetcode.cn/problems/lfu-cache/
// 提交时把类名和构造方法名改为 : LFUCache //最近最不常用规则缓存
public class LfuCache(int k)
{
    private readonly Dictionary<Node, NodeList> _heads = new(); // 表示节点(Node)在哪个桶(NodeList)里
    private readonly Dictionary<int, Node> _records = new(); // 表示key(Integer)由哪个节点(Node)代表

    private NodeList? _headList; // 整个结构中位于最左的桶

    // 缓存的大小限制，即K
    private int _size; // 缓存目前有多少个节点

    // removeNodeList：刚刚减少了一个节点的桶
    // 这个函数的功能是，判断刚刚减少了一个节点的桶是不是已经空了。
    // 1）如果不空，什么也不做
    //
    // 2)如果空了，removeNodeList还是整个缓存结构最左的桶(headList)。
    // 删掉这个桶的同时也要让最左的桶变成removeNodeList的下一个。
    //
    // 3)如果空了，removeNodeList不是整个缓存结构最左的桶(headList)。
    // 把这个桶删除，并保证上一个的桶和下一个桶之间还是双向链表的连接方式
    //
    // 函数的返回值表示刚刚减少了一个节点的桶是不是已经空了，空了返回true；不空返回false
    private bool ModifyHeadList(NodeList removeNodeList)
    {
        if (removeNodeList.Empty)
        {
            if (_headList == removeNodeList)
            {
                _headList = removeNodeList.next;
                if (_headList != null) _headList.last = null;
            }
            else
            {
                removeNodeList.last.next = removeNodeList.next;
                if (removeNodeList.next != null) removeNodeList.next.last = removeNodeList.last;
            }

            return true;
        }

        return false;
    }

    // 函数的功能
    // node这个节点的次数+1了，这个节点原来在oldNodeList里。
    // 把node从oldNodeList删掉，然后放到次数+1的桶中
    // 整个过程既要保证桶之间仍然是双向链表，也要保证节点之间仍然是双向链表
    private void Move(Node node, NodeList oldNodeList)
    {
        oldNodeList.DeleteNode(node);
        // preList表示次数+1的桶的前一个桶是谁
        // 如果oldNodeList删掉node之后还有节点，oldNodeList就是次数+1的桶的前一个桶
        // 如果oldNodeList删掉node之后空了，oldNodeList是需要删除的，所以次数+1的桶的前一个桶，是oldNodeList的前一个
        var preList = ModifyHeadList(oldNodeList) ? oldNodeList.last : oldNodeList;
        // nextList表示次数+1的桶的后一个桶是谁
        var nextList = oldNodeList.next;
        if (nextList == null)
        {
            var newList = new NodeList(node);
            if (preList != null) preList.next = newList;

            newList.last = preList;
            if (_headList == null) _headList = newList;

            _heads[node] = newList;
        }
        else
        {
            if (nextList.head.Times.Equals(node.Times))
            {
                nextList.AddNodeFromHead(node);
                _heads[node] = nextList;
            }
            else
            {
                var newList = new NodeList(node);
                if (preList != null) preList.next = newList;

                newList.last = preList;
                newList.next = nextList;
                nextList.last = newList;
                if (_headList == nextList) _headList = newList;

                _heads[node] = newList;
            }
        }
    }

    public virtual void Put(int key, int value)
    {
        if (k == 0) return;

        if (_records.ContainsKey(key))
        {
            var node = _records[key];
            node.Value = value;
            node.Times++;
            var curNodeList = _heads[node];
            Move(node, curNodeList);
        }
        else
        {
            if (_size == k)
            {
                var node1 = _headList.tail;
                _headList.DeleteNode(node1);
                ModifyHeadList(_headList);
                _records.Remove(node1.Key);
                _heads.Remove(node1);
                _size--;
            }

            var node = new Node(key, value, 1);
            if (_headList == null)
            {
                _headList = new NodeList(node);
            }
            else
            {
                if (_headList.head.Times.Equals(node.Times))
                {
                    _headList.AddNodeFromHead(node);
                }
                else
                {
                    var newList = new NodeList(node);
                    newList.next = _headList;
                    _headList.last = newList;
                    _headList = newList;
                }
            }

            _records[key] = node;
            _heads[node] = _headList;
            _size++;
        }
    }

    public virtual int Get(int key)
    {
        if (!_records.ContainsKey(key)) return -1;

        var node = _records[key];
        node.Times++;
        var curNodeList = _heads[node];
        Move(node, curNodeList);
        return node.Value.Value;
    }

    // 节点的数据结构
    public class Node(int k, int v, int t)
    {
        public readonly int Key = k;
        public Node? Down; // 节点之间是双向链表所以有下一个节点
        public int Times = t; // 这个节点发生get或者set的次数总和
        public Node? Up; // 节点之间是双向链表所以有上一个节点
        public int? Value = v;
    }

    // 桶结构
    public class NodeList(Node node)
    {
        public Node head = node; // 桶的头节点
        public NodeList last; // 桶之间是双向链表所以有前一个桶
        public NodeList next; // 桶之间是双向链表所以有后一个桶
        public Node tail = node; // 桶的尾节点

        // 判断这个桶是不是空的
        public virtual bool Empty => head == null;

        // 把一个新的节点加入这个桶，新的节点都放在顶端变成新的头部
        public virtual void AddNodeFromHead(Node newHead)
        {
            newHead.Down = head;
            head.Up = newHead;
            head = newHead;
        }

        // 删除node节点并保证node的上下环境重新连接
        public virtual void DeleteNode(Node node)
        {
            if (head == tail)
            {
                head = null;
                tail = null;
            }
            else
            {
                if (node == head)
                {
                    head = node.Down;
                    head.Up = null;
                }
                else if (node == tail)
                {
                    tail = node.Up;
                    tail.Down = null;
                }
                else
                {
                    node.Up.Down = node.Down;
                    node.Down.Up = node.Up;
                }
            }

            node.Up = null;
            node.Down = null;
        }
    }
}