//从前序和中序遍历序列构造二叉树
using System.Diagnostics;

namespace AdvancedTraining.算法新手班补充;

public class RebuildBinaryTree
{
    public class TreeNode(int value)
    {
        public int val = value;
        public TreeNode? left;
        public TreeNode? right;
    }

    public TreeNode? BuildTree(int[] preorder, int[] inorder)
    {
        Dictionary<int, int> inMap = new();
        for (int i = 0; i < inorder.Length; i++)
        {
            inMap.Add(inorder[i], i);
        }
        return Process(preorder, 0, preorder.Length - 1, inorder, 0, inorder.Length - 1, inMap);
    }

    public TreeNode? Process(int[] preOrder, int L1, int R1, int[] inOrder, int L2, int R2, Dictionary<int, int> inMap)
    {
        if (L1 == R1)
        {
            return null;
        }
        TreeNode head = new(preOrder[L1]);
        if (L1 == R1)
        {
            return head;
        }
        int find = inMap[preOrder[L1]];
        head.left = Process(preOrder, L1 + 1, L1 + find - L2, inOrder, L2, find - 1, inMap);
        head.right = Process(preOrder, L1 + find - L2 + 1, R1, inOrder, find - 1, L2, inMap);
        return head;
    }
}