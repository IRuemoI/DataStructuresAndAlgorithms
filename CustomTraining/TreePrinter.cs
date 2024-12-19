using System.Text;

namespace CustomTraining;

//todo：右子树节点值的位置打印偏右
public static class TreePrinter
{
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
        public TreeNode<T>? leftChild { get; init; }
        public TreeNode<T>? rightChild { get; init; }
    }

    private static int _originTreeDepth;
    private static StringBuilder[]? _nodeLines;
    private static StringBuilder[]? _branchLines;

    private static void PrintBinaryTree<T>(TreeNode<T>? node)
    {
        _originTreeDepth = GetTreeDepth(node);

        switch (_originTreeDepth)
        {
            case < 1:
                return;
            case 1:
                _nodeLines = new StringBuilder[1];
                _branchLines = null;
                break;
            default:
                _nodeLines = new StringBuilder[_originTreeDepth];
                _branchLines = new StringBuilder[_originTreeDepth - 1];
                break;
        }

        int index;
        for (index = 0; index < _nodeLines.Length; index++)
            _nodeLines[index] = new StringBuilder();
        for (index = 0; index < _branchLines?.Length; index++)
            _branchLines[index] = new StringBuilder();

        Process(node, 0, 0, 0);

        //将打印的二叉树写入到文件
        using var writer = new StreamWriter("BinaryTree.txt");
        int i;
        for (i = 0; i < _branchLines?.Length; i++)
        {
            writer.WriteLine(_nodeLines[i]);
            writer.WriteLine(_branchLines[i]);
        }

        writer.WriteLine(_nodeLines[i]);
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
        var y = currentDepth;
        //设置光标位置，并输出节点值
        _nodeLines?[y].Append(' ', x * 2).Append(node.value);
        //节点深度加1
        currentDepth++;
        //计算新的偏移量
        offset = offset * 2 + (isRightSubTree == 1 ? 2 : 0);
        //offset *= 2;

        //如果左右子树都为空，则返回
        if (leftSubTree == null && rightSubTree == null) return; //左右子树遍历完成

        //如果右子树为空，则只处理左子树
        if (rightSubTree == null)
        {
            //设置光标位置，并输出左子树连接线
            _branchLines?[y].Append(' ', x * 2 - halfOffset)
                .Append('┏')
                .Append('━', halfOffset - 1)
                .Append('┛');
            //处理左子树
            Process(leftSubTree, currentDepth, 0, offset);
        }

        //如果左子树为空，则只处理右子树
        else if (leftSubTree == null)
        {
            //设置光标位置，并输出右子树连接线
            _branchLines?[y].Append(' ', x * 2 - halfOffset)
                .Append('┗')
                .Append('━', halfOffset - 1)
                .Append('┓');

            //处理右子树
            Process(rightSubTree, currentDepth, 1, offset);
        }
        //如果左右子树都不为空，则处理左右子树
        else
        {
            //设置光标位置，并输出左右子树连接线
            _branchLines?[y].Append(' ', x * 2 - halfOffset)
                .Append('┏')
                .Append('━', halfOffset - 1)
                .Append('┻')
                .Append('━', halfOffset - 1)
                .Append('┓');
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

        // var root = new TreeNode<int>()
        // {
        //     value = 1,
        //     leftChild = new TreeNode<int>
        //     {
        //         value = 2
        //     },
        //     rightChild = new TreeNode<int>
        //     {
        //         value = 3
        //     }
        // };
        PrintBinaryTree(root);
        Console.WriteLine("打印完成");
    }
}