//测试通过

namespace Common.Algorithms.Search;

public abstract class BinarySearch
{
    private static bool Code(int[]? sortedList, int num)
    {
        if (sortedList == null || sortedList.Length == 0) return false;

        var leftEdge = 0;
        var rightEdge = sortedList.Length - 1;
        while (leftEdge < rightEdge)
        {
            var middle = leftEdge + ((rightEdge - leftEdge) >> 1);
            if (sortedList[middle] == num) return true;

            if (sortedList[middle] > num)
                rightEdge = middle - 1;
            else
                leftEdge = middle + 1;
        }

        return sortedList[leftEdge] == num;
    }

    public static void Run()
    {
        int[] testList = [54, 26, 93, 17, 77, 31, 44, 55, 20];
        Console.WriteLine("二分查找测试：" + (Code(testList, 26) ? "存在" : "无效"));
    }
}