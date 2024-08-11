#region

using System.Diagnostics;

#endregion

namespace Common.Utilities;

public static class Utility
{
    private static int _originTreeDepth;

    public static void Swap(int[] array, int i, int j)
    {
        (array[i], array[j]) = (array[j], array[i]);
    }

    // 树数据类型  
    public class TreeNode<T>
    {
        public T? Data { get; init; }
        public TreeNode<T>? LeftChild { get; set; }
        public TreeNode<T>? RightChild { get; set; }
    }

    #region 单例产生随机数

    private static Random? _randomInstance;

    public static double GetRandomDouble
    {
        get
        {
            _randomInstance ??= new Random();
            return _randomInstance.NextDouble();
        }
    }

    #endregion

    #region 单例计时器

    private static Stopwatch? _stopwatch;
    private static bool _stopwatchCalled;

    public static void RestartStopwatch()
    {
        if (!_stopwatchCalled)
        {
            _stopwatch ??= new Stopwatch();
            _stopwatch.Start();
            _stopwatchCalled = true;
        }
        else
        {
            _stopwatch?.Restart();
        }
    }

    public static long GetStopwatchElapsedMilliseconds()
    {
        if (_stopwatch != null) return _stopwatch.ElapsedMilliseconds;
        throw new InvalidOperationException("Stopwatch has not been initialized.");
    }

    #endregion

    #region 打印二叉树

    private static void PrintBinaryTree<T>(TreeNode<T> node)
    {
        _originTreeDepth = GetTreeDepth(node);
        Process(node, 0, 0, 0);
    }

    /// <summary>
    ///     遍历二叉树，之后需要修改为泛型节点
    /// </summary>
    /// <param name="node">当前遍历的节点</param>
    /// <param name="currentDepth"></param>
    /// <param name="isRightSubTree"></param>
    /// <param name="offset"></param>
    private static void Process<T>(TreeNode<T>? node, int currentDepth, int isRightSubTree, int offset)
    {
        if (node == null) return;
        BreakBinaryTree(node, out var leftSubTree, out var rightSubTree);
        // 计算父树的偏移量  
        var parentOffset = offset * (int)Math.Pow(2, _originTreeDepth - currentDepth);
        // 计算子树的偏移量  
        var childOffset = isRightSubTree * (int)Math.Pow(2, _originTreeDepth - currentDepth);
        // 计算半偏移量  
        var halfOffset = (int)Math.Pow(2, _originTreeDepth - currentDepth - 1);
        // 获取根的坐标  
        // x 计算方法为：父偏移量 + 子偏移量 + 半偏移量 - 1  
        // y 计算方法为：目前打印的层数 * 2  
        var x = parentOffset + childOffset + halfOffset - 1;
        var y = currentDepth * 2;
        // 打印根的位置  
        Console.SetCursorPosition(x * 2, y);
        Console.Write(node.Data);
        // 在打印子树时，当前层数+1  
        currentDepth++;
        // 计算子树的父偏移量  
        offset = offset * 2 + (isRightSubTree == 1 ? 2 : 0);

        if (leftSubTree == null && rightSubTree == null) return; //左右子树遍历完成

        if (rightSubTree == null)
        {
            // 打印左子树的位置  
            Console.SetCursorPosition(x * 2 - halfOffset, y + 1);
            Console.Write("┏");
            for (var i = 0; i < halfOffset - 1; i++) Console.Write("━");

            Console.Write("┛");
            Process(leftSubTree, currentDepth, 0, offset);
        }

        else if (leftSubTree == null)
        {
            // 打印右子树的位置  
            Console.SetCursorPosition(x * 2, y + 1);
            Console.Write("┗");
            for (var i = 0; i < halfOffset - 1; i++) Console.Write("━");

            Console.Write("┓");
            Process(rightSubTree, currentDepth, 1, offset);
        }
        else
        {
            // 打印左右子树的位置  
            Console.SetCursorPosition(x * 2 - halfOffset, y + 1);
            Console.Write("┏");
            for (var i = 0; i < halfOffset - 1; i++) Console.Write("━");

            Console.Write("┻");
            for (var i = 0; i < halfOffset - 1; i++) Console.Write("━");

            Console.Write("┓");
            Process(leftSubTree, currentDepth, 0, offset);
            Process(rightSubTree, currentDepth, 1, offset);
        }
    }

    private static int GetTreeDepth<T>(TreeNode<T>? node)
    {
        if (node == null) return 0;
        //本成高度加上左右子树较大的一方作为树的最终高度
        return 1 + Math.Max(GetTreeDepth(node.LeftChild), GetTreeDepth(node.RightChild));
    }


    private static void BreakBinaryTree<T>(TreeNode<T>? node, out TreeNode<T>? leftNode, out TreeNode<T>? rightNode)
    {
        if (node == null)
        {
            leftNode = null;
            rightNode = null;
        }
        else
        {
            leftNode = node.LeftChild;
            rightNode = node.RightChild;
        }
    }

    public static void PrintBinaryTreeTest()
    {
        var root3 = new TreeNode<int>
        {
            Data = 1,
            LeftChild = new TreeNode<int> { Data = 2 },
            RightChild = new TreeNode<int> { Data = 3 }
        };

        root3.LeftChild.LeftChild = new TreeNode<int> { Data = 4 };
        root3.LeftChild.RightChild = new TreeNode<int> { Data = 5 };

        root3.RightChild.LeftChild = new TreeNode<int> { Data = 6 };
        root3.RightChild.RightChild = new TreeNode<int> { Data = 7 };

        root3.LeftChild.LeftChild.LeftChild = new TreeNode<int> { Data = 8 };
        root3.LeftChild.LeftChild.RightChild = new TreeNode<int> { Data = 9 };

        root3.LeftChild.RightChild.LeftChild = new TreeNode<int> { Data = 10 };
        root3.LeftChild.RightChild.RightChild = new TreeNode<int> { Data = 11 };

        root3.RightChild.LeftChild.LeftChild = new TreeNode<int> { Data = 12 };

        PrintBinaryTree(root3);
        Console.Read();
    }

    #endregion
}