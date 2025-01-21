#region

using System.Text;

#endregion

namespace AdvancedTraining.Lesson50;

public class DesignInMemoryFileSystem //leetcode_0588
{
    internal class FileSystem
    {
        private readonly Node _head = new("");

        public List<string> Ls(string path)
        {
            var ans = new List<string>();
            var cur = _head;
            var parts = path.Split('/');
            var n = parts.Length;

            for (var i = 1; i < n; i++)
            {
                if (!cur.NextDict.ContainsKey(parts[i])) return ans;
                cur = cur.NextDict[parts[i]];
            }

            // cur指向了path的最后节点  
            if (cur.Content == null)
                ans.AddRange(cur.NextDict.Keys);
            else
                ans.Add(cur.Name);

            return ans;
        }

        public void Mkdir(string path)
        {
            var cur = _head;
            var parts = path.Split("/");
            var n = parts.Length;
            for (var i = 1; i < n; i++)
            {
                if (!cur.NextDict.ContainsKey(parts[i])) cur.NextDict[parts[i]] = new Node(parts[i]);
                cur = cur.NextDict[parts[i]];
            }
        }

        public void AddContentToFile(string path, string content)
        {
            var cur = _head;
            var parts = path.Split("/");
            var n = parts.Length;
            for (var i = 1; i < n - 1; i++)
            {
                if (!cur.NextDict.ContainsKey(parts[i])) cur.NextDict[parts[i]] = new Node(parts[i]);
                cur = cur.NextDict[parts[i]];
            }

            // 来到的是倒数第二的节点了！注意for！
            if (!cur.NextDict.ContainsKey(parts[n - 1])) cur.NextDict[parts[n - 1]] = new Node(parts[n - 1], "");
            cur.NextDict[parts[n - 1]].Content?.Append(content);
        }

        public string ReadContentFromFile(string path)
        {
            var cur = _head;
            var parts = path.Split("/");
            var n = parts.Length;
            for (var i = 1; i < n; i++)
            {
                if (!cur.NextDict.ContainsKey(parts[i])) cur.NextDict[parts[i]] = new Node(parts[i]);
                cur = cur.NextDict[parts[i]];
            }

            return cur.Content!.ToString();
        }


        private class Node
        {
            // content == null 意味着这个节点是目录
            // content != null 意味着这个节点是文件
            public readonly StringBuilder? Content;

            // 文件名、目录名
            public readonly string Name;
            public readonly SortedDictionary<string, Node> NextDict;

            // 构造目录
            public Node(string n)
            {
                Name = n;
                Content = null;
                NextDict = new SortedDictionary<string, Node>();
            }

            // 构造文件，c是文件内容
            public Node(string n, string c)
            {
                Name = n;
                Content = new StringBuilder(c);
                NextDict = new SortedDictionary<string, Node>();
            }
        }
    }
}