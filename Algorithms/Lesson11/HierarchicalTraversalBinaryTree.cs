//通过

namespace Algorithms.Lesson11;

//二叉树按层遍历
public class HierarchicalTraversalBinaryTree
{
    private static void HierarchicalTraversal(Node? head)
    {
        if (head == null) return;

        //准备一个队列
        Queue<Node> queue = new();
        //先将根节点入队
        queue.Enqueue(head);
        while (queue.Count != 0)
        {
            var cur = queue.Dequeue(); //队头节点出列作为当前节点
            Console.Write(cur.value + ","); //打印当前节点
            if (cur.leftChild != null) queue.Enqueue(cur.leftChild); //将当前节点的左子节点入队
            if (cur.rightChild != null) queue.Enqueue(cur.rightChild); //将当前节点的右子节点入队
        }

        Console.WriteLine();
    }

    public static void Run()
    {
        var root = new Node
        {
            value = 1,
            leftChild = new Node
            {
                value = 2,
                leftChild = new Node
                {
                    value = 4,
                    leftChild = new Node { value = 8 },
                    rightChild = new Node { value = 9 }
                },
                rightChild = new Node
                {
                    value = 5,
                    leftChild = new Node { value = 10 },
                    rightChild = new Node { value = 11 }
                }
            },
            rightChild = new Node
            {
                value = 3,
                leftChild = new Node
                {
                    value = 6,
                    leftChild = new Node
                    {
                        value = 12,
                        leftChild = new Node { value = 13 }
                    }
                },
                rightChild = new Node { value = 7 }
            }
        };
        HierarchicalTraversal(root);
    }

    private class Node
    {
        public int value { init; get; }
        public Node? leftChild { init; get; }
        public Node? rightChild { init; get; }
    }
}