namespace AdvancedTraining.Lesson02;

public class ChooseWork
{
    private static int[] GetMoneys(Job[] job, int[] ability)
    {
        Array.Sort(job, (x, y) => x.Hard != y.Hard ? x.Hard - y.Hard : y.Money - x.Money);
        // key : 难度   value：报酬
        var map = new SortedDictionary<int, int>
        {
            [job[0].Hard] = job[0].Money
        };
        // pre : 上一份进入map的工作
        var pre = job[0];
        for (var i = 1; i < job.Length; i++)
            if (job[i].Hard != pre.Hard && job[i].Money > pre.Money)
            {
                pre = job[i];
                map[pre.Hard] = pre.Money;
            }

        var ans = new int[ability.Length];
        for (var i = 0; i < ability.Length; i++)
        {
            // ability[i] 当前人的能力 <= ability[i]  且离它最近的
            int? key = map.LastOrDefault(x => x.Key <= ability[i]).Key;
            ans[i] = key != null ? map[key ?? throw new InvalidOperationException()] : 0;
        }

        return ans;
    }

    private class Job(int m, int h)
    {
        public readonly int Hard = h;
        public readonly int Money = m;
    }
}