namespace Common.Algorithms.Sort;

public class MergeSort
{
    private static void RecursionMergeSort(int[] arr)
    {
        if (arr.Length <= 1) return;
        Process(arr, 0, arr.Length - 1);
    }

    private static void Process(int[] arr, int leftEdge, int rightEdge)
    {
        if (leftEdge == rightEdge) return;
        var mid = leftEdge + ((rightEdge - leftEdge) >> 1);
        Process(arr, leftEdge, mid); //左半部分排序
        Process(arr, mid + 1, rightEdge); //右半部分排序
        Merge(arr, leftEdge, mid, rightEdge); //合并
    }

    private static void Merge(int[] arr, int leftEdge, int middle, int rightEdge)
    {
        var help = new int[rightEdge - leftEdge + 1];
        var helpIndex = 0;
        var leftPartIndex = leftEdge;
        var rightPartIndex = middle + 1;
        //比较左右两部分的元素，每次将较小的元素放入help数组中
        while (leftPartIndex <= middle && rightPartIndex <= rightEdge)
            help[helpIndex++] = arr[leftPartIndex] < arr[rightPartIndex] ? arr[leftPartIndex++] : arr[rightPartIndex++];
        //复制左半部分还有剩余的元素
        if (leftPartIndex <= middle) Array.Copy(arr, leftPartIndex, help, helpIndex, middle - leftPartIndex + 1);
        //复制右半部分还有剩余的元素
        if (rightPartIndex <= rightEdge)
            Array.Copy(arr, rightPartIndex, help, helpIndex, rightEdge - rightPartIndex + 1);
        //将排好序的help数组拷贝到原数组
        Array.Copy(help, 0, arr, leftEdge, help.Length);
    }

    // 迭代方法实现
    private static void IterateMergeSort(int[] arr)
    {
        var arrayLength = arr.Length;
        if (arrayLength <= 1) return;
        var stepSize = 1;
        //如果步长小于数组长度，则继续循环(如果超过则说明已经遍历完所有元素)
        while (stepSize < arr.Length)
        {
            var leftEdge = 0;
            //如果剩下的元素还能够凑齐一个左组，则继续循环
            while (leftEdge < arrayLength)
            {
                //如果当前步长的大小超过了一个左组，退出循环
                if (stepSize >= arrayLength - leftEdge) break;
                var middle = leftEdge + stepSize - 1;
                //如果当前步长的大小超过了右组，则将右组边界设置为右组最后一个元素
                //arrayLength-1表示右组最后一个元素的下标，减去middle表示右组元素个数
                var rightEdge = middle + Math.Min(stepSize, arrayLength - 1 - middle);
                Merge(arr, leftEdge, middle, rightEdge);
                leftEdge = rightEdge + 1;
            }

            //如果步长已经超过了数组长度的一半，那么说明整个数组已经排序完成，也可以避免类型溢出的问题
            if (stepSize > arrayLength / 2) break;
            stepSize <<= 1;
        }
    }

    public static void Run()
    {
        int[] testList = [54, 26, 93, 17, 77, 31, 44, 55, 20];
        Console.WriteLine("归并排序升序(递归)：");
        RecursionMergeSort(testList);
        Console.WriteLine(string.Join(",", testList));
        Console.WriteLine("-------------------");
        Console.WriteLine("归并排序升序(迭代)：");
        IterateMergeSort(testList);
        Console.WriteLine(string.Join(",", testList));
    }
}