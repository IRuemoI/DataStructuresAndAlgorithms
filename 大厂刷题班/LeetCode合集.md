# leetcode高频题目全讲

001、[两数之和](https://leetcode-cn.com/problems/two-sum/)[简单]

```csharp
public int[] TwoSum(int[] numbers, int target){
    Dictionary<int,int> dict= new();
    //对于C#而言要获取数组的大小使用Length,集合容器使用Count
    //核心思想，每次添加新的元素时，尝试获取(target-numbers[i]),如果没有获取到，那么包numbers[i]放进去
    for(int i=0;i<numbers.Length;i++){
        var temp = target - numbers[i];
        if(dict.ContainsKey(temp)) 
            return[dict[target-numbers[i]],i];
        else 
            dict.Add(numbers[i],i); 
    }
    return [-1,-1];
}
```

002、[两数相加](https://leetcode-cn.com/problems/add-two-numbers/)[中等]

```csharp
//模拟列竖式做加法记得加上进位
public ListNode? AddTwoNumbers(ListNode? l1, ListNode? l2)
{
    if (l1 == null) return l2;
    if (l2 == null) return l1;

    var length1 = GetListLength(l1);
    var length2 = GetListLength(l2);
    var longer = length1 >= length2 ? l1 : l2;
    var shorter = longer == l1 ? l2 : l1;
    var curLonger = longer;
    var curShorter = shorter;
    var last = curLonger;
    var carry = 0; //进位
    var curNum = 0;

    while (curShorter != null)
    {
        curNum = curLonger.val + curShorter.val + carry;
        curLonger.val = curNum % 10;
        carry = curNum / 10;
        last = curLonger;
        curLonger = curLonger.next;
        curShorter = curShorter.next;
    }

    while (curLonger != null)
    {
        curNum = curLonger.val + carry;
        curLonger.val = curNum % 10;
        carry = curNum / 10;
        last = curLonger;
        curLonger = curLonger.next;
    }

    if (carry != 0) last.next = new ListNode(1);

    return longer;
}

private static int GetListLength(ListNode? head)
{
    var length = 0;
    var temp = head;

    while (temp != null)
    {
        length++;
        temp = temp.next;
    }

    return length;
}
```

003、[无重复字符的最长子串](https://leetcode-cn.com/problems/longest-substring-without-repeating-characters/)[中等]

```csharp
public int LengthOfLongestSubstring(string s) {
    if (s is null || s.Equals("")) return 0;
    var str = s.ToCharArray();
    var map = new int[256];//创建一个存储上一次次字符出现的在字符串中下表的数组
    for (var i = 0; i < 256; i++) map[i] = -1;//初始时，这个数组的内容全部为-1
    map[str[0]] = 0;//字符串中的第一个字符的下标在0位置
    var n = str.Length;
    var ans = 1;//记录的答案
    var pre = 1;//上一轮最长不重复子串的长度
    for (var i = 1; i < n; i++)//对于字符串后续的所有字符
    {
        pre = Math.Min(i - map[str[i]], pre + 1);//将当前上次出现的下标与本次下标的距离(不重复距离)和又一个没出现的字符之后增加的距离中较小的保存为上一次的最大长度
        ans = Math.Max(ans, pre);//更新最大长度
        map[str[i]] = i;//更新字符上次出席的下标
    }

    return ans;
}
```

004、[寻找两个正序数组的中位数](https://leetcode-cn.com/problems/median-of-two-sorted-arrays/)[困难]

```csharp
//核心思想：通过将两个数组分为四个部分：第一第二数组，可能包含目标数组的区域，和不包含的部分，使用二分法查找。前提是两数组有序
public static double FindMedianSortedArrays(int[] nums1, int[] nums2)
{
    var size = nums1.Length + nums2.Length;
    var even = (size & 1) == 0;//true是偶数情况，false是奇数情况
    if (nums1.Length != 0 && nums2.Length != 0)
    {
        if (even)
            return (FindKthNum(nums1, nums2, size / 2) + FindKthNum(nums1, nums2, size / 2 + 1)) / 2D;
        return FindKthNum(nums1, nums2, size / 2 + 1);
    }

    if (nums1.Length != 0)
    {
        if (even)
            return (double)(nums1[(size - 1) / 2] + nums1[size / 2]) / 2;
        return nums1[size / 2];
    }

    if (nums2.Length != 0)
    {
        if (even)
            return (double)(nums2[(size - 1) / 2] + nums2[size / 2]) / 2;
        return nums2[size / 2];
    }

    return 0;
}

// 进阶问题 : 在两个都有序的数组中，找整体第K小的数
// 可以做到O(log(Min(M,N)))
private static int FindKthNum(int[] arr1, int[] arr2, int kth)
{
    var longs = arr1.Length >= arr2.Length ? arr1 : arr2;
    var shorts = arr1.Length < arr2.Length ? arr1 : arr2;
    var l = longs.Length;
    var s = shorts.Length;
    if (kth <= s)
        // 1)
        return GetUpMedian(shorts, 0, kth - 1, longs, 0, kth - 1);
    if (kth > l)
    {
        // 3)
        if (shorts[kth - l - 1] >= longs[l - 1]) return shorts[kth - l - 1];
        if (longs[kth - s - 1] >= shorts[s - 1]) return longs[kth - s - 1];
        return GetUpMedian(shorts, kth - l, s - 1, longs, kth - s, l - 1);
    }

    // 2)  s < k <= l
    if (longs[kth - s - 1] >= shorts[s - 1]) return longs[kth - s - 1];
    return GetUpMedian(shorts, 0, s - 1, longs, kth - s, kth - 1);
}


// A[s1...e1]
// B[s2...e2]
// 一定等长！
// 返回整体的，上中位数！8（4） 10（5） 12（6）
private static int GetUpMedian(int[] a, int s1, int e1, int[] b, int s2, int e2)
{
    while (s1 < e1)
    {
        // mid1 = s1 + (e1 - s1) >> 1
        var mid1 = (s1 + e1) / 2;
        var mid2 = (s2 + e2) / 2;
        if (a[mid1] == b[mid2]) return a[mid1];
        // 两个中点一定不等！
        if (((e1 - s1 + 1) & 1) == 1)
        {
            // 奇数长度
            if (a[mid1] > b[mid2])
            {
                if (b[mid2] >= a[mid1 - 1]) return b[mid2];
                e1 = mid1 - 1;
                s2 = mid2 + 1;
            }
            else
            {
                // A[mid1] < B[mid2]
                if (a[mid1] >= b[mid2 - 1]) return a[mid1];
                e2 = mid2 - 1;
                s1 = mid1 + 1;
            }
        }
        else
        {
            // 偶数长度
            if (a[mid1] > b[mid2])
            {
                e1 = mid1;
                s2 = mid2 + 1;
            }
            else
            {
                e2 = mid2;
                s1 = mid1 + 1;
            }
        }
    }

    return Math.Min(a[s1], b[s2]);
}
```

005、[最长回文子串](https://leetcode-cn.com/problems/longest-palindromic-substring/)[中等]

```csharp
//需要修改成返回回文串
private static int Manacher(string? s)
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
        while (i + pArr[i] < str.Length && i - pArr[i] > -1)
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
```

007、[整数反转](https://leetcode-cn.com/problems/reverse-integer/)[简单]

```csharp
public int Reverse(int x) {
    var neg = ((x >>> 31) & 1) == 1;//判断它是不是负数
    x = neg ? x : -x;
    var m = int.MinValue / 10;
    var o = int.MinValue % 10;
    var res = 0;
    while (x != 0)
    {
        if (res < m || (res == m && x % 10 < o)) return 0;
        res = res * 10 + x % 10;
        x /= 10;
    }

    return neg ? res : Math.Abs(res);
}
```

008、[字符串转换整数/ASCIIToInt](https://leetcode-cn.com/problems/string-to-integer-atoi/)[中等]

```csharp
private static int MyAsciiToInt(string s)
{
    if (s is null or "") return 0;
    s = RemoveHeadZero(s.Trim());
    if (s is null or "") return 0;
    var str = s.ToCharArray();
    if (!IsValid(str)) return 0;
    // str 是符合日常书写的，正经整数形式
    var posI = str[0] != '-';
    const int minQ = int.MinValue / 10;
    const int minR = int.MinValue % 10;
    var res = 0;
    for (var i = str[0] == '-' || str[0] == '+' ? 1 : 0; i < str.Length; i++)
    {
        // 3  cur = -3   '5'  cur = -5    '0' cur = 0
        var cur = '0' - str[i];
        if (res < minQ || (res == minQ && cur < minR)) return posI ? int.MaxValue : int.MinValue;
        res = res * 10 + cur;
    }

    // res 负
    if (posI && res == int.MinValue) return int.MaxValue;
    return posI ? -res : res;
}

private static string RemoveHeadZero(string str)
{
    var r = str.StartsWith("+", StringComparison.Ordinal) || str.StartsWith("-", StringComparison.Ordinal);
    var s = r ? 1 : 0;
    for (; s < str.Length; s++)
        if (str[s] != '0')
            break;
    // s 到了第一个不是'0'字符的位置
    var e = -1;
    // 左<-右
    for (var i = str.Length - 1; i >= (r ? 1 : 0); i--)
        if (str[i] < '0' || str[i] > '9')
            e = i;
    // e 到了最左的 不是数字字符的位置
    return (r ? str[0].ToString() : "") + str.Substring(s, (e == -1 ? str.Length : e) - s);
}

private static bool IsValid(char[] chas)
{
    if (chas[0] != '-' && chas[0] != '+' && (chas[0] < '0' || chas[0] > '9')) return false;
    if ((chas[0] == '-' || chas[0] == '+') && chas.Length == 1) return false;
    // 0 +... -... num
    for (var i = 1; i < chas.Length; i++)
        if (chas[i] < '0' || chas[i] > '9')
            return false;
    return true;
}
```

010、[正则表达式匹配](https://leetcode-cn.com/problems/regular-expression-matching/)[困难]

```csharp
private bool IsValid(char[] s, char[] e)
{
    // s中不能有'.' or '*'
    foreach (var item in s)
        if (item == '*' || item == '.')
            return false;

    // 开头的e[0]不能是'*'，没有相邻的'*'
    for (var i = 0; i < e.Length; i++)
        if (e[i] == '*' && (i == 0 || e[i - 1] == '*'))
            return false;

    return true;
}
public bool IsMatch(string str, string pattern)
{
    if (ReferenceEquals(str, null) || ReferenceEquals(pattern, null)) return false;
    var s = str.ToCharArray();
    var p = pattern.ToCharArray();
    if (!IsValid(s, p)) return false;
    var n = s.Length;
    var m = p.Length;
    var dp = new bool[n + 1, m + 1];
    dp[n, m] = true;
    for (var j = m - 1; j >= 0; j--) dp[n, j] = j + 1 < m && p[j + 1] == '*' && dp[n, j + 2];
    // dp[0..N-2,M-1]都等于false，只有dp[N-1,M-1]需要讨论
    if (n > 0 && m > 0) dp[n - 1, m - 1] = s[n - 1] == p[m - 1] || p[m - 1] == '.';
    for (var i = n - 1; i >= 0; i--)
        for (var j = m - 2; j >= 0; j--)
            if (p[j + 1] != '*')
            {
                dp[i, j] = (s[i] == p[j] || p[j] == '.') && dp[i + 1, j + 1];
            }
    else
    {
        if ((s[i] == p[j] || p[j] == '.') && dp[i + 1, j])
            dp[i, j] = true;
        else
            dp[i, j] = dp[i, j + 2];
    }

    return dp[0, 0];
}
```

011、[盛最多水的容器](https://leetcode-cn.com/problems/container-with-most-water/)[中等]

```csharp
public int MaxArea(int[] h) {
    var max = 0;
    var l = 0;
    var r = h.Length - 1;
    while (l < r)
    {
        max = Math.Max(max, Math.Min(h[l], h[r]) * (r - l));
        if (h[l] > h[r])
            r--;
        else
            l++;
    }

    return max;
}
```

011、[整数转罗马数字](https://leetcode-cn.com/problems/integer-to-roman/)[中等]

```csharp
private static string IntToRoman(int num)
{
    string[][] c =
        [
        ["", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"],
        ["", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC"],
        ["", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM"],
        ["", "M", "MM", "MMM"]
    ];
    var roman = new StringBuilder();
    roman.Append(c[3][num / 1000 % 10]).Append(c[2][num / 100 % 10]).Append(c[1][num / 10 % 10]).Append(c[0][num % 10]);
    return roman.ToString();
}
```

012、[罗马数字转整数](https://leetcode-cn.com/problems/roman-to-integer/)[简单]

```csharp
private static int RomanToInt(string s)
{
    // C     M     X   C     I   X
    // 100  1000  10   100   1   10
    var nums = new int[s.Length];
    for (var i = 0; i < s.Length; i++)
        nums[i] = s[i] switch
    {
            'M' => 1000,
            'D' => 500,
            'C' => 100,
            'L' => 50,
            'X' => 10,
            'V' => 5,
            'I' => 1,
            _ => nums[i]
    };

    var sum = 0;
    for (var i = 0; i < nums.Length - 1; i++)
        if (nums[i] < nums[i + 1])
            sum -= nums[i];
    else
        sum += nums[i];
    return sum + nums[^1];
}
```

014、[最长公共前缀](https://leetcode-cn.com/problems/longest-common-prefix/)[简单]

```csharp
public string LongestCommonPrefix(string[] strings) {
    if (strings == null || strings.Length == 0) return "";
    //先取出第一个字符串作为比较的基础
    var chs = strings[0].ToCharArray();
    var min = int.MaxValue;
    foreach (var str in strings)
    {
        var tmp = str.ToCharArray();
        var index = 0;
        while (index < tmp.Length && index < chs.Length)
        {
            if (chs[index] != tmp[index]) break;
            index++;
        }

        min = Math.Min(index, min);
        if (min == 0) return "";
    }

    return strings[0][..min];
}
```

015、[三数之和](https://leetcode-cn.com/problems/3sum/)[中等]

```csharp
public IList<IList<int>> ThreeSum(int[] numbers) {
    Array.Sort(numbers);
    var n = numbers.Length;
    IList<IList<int>> ans = new List<IList<int>>();
    for (var i = n - 1; i > 1; i--)
        // 三元组最后一个数，是arr[i]   之前....二元组 + arr[i]
        if (i == n - 1 || numbers[i] != numbers[i + 1])
        {
            var nexts = TwoSum(numbers, i - 1, -numbers[i]);
            foreach (var cur in nexts)
            {
                cur.Add(numbers[i]);
                ans.Add(cur);
            }
        }

    return ans;
}

// nums[0...end]这个范围上，有多少个不同二元组，相加==target，全返回
// {-1,5}     K = 4
// {1, 3}
private static IList<IList<int>> TwoSum(int[] numbers, int end, int target)
{
    var l = 0;
    var r = end;
    IList<IList<int>> ans = new List<IList<int>>();
    while (l < r)
        if (numbers[l] + numbers[r] > target)
        {
            r--;
        }
    else if (numbers[l] + numbers[r] < target)
    {
        l++;
    }
    else
    {
        // nums[L] + nums[R] == target
        if (l == 0 || numbers[l - 1] != numbers[l])
        {
            IList<int> cur = new List<int>();
            cur.Add(numbers[l]);
            cur.Add(numbers[r]);
            ans.Add(cur);
        }

        l++;
    }

    return ans;
}
```

016、[电话号码的字母组合](https://leetcode-cn.com/problems/letter-combinations-of-a-phone-number/)[中等]

```csharp
private static readonly char[][] Phone =
    [
    ['a', 'b', 'c'],
    ['d', 'e', 'f'],
    ['g', 'h', 'i'],
    ['j', 'k', 'l'],
    ['m', 'n', 'o'],
    ['p', 'q', 'r', 's'],
    ['t', 'u', 'v'],
    ['w', 'x', 'y', 'z']
];

public IList<string> LetterCombinations(string digits)
{
    IList<string> ans = new List<string>();
    if (ReferenceEquals(digits, null) || digits.Length == 0) return ans;
    var str = digits.ToCharArray();
    var path = new char[str.Length];
    Process(str, 0, path, ans);
    return ans;
}

private void Process(char[] str, int index, char[] path, IList<string> ans)
{
    if (index == str.Length)
    {
        ans.Add(new string(path));
    }
    else
    {
        var cands = Phone[str[index] - '2'];
        foreach (var cur in cands)
        {
            path[index] = cur;
            Process(str, index + 1, path, ans);
        }
    }
}
```

017、[删除链表的倒数第N个节点](https://leetcode-cn.com/problems/remove-nth-node-from-end-of-list/)[中等]

```csharp
public ListNode RemoveNthFromEnd(ListNode head, int n) {
    var cur = head;
    ListNode? pre = null;
    while (cur != null)
    {
        n--;
        if (n == -1) pre = head;
        if (n < -1) pre = pre?.next;
        cur = cur.next;
    }

    if (n > 0) return head;
    if (pre == null) return head?.next;
    pre.next = pre.next?.next;
    return head;
}
```

018、[有效的括号](https://leetcode-cn.com/problems/valid-parentheses/)[简单]

```csharp
private static bool IsValid(string s)
{
    if (ReferenceEquals(s, null) || s.Length == 0) return true;
    var str = s.ToCharArray();
    var n = str.Length;
    var stack = new char[n];
    var size = 0;
    for (var i = 0; i < n; i++)
    {
        var cha = str[i];
        if (cha == '(' || cha == '[' || cha == '{')
        {
            stack[size++] = cha == '(' ? ')' : cha == '[' ? ']' : '}';
        }
        else
        {
            if (size == 0) return false;
            var last = stack[--size];
            if (cha != last) return false;
        }
    }

    return size == 0;
}
```

019、[合并两个有序链表](https://leetcode-cn.com/problems/merge-two-sorted-lists/)[简单]

```csharp
public ListNode? MergeTwoListsCode(ListNode? head1, ListNode? head2)
{
    if (head1 == null || head2 == null)
    {
        return head1 == null ? head2 : head1;
    }

    ListNode head = head1.val <= head2.val ? head1 : head2;//先拿到开头数字小的，在后面插入大数字的节点
    ListNode? cur1 = head.next;//指向小头的下一个节点
    ListNode? cur2 = head == head1 ? head2 : head1;//指向大头
    ListNode pre = head;//保存上一个位置

    while (cur1 != null && cur2 != null)
    {
        if (cur1.val <= cur2.val)
        {//将下一个较小的节点放到pre的下一个，并向后移动已赋值的指针
            pre.next = cur1;
            cur1 = cur1.next;
        }
        else
        {
            pre.next = cur2;
            cur2 = cur2.next;
        }
        pre = pre.next;
    }
    pre.next = cur1 ?? cur2;//将剩余的链表连接到pre的后续节点
    return head;//返回头节点
}
```

020、[括号生成](https://leetcode-cn.com/problems/generate-parentheses/)[中等]

```csharp
public IList<string> GenerateParenthesis(int n)
{
    var path = new char[n << 1];
    IList<string> ans = new List<string>();
    Process(path, 0, 0, n, ans);
    return ans;
}

private void Process(char[] path, int index, int leftMinusRight, int leftRest, IList<string> ans)
{
    if (index == path.Length)
    {
        ans.Add(new string(path));
    }
    else
    {
        // index (   )
        if (leftRest > 0)
        {
            path[index] = '(';
            Process(path, index + 1, leftMinusRight + 1, leftRest - 1, ans);
        }

        if (leftMinusRight > 0)
        {
            path[index] = ')';
            Process(path, index + 1, leftMinusRight - 1, leftRest, ans);
        }
    }
}
```

021、[合并K个升序链表](https://leetcode-cn.com/problems/merge-k-sorted-lists/)[困难]

```csharp
public ListNode? MergeKList(ListNode[] lists)
{
    if (lists == null)
    {
        return null;
    }
    Heap<ListNode> minHeap = new((o1, o2) => o1.val - o2.val);//leetcode上要使用的堆容量大概需要10000才能通过所有测试
    for (int i = 0; i < lists.Length; i++)
    {
        if (lists[i] != null)
        {
            minHeap.Push(lists[i]);
        }
    }
    if (minHeap.isEmpty)
    {
        return null;
    }
    ListNode head = minHeap.Pop();
    ListNode pre = head;
    if (pre.next != null)
    {
        minHeap.Push(pre.next);
    }
    while (!minHeap.isEmpty)
    {
        ListNode cur = minHeap.Pop();
        pre.next = cur;
        pre = cur;
        if (cur.next != null)
        {
            minHeap.Push(cur.next);
        }
    }
    return head;
}
```

026、[删除排序数组中的重复项](https://leetcode-cn.com/problems/remove-duplicates-from-sorted-array/)[简单]

```csharp
public int RemoveDuplicates(int[] numbers) {
    if (numbers == null) return 0;
    if (numbers.Length < 2) return numbers.Length;
    var done = 0;
    for (var i = 1; i < numbers.Length; i++)
        if (numbers[i] != numbers[done])
            numbers[++done] = numbers[i];
    return done + 1;
}
```

027、[实现strStr()](https://leetcode-cn.com/problems/implement-strstr/)[困难]KMP

```csharp
public int StrStr(string s1, string s2) {
    if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2)) return -1;

    var str1 = s1.ToCharArray();
    var str2 = s2.ToCharArray();
    var x = 0;
    var y = 0;
    // O(M) m <= n
    var next = GetNextArray(str2);
    // O(N)
    while (x < str1.Length && y < str2.Length)
        if (str1[x] == str2[y])
        {
            x++;
            y++;
        }
    else if (next[y] == -1)
    {
        // y == 0
        x++;
    }
    else
    {
        y = next[y];
    }

    return y == str2.Length ? x - y : -1;
}
private static int[] GetNextArray(char[] str2)
{
    if (str2.Length == 1) return [-1];

    var next = new int[str2.Length];
    next[0] = -1;
    next[1] = 0;
    var i = 2; // 目前在哪个位置上求next数组的值
    var cn = 0; // 当前是哪个位置的值再和i-1位置的字符比较
    while (i < next.Length)
        if (str2[i - 1] == str2[cn])
            // 配成功的时候
            next[i++] = ++cn;
    else if (cn > 0)
        cn = next[cn];
    else
        next[i++] = 0;

    return next;
}
```

028、[两数相除](https://leetcode-cn.com/problems/divide-two-integers/)[中等]

```csharp
// 加法: 无进位相加 + 进位信息 
public int Add(int a, int b)
{
    int carrylessSum = a;
    while (b != 0)
    {
        carrylessSum = a ^ b; // 无进位相加 
        b = (a & b) << 1; // 进位信息 
        a = carrylessSum;
    }
    return carrylessSum;
}

// 负数: 补码 (取反加一)
public int NegNum(int n)
{
    return ~n + 1;
}

// 减法 
public int Minus(int a, int b)
{
    return Add(a, NegNum(b));
}

// 乘法 
public int Multiply(int a, int b)
{
    int result = 0;
    while (b != 0)
    {
        if ((b & 1) != 0)
        {
            result = Add(result, a);
        }
        a <<= 1; // 左移 
        b >>= 1; // 右移 
    }
    return result;
}

// 判断是否为负数 
public bool IsNeg(int n)
{
    return n < 0;
}

// 除法 
public int Div(int a, int b)
{
    int x = IsNeg(a) ? NegNum(a) : a;
    int y = IsNeg(b) ? NegNum(b) : b;
    int result = 0;
    for (int i = 30; i >= 0; i = Minus(i, 1))
    {
        if ((x >> i) >= y)
        {
            result = Add(result, 1 << i); // 将 1 << i 加到 result 上 
            x = Minus(x, y << i);
        }
    }
    return IsNeg(a) ^ IsNeg(b) ? NegNum(result) : result;
}

// 除法 (处理边界条件)
public int Divide(int dividend, int divisor)
{
    if (dividend == int.MinValue && divisor == -1)
    {
        return int.MaxValue; // 直接返回 int.MaxValue 
    }
    else if (divisor == int.MinValue)
    {
        return 0;
    }
    else if (dividend == int.MinValue)
    {
        int ans = Div(Add(dividend, 1), divisor);
        return Add(ans, Div(Minus(dividend, Multiply(ans, divisor)), divisor));
    }
    else
    {
        return Div(dividend, divisor);
    }
}

//方法二
public int Divide(int dividend, int divisor) {
    // 考虑被除数为最小值的情况
    if (dividend == int.MinValue) {
        if (divisor == 1) {
            return int.MinValue;
        }
        if (divisor == -1) {
            return int.MaxValue;
        }
    }
    // 考虑除数为最小值的情况
    if (divisor == int.MinValue) {
        return dividend == int.MinValue ? 1 : 0;
    }
    // 考虑被除数为 0 的情况
    if (dividend == 0) {
        return 0;
    }

    // 一般情况，使用二分查找
    // 将所有的正数取相反数，这样就只需要考虑一种情况
    bool rev = false;
    if (dividend > 0) {
        dividend = -dividend;
        rev = !rev;
    }
    if (divisor > 0) {
        divisor = -divisor;
        rev = !rev;
    }

    int left = 1, right = int.MaxValue, ans = 0;
    while (left <= right) {
        // 注意溢出，并且不能使用除法
        int mid = left + ((right - left) >> 1);
        bool check = quickAdd(divisor, mid, dividend);
        if (check) {
            ans = mid;
            // 注意溢出
            if (mid == int.MaxValue) {
                break;
            }
            left = mid + 1;
        } else {
            right = mid - 1;
        }
    }

    return rev ? -ans : ans;
}

// 快速乘
public bool quickAdd(int y, int z, int x) {
    // x 和 y 是负数，z 是正数
    // 需要判断 z * y >= x 是否成立
    int result = 0, add = y;
    while (z != 0) {
        if ((z & 1) != 0) {
            // 需要保证 result + add >= x
            if (result < x - add) {
                return false;
            }
            result += add;
        }
        if (z != 1) {
            // 需要保证 add + add >= x
            if (add < x - add) {
                return false;
            }
            add += add;
        }
        // 不能使用除法
        z >>= 1;
    }
    return true;
}
```

029、[搜索旋转排序数组](https://leetcode-cn.com/problems/search-in-rotated-sorted-array)[中等]

```csharp
public int Search(int[] arr, int num) {
    var l = 0;
    var r = arr.Length - 1;
    while (l <= r)
    {
        // M = L + ((R - L) >> 1)
        var m = (l + r) / 2;
        if (arr[m] == num) return m;
        // arr[M] != num
        // [L] == [M] == [R] != num 无法二分
        if (arr[l] == arr[m] && arr[m] == arr[r])
        {
            while (l != m && arr[l] == arr[m]) l++;
            // 1) L == M L...M 一路都相等
            // 2) 从L到M终于找到了一个不等的位置
            if (l == m)
            {
                // L...M 一路都相等
                l = m + 1;
                continue;
            }
        }

        // ...
        // arr[M] != num
        // [L] [M] [R] 不都一样的情况, 如何二分的逻辑
        if (arr[l] != arr[m])
        {
            if (arr[m] > arr[l])
            {
                // L...M 一定有序
                if (num >= arr[l] && num < arr[m])
                    //  3  [L] == 1    [M]   = 5   L...M - 1
                    r = m - 1;
                else
                    // 9    [L] == 2    [M]   =  7   M... R
                    l = m + 1;
            }
            else
            {
                // [L] > [M]    L....M  存在断点
                if (num > arr[m] && num <= arr[r])
                    l = m + 1;
                else
                    r = m - 1;
            }
        }
        else
        {
            // [L] [M] [R] 不都一样，  [L] === [M] -> [M]!=[R]
            if (arr[m] < arr[r])
            {
                if (num > arr[m] && num <= arr[r])
                    l = m + 1;
                else
                    r = m - 1;
            }
            else
            {
                if (num >= arr[l] && num < arr[m])
                    r = m - 1;
                else
                    l = m + 1;
            }
        }
    }

    return -1;
}
```

030、[在排序数组中查找元素的第一个和最后一个位置](https://leetcode-cn.com/problems/find-first-and-last-position-of-element-in-sorted-array/)[中等]

```csharp
public  int[] SearchRange(int[]? numbers, int target)
{
    if (numbers == null || numbers.Length == 0) return [-1, -1];
    var l = LessMostRight(numbers, target) + 1;
    if (l == numbers.Length || numbers[l] != target) return [-1, -1];
    return new[] { l, LessMostRight(numbers, target + 1) };
}

private  int LessMostRight(int[] arr, int num)
{
    var l = 0;
    var r = arr.Length - 1;
    var ans = -1;
    while (l <= r)
    {
        var m = l + ((r - l) >> 1);
        if (arr[m] < num)
        {
            ans = m;
            l = m + 1;
        }
        else
        {
            r = m - 1;
        }
    }

    return ans;
}
```

031、[有效的数独](https://leetcode-cn.com/problems/valid-sudoku/)[中等]

```csharp
public bool IsValidSudoku(char[][] board) {
    var row = new bool[9, 10];
    var col = new bool[9, 10];
    var bucket = new bool[9, 10];
    for (var i = 0; i < 9; i++)
        for (var j = 0; j < 9; j++)
        {
            var bid = 3 * (i / 3) + j / 3;
            if (board[i][ j] != '.')
            {
                var num = board[i][ j] - '0';
                if (row[i, num] || col[j, num] || bucket[bid, num]) return false;
                row[i, num] = true;
                col[j, num] = true;
                bucket[bid, num] = true;
            }
        }

    return true;
}
```

032、[解数独](https://leetcode-cn.com/problems/sudoku-solver/)[困难]

```csharp
public void SolveSudoku(char[][] board) {
    var row = new bool[9, 10];
    var col = new bool[9, 10];
    var bucket = new bool[9, 10];
    InitMaps(board, row, col, bucket);
    Process(board, 0, 0, row, col, bucket);
}

private static void InitMaps(char[][] board, bool[,] row, bool[,] col, bool[,] bucket)
{
    for (var i = 0; i < 9; i++)
        for (var j = 0; j < 9; j++)
        {
            var bid = 3 * (i / 3) + j / 3;
            if (board[i][j] != '.')
            {
                var num = board[i][j] - '0';
                row[i, num] = true;
                col[j, num] = true;
                bucket[bid, num] = true;
            }
        }
}

//  当前来到(i,j)这个位置，如果已经有数字，跳到下一个位置上
//                      如果没有数字，尝试1~9，不能和row、col、bucket冲突
private static bool Process(char[][] board, int i, int j, bool[,] row, bool[,] col, bool[,] bucket)
{
    if (i == 9) return true;
    // 当离开(i，j)，应该去哪？(nexti, nextj)
    var nexti = j != 8 ? i : i + 1;
    var nextj = j != 8 ? j + 1 : 0;
    if (board[i][j] != '.') return Process(board, nexti, nextj, row, col, bucket);

    // 可以尝试1~9
    var bid = 3 * (i / 3) + j / 3;
    for (var num = 1; num <= 9; num++)
        // 尝试每一个数字1~9
        if (!row[i, num] && !col[j, num] && !bucket[bid, num])
        {
            // 可以尝试num
            row[i, num] = true;
            col[j, num] = true;
            bucket[bid, num] = true;
            board[i][j] = (char)(num + '0');
            if (Process(board, nexti, nextj, row, col, bucket)) return true;
            row[i, num] = false;
            col[j, num] = false;
            bucket[bid, num] = false;
            board[i][j] = '.';
        }

    return false;
}
```

038[外观数列](https://leetcode-cn.com/problems/count-and-say/)[简单]

```csharp
public string CountAndSay(int n) {
    if (n < 1) return "";
    if (n == 1) return "1";
    var last = CountAndSay(n - 1).ToCharArray();
    var ans = new StringBuilder();
    var times = 1;
    for (var i = 1; i < last.Length; i++)
        if (last[i - 1] == last[i])
        {
            times++;
        }
    else
    {
        ans.Append(times);
        ans.Append(last[i - 1]);
        times = 1;
    }

    ans.Append(times);
    ans.Append(last[^1]);
    return ans.ToString();
}
```

039[缺失的第一个正数](https://leetcode-cn.com/problems/first-missing-positive/)[困难]

```csharp
public int FirstMissingPositive(int[] arr)
{
    // l是盯着的位置
    // 0 ~ L-1有效区
    var l = 0;
    var r = arr.Length;
    while (l != r)
        if (arr[l] == l + 1)
            l++;
    else if (arr[l] <= l || arr[l] > r || arr[arr[l] - 1] == arr[l])
        // 垃圾的情况
        Swap(arr, l, --r);
    else
        Swap(arr, l, arr[l] - 1);
    return l + 1;
}

private void Swap(int[] arr, int i, int j)
{
    (arr[i], arr[j]) = (arr[j], arr[i]);
}
```

040[接雨水](https://leetcode-cn.com/problems/trapping-rain-water/)[困难]

```csharp
public int Trap(int[]? arr)
{
    if (arr == null || arr.Length < 2) return 0;
    var n = arr.Length;
    var l = 1;
    var leftMax = arr[0];
    var r = n - 2;
    var rightMax = arr[n - 1];
    var water = 0;
    while (l <= r)
        if (leftMax <= rightMax)
        {
            water += Math.Max(0, leftMax - arr[l]);
            leftMax = Math.Max(leftMax, arr[l++]);
        }
    else
    {
        water += Math.Max(0, rightMax - arr[r]);
        rightMax = Math.Max(rightMax, arr[r--]);
    }

    return water;
}
```

# 当前标记

041[通配符匹配](https://leetcode-cn.com/problems/wildcard-matching/)[困难]

```csharp
```

055[跳跃游戏](https://leetcode-cn.com/problems/jump-game/)[中等]

```csharp
```

056[跳跃游戏II](https://leetcode-cn.com/problems/jump-game-ii/)[困难]

```csharp
```

057[全排列](https://leetcode-cn.com/problems/permutations/)[中等]

```csharp
```

687、[最长同值路径](https://leetcode-cn.com/problems/longest-univalue-path/)[简单]

```csharp
```

688、[旋转图像](https://leetcode-cn.com/problems/rotate-image/)[中等]

```csharp
```

689、[字母异位词分组](https://leetcode-cn.com/problems/group-anagrams/)[中等]

```csharp
```

690、[Pow(x,n)](https://leetcode-cn.com/problems/powx-n/)[中等]

```csharp
```

691、[N皇后问题|N皇后](https://leetcode-cn.com/problems/n-queens/)[困难]

```csharp
```

-带分数和

053[最大子序和](https://leetcode-cn.com/problems/maximum-subarray/)[简单]

```csharp
```

054[不同路径](https://leetcode-cn.com/problems/unique-paths)[中等]

```csharp
```

056[合并区间](https://leetcode-cn.com/problems/merge-intervals/)[中等]

```csharp
```

066[加一](https://leetcode-cn.com/problems/plus-one/)[简单]

```csharp
```

069[x的平方根](https://leetcode-cn.com/problems/sqrtx/)[简单]

```csharp
```

070[爬楼梯](https://leetcode-cn.com/problems/climbing-stairs/)[简单]

```csharp
```

073[矩阵置零](https://leetcode-cn.com/problems/set-matrix-zeroes/)[中等]

```csharp
```

076[最小覆盖子串](https://leetcode-cn.com/problems/minimum-window-substring/)[困难]

```csharp
```

078[子集](https://leetcode-cn.com/problems/subsets/)[中等]

```csharp
```

079[单词搜索](https://leetcode-cn.com/problems/word-search/)[中等]

```csharp
```

084[柱状图中最大的矩形](https://leetcode-cn.com/problems/largest-rectangle-in-histogram/)[困难]

```csharp
```

088[合并两个有序数组](https://leetcode-cn.com/problems/merge-sorted-array/)[简单]

```csharp
```

091[解码方法](https://leetcode-cn.com/problems/decode-ways/)[中等]

```csharp
```

094[二叉树的中序遍历](https://leetcode-cn.com/problems/binary-tree-inorder-traversal/)[中等]

```csharp
```

098[验证二叉搜索树](https://leetcode-cn.com/problems/validate-binary-search-tree/)[中等]

```csharp
```

101、[对称二叉树](https://leetcode-cn.com/problems/symmetric-tree/)[简单]

```csharp
```

102、[二叉树的层序遍历](https://leetcode-cn.com/problems/binary-tree-level-order-traversal)[中等]

```csharp
```

103、[二叉树的锯齿形层次遍历](https://leetcode-cn.com/problems/binary-tree-zigzag-level-order-traversal)[中等]

```csharp
```

104、[二叉树的最大深度](https://leetcode-cn.com/problems/maximum-depth-of-binary-tree)[简单]

```csharp
```

105、[从前序与中序遍历序列构造二叉树](https://leetcode-cn.com/problems/construct-binary-tree-from-preorder-and-inorder-traversal)[中等]

```csharp
```

108、[将有序数组转换为二叉搜索树](https://leetcode-cn.com/problems/convert-sorted-array-to-binary-search-tree)[简单]

```csharp
```

116、[填充每个节点的下一个右侧节点指针](https://leetcode-cn.com/problems/populating-next-right-pointers-in-each-node)[中等]

```csharp
```

118、[杨辉三角](https://leetcode-cn.com/problems/pascals-triangle)[简单]

```csharp
```

一种高效的insert跟get结构
121、[买卖股票的最佳时机](https://leetcode-cn.com/problems/best-time-to-buy-and-sell-stock)[简单]

```csharp
```

122、[买卖股票的最佳时机II](https://leetcode-cn.com/problems/best-time-to-buy-and-sell-stock-ii)[简单]

```csharp
```

188、[买卖股票的最佳时机IV](https://leetcode-cn.com/problems/best-time-to-buy-and-sell-stock-iv)[困难]

```csharp
```

309、[最佳买卖股票时机含冷冻期](https://leetcode-cn.com/problems/best-time-to-buy-and-sell-stock-with-cooldown)[中等]

```csharp
```

124、[二叉树中的最大路径和](https://leetcode-cn.com/problems/binary-tree-maximum-path-sum)[困难]

```csharp
```

125、[验证回文串](https://leetcode-cn.com/problems/valid-palindrome)[简单]

```csharp
```

127、[单词接龙](https://leetcode-cn.com/problems/word-ladder)[中等]

```csharp
```

128、[最长连续序列](https://leetcode-cn.com/problems/longest-consecutive-sequence)[困难]

```csharp
```

130、[被围绕的区域](https://leetcode-cn.com/problems/surrounded-regions)[中等]

```csharp
```

131、[分割回文串](https://leetcode-cn.com/problems/palindrome-partitioning)[中等]

```csharp
```

134、[加油站](https://leetcode-cn.com/problems/gas-station)[中等]

```csharp
```

136、[只出现一次的数字](https://leetcode-cn.com/problems/single-number)[简单]

```csharp
```

138、[深度复制带有rand指针的链表](https://leetcode-cn.com/problems/copy-list-with-random-pointer)[中等]

```csharp
```

139、[单词拆分](https://leetcode-cn.com/problems/word-break)[中等]

```csharp
```

140、[单词拆分II](https://leetcode-cn.com/problems/word-break-ii)[困难]

```csharp
```

141、[环形链表](https://leetcode-cn.com/problems/linked-list-cycle)[简单]

```csharp
```

146、[LRU缓存机制](https://leetcode-cn.com/problems/lru-cache)[中等]

```csharp
```

148、[排序链表](https://leetcode-cn.com/problems/sort-list)[中等]

```csharp
```

149、[直线上最多的点数](https://leetcode-cn.com/problems/max-points-on-a-line)[困难]

```csharp
```

150、[逆波兰表达式求值](https://leetcode-cn.com/problems/evaluate-reverse-polish-notation)[中等]

```csharp
```

152、[乘积最大子数组](https://leetcode-cn.com/problems/maximum-product-subarray)[中等]

```csharp
```

155、[最小栈](https://leetcode-cn.com/problems/min-stack)[简单]

```csharp
```

160、[相交链表](https://leetcode-cn.com/problems/intersection-of-two-linked-lists)[简单]

```csharp
```

162、[寻找峰值](https://leetcode-cn.com/problems/find-peak-element)[中等]

```csharp
```

163、[缺失的区间](https://leetcode-cn.com/problems/missing-ranges)[中等]

```csharp
```

166、[分数到小数](https://leetcode-cn.com/problems/fraction-to-recurring-decimal)[中等]

```csharp
```

169、[多数元素](https://leetcode-cn.com/problems/majority-element)[简单]

```csharp
```

190、[200、★算法题目汇总★/★算法面试题汇总/颠倒二进制位](https://leetcode-cn.com/problems/reverse-bits)[简单]

```csharp
```

171、[Excel表列序号](https://leetcode-cn.com/problems/excel-sheet-column-number)[简单]

```csharp
```

172、[阶乘后的零](https://leetcode-cn.com/problems/factorial-trailing-zeroes)[简单]

```csharp
```

179、[最大数](https://leetcode-cn.com/problems/largest-number)[中等]

```csharp
```

189、[旋转数组](https://leetcode-cn.com/problems/rotate-array)[简单]

```csharp
```

191、[位1的个数](https://leetcode-cn.com/problems/number-of-1-bits)[简单]

```csharp
```

198、[打家劫舍](https://leetcode-cn.com/problems/house-robber)[简单]

```csharp
```

200、[岛屿数量](https://leetcode-cn.com/problems/number-of-islands)[中等]

```csharp
```

202、[快乐数](https://leetcode-cn.com/problems/happy-number)[简单]

```csharp
```

204、[计数质数](https://leetcode-cn.com/problems/count-primes)[简单]

```csharp
```

206、[反转链表](https://leetcode-cn.com/problems/reverse-linked-list)[简单]

```csharp
```

207、[课程表](https://leetcode-cn.com/problems/course-schedule)[中等]

```csharp
```

208、[实现Trie(前缀树)](https://leetcode-cn.com/problems/implement-trie-prefix-tree)[中等]

```csharp
```

210、[课程表II](https://leetcode-cn.com/problems/course-schedule-ii)[中等]

```csharp
```

212、[单词搜索II](https://leetcode-cn.com/problems/word-search-ii)[困难]

```csharp
```

215、[数组中的第K个最大元素](https://leetcode-cn.com/problems/kth-largest-element-in-an-array)[中等]

```csharp
```

217、[存在重复元素](https://leetcode-cn.com/problems/contains-duplicate)[简单]

```csharp
```

218、[天际线问题](https://leetcode-cn.com/problems/the-skyline-problem)[困难]

```csharp
```

补充：
推荐饭店问题

227、[基本计算器II](https://leetcode-cn.com/problems/basic-calculator-ii)[中等]

```csharp
```

772、[基本计算器III](https://leetcode-cn.com/problems/basic-calculator-iii/)[困难]

```csharp
```

230、[二叉搜索树中第K小的元素](https://leetcode-cn.com/problems/kth-smallest-element-in-a-bst)[中等]

```csharp
```

234、[回文链表](https://leetcode-cn.com/problems/palindrome-linked-list)[简单]

```csharp
```

236、[二叉树的最近公共祖先](https://leetcode-cn.com/problems/lowest-common-ancestor-of-a-binary-tree)[中等]

```csharp
```

237、[删除链表中的节点](https://leetcode-cn.com/problems/delete-node-in-a-linked-list)[简单]

```csharp
```

238、[除自身以外数组的乘积](https://leetcode-cn.com/problems/product-of-array-except-self)[中等]

```csharp
```

239、[滑动窗口最大值](https://leetcode-cn.com/problems/sliding-window-maximum)[困难]

```csharp
```

240、[搜索二维矩阵II](https://leetcode-cn.com/problems/search-a-2d-matrix-ii)[中等]

```csharp
```

242、[有效的字母异位词](https://leetcode-cn.com/problems/valid-anagram)[简单]

```csharp
```

251、[展开二维向量](https://leetcode-cn.com/problems/flatten-2d-vector)[中等]

```csharp
```

253、[会议室II](https://leetcode-cn.com/problems/meeting-rooms-ii)[中等]

```csharp
```

268、[缺失数字](https://leetcode-cn.com/problems/missing-number)[简单]

```csharp
```

300、[最长递增子序列](https://leetcode-cn.com/problems/longest-increasing-subsequence)[中等]

```csharp
```

673、[最长递增子序列的个数](https://leetcode-cn.com/problems/number-of-longest-increasing-subsequence/)[中等]

```csharp
```

277、[搜寻名人](https://leetcode-cn.com/problems/find-the-celebrity)[中等]

```csharp
```

287、[寻找重复数](https://leetcode-cn.com/problems/find-the-duplicate-number)[中等]

```csharp
```

269、[火星词典](https://leetcode-cn.com/problems/alien-dictionary)[困难]

```csharp
```

279、[完全平方数](https://leetcode-cn.com/problems/perfect-squares)[中等]

```csharp
```

283、[移动零](https://leetcode-cn.com/problems/move-zeroes)[简单]

```csharp
```

285、[二叉搜索树中的顺序后继](https://leetcode-cn.com/problems/inorder-successor-in-bst)[中等]

```csharp
```

289、[生命游戏](https://leetcode-cn.com/problems/game-of-life)[中等]

```csharp
```

295、[数据流的中位数](https://leetcode-cn.com/problems/find-median-from-data-stream)[困难]

```csharp
```

297、[二叉树的序列化与反序列化](https://leetcode-cn.com/problems/serialize-and-deserialize-binary-tree)[困难]

```csharp
```

300、[最长递增子序列](https://leetcode-cn.com/problems/longest-increasing-subsequence)[中等]

```csharp
```

308、[二维区域和检索-可变](https://leetcode-cn.com/problems/range-sum-query-2d-mutable)[困难]

```csharp
```

326、[3的幂](https://leetcode-cn.com/problems/power-of-three)[简单]

```csharp
```

344、[反转字符串](https://leetcode-cn.com/problems/reverse-string)[简单]

```csharp
```

454、[四数相加II](https://leetcode-cn.com/problems/4sum-ii)[中等]

```csharp
```

315、[计算右侧小于当前元素的个数](https://leetcode-cn.com/problems/count-of-smaller-numbers-after-self)[困难]

```csharp
```

322、[零钱兑换](https://leetcode-cn.com/problems/coin-change)[中等]

```csharp
```

324、[摆动排序II](https://leetcode-cn.com/problems/wiggle-sort-ii)[中等]

```csharp
```

328、[奇偶链表](https://leetcode-cn.com/problems/odd-even-linked-list)[中等]

```csharp
```

329、[矩阵中的最长递增路径](https://leetcode-cn.com/problems/longest-increasing-path-in-a-matrix)[困难]

```csharp
```

334、[递增的三元子序列](https://leetcode-cn.com/problems/increasing-triplet-subsequence)[中等]

```csharp
```

340、[至多包含K个不同字符的最长子串](https://leetcode-cn.com/problems/longest-substring-with-at-most-k-distinct-characters)[困难]

```csharp
```

341、[扁平化嵌套列表迭代器](https://leetcode-cn.com/problems/flatten-nested-list-iterator)[中等]

```csharp
```

347、[前K个高频元素](https://leetcode-cn.com/problems/top-k-frequent-elements)[中等]

```csharp
```

348、[判定井字棋胜负](https://leetcode-cn.com/problems/design-tic-tac-toe)[中等]

```csharp
```

350、[两个数组的交集II](https://leetcode-cn.com/problems/intersection-of-two-arrays-ii)[简单]

```csharp
```

371、[两整数之和](https://leetcode-cn.com/problems/sum-of-two-integers)[简单]

```csharp
```

378、[有序矩阵中第K小的元素](https://leetcode-cn.com/problems/kth-smallest-element-in-a-sorted-matrix)[中等]

```csharp
```

380、[常数时间插入、删除和获取随机元素](https://leetcode-cn.com/problems/insert-delete-getrandom-o1)[中等]

```csharp
```

384、[打乱数组](https://leetcode-cn.com/problems/shuffle-an-array)[中等]

```csharp
```

387、[字符串中的第一个唯一字符](https://leetcode-cn.com/problems/first-unique-character-in-a-string)[简单]

```csharp
```

395、[至少有K个重复字符的最长子串](https://leetcode-cn.com/problems/longest-substring-with-at-least-k-repeating-characters)[中等]

```csharp
```

412、[FizzBuzz](https://leetcode-cn.com/problems/fizz-buzz)[简单]

```csharp
```

#近期面试题讲解

-买饮料
-司机调度
-新手游游的数组难题
-扑克牌问题
-棋盘染色问题

# 1017(下午)

32.[[最长有效括号]] [困难]  
https://leetcode-cn.com/problems/longest-valid-parentheses/

- [[只出现两次的数]]

# 1018(上午)

[[一个数组中有一种数出现K次，其他数都出现了M次]]

39.[[组合总和]] [M]  
https://leetcode-cn.com/problems/combination-sum/

64.[[矩阵中的最小路径和|最小路径和]] [M]  
https://leetcode-cn.com/problems/minimum-path-sum/

114.[[二叉树展开为链表]] [M]  
https://leetcode-cn.com/problems/flatten-binary-tree-to-linked-list/

221.[[最大正方形]] [M]  
https://leetcode-cn.com/problems/maximal-square/

226.[[翻转二叉树]] [E]  
https://leetcode-cn.com/problems/invert-binary-tree/

337.[[打家劫舍 III]] [M]  
https://leetcode-cn.com/problems/house-robber-iii/

# 1018(下午)

394.[[字符串解码]] [M]  
https://leetcode-cn.com/problems/decode-string/

406.[[根据身高重建队列]] [M]  
https://leetcode-cn.com/problems/queue-reconstruction-by-height/

416.[[分割等和子集]]  [M]  
https://leetcode-cn.com/problems/partition-equal-subset-sum/

437.[[路径总和 III]]  [M]  
https://leetcode-cn.com/problems/path-sum-iii/

560.[[和为K的子数组]] [M]   
https://leetcode-cn.com/problems/subarray-sum-equals-k/

438.[[找到字符串中所有字母异位词]] [M]  
https://leetcode-cn.com/problems/find-all-anagrams-in-a-string/

# 1101(上午)

[[最少按几次开关才能点亮所有的灯]]

# 1101(下午)


448.[[找到所有数组中消失的数字]] [简单]  
https://leetcode-cn.com/problems/find-all-numbers-disappeared-in-an-array

494.[[目标和]] [中等]  
https://leetcode-cn.com/problems/target-sum

543.[[二叉树的直径]] [E]  
https://leetcode-cn.com/problems/diameter-of-binary-tree/

581.[[最短无序连续子数组]] [中等]  
https://leetcode-cn.com/problems/shortest-unsorted-continuous-subarray

617.[[合并二叉树]] [E]  
https://leetcode-cn.com/problems/merge-two-binary-trees

739.[[每日温度]] [M]  
https://leetcode-cn.com/problems/daily-temperatures

621.[[任务调度器]] [M]  
https://leetcode-cn.com/problems/task-scheduler

296.[[最佳的碰头地点]] [H]  
https://leetcode-cn.com/problems/best-meeting-point/

# leetcode高频题

说明：

1. 算法新手班对应"0入门"文件夹
2. 体系学习班对应"2学习"文件夹
3. 大厂刷题班对应"3算题"文件夹

本节课解决leetcode高频题列表中的如下题目:

* 0001:大厂刷题班, 第28节, 本节
* 0002:算法新手班, 第4节第5题@01:34:34
* 0003:大厂刷题班, 第3节第1题
* 0004:大厂刷题班, 第12节第3题
* 0005:体系学习班, Manacher算法
* 0007:大厂刷题班, 第27节, 本节

# leetcode高频题

本节课解决leetcode高频题列表中的如下题目 :

* 0008 : 大厂刷题班, 第28节, 本节
* 0010 : 大厂刷题班, 第12节第4题
* 0011 : 大厂刷题班, 第8节第2题
* 0012 : 大厂刷题班, 第28节, 本节
* 0013 : 大厂刷题班, 第28节, 本节
* 0014 : 大厂刷题班, 第28节, 本节
* 0015 : 大厂刷题班, 第25节第2题
* 0017 : 大厂刷题班, 第28节, 本节
* 0019 : 大厂刷题班, 第28节, 本节
* 0020 : 大厂刷题班, 第28节, 本节
* 0021 : 算法新手班, 第4节第6题
* 0022 : 大厂刷题班, 第28节, 本节
* 0023 : 算法新手班, 第6节第1题
* 0026 : 大厂刷题班, 第28节, 本节
* 0028 : 体系学习班, KMP算法
* 0029 : 算法新手班, 第5节第3题
* 0034 : 大厂刷题班, 第28节, 本节
* 0036 : 大厂刷题班, 第28节, 本节
* 0037 : 大厂刷题班, 第28节, 本节
* 0038 : 大厂刷题班, 第28节, 本节
* 0041 : 大厂刷题班, 第14节第6题
* 0042 : 大厂刷题班, 第22节第2题
* 0044 : 大厂刷题班, 第12节第4题
* 0045 : 大厂刷题班, 第10节第1题
* 0046 : 体系学习班, KMP算法
* 0048 : 体系学习班, 第40节第6题
* 0049 : 大厂刷题班, 第28节, 本节

# leetcode高频题

本节课解决leetcode高频题列表中的如下题目 :

* 0033 : 大厂刷题班, 第29节, 本节
* 0050 : 大厂刷题班, 第29节, 本节
* 0053 : 体系学习班, 第40节第2题
* 0054 : 体系学习班, 第40节第5题
* 0055 : 大厂刷题班, 第10节第1题
* 0056 : 大厂刷题班, 第29节, 本节
* 0062 : 大厂刷题班, 第29节, 本节
* 0066 : 大厂刷题班, 第29节, 本节
* 0069 : 大厂刷题班, 第29节, 本节
* 0070 : 体系学习班, 第26节第2题
* 0073 : 大厂刷题班, 第29节, 本节
* 0075 : 体系学习班, 第5节第2题, 快排中的荷兰国旗问题
* 0076 : 大厂刷题班, 第24节第5题
* 0078 : 体系学习班, 第17节题目3, 生成子序列问题和本题一样的

# leetcode高频题

本节课解决leetcode高频题列表中的如下题目 :

* 0079 : 大厂刷题班, 第30节, 本节
* 0084 : 体系学习班, 第25节第3题
* 0088 : 大厂刷题班, 第30节, 本节
* 0091 : 体系学习班, 第19节第2题
* 0639 : 本题不再高频题列表中, 但本题是0091的难度加强题, 相似度很强, 大厂刷题班, 第30节, 本节
* 0094 : 体系学习班, 第30节第1题, Morris遍历
* 0098 : 大厂刷题班, 第30节, 本节
* 0101 : 大厂刷题班, 第30节, 本节
* 0102 : 算法新手班, 第7节第1题
* 0103 : 大厂刷题班, 第30节, 本节
* 0104 : 太简单了, 体系学习班, 二叉树的递归套路、Morris遍历都可以做, 跳过
* 0105 : 算法新手班, 第6节第5题
* 0108 : 大厂刷题班, 第30节, 本节
* 0116 : 大厂刷题班, 第30节, 本节
* 0118 : 大厂刷题班, 第30节, 本节
* 0119 : 本题不在高频题列表中，但和0118类似, 大厂刷题班, 第30节, 本节
* 0121 : 大厂刷题班, 第15节第1题
* 0122 : 大厂刷题班, 第15节第2题
* 0123 : 大厂刷题班, 第15节第3题
* 0124 : 大厂刷题班, 第30节, 本节

# leetcode高频题

本节课解决leetcode高频题列表中的如下题目 :

* 0125:大厂刷题班, 第31节, 本节
* 0127:大厂刷题班, 第31节, 本节
* 0128:大厂刷题班, 第12节第3题
* 0130:大厂刷题班, 第31节, 本节
* 0131:大厂刷题班, 第11节第2题
* 0134:体系学习班, 第24节第3题 & 大厂刷题班, 第25节第4题
* 0136:体系学习班, 第2节第2题
* 0138:体系学习班, 第9节第4题
* 0139:大厂刷题班, 第31节, 本节
* 0140:大厂刷题班, 第31节, 本节
* 0141:体系学习班, 第10节第1题
* 0146:大厂刷题班, 第19节第1题
* 0148:大厂刷题班, 第31节, 本节
* 0149:大厂刷题班, 第25节第3题
* 0150:大厂刷题班, 第31节, 本节

# leetcode高频题

本节课解决leetcode高频题列表中的如下题目 :

* 0152:大厂刷题班, 第32节, 本节
* 0155:体系学习班, 第3节第5题
* 0160:体系学习班, 第10节第1题
* 0162:体系学习班, 第1节第6题
* 0163:大厂刷题班, 第32节, 本节
* 0166:大厂刷题班, 第32节, 本节
* 0169:大厂刷题班, 第23节第4题
* 0171:大厂刷题班, 第32节, 本节
* 0172:大厂刷题班, 第32节, 本节
* 0179:体系学习班, 第13节第5题
* 0188:大厂刷题班, 第15节第4题
* 0189:大厂刷题班, 第32节, 本节
* 0190:大厂刷题班, 第32节, 本节
* 0191:大厂刷题班, 第32节, 本节
* 0198:大厂刷题班, 第4节第4题
* 0200:体系学习班, 第15节第2题、第3题
* 0202:大厂刷题班, 第32节, 本节
* 0204:大厂刷题班, 第32节, 本节
* 补充题 SequenceM:拼多多, 卷子合法数量问题

# leetcode高频题

本节课解决leetcode高频题列表中的如下题目 :

* 0206:太简单，跳过
* 0207:大厂刷题班, 第33节, 本节
* 0208:体系学习班, 第8节第1题
* 0210:大厂刷题班, 第33节, 本节
* 0212:大厂刷题班, 第26节第1题
* 0213:大厂刷题班, 第33节, 本节
* 0215:体系学习班, 第29节第1题
* 0217:太简单，跳过
* 0218:大厂刷题班, 第4节第8题
* 0227:大厂刷题班, 第8节第1题
* 0230:太简单了，跳过。二叉树基本遍历、Morris遍历都可以解决
* 0234:体系学习班, 第9节第2题
* 0236:体系学习班, 第13节第3题 扩展在 大厂刷题班, 第23节第1题
* 0237:大厂刷题班, 第33节, 本节
* 0238:大厂刷题班, 第33节, 本节
* 0239:体系学习班, 第24节第1题
* 0240:大厂刷题班, 第17节第1题
* 0242:大厂刷题班, 第33节, 本节
* 0251:大厂刷题班, 第33节, 本节
* 0253:体系学习班, 第14节第3题
* 0268:大厂刷题班, 第14节第6题
* 0269:大厂刷题班, 第33节, 本节
* 0277:大厂刷题班, 第33节, 本节
* 0279:大厂刷题班, 第33节, 本节
* 0283:大厂刷题班, 第33节, 本节
* 0285:太简单了，跳过。用二叉树普通遍历或者Morris遍历都可以解决

# leetcode高频题

本节课解决leetcode高频题列表中的如下题目 :

* 0287:大厂刷题班, 第34节, 本节
* 0289:大厂刷题班, 第34节, 本节
* 0295:大厂刷题班, 第34节, 本节
* 0297:体系学习班, 第11节第2题
* 0300:大厂刷题班, 第9节第3题
* 0308:体系学习班, 第32节第2题
* 0309:大厂刷题班, 第15节第5题
* 0315:大厂刷题班, 第34节, 本节
* 0322:体系学习班, 硬币找零专题:第21节第2、3、4题, 第22节第2题, 第24节第4题
* 0324:大厂刷题班, 第34节, 本节
* 0326:大厂刷题班, 第34节, 本节
* 0328:大厂刷题班, 第34节, 本节
* 0329:大厂刷题班, 第1节第5题
* 0334:大厂刷题班, 第9节第3题的变形, 问:最长递增子序列长度能否超过2而已, 跳过
* 0340:大厂刷题班, 第34节, 本节
* 0341:大厂刷题班, 第34节, 本节
* 0344:太简单了, 跳过
* 0348:大厂刷题班, 第34节, 本节
* 0350:太简单了, 跳过
* 0371:算法新手班, 第5节第3题
* 0378:大厂刷题班, 第17节第2题
* 0380:大厂刷题班, 第34节, 本节
* 0384:大厂刷题班, 第34节, 本节
* 0387:太简单了, 跳过

# leetcode高频题

本节课解决leetcode高频题列表中的如下题目 :

* 0347:大厂刷题班, 第35节, 本节
* 0395:大厂刷题班, 第35节, 本节
* 0412:大厂刷题班, 第35节, 本节
* 0454:大厂刷题班, 第35节, 本节
* 0673:大厂刷题班, 第35节, 本节
* 0687:大厂刷题班, 第35节, 本节
* 0772:大厂刷题班, 第8节第1题

至此，Leetcode高频题系列完结

本节附加题

* Code01:2021年8月大厂真实笔试题
* Code02:2021年8月大厂真实笔试题
* Code03:2021年8月大厂真实笔试题
* Code04:2021年8月大厂真实笔试题
* Code05:2021年8月大厂真实笔试题

# leetcode最受欢迎100题

大厂刷题班27节~35节已经讲完leetcode高频题系列

leetcode高频题系列和leetcode最受欢迎100题系列题目有重合。以下为leetcode最受欢迎100题不和leetcode高频题重合的题号，其他的都重复了，不再讲述

* 0032:大厂刷题班, 第14节第1题
* 0039:体系学习班, 硬币找零专题:第21节第2、3、4题, 第22节第2题, 第24节第4题。本题就是无限张找零问题，不再重复讲述
* 0064:体系学习班, 第21节第1题
* 0072:大厂刷题班, 第5节第3题
* 0085:体系学习班, 第25节第4题
* 0096:体系学习班, 第39节第4题, 卡特兰数
* 0114:大厂刷题班, 第37节, 本节
* 0142:体系学习班, 第10节第1题
* 0221:大厂刷题班, 第37节, 本节
* 0226:大厂刷题班, 第37节, 本节
* 0337:体系学习班, 第13节, 第4题, 还是这道题的加强版(多叉树)
* 0338:和leetcode第191题重复, 大厂刷题班第30节讲过了
* 0394:大厂刷题班, 第37节, 本节
* 0406:大厂刷题班, 第37节, 本节
* 0416:体系学习班, 第23节第1题, 还是这道题的加强版
* 0437:大厂刷题班, 第37节, 本节

剩余题目在下一节

本节附加题

* code01:2021年8月大厂真实笔试题
* code02:2021年8月大厂真实笔试题

# leetcode最受欢迎100题

大厂刷题班27节~35节已经讲完leetcode高频题系列

* 0438:大厂刷题班, 第38节, 本节
* 0448:大厂刷题班, 第38节, 本节
* 0494:大厂刷题班, 第1节第7题
* 0543:体系学习班, 第12节第6题
* 0560:大厂刷题班, 第37节, Leetcode题目437与本题思路相同, 课上也讲了该题做法
* 0581:大厂刷题班, 第1节第6题
* 0617:大厂刷题班, 第38节, 本节
* 0621:大厂刷题班, 第38节, 本节
* 0647:大厂刷题班, 第38节, 本节
* 0739:大厂刷题班, 第38节, 本节
* 0763:大厂刷题班, 第38节, 本节

至此，leetcode最受欢迎100题系列完结

本节附加题

* code01:2021年8月大厂真实笔试题
* code02:2021年8月大厂真实笔试题
