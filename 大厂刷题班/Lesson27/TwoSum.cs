//pass

namespace AdvancedTraining.Lesson27;

public class TwoSum //leetcode_0001
{
    private static int[] TwoSumCode(int[] numbers, int target)
    {
        // key 某个之前的数   value 这个数出现的位置
        var map = new Dictionary<int, int>();
        for (var i = 0; i < numbers.Length; i++)
        {
            if (map.ContainsKey(target - numbers[i])) return [map[target - numbers[i]], i];
            map[numbers[i]] = i;
        }

        return [-1, -1];
    }

    public static void Run()
    {
        foreach (var item in TwoSumCode([2, 7, 11, 15], 9)) Console.Write(item + ","); //输出[0,1]
    }
}