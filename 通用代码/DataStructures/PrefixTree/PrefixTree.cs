namespace Common.DataStructures.PrefixTree;

public class PrefixTree
{
    private readonly DictionaryNode _root = new();

    //将单词加入前缀树
    public void Insert(string? word)
    {
        if (word == null) return;

        var characters = word.ToCharArray(); //将单词中的每个字符放入数组中
        var currentNode = _root; //设置当前节点为根节点
        currentNode.Pass++; //经过根节点的字符增加
        //对于单词数组中的每个字符
        foreach (var character in characters)
        {
            //如果本层的字典中不包含这个字符节点
            if (!currentNode.NextCharDict.ContainsKey(character))
                //把这个字符添加进字典，并准备下层的节点
                currentNode.NextCharDict.Add(character, new DictionaryNode());
            currentNode = currentNode.NextCharDict[character]; //当前节点向下层移动
            currentNode.Pass++; //增加经过当前节点字符的单词数
        }

        currentNode.End++; //增加以当前节点字符结尾的单词数
    }

    //搜索单词在前缀树中出现的次数
    public int Search(string? word)
    {
        if (word == null) return 0;

        var characters = word.ToCharArray(); //将单词中的每个字符放入数组中
        var currentNode = _root; //设置当前节点为根节点
        //对于单词数组中的每个字符
        foreach (var character in characters)
        {
            //如果当前节点的下一层不存在这个字符，那么直接返回0
            if (!currentNode.NextCharDict.TryGetValue(character, out var value)) return 0;
            currentNode = value; //当前节点向下层移动
        }

        return currentNode.End; //返回以当前节点字符结尾的单词数
    }

    //从前缀树中删除单词
    public void Delete(string word)
    {
        if (Search(word) != 0)
        {
            //如果能从前缀树中搜到的这个单词
            var characters = word.ToCharArray(); //将单词中的每个字符放入数组中
            var currentNode = _root; //设置当前节点为根节点
            currentNode.Pass--; //经过根节点的字符减少
            //对于单词数组中的每个字符
            foreach (var character in characters)
            {
                //如果下一个节点的单词经过数减少后到零了
                if (--currentNode.NextCharDict[character].Pass == 0)
                {
                    currentNode.NextCharDict.Remove(character); //移除下面的子树，内存会被自动回收
                    return;
                    //注意：对于没有GC的语言比如C++可以将需要销毁的节点放入栈中然后依次销毁
                }

                currentNode = currentNode.NextCharDict[character]; //当前节点向下层移动
            }

            currentNode.End--; //减少以当前节点字符结尾的单词数
        }
    }

    //搜索前缀树中以某个单词作为前缀的单词数
    public int GetPrefixNumber(string? prefix)
    {
        if (prefix == null) return 0;

        var characters = prefix.ToCharArray(); //将前缀中的每个字符放入数组中
        var currentNode = _root; //设置当前节点为根节点
        //对于单词数组中的每个字符
        foreach (var character in characters)
        {
            //如果当前节点的下一层不存在这个字符，那么直接返回0
            if (!currentNode.NextCharDict.TryGetValue(character, out var value)) return 0;
            currentNode = value; //当前节点向下层移动
        }

        return currentNode.Pass; //返回经过当前节点字符的单词数
    }

    private class DictionaryNode
    {
        //存储下一个字符节点的地址的字典(对于非ASCII字符可以将字符转换为unicode或者utf-8编码作为字典的key)
        public readonly Dictionary<int, DictionaryNode> NextCharDict = new();
        public int End; //以当前节点字符结尾的单词个数
        public int Pass; //经过当前节点的单词个数
    }
}