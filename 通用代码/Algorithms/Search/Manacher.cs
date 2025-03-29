//测试通过

namespace Common.Algorithms.Search;

public static class Manacher
{
    private static int Code(string? s)
    {
        if (string.IsNullOrEmpty(s)) return 0;

        // "12132" -> "#1#2#1#3#2#"
        var str = ManacherString(s);
        // 回文半径的大小
        var pArr = new int[str.Length];
        var c = -1;
        // 讲述中：R代表最右的扩成功的位置
        // coding：最右的扩成功位置的，再下一个位置
        var r = -1;
        var max = int.MinValue;
        for (var i = 0; i < str.Length; i++)
        {
            // 0 1 2
            // R第一个违规的位置，i>= R
            // i位置扩出来的答案，i位置扩的区域，至少是多大。
            pArr[i] = r > i ? Math.Min(pArr[2 * c - i], r - i) : 1;
            // R > i 就是之前描述的 i在R内部  因为现在的R的含义是初次失败的位置。
            // i 在 R 外 i 至少的回文半径也是 1  他自己。
            // 如果在 R 内  2 * C - i 就是 i'
            // min的意思是 i'的回文半径和i到R的距离，谁小就是我至少不用验的区域
            //  i 在 R内
            //  i' 在R内不就是i'的回文半径小嘛， i'在R外 就是 i 到 R的距离小嘛 压线他俩相等嘛
            while (i + pArr[i] < str.Length && i - pArr[i] > -1)
                // 在上面讲解中 i 在 R内第一种情况是不用验直接出答案的，这里为什么没做区分呢
                //  为了省代码，这种不用验的，进到这里面也是直接break
                if (str[i + pArr[i]] == str[i - pArr[i]])
                    pArr[i]++;
                else
                    break;

            if (i + pArr[i] > r)
            {
                r = i + pArr[i];
                c = i;
            }

            max = Math.Max(max, pArr[i]);
        }

        // max 记录的是我们的回文半径，为什么是回文半径减1呢
        // 别忘了， 这里我们用的是处理串
        // 举个例子  1 2 1  -> # 1 # 2 # 1 #
        // 回文长度是 3   看我们处理串回文半径 是不是 4
        // 偶回文也一样 1 2 2 1 ->  # 1 # 2 # 2 # 1 #
        // 回文长度是 4   而处理串的回文半径是多少啊   是 5！
        // 所以 处理串的回文半径 -1 就是原始串的回文长度
        return max - 1;
    }

    private static char[] ManacherString(string str)
    {
        var charArr = str.ToCharArray();
        var res = new char[str.Length * 2 + 1];
        var index = 0;
        for (var i = 0; i != res.Length; i++) res[i] = (i & 1) == 0 ? '#' : charArr[index++];

        return res;
    }
}