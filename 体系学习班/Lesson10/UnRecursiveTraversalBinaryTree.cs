//测试通过

namespace Algorithms.Lesson10;

//非递归遍历二叉树
public static class UnRecursiveTraversalBinaryTree
{
    private static void PreOrderTraversal(Node? head)
    {
        if (head != null)
        {
            var stack = new Stack<Node>();
            stack.Push(head);
            while (stack.Count != 0)
            {
                head = stack.Pop();
                //模拟根左右，也就是先输出本节点，先放入右节点，再放入左节点
                //再次循环pop出来的作为head的就是左子树
                Console.Write(head.Value + " ");
                if (head.Right != null) stack.Push(head.Right);
                if (head.Left != null) stack.Push(head.Left);
            }
        }

        Console.WriteLine();
    }

    private static void InOrderTraversal(Node? head)
    {
        if (head != null)
        {
            var stack = new Stack<Node>();
            while (stack.Count != 0 || head != null)
                //模拟左根右
                if (head != null) //先尽可能压入左子树
                {
                    stack.Push(head);
                    head = head.Left;
                }
                else
                {
                    head = stack.Pop();
                    Console.Write(head.Value + " ");
                    head = head.Right; //输出之后压入右子树
                }
        }

        Console.WriteLine();
    }

    private static void PostOrderTraversal(Node? head)
    {
        if (head != null)
        {
            var s1 = new Stack<Node>();
            var s2 = new Stack<Node>();
            //按照上面的先序遍历更改为头右左遍历
            s1.Push(head);
            while (s1.Count != 0)
            {
                head = s1.Pop();
                s2.Push(head);
                if (head.Left != null) s1.Push(head.Left);
                if (head.Right != null) s1.Push(head.Right);
            }

            //用栈翻转结果为左右头
            while (s2.Count != 0) Console.Write(s2.Pop().Value + " ");
        }

        Console.WriteLine();
    }

    //这种方法并不常见
    private static void PostOrderTraversal1(Node? head)
    {
        if (head != null)
        {
            var stack = new Stack<Node>();
            stack.Push(head);
            while (stack.Count != 0)
            {
                //模拟左右根
                var current = stack.Peek();
                //如果当前节点的左子节点不为空，且左右节点都没去过
                if (current.Left != null && head != current.Left && head != current.Right)
                {
                    //把左子节点压入栈
                    stack.Push(current.Left);
                }
                else if (current.Right != null && head != current.Right) //如果当前节点的右子节点不为空，且右子节点没去过
                {
                    //把右子节点压入栈
                    stack.Push(current.Right);
                }
                else
                {
                    //左右两个子节点都去过了，弹出栈顶存储的当前子树的根节点的值并输出
                    Console.Write(stack.Pop().Value + " ");
                    //将当前的节点设置为已经去过的节点
                    head = current;
                }
            }
        }

        Console.WriteLine();
    }

    public static void Run()
    {
        var head = new Node(1)
        {
            Left = new Node(2)
            {
                Left = new Node(3),
                Right = new Node(4)
            },
            Right = new Node(5)
            {
                Left = new Node(6)
                {
                    Left = new Node(8),
                    Right = new Node(9)
                },
                Right = new Node(7)
            }
        };

        Console.Write("pre-order:\t");
        PreOrderTraversal(head);
        Console.WriteLine("========");
        Console.Write("in-order:\t");
        InOrderTraversal(head);
        Console.WriteLine("========");
        Console.Write("post-order:\t");
        PostOrderTraversal(head);
        Console.WriteLine("========");
        Console.Write("post-order:\t");
        PostOrderTraversal1(head);
        Console.WriteLine("========");
    }

    private class Node(int v)
    {
        public readonly int Value = v;
        public Node? Left;
        public Node? Right;
    }
}