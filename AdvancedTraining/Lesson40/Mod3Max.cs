#region

using System.Text;
using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson40;

// 来自去哪儿网
// 给定一个arr，里面的数字都是0~9
// 你可以随意使用arr中的数字，哪怕打乱顺序也行
// 请拼出一个能被3整除的，最大的数字，用str形式返回
public class Mod3Max
{
    //todo:待修复
    public static void Run()
    {
        StringComparer.Run();
    }

    private class StringComparer : IComparer<string>
    {
        public int Compare(string? a, string? b)
        {
            return Convert.ToInt32(b).CompareTo(Convert.ToInt32(a));
        }

        private static string? Max1(int[] arr)
        {
            Array.Sort(arr);
            for (int l = 0, r = arr.Length - 1; l < r; l++, r--) (arr[l], arr[r]) = (arr[r], arr[l]);

            var builder = new StringBuilder();
            var set = new SortedSet<string>(new StringComparer());
            Process1(arr, 0, builder, set);
            return set.Count == 0 ? "" : set.Min;
        }

        private static void Process1(int[] arr, int index, StringBuilder builder, SortedSet<string> set)
        {
            if (index == arr.Length)
            {
                if (builder.Length != 0 && Convert.ToInt32(builder.ToString()) % 3 == 0) set.Add(builder.ToString());
            }
            else
            {
                Process1(arr, index + 1, builder, set);
                builder.Append(arr[index]);
                Process1(arr, index + 1, builder, set);
                builder.Remove(builder.Length - 1, 1);
            }
        }

        private static string Max2(int[]? arr)
        {
            if (arr == null || arr.Length == 0) return "";

            Array.Sort(arr);
            for (int l = 0, r = arr.Length - 1; l < r; l++, r--) (arr[l], arr[r]) = (arr[r], arr[l]);

            if (arr[0] == 0) return "0";

            var ans = Process2(arr, 0, 0);
            var res = ans.Replace("^(0+)", "");
            if (!res.Equals("")) return res;

            return ans.Equals("") ? ans : "0";
        }

        // arr中的数字一定是0~9
        // arr是经过排序的，并且是从大到小排序，比如[9,8,7,7,7,3,1]等
        // 这个递归函数的含义 :
        // 在arr[index...一直到最后]上做选择，arr[0...index-1]就当不存在
        // 每个位置的字符可以要、也可以不要，但是！选出来的数字拼完之后的结果，在%3之后，余数一定要是mod！
        // 返回在上面设定的情况下，最大的数是多少？
        // 如果存在这样的数，返回字符串的形式
        // 如果不存在这样的数，返回特殊字符串，比如"$"，代表不可能
        // 这个递归函数可以很轻易的改出动态规划
        private static string Process2(int[] arr, int index, int mod)
        {
            if (index == arr.Length) return mod == 0 ? "" : "$";

            var p1 = "$";
            var nextMod = NextMod(mod, arr[index] % 3);
            var next = Process2(arr, index + 1, nextMod);
            if (!next.Equals("$")) p1 = arr[index] + next;

            var p2 = Process2(arr, index + 1, mod);
            if (p1.Equals("$") && p2.Equals("$")) return "$";

            if (!p1.Equals("$") && !p2.Equals("$")) return Smaller(p1, p2) ? p2 : p1;

            return p1.Equals("$") ? p2 : p1;
        }

        private static int NextMod(int require, int current)
        {
            if (require == 0)
            {
                if (current == 0)
                    return 0;
                if (current == 1)
                    return 2;
                return 1;
            }

            if (require == 1)
            {
                if (current == 0)
                    return 1;
                if (current == 1)
                    return 0;
                return 2;
            }

            // require == 2
            if (current == 0)
                return 2;
            if (current == 1)
                return 1;
            return 0;
        }

        private static bool Smaller(string p1, string p2)
        {
            if (p1.Length != p2.Length) return p1.Length < p2.Length;

            return string.Compare(p1, p2, StringComparison.Ordinal) < 0;
        }

        // 贪心的思路解法 :
        // 先得到数组的累加和，记为sum
        // 1) 如果sum%3==0，说明所有数从大到小拼起来就可以了
        // 2) 如果sum%3==1，说明多了一个余数1，
        // 只需要删掉一个最小的数(该数是%3==1的数);
        // 如果没有，只需要删掉两个最小的数(这两个数都是%3==2的数);
        // 3) 如果sum%3==2，说明多了一个余数2，
        // 只需要删掉一个最小的数(该数是%3==2的数);
        // 如果没有，只需要删掉两个最小的数(这两个数都是%3==1的数);
        // 如果上面都做不到，说明拼不成
        private static string Max3(int[]? a1)
        {
            if (a1 == null || a1.Length == 0) return "";

            var mod = 0;
            var arr = new List<int>();
            foreach (var num in a1)
            {
                arr.Add(num);
                mod += num;
                mod %= 3;
            }

            if ((mod == 1 || mod == 2) && !Remove(arr, mod, 3 - mod)) return "";

            if (arr.Count == 0) return "";

            arr.Sort((a, b) => b - a);
            if (arr[0] == 0) return "0";

            var builder = new StringBuilder();
            foreach (var num in arr) builder.Append(num);

            return builder.ToString();
        }

        // 在arr中，要么删掉最小的一个、且%3之后余数是first的数
        // 如果做不到，删掉最小的两个、且%3之后余数是second的数
        // 如果能做到返回true，不能做到返回false
        private static bool Remove(List<int> arr, int first, int second)
        {
            if (arr.Count == 0) return false;

            arr.Sort((a, b) => Compare(a, b, first, second));
            var size = arr.Count;
            if (arr[size - 1] % 3 == first)
            {
                arr.RemoveAt(size - 1);
                return true;
            }

            if (size > 1 && arr[size - 1] % 3 == second && arr[size - 2] % 3 == second)
            {
                arr.RemoveAt(size - 1);
                arr.RemoveAt(size - 2);
                return true;
            }

            return false;
        }

        // a和b比较:
        // 如果余数一样，谁大谁放前面
        // 如果余数不一样，余数是0的放最前面、余数是s的放中间、余数是f的放最后
        private static int Compare(int a, int b, int f, int s)
        {
            var ma = a % 3;
            var mb = b % 3;
            if (ma == mb) return b - a;

            if (ma == 0 || mb == 0)
                return ma == 0 ? -1 : 1;
            return ma == s ? -1 : 1;
        }

        // 为了测试
        private static int[] RandomArray(int len)
        {
            var arr = new int[len];
            for (var i = 0; i < len; i++) arr[i] = (int)(Utility.GetRandomDouble * 10);

            return arr;
        }

        // 为了测试
        private static int[] CopyArray(int[] arr)
        {
            var ans = new int[arr.Length];
            for (var i = 0; i < arr.Length; i++) ans[i] = arr[i];

            return ans;
        }

        // 为了测试
        public static void Run()
        {
            const int n = 10;
            const int testTimes = 10000;
            Console.WriteLine("测试开始");
            for (var i = 0; i < testTimes; i++)
            {
                var len = (int)(Utility.GetRandomDouble * n);
                var arr1 = RandomArray(len);
                var arr2 = CopyArray(arr1);
                var arr3 = CopyArray(arr1);
                var ans1 = Max1(arr1);
                var ans2 = Max2(arr2);
                var ans3 = Max3(arr3);
                if (ans1 != null && (!ans1.Equals(ans2) || !ans1.Equals(ans3)))
                {
                    Console.WriteLine("出错了！");
                    foreach (var num in arr3) Console.Write(num + " ");

                    Console.WriteLine();
                    Console.WriteLine(ans1);
                    Console.WriteLine(ans2);
                    Console.WriteLine(ans3);
                    break;
                }
            }

            Console.WriteLine("测试结束");
        }
    }
}