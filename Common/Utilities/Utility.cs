#region

using System.Diagnostics;

#endregion

namespace Common.Utilities;

public static class Utility
{
    public static void Swap(int[] array, int i, int j)
    {
        (array[i], array[j]) = (array[j], array[i]);
    }


    #region 单例产生随机数

    private static Random? _randomInstance;

    public static double getRandomDouble
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

    // 树数据类型  
    public class TreeNode<T>
    {
        public TreeNode()
        {
        }

        public TreeNode(T v)
        {
            value = v;
        }

        public T? value { get; init; }
        public TreeNode<T>? leftChild { get; set; }
        public TreeNode<T>? rightChild { get; set; }
    }

    private static int _originTreeDepth;
    private static int _originCursorTop;

    //TODO:二叉树可视化函数的增强
    //对于这个可视化输出二叉树的C#函数，他的核心显示逻辑是通过调整终端光标的位置来输出二叉树，
    //但是用于终端Console.BufferWidth和Console.BufferHeight两个属性的限制(终端缓冲区的限制最多输出5层的二叉树)，
    //所以并不能绘制较大规模的二叉树。那么将显示的结果写入文本文件来解决这一问题
    public static void PrintBinaryTree<T>(TreeNode<T>? node)
    {
        _originCursorTop = Console.CursorTop;
        _originTreeDepth = GetTreeDepth(node);
        Process(node, 0, 0, 0);
        var finalCursorTop = _originTreeDepth * 2 - 1 > 0 ? _originTreeDepth * 2 - 1 : _originCursorTop;
        Console.SetCursorPosition(0, finalCursorTop); //设置光标到新行，否则会覆盖二叉树输出的内容
    }

    private static void Process<T>(TreeNode<T>? node, int currentDepth, int isRightSubTree, int offset)
    {
        //如果节点为空，则返回
        if (node == null) return;
        //将二叉树分为左右子树
        BreakBinaryTree(node, out var leftSubTree, out var rightSubTree);
        //计算父节点和子节点的偏移量
        var parentOffset = offset * (int)Math.Pow(2, _originTreeDepth - currentDepth);
        var childOffset = isRightSubTree * (int)Math.Pow(2, _originTreeDepth - currentDepth);
        //计算中间偏移量
        var halfOffset = (int)Math.Pow(2, _originTreeDepth - currentDepth - 1);
        //计算节点横坐标
        var x = parentOffset + childOffset + halfOffset - 1;
        //计算节点纵坐标
        var y = currentDepth * 2;
        //设置光标位置，并输出节点值
        Console.SetCursorPosition(x * 2, y + _originCursorTop);
        Console.Write(node.value);
        //节点深度加1
        currentDepth++;
        //计算新的偏移量
        offset = offset * 2 + (isRightSubTree == 1 ? 2 : 0);

        //如果左右子树都为空，则返回
        if (leftSubTree == null && rightSubTree == null) return; //左右子树遍历完成

        //如果右子树为空，则只处理左子树
        if (rightSubTree == null)
        {
            //设置光标位置，并输出左子树连接线
            Console.SetCursorPosition(x * 2 - halfOffset, y + 1 + _originCursorTop);
            Console.Write("┏");
            for (var i = 0; i < halfOffset - 1; i++) Console.Write("━");

            Console.Write("┛");
            //处理左子树
            Process(leftSubTree, currentDepth, 0, offset);
        }

        //如果左子树为空，则只处理右子树
        else if (leftSubTree == null)
        {
            //设置光标位置，并输出右子树连接线
            Console.SetCursorPosition(x * 2, y + 1 + _originCursorTop);
            Console.Write("┗");
            for (var i = 0; i < halfOffset - 1; i++) Console.Write("━");

            Console.Write("┓");
            //处理右子树
            Process(rightSubTree, currentDepth, 1, offset);
        }
        //如果左右子树都不为空，则处理左右子树
        else
        {
            //设置光标位置，并输出左右子树连接线
            Console.SetCursorPosition(x * 2 - halfOffset, y + 1 + _originCursorTop);
            Console.Write("┏");
            for (var i = 0; i < halfOffset - 1; i++) Console.Write("━");

            Console.Write("┻");
            for (var i = 0; i < halfOffset - 1; i++) Console.Write("━");

            Console.Write("┓");
            //处理左子树
            Process(leftSubTree, currentDepth, 0, offset);
            //处理右子树
            Process(rightSubTree, currentDepth, 1, offset);
        }
    }

    private static int GetTreeDepth<T>(TreeNode<T>? node)
    {
        if (node == null) return 0;
        //本成高度加上左右子树较大的一方作为树的最终高度
        return 1 + Math.Max(GetTreeDepth(node.leftChild), GetTreeDepth(node.rightChild));
    }

    //获取当前节点的左子树和右子树的根节点
    private static void BreakBinaryTree<T>(TreeNode<T>? node, out TreeNode<T>? leftNode, out TreeNode<T>? rightNode)
    {
        if (node == null)
        {
            leftNode = null;
            rightNode = null;
        }
        else
        {
            leftNode = node.leftChild;
            rightNode = node.rightChild;
        }
    }

    public static void PrintBinaryTreeTest()
    {
        var root = new TreeNode<int>()
        {
            value = 1,
            leftChild = new TreeNode<int>
            {
                value = 2,
                leftChild = new TreeNode<int>
                {
                    value = 4,
                    leftChild = new TreeNode<int> { value = 8 },
                    rightChild = new TreeNode<int> { value = 9 }
                },
                rightChild = new TreeNode<int>
                {
                    value = 5,
                    leftChild = new TreeNode<int> { value = 10 },
                    rightChild = new TreeNode<int> { value = 11 }
                }
            },
            rightChild = new TreeNode<int>
            {
                value = 3,
                leftChild = new TreeNode<int>
                {
                    value = 6,
                    leftChild = new TreeNode<int>
                    {
                        value = 12,
                        leftChild = new TreeNode<int> { value = 13 }
                    }
                },
                rightChild = new TreeNode<int> { value = 7 }
            }
        };
        PrintBinaryTree(root);
        Console.WriteLine("打印完成");
    }

    #endregion
}