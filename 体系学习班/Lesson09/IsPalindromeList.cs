//测试通过

namespace Algorithms.Lesson09;

public class IsPalindromeList
{
    // 需要O(n)的额外空间
    private static bool IsPalindrome1(Node? head)
    {
        var stack = new Stack<Node>();
        var current = head;
        //将链表的所有节点放入栈中
        while (current != null)
        {
            stack.Push(current);
            current = current.Next;
        }

        //从头重新遍历链表并同时弹出栈中节点，如果不相同那么不是回文
        while (head != null)
        {
            if (head.Value != stack.Pop().Value) return false;
            head = head.Next;
        }

        return true; //全部相同那么就是回文
    }

    // 需要O(n/2)的额外空间
    private static bool IsPalindrome2(Node? head)
    {
        if (head?.Next == null) return true; //链表只有一个节点是回文

        //找到链表的下中点middle
        var middle = head.Next;
        var current = head;
        while (current.Next is { Next: not null })
        {
            middle = middle?.Next;
            current = current.Next.Next;
        }

        //将下中点之后的节点放入栈中
        var stack = new Stack<Node>();
        while (middle != null)
        {
            stack.Push(middle);
            middle = middle.Next;
        }

        //从头重新遍历链表并同时弹出栈中节点，如果不相同那么不是回文
        while (stack.Count != 0)
        {
            if (head?.Value != stack.Pop().Value) return false;
            head = head.Next;
        }

        return true; //链表的前后两部分对称是回文
    }

    // 需要O(1)的额外空间
    private static bool IsPalindrome3(Node? head)
    {
        if (head?.Next == null) return true;

        // 找到链表的下中点middle
        var middle = head;
        var current = head;
        while (current.Next is { Next: not null })
        {
            middle = middle?.Next;
            current = current.Next.Next;
        }

        current = middle?.Next; //current指向右半部分的第一个节点 
        var preNode = middle;
        if (middle != null)
        {
            //翻转链表右半部分
            middle.Next = null; //将链表从中间断开 
            while (current != null)
            {
                //右半部分翻转
                var nextNode = current.Next; //记录下个节点 
                current.Next = preNode; //当前节点的Next指向前一个节点
                preNode = current; //上个节点后移
                current = nextNode; //当前节点后移
            }

            var leftHead = head; //获取左侧的头指针
            var rightHead = preNode; //获取右侧的头指针
            var result = true;
            while (leftHead != null && rightHead != null)
            {
                //判断回文
                if (leftHead.Value != rightHead.Value)
                {
                    result = false;
                    break;
                }

                //两端指针向中间移动
                leftHead = leftHead.Next;
                rightHead = rightHead.Next;
            }

            // 重新连接并恢复后半部分链表的顺序
            var tail = preNode; //右半部分的头节点
            var currentNode = preNode?.Next; //设置右半部分的第二个节点为当前节点
            tail!.Next = null; //将右半部分的头节点设置为空
            while (currentNode != null)
            {
                var nextNode = currentNode.Next; //获取右侧的下一个节点
                currentNode.Next = preNode; //将当前节点的Next指向前一个节点
                preNode = currentNode; //前一个节点后移
                currentNode = nextNode; //当前节点后移
            }

            return result;
        }

        throw new InvalidOperationException();
    }

    public static void Run()
    {
        Node? head = null;
        Test(head);

        head = new Node(1);
        Test(head);

        head = new Node(1)
        {
            Next = new Node(2)
        };
        Test(head);

        head = new Node(1)
        {
            Next = new Node(1)
        };
        Test(head);

        head = new Node(1)
        {
            Next = new Node(2)
            {
                Next = new Node(3)
            }
        };
        Test(head);

        head = new Node(1)
        {
            Next = new Node(2)
            {
                Next = new Node(1)
            }
        };
        Test(head);

        head = new Node(1)
        {
            Next = new Node(2)
            {
                Next = new Node(3)
                {
                    Next = new Node(1)
                }
            }
        };
        Test(head);

        head = new Node(1)
        {
            Next = new Node(2)
            {
                Next = new Node(2)
                {
                    Next = new Node(1)
                }
            }
        };
        Test(head);

        head = new Node(1)
        {
            Next = new Node(2)
            {
                Next = new Node(3)
                {
                    Next = new Node(2)
                    {
                        Next = new Node(1)
                    }
                }
            }
        };
        Test(head);
    }

    # region 用于测试

    private static void PrintLinkedList(Node? node)
    {
        while (node != null)
        {
            Console.Write(node.Value + "->");
            node = node.Next;
        }

        Console.WriteLine("null");
    }

    private class Node(int data)
    {
        public readonly int Value = data;
        public Node? Next;
    }

    private static void Test(Node? head)
    {
        Console.Write("运行前的链表：");
        PrintLinkedList(head);
        var result1 = IsPalindrome1(head);
        var result2 = IsPalindrome2(head);
        var result3 = IsPalindrome3(head);
        Console.WriteLine(result1 == result2 && result2 == result3 ? "Pass" : "Error");
        Console.Write("复原后的链表：");
        PrintLinkedList(head);
        Console.WriteLine("=========================");
    }

    #endregion
}