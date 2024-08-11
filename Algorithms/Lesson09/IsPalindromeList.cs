//测试通过

namespace Algorithms.Lesson09;

public class IsPalindromeList
{
    // need n extra space
    private static bool IsPalindrome1(Node? head)
    {
        var stack = new Stack<Node>();
        var cur = head;
        while (cur != null)
        {
            stack.Push(cur);
            cur = cur.Next;
        }

        while (head != null)
        {
            if (head.Value != stack.Pop().Value) return false;

            if (head.Next != null) head = head.Next;
        }

        return true;
    }

    // need n/2 extra space
    private static bool IsPalindrome2(Node? head)
    {
        if (head?.Next == null) return true;

        var right = head.Next;
        var cur = head;
        while (cur.Next is { Next: not null })
        {
            right = right?.Next;
            cur = cur.Next.Next;
        }

        var stack = new Stack<Node>();
        while (right != null)
        {
            stack.Push(right);
            right = right.Next;
        }

        while (stack.Count != 0)
        {
            if (head?.Value != stack.Pop().Value) return false;

            head = head.Next;
        }

        return true;
    }

    // need O(1) extra space
    private static bool IsPalindrome3(Node? head)
    {
        if (head == null || head.Next == null) return true;

        var n1 = head;
        var n2 = head;
        while (n2.Next is { Next: not null })
        {
            // find mid node
            n1 = n1?.Next; // n1 -> mid
            n2 = n2.Next.Next; // n2 -> end
        }
        // n1 中点


        n2 = n1?.Next; // n2 -> right part first node
        if (n1 != null)
        {
            n1.Next = null; // mid.next -> null
            Node? n3;
            while (n2 != null)
            {
                // right part convert
                n3 = n2.Next; // n3 -> save next node
                n2.Next = n1; // next of right node convert
                n1 = n2; // n1 move
                n2 = n3; // n2 move
            }

            n3 = n1; // n3 -> save last node
            n2 = head; // n2 -> left first node
            var res = true;
            while (n1 != null && n2 != null)
            {
                // check palindrome
                if (n1.Value != n2.Value)
                {
                    res = false;
                    break;
                }

                n1 = n1.Next; // left to mid
                n2 = n2.Next; // right to mid
            }

            n1 = n3.Next;
            n3.Next = null;
            while (n1 != null)
            {
                // recover list
                n2 = n1.Next;
                n1.Next = n3;
                n3 = n1;
                n1 = n2;
            }

            return res;
        }

        throw new InvalidOperationException();
    }

    private static void PrintLinkedList(Node? node)
    {
        Console.Write("Linked List: ");
        while (node != null)
        {
            Console.Write(node.Value + " ");
            node = node.Next;
        }

        Console.WriteLine();
    }

    public static void Run()
    {
        Node? head = null;
        PrintLinkedList(head);
        Console.Write(IsPalindrome1(head) + " | ");
        Console.Write(IsPalindrome2(head) + " | ");
        Console.WriteLine(IsPalindrome3(head) + " | ");
        PrintLinkedList(head);
        Console.WriteLine("=========================");

        head = new Node(1);
        PrintLinkedList(head);
        Console.Write(IsPalindrome1(head) + " | ");
        Console.Write(IsPalindrome2(head) + " | ");
        Console.WriteLine(IsPalindrome3(head) + " | ");
        PrintLinkedList(head);
        Console.WriteLine("=========================");

        head = new Node(1)
        {
            Next = new Node(2)
        };
        PrintLinkedList(head);
        Console.Write(IsPalindrome1(head) + " | ");
        Console.Write(IsPalindrome2(head) + " | ");
        Console.WriteLine(IsPalindrome3(head) + " | ");
        PrintLinkedList(head);
        Console.WriteLine("=========================");

        head = new Node(1)
        {
            Next = new Node(1)
        };
        PrintLinkedList(head);
        Console.Write(IsPalindrome1(head) + " | ");
        Console.Write(IsPalindrome2(head) + " | ");
        Console.WriteLine(IsPalindrome3(head) + " | ");
        PrintLinkedList(head);
        Console.WriteLine("=========================");

        head = new Node(1)
        {
            Next = new Node(2)
            {
                Next = new Node(3)
            }
        };
        PrintLinkedList(head);
        Console.Write(IsPalindrome1(head) + " | ");
        Console.Write(IsPalindrome2(head) + " | ");
        Console.WriteLine(IsPalindrome3(head) + " | ");
        PrintLinkedList(head);
        Console.WriteLine("=========================");

        head = new Node(1)
        {
            Next = new Node(2)
            {
                Next = new Node(1)
            }
        };
        PrintLinkedList(head);
        Console.Write(IsPalindrome1(head) + " | ");
        Console.Write(IsPalindrome2(head) + " | ");
        Console.WriteLine(IsPalindrome3(head) + " | ");
        PrintLinkedList(head);
        Console.WriteLine("=========================");

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
        PrintLinkedList(head);
        Console.Write(IsPalindrome1(head) + " | ");
        Console.Write(IsPalindrome2(head) + " | ");
        Console.WriteLine(IsPalindrome3(head) + " | ");
        PrintLinkedList(head);
        Console.WriteLine("=========================");

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
        PrintLinkedList(head);
        Console.Write(IsPalindrome1(head) + " | ");
        Console.Write(IsPalindrome2(head) + " | ");
        Console.WriteLine(IsPalindrome3(head) + " | ");
        PrintLinkedList(head);
        Console.WriteLine("=========================");

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
        PrintLinkedList(head);
        Console.Write(IsPalindrome1(head) + " | ");
        Console.Write(IsPalindrome2(head) + " | ");
        Console.WriteLine(IsPalindrome3(head) + " | ");
        PrintLinkedList(head);
        Console.WriteLine("=========================");
    }

    public class Node
    {
        public readonly int Value;
        public Node? Next;

        public Node(int data)
        {
            Value = data;
        }
    }
}