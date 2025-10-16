import java.util.ArrayList;
import java.util.Comparator;
import java.util.HashMap;
import java.util.List;

public class Code02_EveryStepShowBoss {

	public static class Customer {
		public int id;
		public int buy;
		public int enterTime;

		public Customer(int v, int b, int o) {
			id = v;
			buy = b;
			enterTime = 0;
		}
	}

	public static class CandidateComparator implements Comparator<Customer> {

		@Override
		public int compare(Customer o1, Customer o2) {
			return o1.buy != o2.buy ? (o2.buy - o1.buy) : (o1.enterTime - o2.enterTime);
		}

	}

	public static class DaddyComparator implements Comparator<Customer> {

		@Override
		public int compare(Customer o1, Customer o2) {
			return o1.buy != o2.buy ? (o1.buy - o2.buy) : (o1.enterTime - o2.enterTime);
		}

	}

	public static class WhosYourDaddy {
		private HashMap<Integer, Customer> customers;
		private HeapGreater<Customer> candHeap;
		private HeapGreater<Customer> daddyHeap;
		private final int daddyLimit;

		public WhosYourDaddy(int limit) {
			customers = new HashMap<Integer, Customer>();
			candHeap = new HeapGreater<>(new CandidateComparator());
			daddyHeap = new HeapGreater<>(new DaddyComparator());
			daddyLimit = limit;
		}

		// ��ǰ����i���¼���arr[i] -> id,  buyOrRefund
		public void operate(int time, int id, boolean buyOrRefund) {
			if (!buyOrRefund && !customers.containsKey(id)) {
				return;
			}
			if (!customers.containsKey(id)) {
				customers.put(id, new Customer(id, 0, 0));
			}
			Customer c = customers.get(id);
			if (buyOrRefund) {
				c.buy++;
			} else {
				c.buy--;
			}
			if (c.buy == 0) {
				customers.remove(id);
			}
			if (!candHeap.contains(c) && !daddyHeap.contains(c)) {
				if (daddyHeap.size() < daddyLimit) {
					c.enterTime = time;
					daddyHeap.push(c);
				} else {
					c.enterTime = time;
					candHeap.push(c);
				}
			} else if (candHeap.contains(c)) {
				if (c.buy == 0) {
					candHeap.remove(c);
				} else {
					candHeap.resign(c);
				}
			} else {
				if (c.buy == 0) {
					daddyHeap.remove(c);
				} else {
					daddyHeap.resign(c);
				}
			}
			daddyMove(time);
		}

		public List<Integer> getDaddies() {
			List<Customer> customers = daddyHeap.getAllElements();
			List<Integer> ans = new ArrayList<>();
			for (Customer c : customers) {
				ans.add(c.id);
			}
			return ans;
		}

		private void daddyMove(int time) {
			if (candHeap.isEmpty()) {
				return;
			}
			if (daddyHeap.size() < daddyLimit) {
				Customer p = candHeap.pop();
				p.enterTime = time;
				daddyHeap.push(p);
			} else {
				if (candHeap.peek().buy > daddyHeap.peek().buy) {
					Customer oldDaddy = daddyHeap.pop();
					Customer newDaddy = candHeap.pop();
					oldDaddy.enterTime = time;
					newDaddy.enterTime = time;
					daddyHeap.push(newDaddy);
					candHeap.push(oldDaddy);
				}
			}
		}

	}

	public static List<List<Integer>> topK(int[] arr, boolean[] op, int k) {
		List<List<Integer>> ans = new ArrayList<>();
		WhosYourDaddy whoDaddies = new WhosYourDaddy(k);
		for (int i = 0; i < arr.length; i++) {
			whoDaddies.operate(i, arr[i], op[i]);
			ans.add(whoDaddies.getDaddies());
		}
		return ans;
	}

	// �������е��£�ģ�⣬���Ż�
	public static List<List<Integer>> compare(int[] arr, boolean[] op, int k) {
		HashMap<Integer, Customer> map = new HashMap<>();
		ArrayList<Customer> cands = new ArrayList<>();
		ArrayList<Customer> daddy = new ArrayList<>();
		List<List<Integer>> ans = new ArrayList<>();
		for (int i = 0; i < arr.length; i++) {
			int id = arr[i];
			boolean buyOrRefund = op[i];
			if (!buyOrRefund && !map.containsKey(id)) {
				ans.add(getCurAns(daddy));
				continue;
			}
			// û�з������û�������Ϊ0�������˻���
			// �û�֮ǰ��������0����ʱ����¼�
			// �û�֮ǰ������>0�� ��ʱ���
			// �û�֮ǰ������>0, ��ʱ�˻�
			if (!map.containsKey(id)) {
				map.put(id, new Customer(id, 0, 0));
			}
			// ����
			Customer c = map.get(id);
			if (buyOrRefund) {
				c.buy++;
			} else {
				c.buy--;
			}
			if (c.buy == 0) {
				map.remove(id);
			}
			// c
			// ������
			if (!cands.contains(c) && !daddy.contains(c)) {
				if (daddy.size() < k) {
					c.enterTime = i;
					daddy.add(c);
				} else {
					c.enterTime = i;
					cands.add(c);
				}
			}
			cleanZeroBuy(cands);
			cleanZeroBuy(daddy);
			cands.sort(new CandidateComparator());
			daddy.sort(new DaddyComparator());
			move(cands, daddy, k, i);
			ans.add(getCurAns(daddy));
		}
		return ans;
	}

	public static void move(ArrayList<Customer> cands, ArrayList<Customer> daddy, int k, int time) {
		if (cands.isEmpty()) {
			return;
		}
		// ��ѡ����Ϊ��
		if (daddy.size() < k) {
			Customer c = cands.get(0);
			c.enterTime = time;
			daddy.add(c);
			cands.remove(0);
		} else { // �Ƚ������ˣ���ѡ���ж���
			if (cands.get(0).buy > daddy.get(0).buy) {
				Customer oldDaddy = daddy.get(0);
				daddy.remove(0);
				Customer newDaddy = cands.get(0);
				cands.remove(0);
				newDaddy.enterTime = time;
				oldDaddy.enterTime = time;
				daddy.add(newDaddy);
				cands.add(oldDaddy);
			}
		}
	}

	public static void cleanZeroBuy(ArrayList<Customer> arr) {
		List<Customer> noZero = new ArrayList<Customer>();
		for (Customer c : arr) {
			if (c.buy != 0) {
				noZero.add(c);
			}
		}
		arr.clear();
		for (Customer c : noZero) {
			arr.add(c);
		}
	}

	public static List<Integer> getCurAns(ArrayList<Customer> daddy) {
		List<Integer> ans = new ArrayList<>();
		for (Customer c : daddy) {
			ans.add(c.id);
		}
		return ans;
	}

	// Ϊ�˲���
	public static class Data {
		public int[] arr;
		public boolean[] op;

		public Data(int[] a, boolean[] o) {
			arr = a;
			op = o;
		}
	}

	// Ϊ�˲���
	public static Data randomData(int maxValue, int maxLen) {
		int len = (int) (Math.random() * maxLen) + 1;
		int[] arr = new int[len];
		boolean[] op = new boolean[len];
		for (int i = 0; i < len; i++) {
			arr[i] = (int) (Math.random() * maxValue);
			op[i] = Math.random() < 0.5 ? true : false;
		}
		return new Data(arr, op);
	}

	// Ϊ�˲���
	public static boolean sameAnswer(List<List<Integer>> ans1, List<List<Integer>> ans2) {
		if (ans1.size() != ans2.size()) {
			return false;
		}
		for (int i = 0; i < ans1.size(); i++) {
			List<Integer> cur1 = ans1.get(i);
			List<Integer> cur2 = ans2.get(i);
			if (cur1.size() != cur2.size()) {
				return false;
			}
			cur1.sort((a, b) -> a - b);
			cur2.sort((a, b) -> a - b);
			for (int j = 0; j < cur1.size(); j++) {
				if (!cur1.get(j).equals(cur2.get(j))) {
					return false;
				}
			}
		}
		return true;
	}

	public class HeapGreater<T> {

		private ArrayList<T> heap;
		private HashMap<T, Integer> indexMap;
		private int heapSize;
		private Comparator<? super T> comp;
	
		public HeapGreater(Comparator<T> c) {
			heap = new ArrayList<>();
			indexMap = new HashMap<>();
			heapSize = 0;
			comp = c;
		}
	
		public boolean isEmpty() {
			return heapSize == 0;
		}
	
		public int size() {
			return heapSize;
		}
	
		public boolean contains(T obj) {
			return indexMap.containsKey(obj);
		}
	
		public T peek() {
			return heap.get(0);
		}
	
		public void push(T obj) {
			heap.add(obj);
			indexMap.put(obj, heapSize);
			heapInsert(heapSize++);
		}
	
		public T pop() {
			T ans = heap.get(0);
			swap(0, heapSize - 1);
			indexMap.remove(ans);
			heap.remove(--heapSize);
			heapify(0);
			return ans;
		}
	
		public void remove(T obj) {
			T replace = heap.get(heapSize - 1);
			int index = indexMap.get(obj);
			indexMap.remove(obj);
			heap.remove(--heapSize);
			if (obj != replace) {
				heap.set(index, replace);
				indexMap.put(replace, index);
				resign(replace);
			}
		}
	
		public void resign(T obj) {
			heapInsert(indexMap.get(obj));
			heapify(indexMap.get(obj));
		}
	
		// �뷵�ض��ϵ�����Ԫ��
		public List<T> getAllElements() {
			List<T> ans = new ArrayList<>();
			for (T c : heap) {
				ans.add(c);
			}
			return ans;
		}
	
		private void heapInsert(int index) {
			while (comp.compare(heap.get(index), heap.get((index - 1) / 2)) < 0) {
				swap(index, (index - 1) / 2);
				index = (index - 1) / 2;
			}
		}
	
		private void heapify(int index) {
			int left = index * 2 + 1;
			while (left < heapSize) {
				int best = left + 1 < heapSize && comp.compare(heap.get(left + 1), heap.get(left)) < 0 ? (left + 1) : left;
				best = comp.compare(heap.get(best), heap.get(index)) < 0 ? best : index;
				if (best == index) {
					break;
				}
				swap(best, index);
				index = best;
				left = index * 2 + 1;
			}
		}
	
		private void swap(int i, int j) {
			T o1 = heap.get(i);
			T o2 = heap.get(j);
			heap.set(i, o2);
			heap.set(j, o1);
			indexMap.put(o2, i);
			indexMap.put(o1, j);
		}
	
	}
	

	public static void main(String[] args) {
		int maxValue = 10;
		int maxLen = 100;
		int maxK = 6;
		int testTimes = 100000;
		System.out.println("���Կ�ʼ");
		for (int i = 0; i < testTimes; i++) {
			Data testData = randomData(maxValue, maxLen);
			int k = (int) (Math.random() * maxK) + 1;
			int[] arr = testData.arr;
			boolean[] op = testData.op;
			List<List<Integer>> ans1 = topK(arr, op, k);
			List<List<Integer>> ans2 = compare(arr, op, k);
			if (!sameAnswer(ans1, ans2)) {
				for (int j = 0; j < arr.length; j++) {
					System.out.println(arr[j] + " , " + op[j]);
				}
				System.out.println(k);
				System.out.println(ans1);
				System.out.println(ans2);
				System.out.println("�����ˣ�");
				break;
			}
		}
		System.out.println("���Խ���");
	}

}
