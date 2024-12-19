namespace CustomTraining.Leetcode;

//leetcode: https://leetcode.cn/problems/shu-zu-zhong-zhong-fu-de-shu-zi-lcof/
public class FindRepeatDocument
{
    private static int FindRepeatDocumentCode(int[] docs)
    {
        var i = 0;
        while (i < docs.Length)
        {
            if (docs[i] == i)
            {
                //如果当前位置的数字等于当前位置，则继续下一个位置
                i++;
                continue;
            }

            if (docs[i] == docs[docs[i]]) return docs[i];
            var temp = docs[i];
            docs[i] = docs[temp];
            docs[temp] = temp;
        }

        return -1;
    }

    public static void Run()
    {
        int[] documents = [2, 5, 3, 0, 5, 0];
        var result = FindRepeatDocumentCode(documents);
        Console.WriteLine(result);
    }
}