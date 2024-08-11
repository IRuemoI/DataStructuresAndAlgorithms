namespace AdvancedTraining.Lesson13;

//todo:待整理
// 本题测试链接 : https://leetcode.cn/problems/scramble-string/
public class ScrambleString
{
    private static bool IsScramble0(string s1, string s2)
    {
        if ((ReferenceEquals(s1, null) && !ReferenceEquals(s2, null)) ||
            (!ReferenceEquals(s1, null) && ReferenceEquals(s2, null))) return false;
        if (ReferenceEquals(s1, null) && ReferenceEquals(s2, null)) return true;
        if (s1.Equals(s2)) return true;
        var str1 = s1.ToCharArray();
        var str2 = s2.ToCharArray();
        if (!SameTypeSameNumber(str1, str2)) return false;
        return Process0(str1, 0, str1.Length - 1, str2, 0, str2.Length - 1);
    }

    // str1[L1...R1] str2[L2...R2] 是否互为玄变串
    // 一定保证这两段是等长的！
    private static bool Process0(char[] str1, int L1, int R1, char[] str2, int L2, int R2)
    {
        if (L1 == R1) return str1[L1] == str2[L2];
        for (var leftEnd = L1; leftEnd < R1; leftEnd++)
        {
            var p1 = Process0(str1, L1, leftEnd, str2, L2, L2 + leftEnd - L1) &&
                     Process0(str1, leftEnd + 1, R1, str2, L2 + leftEnd - L1 + 1, R2);
            var p2 = Process0(str1, L1, leftEnd, str2, R2 - (leftEnd - L1), R2) &&
                     Process0(str1, leftEnd + 1, R1, str2, L2, R2 - (leftEnd - L1) - 1);
            if (p1 || p2) return true;
        }

        return false;
    }

    private static bool SameTypeSameNumber(char[] str1, char[] str2)
    {
        if (str1.Length != str2.Length) return false;
        var map = new int[256];
        foreach (var item in str1)
            map[item]++;

        foreach (var item in str2)
            if (--map[item] < 0)
                return false;

        return true;
    }

    private static bool IsScramble1(string s1, string s2)
    {
        if ((ReferenceEquals(s1, null) && !ReferenceEquals(s2, null)) ||
            (!ReferenceEquals(s1, null) && ReferenceEquals(s2, null))) return false;
        if (ReferenceEquals(s1, null) && ReferenceEquals(s2, null)) return true;
        if (s1.Equals(s2)) return true;
        var str1 = s1.ToCharArray();
        var str2 = s2.ToCharArray();
        if (!SameTypeSameNumber(str1, str2)) return false;
        var N = s1.Length;
        return Process1(str1, str2, 0, 0, N);
    }

    // 返回str1[从L1开始往右长度为size的子串]和str2[从L2开始往右长度为size的子串]是否互为旋变字符串
    // 在str1中的这一段和str2中的这一段一定是等长的，所以只用一个参数size
    private static bool Process1(char[] str1, char[] str2, int L1, int L2, int size)
    {
        if (size == 1) return str1[L1] == str2[L2];
        // 枚举每一种情况，有一个计算出互为旋变就返回true。都算不出来最后返回false
        for (var leftPart = 1; leftPart < size; leftPart++)
            if ((Process1(str1, str2, L1, L2, leftPart) &&
                 Process1(str1, str2, L1 + leftPart, L2 + leftPart, size - leftPart)) ||
                (Process1(str1, str2, L1, L2 + size - leftPart, leftPart) &&
                 Process1(str1, str2, L1 + leftPart, L2, size - leftPart)))
                return true;
        return false;
    }

    private static bool IsScramble2(string s1, string s2)
    {
        if ((ReferenceEquals(s1, null) && !ReferenceEquals(s2, null)) ||
            (!ReferenceEquals(s1, null) && ReferenceEquals(s2, null))) return false;
        if (ReferenceEquals(s1, null) && ReferenceEquals(s2, null)) return true;
        if (s1.Equals(s2)) return true;
        var str1 = s1.ToCharArray();
        var str2 = s2.ToCharArray();
        if (!SameTypeSameNumber(str1, str2)) return false;
        var N = s1.Length;
        var dp = new int[N, N, N + 1];
        // dp[i,j,k] = 0 processDP(i,j,k)状态之前没有算过的
        // dp[i,j,k] = -1 processDP(i,j,k)状态之前算过的,返回值是false
        // dp[i,j,k] = 1 processDP(i,j,k)状态之前算过的,返回值是true
        return Process2(str1, str2, 0, 0, N, dp);
    }

    private static bool Process2(char[] str1, char[] str2, int L1, int L2, int size, int[,,] dp)
    {
        if (dp[L1, L2, size] != 0) return dp[L1, L2, size] == 1;
        var ans = false;
        if (size == 1)
            ans = str1[L1] == str2[L2];
        else
            for (var leftPart = 1; leftPart < size; leftPart++)
                if ((Process2(str1, str2, L1, L2, leftPart, dp) &&
                     Process2(str1, str2, L1 + leftPart, L2 + leftPart, size - leftPart, dp)) ||
                    (Process2(str1, str2, L1, L2 + size - leftPart, leftPart, dp) &&
                     Process2(str1, str2, L1 + leftPart, L2, size - leftPart, dp)))
                {
                    ans = true;
                    break;
                }

        dp[L1, L2, size] = ans ? 1 : -1;
        return ans;
    }

    private static bool IsScramble3(string s1, string s2)
    {
        if ((ReferenceEquals(s1, null) && !ReferenceEquals(s2, null)) ||
            (!ReferenceEquals(s1, null) && ReferenceEquals(s2, null))) return false;
        if (ReferenceEquals(s1, null) && ReferenceEquals(s2, null)) return true;
        if (s1.Equals(s2)) return true;
        var str1 = s1.ToCharArray();
        var str2 = s2.ToCharArray();
        if (!SameTypeSameNumber(str1, str2)) return false;
        var N = s1.Length;
        var dp = new bool[N, N, N + 1];
        for (var L1 = 0; L1 < N; L1++)
        for (var L2 = 0; L2 < N; L2++)
            dp[L1, L2, 1] = str1[L1] == str2[L2];
        // 第一层for循环含义是：依次填size=2层、size=3层..size=N层，每一层都是一个二维平面
        // 第二、三层for循环含义是：在具体的一层，整个面都要填写，所以用两个for循环去填一个二维面
        // L1的取值氛围是[0,N-size]，因为从L1出发往右长度为size的子串，L1是不能从N-size+1出发的，这样往右就不够size个字符了
        // L2的取值范围同理
        // 第4层for循环完全是递归函数怎么写，这里就怎么改的
        for (var size = 2; size <= N; size++)
        for (var L1 = 0; L1 <= N - size; L1++)
        for (var L2 = 0; L2 <= N - size; L2++)
        for (var leftPart = 1; leftPart < size; leftPart++)
            if ((dp[L1, L2, leftPart] && dp[L1 + leftPart, L2 + leftPart, size - leftPart]) ||
                (dp[L1, L2 + size - leftPart, leftPart] && dp[L1 + leftPart, L2, size - leftPart]))
            {
                dp[L1, L2, size] = true;
                break;
            }

        return dp[0, 0, N];
    }
}