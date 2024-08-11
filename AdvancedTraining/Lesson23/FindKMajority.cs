namespace AdvancedTraining.Lesson23;

public class FindKMajority
{
    private static void PrintHalfMajor(int[] arr)
    {
        var cand = 0;
        var hp = 0;
        foreach (var item in arr)
            if (hp == 0)
            {
                cand = item;
                hp = 1;
            }
            else if (item == cand)
            {
                hp++;
            }
            else
            {
                hp--;
            }

        if (hp == 0)
        {
            Console.WriteLine("no such number.");
            return;
        }

        hp = 0;
        foreach (var item in arr)
            if (item == cand)
                hp++;

        if (hp > arr.Length / 2)
            Console.WriteLine(cand);
        else
            Console.WriteLine("no such number.");
    }

    private static void PrintKMajor(int[] arr, int k)
    {
        if (k < 2)
        {
            Console.WriteLine("the value of K is invalid.");
            return;
        }

        // 攒候选，cands，候选表，最多K-1条记录！ > N / K次的数字，最多有K-1个
        var cands = new Dictionary<int, int>();
        for (var i = 0; i != arr.Length; i++)
            if (cands.ContainsKey(arr[i]))
            {
                cands[arr[i]] = cands[arr[i]] + 1;
            }
            else
            {
                // arr[i] 不是候选
                if (cands.Count == k - 1)
                    // 当前数肯定不要！，每一个候选付出1点血量，血量变成0的候选，要删掉！
                    AllCandsMinusOne(cands);
                else
                    cands[arr[i]] = 1;
            }
        // 所有可能的候选，都在cands表中！遍历一遍arr，每个候选收集真实次数


        var reals = getReals(arr, cands);
        var hasPrint = false;
        foreach (var set in cands)
        {
            var key = set.Key;
            if (reals[key] > arr.Length / k)
            {
                hasPrint = true;
                Console.Write(key + " ");
            }
        }

        Console.WriteLine(hasPrint ? "" : "no such number.");
    }

    private static void AllCandsMinusOne(Dictionary<int, int> map)
    {
        var removeList = new List<int>();
        foreach (var set in map)
        {
            var key = set.Key;
            var value = set.Value;
            if (value == 1) removeList.Add(key);
            map[key] = value - 1;
        }

        foreach (var removeKey in removeList) map.Remove(removeKey);
    }

    private static Dictionary<int, int> getReals(int[] arr, Dictionary<int, int> cands)
    {
        var reals = new Dictionary<int, int>();
        for (var i = 0; i != arr.Length; i++)
        {
            var curNum = arr[i];
            if (cands.ContainsKey(curNum))
                if (!reals.TryAdd(curNum, 1))
                    reals[curNum] += 1;
        }

        return reals;
    }

    public static void Run()
    {
        int[] arr = [1, 2, 3, 1, 1, 2, 1];
        PrintHalfMajor(arr);
        const int k = 4;
        PrintKMajor(arr, k);
    }
}