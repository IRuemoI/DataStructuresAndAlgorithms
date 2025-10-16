import java.util.ArrayList;

public class Code02_SkipListMap {

	// ����Ľڵ㶨��
	public static class SkipListNode<K extends Comparable<K>, V> {
		public K key;
		public V val;
		public ArrayList<SkipListNode<K, V>> nextNodes;

		public SkipListNode(K k, V v) {
			key = k;
			val = v;
			nextNodes = new ArrayList<SkipListNode<K, V>>();
		}

		// ������ʱ����������ұ�������null(next == null), ��������
		// ͷ(null), ͷ�ڵ��null����Ϊ��С
		// node  -> ͷ��node(null, "")  node.isKeyLess(!null)  true
		// node�����key�Ƿ��otherKeyС��true������false
		public boolean isKeyLess(K otherKey) {
			//  otherKey == null -> false 
			return otherKey != null && (key == null || key.compareTo(otherKey) < 0);
		}

		public boolean isKeyEqual(K otherKey) {
			return (key == null && otherKey == null)
					|| (key != null && otherKey != null && key.compareTo(otherKey) == 0);
		}

	}

	public static class SkipListMap<K extends Comparable<K>, V> {
		private static final double PROBABILITY = 0.5; // < 0.5 ��������>=0.5 ͣ
		private SkipListNode<K, V> head;
		private int size;
		private int maxLevel;

		public SkipListMap() {
			head = new SkipListNode<K, V>(null, null);
			head.nextNodes.add(null); // 0
			size = 0;
			maxLevel = 0;
		}

		// ����߲㿪ʼ��һ·����ȥ��
		// ���գ��ҵ���0���<key�����ҵĽڵ�
		private SkipListNode<K, V> mostRightLessNodeInTree(K key) {
			if (key == null) {
				return null;
			}
			int level = maxLevel;
			SkipListNode<K, V> cur = head;
			while (level >= 0) { // ���ϲ����²�
				//  cur  level  -> level-1
				cur = mostRightLessNodeInLevel(key, cur, level--);
			}
			return cur;
		}

		// ��level�����������ƶ�
		// ���������Ľڵ���cur��������cur��level�㣬��level���ϣ��ҵ�<key���һ���ڵ㲢����
		private SkipListNode<K, V> mostRightLessNodeInLevel(K key, 
				SkipListNode<K, V> cur, 
				int level) {
			SkipListNode<K, V> next = cur.nextNodes.get(level);
			while (next != null && next.isKeyLess(key)) {
				cur = next;
				next = cur.nextNodes.get(level);
			}
			return cur;
		}

		public boolean containsKey(K key) {
			if (key == null) {
				return false;
			}
			SkipListNode<K, V> less = mostRightLessNodeInTree(key);
			SkipListNode<K, V> next = less.nextNodes.get(0);
			return next != null && next.isKeyEqual(key);
		}

		// ��������value
		public void put(K key, V value) {
			if (key == null) {
				return;
			}
			// 0���ϣ�����һ����< key ��Node -> >key
			SkipListNode<K, V> less = mostRightLessNodeInTree(key);
			SkipListNode<K, V> find = less.nextNodes.get(0);
			if (find != null && find.isKeyEqual(key)) {
				find.val = value;
			} else { // find == null   8   7   9
				size++;
				int newNodeLevel = 0;
				while (Math.random() < PROBABILITY) {
					newNodeLevel++;
				}
				// newNodeLevel
				while (newNodeLevel > maxLevel) {
					head.nextNodes.add(null);
					maxLevel++;
				}
				SkipListNode<K, V> newNode = new SkipListNode<K, V>(key, value);
				for (int i = 0; i <= newNodeLevel; i++) {
					newNode.nextNodes.add(null);
				}
				int level = maxLevel;
				SkipListNode<K, V> pre = head;
				while (level >= 0) {
					// level ���У��ҵ����ҵ� < key �Ľڵ�
					pre = mostRightLessNodeInLevel(key, pre, level);
					if (level <= newNodeLevel) {
						newNode.nextNodes.set(level, pre.nextNodes.get(level));
						pre.nextNodes.set(level, newNode);
					}
					level--;
				}
			}
		}

		public V get(K key) {
			if (key == null) {
				return null;
			}
			SkipListNode<K, V> less = mostRightLessNodeInTree(key);
			SkipListNode<K, V> next = less.nextNodes.get(0);
			return next != null && next.isKeyEqual(key) ? next.val : null;
		}

		public void remove(K key) {
			if (containsKey(key)) {
				size--;
				int level = maxLevel;
				SkipListNode<K, V> pre = head;
				while (level >= 0) {
					pre = mostRightLessNodeInLevel(key, pre, level);
					SkipListNode<K, V> next = pre.nextNodes.get(level);
					// 1������һ���У�pre��һ������key
					// 2������һ���У�pre����һ��key��>Ҫɾ��key
					if (next != null && next.isKeyEqual(key)) {
						// free delete node memory -> C++
						// level : pre -> next(key) -> ...
						pre.nextNodes.set(level, next.nextNodes.get(level));
					}
					// ��level��ֻ��һ���ڵ��ˣ�����Ĭ�Ͻڵ�head
					if (level != 0 && pre == head && pre.nextNodes.get(level) == null) {
						head.nextNodes.remove(level);
						maxLevel--;
					}
					level--;
				}
			}
		}

		public K firstKey() {
			return head.nextNodes.get(0) != null ? head.nextNodes.get(0).key : null;
		}

		public K lastKey() {
			int level = maxLevel;
			SkipListNode<K, V> cur = head;
			while (level >= 0) {
				SkipListNode<K, V> next = cur.nextNodes.get(level);
				while (next != null) {
					cur = next;
					next = cur.nextNodes.get(level);
				}
				level--;
			}
			return cur.key;
		}

		public K ceilingKey(K key) {
			if (key == null) {
				return null;
			}
			SkipListNode<K, V> less = mostRightLessNodeInTree(key);
			SkipListNode<K, V> next = less.nextNodes.get(0);
			return next != null ? next.key : null;
		}

		public K floorKey(K key) {
			if (key == null) {
				return null;
			}
			SkipListNode<K, V> less = mostRightLessNodeInTree(key);
			SkipListNode<K, V> next = less.nextNodes.get(0);
			return next != null && next.isKeyEqual(key) ? next.key : less.key;
		}

		public int size() {
			return size;
		}

	}

	// for test
	public static void printAll(SkipListMap<String, String> obj) {
		for (int i = obj.maxLevel; i >= 0; i--) {
			System.out.print("Level " + i + " : ");
			SkipListNode<String, String> cur = obj.head;
			while (cur.nextNodes.get(i) != null) {
				SkipListNode<String, String> next = cur.nextNodes.get(i);
				System.out.print("(" + next.key + " , " + next.val + ") ");
				cur = next;
			}
			System.out.println();
		}
	}

	public static void main(String[] args) {
		SkipListMap<String, String> test = new SkipListMap<>();
		printAll(test);
		System.out.println("======================");
		test.put("A", "10");
		printAll(test);
		System.out.println("======================");
		test.remove("A");
		printAll(test);
		System.out.println("======================");
		test.put("E", "E");
		test.put("B", "B");
		test.put("A", "A");
		test.put("F", "F");
		test.put("C", "C");
		test.put("D", "D");
		printAll(test);
		System.out.println("======================");
		System.out.println(test.containsKey("B"));
		System.out.println(test.containsKey("Z"));
		System.out.println("firstKey:"+test.firstKey());
		System.out.println("lastKey:"+test.lastKey());
		System.out.println(test.floorKey("D"));
		System.out.println(test.ceilingKey("D"));
		System.out.println("======================");
		test.remove("D");
		printAll(test);
		System.out.println("======================");
		System.out.println(test.floorKey("D"));
		System.out.println(test.ceilingKey("D"));
		

	}

}
