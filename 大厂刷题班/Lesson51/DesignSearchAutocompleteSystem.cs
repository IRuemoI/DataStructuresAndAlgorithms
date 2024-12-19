namespace AdvancedTraining.Lesson51;

//todo:待整理
public class DesignSearchAutocompleteSystem //leetcode_0642
{
    internal class AutocompleteSystem
    {
        private readonly DesignSearchAutocompleteSystem outerInstance;

        // 题目的要求，只输出排名前3的列表
        private readonly int top = 3;
        private TrieNode cur;

        private readonly bool InstanceFieldsInitialized;

        // 某个前缀树节点，上面的有序表，不在这个节点内部
        // 外挂
        public Dictionary<TrieNode, SortedSet<WordCount>> nodeRankMap = new();


        public string path;
        public TrieNode root;

        // 字符串 "abc"  7次   ->  ("abc", 7)
        public Dictionary<string, WordCount> wordCountMap = new();

        public AutocompleteSystem(DesignSearchAutocompleteSystem outerInstance, string[] sentences, int[] times)
        {
            this.outerInstance = outerInstance;

            if (!InstanceFieldsInitialized)
            {
                InitializeInstanceFields();
                InstanceFieldsInitialized = true;
            }

            path = "";
            cur = root;
            for (var i = 0; i < sentences.Length; i++)
            {
                var word = sentences[i];
                var count = times[i];
                var wc = new WordCount(this, word, count - 1);
                wordCountMap[word] = wc;
                foreach (var c in word) input(c);
                input('#');
            }
        }

        internal virtual void InitializeInstanceFields()
        {
            root = new TrieNode(this, null, "");
        }

        // ' ' -> 0
        // 'a' -> 1
        // 'b' -> 2
        // ...
        // 'z' -> 26 
        //  '`'  a b  .. z
        internal virtual int F(char c)
        {
            return c == ' ' ? 0 : c - '`';
        }

        // 之前已经有一些历史了！
        // 当前键入 c 字符
        // 请顺着之前的历史，根据c字符是什么，继续
        // path : 之前键入的字符串整体
        // cur : 当前滑到了前缀树的哪个节点

        public virtual IList<string> input(char c)
        {
            IList<string> ans = new List<string>();
            if (c != '#')
            {
                path += c;
                var i = F(c);
                if (cur.nexts[i] == null) cur.nexts[i] = new TrieNode(this, cur, path);
                cur = cur.nexts[i];
                if (!nodeRankMap.ContainsKey(cur)) nodeRankMap[cur] = new SortedSet<WordCount>();
                var k = 0;
                // for循环本身就是根据排序后的顺序来遍历！
                foreach (var wc in nodeRankMap[cur])
                {
                    if (k == top) break;
                    ans.Add(wc.word);
                    k++;
                }
            }

            // c = #   path = "abcde" 
            // #
            // #
            // #
            // a b .. #
            if (c == '#' && !path.Equals(""))
            {
                // 真的有一个，有效字符串需要加入！path
                if (!wordCountMap.ContainsKey(path)) wordCountMap[path] = new WordCount(this, path, 0);
                // 有序表内部的小对象，该小对象参与排序的指标数据改变
                // 但是有序表并不会自动刷新
                // 所以，删掉老的，加新的！
                var oldOne = wordCountMap[path];
                var newOne = new WordCount(this, path, oldOne.count + 1);
                while (cur != root)
                {
                    nodeRankMap[cur].Remove(oldOne);
                    nodeRankMap[cur].Add(newOne);
                    cur = cur.father;
                }

                wordCountMap[path] = newOne;
                path = "";
                // cur 回到头了
            }

            return ans;
        }


        public class TrieNode
        {
            private readonly AutocompleteSystem outerInstance;

            public TrieNode father;
            public TrieNode[] nexts;
            public string path;

            public TrieNode(AutocompleteSystem outerInstance, TrieNode F, string p)
            {
                this.outerInstance = outerInstance;
                father = F;
                path = p;
                nexts = new TrieNode[27];
            }
        }

        public class WordCount : IComparable<WordCount>
        {
            private readonly AutocompleteSystem outerInstance;
            public int count;

            public string word;

            public WordCount(AutocompleteSystem outerInstance, string w, int c)
            {
                this.outerInstance = outerInstance;
                word = w;
                count = c;
            }

            public virtual int CompareTo(WordCount o)
            {
                return count != o.count ? o.count - count : word.CompareTo(o.word);
            }
        }
    }
}