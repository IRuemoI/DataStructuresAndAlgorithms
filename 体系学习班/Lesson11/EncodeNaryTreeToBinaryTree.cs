//测试通过

namespace Algorithms.Lesson11;

// 本题测试链接：https://leetcode.cn/problems/encode-n-ary-tree-to-binary-tree
public class EncodeNaryTreeToBinaryTree
{
    public static void Run()
    {
        //构造一个多叉树
        var root = new Node(1, [
            new Node(3, [
                    new Node(5),
                    new Node(6),
                    new Node(7)
                ]
            ),

            new Node(2),
            new Node(4),
            new Node(8)
        ]);
        
        var encoded = NaryTreeEncoder.Encode(root);
        var nAryTree = NaryTreeEncoder.Decode(encoded);
        //结构调试看内存
        Console.WriteLine(nAryTree);
    }

    // 提交时不要提交这个类
    //多叉树节点结构
    private class Node
    {
        public readonly List<Node>? Children;
        public readonly int Val;

        public Node(int val)
        {
            Val = val;
        }

        public Node(int val, List<Node> children)
        {
            Val = val;
            Children = children;
        }
    }

    // 提交时不要提交这个类
    //二叉树节点结构
    public class TreeNode(int x)
    {
        public readonly int Val = x;
        public TreeNode? Left;
        public TreeNode? Right;
    }

    // 只提交这个类即可
    private static class NaryTreeEncoder
    {

        //将多叉数转换为二叉树
        public static TreeNode? Encode(Node? root)
        {
            if (root == null) return null;

            var head = new TreeNode(root.Val)
            {
                Left = EncodeProcess(root.Children)
            };
            return head;
        }

        private static TreeNode? EncodeProcess(List<Node>? children)
        {
            TreeNode? head = null;
            TreeNode? cur = null;
            if (children == null) return head;
            //对于子节点数组中的每个子节点
            foreach (var child in children)
            {
                //使用多叉树中子节点的值创建二叉树节点
                var tNode = new TreeNode(child.Val);
                
                //如果当前节点为空，则将当前节点设置为头节点
                if (head == null)
                    head = tNode;
                //如果当前节点不为空，则将当前节点的右子节点设置为tNode
                else if (cur != null) cur.Right = tNode;

                cur = tNode;
                //将当前节点的
                cur.Left = EncodeProcess(child.Children);
            }

            return head;
        }

        //将二叉树转换为多叉树
        public static Node? Decode(TreeNode? root)
        {
            return root == null ? null : new Node(root.Val, DecodeProcess(root.Left));
        }

        private static List<Node> DecodeProcess(TreeNode? root)
        {
            var children = new List<Node>();
            while (root != null)
            {
                var cur = new Node(root.Val, DecodeProcess(root.Left));
                children.Add(cur);
                root = root.Right;
            }

            return children;
        }
    }
}