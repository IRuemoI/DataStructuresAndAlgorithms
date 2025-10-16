import java.util.Arrays;
import java.util.Comparator;
import java.util.PriorityQueue;

// ��Ŀ
// ����arr����ÿһ�����Ȼ���һ�����ȵ�ʱ�䣬ÿ�����Ȼ�ֻ�ܴ��е����쿧�ȡ�
// ������n������Ҫ�ȿ��ȣ�ֻ���ÿ��Ȼ������쿧�ȡ�
// ��Ϊÿ���˺ȿ��ȵ�ʱ��ǳ��̣���õ�ʱ�伴�Ǻ����ʱ�䡣
// ÿ���˺���֮�󿧷ȱ�����ѡ��ϴ������Ȼ�ӷ��ɾ���ֻ��һ̨ϴ���ȱ��Ļ�����ֻ�ܴ��е�ϴ���ȱ���
// ϴ���ӵĻ���ϴ��һ������ʱ��Ϊa���κ�һ��������Ȼ�ӷ��ɾ���ʱ��Ϊb��
// �ĸ�������arr, n, a, b
// ����ʱ����0��ʼ�����������˺��꿧�Ȳ�ϴ�꿧�ȱ���ȫ�����̽�������������ʲôʱ��㡣
public class Code03_Coffee {

	// ��֤�ķ���
	// ���׵ı���
	// �������Ǿ�����ȷ
	public static int right(int[] arr, int n, int a, int b) {
		int[] times = new int[arr.length];
		int[] drink = new int[n];
		return forceMake(arr, times, 0, drink, n, a, b);
	}

	// ÿ���˱���������ÿһ�����Ȼ����Լ�������
	public static int forceMake(int[] arr, int[] times, int kth, int[] drink, int n, int a, int b) {
		if (kth == n) {
			int[] drinkSorted = Arrays.copyOf(drink, kth);
			Arrays.sort(drinkSorted);
			return forceWash(drinkSorted, a, b, 0, 0, 0);
		}
		int time = Integer.MAX_VALUE;
		for (int i = 0; i < arr.length; i++) {
			int work = arr[i];
			int pre = times[i];
			drink[kth] = pre + work;
			times[i] = pre + work;
			time = Math.min(time, forceMake(arr, times, kth + 1, drink, n, a, b));
			drink[kth] = 0;
			times[i] = pre;
		}
		return time;
	}

	public static int forceWash(int[] drinks, int a, int b, int index, int washLine, int time) {
		if (index == drinks.length) {
			return time;
		}
		// ѡ��һ����ǰindex�ſ��ȱ���ѡ����ϴ���Ȼ�ˢ�ɾ�
		int wash = Math.max(drinks[index], washLine) + a;
		int ans1 = forceWash(drinks, a, b, index + 1, wash, Math.max(wash, time));

		// ѡ�������ǰindex�ſ��ȱ���ѡ����Ȼ�ӷ�
		int dry = drinks[index] + b;
		int ans2 = forceWash(drinks, a, b, index + 1, washLine, Math.max(dry, time));
		return Math.min(ans1, ans2);
	}

	// ����Ϊ̰��+��������
	public static class Machine {
		public int timePoint;
		public int workTime;

		public Machine(int t, int w) {
			timePoint = t;
			workTime = w;
		}
	}

	public static class MachineComparator implements Comparator<Machine> {

		@Override
		public int compare(Machine o1, Machine o2) {
			return (o1.timePoint + o1.workTime) - (o2.timePoint + o2.workTime);
		}

	}

	// ����һ��ı������Եķ���
	public static int minTime1(int[] arr, int n, int a, int b) {
		PriorityQueue<Machine> heap = new PriorityQueue<Machine>(new MachineComparator());
		for (int i = 0; i < arr.length; i++) {
			heap.add(new Machine(0, arr[i]));
		}
		int[] drinks = new int[n];
		for (int i = 0; i < n; i++) {
			Machine cur = heap.poll();
			cur.timePoint += cur.workTime;
			drinks[i] = cur.timePoint;
			heap.add(cur);
		}
		return bestTime(drinks, a, b, 0, 0);
	}

	// drinks ���б��ӿ��Կ�ʼϴ��ʱ��
	// wash ����ϴ�ɾ���ʱ�䣨���У�
	// air �ӷ��ɾ���ʱ��(����)
	// free ϴ�Ļ���ʲôʱ�����
	// drinks[index.....]����ɾ�������Ľ���ʱ�䣨���أ�
	public static int bestTime(int[] drinks, int wash, int air, int index, int free) {
		if (index == drinks.length) {
			return 0;
		}
		// index�ű��� ����ϴ
		int selfClean1 = Math.max(drinks[index], free) + wash;
		int restClean1 = bestTime(drinks, wash, air, index + 1, selfClean1);
		int p1 = Math.max(selfClean1, restClean1);

		// index�ű��� �����ӷ�
		int selfClean2 = drinks[index] + air;
		int restClean2 = bestTime(drinks, wash, air, index + 1, free);
		int p2 = Math.max(selfClean2, restClean2);
		return Math.min(p1, p2);
	}

	// ̰��+�������Ըĳɶ�̬�滮
	public static int minTime2(int[] arr, int n, int a, int b) {
		PriorityQueue<Machine> heap = new PriorityQueue<Machine>(new MachineComparator());
		for (int i = 0; i < arr.length; i++) {
			heap.add(new Machine(0, arr[i]));
		}
		int[] drinks = new int[n];
		for (int i = 0; i < n; i++) {
			Machine cur = heap.poll();
			cur.timePoint += cur.workTime;
			drinks[i] = cur.timePoint;
			heap.add(cur);
		}
		return bestTimeDp(drinks, a, b);
	}

	public static int bestTimeDp(int[] drinks, int wash, int air) {
		int N = drinks.length;
		int maxFree = 0;
		for (int i = 0; i < drinks.length; i++) {
			maxFree = Math.max(maxFree, drinks[i]) + wash;
		}
		int[][] dp = new int[N + 1][maxFree + 1];
		for (int index = N - 1; index >= 0; index--) {
			for (int free = 0; free <= maxFree; free++) {
				int selfClean1 = Math.max(drinks[index], free) + wash;
				if (selfClean1 > maxFree) {
					break; // ��Ϊ�����Ҳ����������
				}
				// index�ű��� ����ϴ
				int restClean1 = dp[index + 1][selfClean1];
				int p1 = Math.max(selfClean1, restClean1);
				// index�ű��� �����ӷ�
				int selfClean2 = drinks[index] + air;
				int restClean2 = dp[index + 1][free];
				int p2 = Math.max(selfClean2, restClean2);
				dp[index][free] = Math.min(p1, p2);
			}
		}
		return dp[0][0];
	}

	// for test
	public static int[] randomArray(int len, int max) {
		int[] arr = new int[len];
		for (int i = 0; i < len; i++) {
			arr[i] = (int) (Math.random() * max) + 1;
		}
		return arr;
	}

	// for test
	public static void printArray(int[] arr) {
		System.out.print("arr : ");
		for (int j = 0; j < arr.length; j++) {
			System.out.print(arr[j] + ", ");
		}
		System.out.println();
	}

	public static void main(String[] args) {
		int len = 10;
		int max = 10;
		int testTime = 10;
		System.out.println("���Կ�ʼ");
		for (int i = 0; i < testTime; i++) {
			int[] arr = randomArray(len, max);
			int n = (int) (Math.random() * 7) + 1;
			int a = (int) (Math.random() * 7) + 1;
			int b = (int) (Math.random() * 10) + 1;
			int ans1 = right(arr, n, a, b);
			int ans2 = minTime1(arr, n, a, b);
			int ans3 = minTime2(arr, n, a, b);
			if (ans1 != ans2 || ans2 != ans3) {
				printArray(arr);
				System.out.println("n : " + n);
				System.out.println("a : " + a);
				System.out.println("b : " + b);
				System.out.println(ans1 + " , " + ans2 + " , " + ans3);
				System.out.println("===============");
				break;
			}
		}
		System.out.println("���Խ���");

	}

}
