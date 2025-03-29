//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson11;

public class SerializeAndDeserializationBinaryTree
{
    private static Queue<string?> PreOrderSerialization(Utility.TreeNode<int>? head)
    {
        Queue<string?> ans = new();
        PreOrderSerializationProcess(head, ans);
        return ans;
    }

    private static void PreOrderSerializationProcess(Utility.TreeNode<int>? head, Queue<string?> ans)
    {
        if (head == null)
        {
            ans.Enqueue(null);
        }
        else
        {
            ans.Enqueue(head.value.ToString());
            PreOrderSerializationProcess(head.leftChild, ans);
            PreOrderSerializationProcess(head.rightChild, ans);
        }
    }

    private static Queue<string?> PostOrderSerialization(Utility.TreeNode<int>? head)
    {
        Queue<string?> ans = new();
        PostOrderSerializationProcess(head, ans);
        return ans;
    }

    private static void PostOrderSerializationProcess(Utility.TreeNode<int>? head, Queue<string?> ans)
    {
        if (head == null)
        {
            ans.Enqueue(null);
        }
        else
        {
            PostOrderSerializationProcess(head.leftChild, ans);
            PostOrderSerializationProcess(head.rightChild, ans);
            ans.Enqueue(head.value.ToString());
        }
    }

    private static Queue<string?> HierarchicalSerialization(Utility.TreeNode<int>? head)
    {
        Queue<string?> ans = new();
        if (head == null)
        {
            ans.Enqueue(null);
        }
        else
        {
            ans.Enqueue(head.value.ToString());
            Queue<Utility.TreeNode<int>> queue = new();
            queue.Enqueue(head);
            while (queue.Count != 0)
            {
                head = queue.Dequeue(); // head 父   子
                if (head.leftChild != null)
                {
                    ans.Enqueue(head.leftChild.value.ToString());
                    queue.Enqueue(head.leftChild);
                }
                else
                {
                    ans.Enqueue(null);
                }

                if (head.rightChild != null)
                {
                    ans.Enqueue(head.rightChild.value.ToString());
                    queue.Enqueue(head.rightChild);
                }
                else
                {
                    ans.Enqueue(null);
                }
            }
        }

        return ans;
    }

    private static Utility.TreeNode<int>? PreOrderDeserialization(Queue<string?>? preList)
    {
        if (preList == null || preList.Count == 0) return null;

        return PreOrderDeserializationProcess(preList);
    }

    private static Utility.TreeNode<int>? PreOrderDeserializationProcess(Queue<string?> preList)
    {
        var value = preList.Dequeue();
        if (value == null) return null;

        var head = new Utility.TreeNode<int>(int.Parse(value))
        {
            leftChild = PreOrderDeserializationProcess(preList),
            rightChild = PreOrderDeserializationProcess(preList)
        };
        return head;
    }

    private static Utility.TreeNode<int>? PostOrderDeserialization(Queue<string?>? posList)
    {
        if (posList == null || posList.Count == 0) return null;

        // 左右中  ->  stack(中右左)
        Stack<string?> stack = new();
        while (posList.Count != 0) stack.Push(posList.Dequeue());

        return PostOrderDeserializationProcess(stack);
    }

    private static Utility.TreeNode<int>? PostOrderDeserializationProcess(Stack<string?> posStack)
    {
        var value = posStack.Pop();
        if (value == null) return null;

        var head = new Utility.TreeNode<int>(int.Parse(value))
        {
            rightChild = PostOrderDeserializationProcess(posStack),
            leftChild = PostOrderDeserializationProcess(posStack)
        };
        return head;
    }

    private static Utility.TreeNode<int>? HierarchicalDeserialization(Queue<string?>? levelList)
    {
        if (levelList == null || levelList.Count == 0) return null;

        var head = GenerateNode(levelList.Dequeue());
        Queue<Utility.TreeNode<int>> queue = new();
        if (head != null) queue.Enqueue(head);

        while (queue.Count != 0)
        {
            var node = queue.Dequeue();
            node.leftChild = GenerateNode(levelList.Dequeue());
            node.rightChild = GenerateNode(levelList.Dequeue());
            if (node.leftChild != null) queue.Enqueue(node.leftChild);

            if (node.rightChild != null) queue.Enqueue(node.rightChild);
        }

        return head;
    }

    /*
     * 二叉树可以通过先序、后序或者按层遍历的方式序列化和反序列化，
     * 以下代码全部实现了。
     * 但是，二叉树无法通过中序遍历的方式实现序列化和反序列化
     * 因为不同的两棵树，可能得到同样的中序序列，即便补了空位置也可能一样。
     * 比如如下两棵树
     *         __2
     *        /
     *       1
     *       和
     *       1__
     *          \
     *           2
     * 补足空位置的中序遍历结果都是{ null, 1, null, 2, null}
     *
     * */
    private static Queue<string?> InOrderSerialization(Utility.TreeNode<int>? head)
    {
        Queue<string?> ans = new();
        InOrderSerializationProcess(head, ans);
        return ans;
    }

    private static void InOrderSerializationProcess(Utility.TreeNode<int>? head, Queue<string?> ans)
    {
        if (head == null)
        {
            ans.Enqueue(null);
        }
        else
        {
            InOrderSerializationProcess(head.leftChild, ans);
            ans.Enqueue(head.value.ToString());
            InOrderSerializationProcess(head.rightChild, ans);
        }
    }

    public static void Run()
    {
        Console.WriteLine("中序遍历序列化:");
        var inHeadRoot = GenerateRandomBst(5, 200);
        Utility.PrintBinaryTree(inHeadRoot);
        var inQueue = InOrderSerialization(inHeadRoot);
        Console.WriteLine();
        foreach (var node in inQueue) Console.Write(node ?? ",#" + ",");

        Console.WriteLine();

        var maxLevel = 5;
        var maxValue = 100;
        var testTimes = 1000000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTimes; i++)
        {
            var head = GenerateRandomBst(maxLevel, maxValue);
            var pre = PreOrderSerialization(head);
            var pos = PostOrderSerialization(head);
            var level = HierarchicalSerialization(head);
            var preBuild = PreOrderDeserialization(pre);
            var posBuild = PostOrderDeserialization(pos);
            var levelBuild = HierarchicalDeserialization(level);
            if (!IsSameValueStructure(preBuild, posBuild) || !IsSameValueStructure(posBuild, levelBuild))
                Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");
    }

    #region 用于测试

    private static Utility.TreeNode<int>? GenerateNode(string? val)
    {
        if (val == null) return null;

        return new Utility.TreeNode<int>(int.Parse(val));
    }

    //用于测试
    private static Utility.TreeNode<int>? GenerateRandomBst(int maxLevel, int maxValue)
    {
        return Generate(1, maxLevel, maxValue);
    }

    //用于测试
    private static Utility.TreeNode<int>? Generate(int level, int maxLevel, int maxValue)
    {
        if (level > maxLevel || Utility.getRandomDouble < 0.5) return null;

        var head = new Utility.TreeNode<int>((int)(Utility.getRandomDouble * maxValue))
        {
            leftChild = Generate(level + 1, maxLevel, maxValue),
            rightChild = Generate(level + 1, maxLevel, maxValue)
        };
        return head;
    }

    //用于测试
    private static bool IsSameValueStructure(Utility.TreeNode<int>? head1, Utility.TreeNode<int>? head2)
    {
        if (head1 == null && head2 != null) return false;

        if (head1 != null && head2 == null) return false;

        if (head1 == null && head2 == null) return true;

        if (head1?.value != head2?.value) return false;

        return IsSameValueStructure(head1?.leftChild, head2?.leftChild) &&
               IsSameValueStructure(head1?.rightChild, head2?.rightChild);
    }

    #endregion
}