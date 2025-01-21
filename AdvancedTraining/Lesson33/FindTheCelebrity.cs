namespace AdvancedTraining.Lesson33;

//https://blog.csdn.net/qq_41543888/article/details/102811390
public class FindTheCelebrity //leetcode_0277
{
    // 提交时不要提交这个函数，因为默认系统会给你这个函数
    // knows方法，自己不认识自己
    private static bool Knows(int x, int i)
    {
        return true;
    }

    // 只提交下面的方法 0 ~ n-1
    public int FindCelebrity(int n)
    {
        // 谁可能成为明星，谁就是cand
        var cand = 0;
        for (var i = 0; i < n; ++i)
            if (Knows(cand, i))
                cand = i;
        // cand是什么？唯一可能是明星的人！
        // 下一步就是验证，它到底是不是明星
        // 1) cand是不是不认识所有的人 cand...（右侧cand都不认识）
        // 所以，只用验证 ....cand的左侧即可
        for (var i = 0; i < cand; ++i)
            if (Knows(cand, i))
                return -1;
        // 2) 是不是所有的人都认识cand
        for (var i = 0; i < n; ++i)
            if (!Knows(i, cand))
                return -1;
        return cand;
    }
    //todo:待整理
}