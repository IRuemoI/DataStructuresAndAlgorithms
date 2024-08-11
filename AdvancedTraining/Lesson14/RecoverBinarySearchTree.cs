#region

using System.Text;

#endregion

namespace AdvancedTraining.Lesson14;

// 本题测试链接 : https://leetcode.cn/problems/recover-binary-search-tree/
public class RecoverBinarySearchTree
{
    // 如果能过leetcode，只需要提交这个方法即可
    // 但其实recoverTree2才是正路，只不过leetcode没有那么考
    private static void RecoverTree(TreeNode root)
    {
        var errors = TwoErrors(root);
        if (errors[0] != null && errors[1] != null) (errors[0]!.Val, errors[1]!.Val) = (errors[1]!.Val, errors[0]!.Val);
    }

    private static TreeNode?[] TwoErrors(TreeNode? head)
    {
        var ans = new TreeNode[2];
        if (head == null) return ans;
        var cur = head;
        TreeNode? pre = null;
        TreeNode? e1 = null;
        TreeNode? e2 = null;
        while (cur != null)
        {
            var mostRight = cur.Left;
            if (mostRight != null)
            {
                while (mostRight.Right != null && mostRight.Right != cur) mostRight = mostRight.Right;
                if (mostRight.Right == null)
                {
                    mostRight.Right = cur;
                    cur = cur.Left;
                    continue;
                }

                mostRight.Right = null;
            }

            if (pre != null && pre.Val >= cur.Val)
            {
                e1 = e1 ?? pre;
                e2 = cur;
            }

            pre = cur;
            cur = cur.Right;
        }

        ans[0] = e1;
        ans[1] = e2;
        return ans;
    }

    // 以下的方法，提交leetcode是通过不了的，但那是因为leetcode的验证方式有问题
    // 但其实！以下的方法，才是正路！在结构上彻底交换两个节点，而不是值交换
    private static TreeNode RecoverTree2(TreeNode head)
    {
        var errs = getTwoErrNodes(head);
        var parents = getTwoErrParents(head, errs[0], errs[1]);
        var e1 = errs[0];
        var e1P = parents[0];
        var e1L = e1.Left;
        var e1R = e1.Right;
        var e2 = errs[1];
        var e2P = parents[1];
        var e2L = e2.Left;
        var e2R = e2.Right;
        if (e1 == head)
        {
            if (e1 == e2P)
            {
                e1.Left = e2L;
                e1.Right = e2R;
                e2.Right = e1;
                e2.Left = e1L;
            }
            else if (e2P.Left == e2)
            {
                e2P.Left = e1;
                e2.Left = e1L;
                e2.Right = e1R;
                e1.Left = e2L;
                e1.Right = e2R;
            }
            else
            {
                e2P.Right = e1;
                e2.Left = e1L;
                e2.Right = e1R;
                e1.Left = e2L;
                e1.Right = e2R;
            }

            head = e2;
        }
        else if (e2 == head)
        {
            if (e2 == e1P)
            {
                e2.Left = e1L;
                e2.Right = e1R;
                e1.Left = e2;
                e1.Right = e2R;
            }
            else if (e1P.Left == e1)
            {
                e1P.Left = e2;
                e1.Left = e2L;
                e1.Right = e2R;
                e2.Left = e1L;
                e2.Right = e1R;
            }
            else
            {
                e1P.Right = e2;
                e1.Left = e2L;
                e1.Right = e2R;
                e2.Left = e1L;
                e2.Right = e1R;
            }

            head = e1;
        }
        else
        {
            if (e1 == e2P)
            {
                if (e1P.Left == e1)
                {
                    e1P.Left = e2;
                    e1.Left = e2L;
                    e1.Right = e2R;
                    e2.Left = e1L;
                    e2.Right = e1;
                }
                else
                {
                    e1P.Right = e2;
                    e1.Left = e2L;
                    e1.Right = e2R;
                    e2.Left = e1L;
                    e2.Right = e1;
                }
            }
            else if (e2 == e1P)
            {
                if (e2P.Left == e2)
                {
                    e2P.Left = e1;
                    e2.Left = e1L;
                    e2.Right = e1R;
                    e1.Left = e2;
                    e1.Right = e2R;
                }
                else
                {
                    e2P.Right = e1;
                    e2.Left = e1L;
                    e2.Right = e1R;
                    e1.Left = e2;
                    e1.Right = e2R;
                }
            }
            else
            {
                if (e1P.Left == e1)
                {
                    if (e2P.Left == e2)
                    {
                        e1.Left = e2L;
                        e1.Right = e2R;
                        e2.Left = e1L;
                        e2.Right = e1R;
                        e1P.Left = e2;
                        e2P.Left = e1;
                    }
                    else
                    {
                        e1.Left = e2L;
                        e1.Right = e2R;
                        e2.Left = e1L;
                        e2.Right = e1R;
                        e1P.Left = e2;
                        e2P.Right = e1;
                    }
                }
                else
                {
                    if (e2P.Left == e2)
                    {
                        e1.Left = e2L;
                        e1.Right = e2R;
                        e2.Left = e1L;
                        e2.Right = e1R;
                        e1P.Right = e2;
                        e2P.Left = e1;
                    }
                    else
                    {
                        e1.Left = e2L;
                        e1.Right = e2R;
                        e2.Left = e1L;
                        e2.Right = e1R;
                        e1P.Right = e2;
                        e2P.Right = e1;
                    }
                }
            }
        }

        return head;
    }

    private static TreeNode?[] getTwoErrNodes(TreeNode? head)
    {
        var errs = new TreeNode?[2];
        if (head == null) return errs;
        var stack = new Stack<TreeNode>();
        TreeNode? pre = null;
        while (stack.Count > 0 || head != null)
            if (head != null)
            {
                stack.Push(head);
                head = head.Left;
            }
            else
            {
                head = stack.Pop();
                if (pre != null && pre.Val > head.Val)
                {
                    errs[0] = errs[0] == null ? pre : errs[0];
                    errs[1] = head;
                }

                pre = head;
                head = head.Right;
            }

        return errs;
    }

    private static TreeNode[] getTwoErrParents(TreeNode? head, TreeNode e1, TreeNode e2)
    {
        var parents = new TreeNode[2];
        if (head == null) return parents;
        var stack = new Stack<TreeNode>();
        while (stack.Count > 0 || head != null)
            if (head != null)
            {
                stack.Push(head);
                head = head.Left;
            }
            else
            {
                head = stack.Pop();
                if (head.Left == e1 || head.Right == e1) parents[0] = head;
                if (head.Left == e2 || head.Right == e2) parents[1] = head;
                head = head.Right;
            }

        return parents;
    }

    //用于测试 -- print tree
    private static void PrintTree(TreeNode head)
    {
        Console.WriteLine("Binary Tree:");
        PrintInOrder(head, 0, "H", 17);
        Console.WriteLine();
    }

    private static void PrintInOrder(TreeNode? head, int height, string to, int len)
    {
        if (head == null) return;
        PrintInOrder(head.Right, height + 1, "v", len);
        var val = to + head.Val + to;
        var lenM = val.Length;
        var lenL = (len - lenM) / 2;
        var lenR = len - lenM - lenL;
        val = GetSpace(lenL) + val + GetSpace(lenR);
        Console.WriteLine(GetSpace(height * len) + val);
        PrintInOrder(head.Left, height + 1, "^", len);
    }

    private static string GetSpace(int num)
    {
        var space = " ";
        var buf = new StringBuilder("");
        for (var i = 0; i < num; i++) buf.Append(space);
        return buf.ToString();
    }

    // 为了测试
    private static bool IsBst(TreeNode? head)
    {
        if (head == null) return false;
        var stack = new Stack<TreeNode>();
        TreeNode? pre = null;
        while (stack.Count > 0 || head != null)
            if (head != null)
            {
                stack.Push(head);
                head = head.Left;
            }
            else
            {
                head = stack.Pop();
                if (pre != null && pre.Val > head.Val) return false;
                pre = head;
                head = head.Right;
            }

        return true;
    }

    public static void Run()
    {
        var head = new TreeNode(5)
        {
            Left = new TreeNode(3),
            Right = new TreeNode(7)
        };
        head.Left.Left = new TreeNode(2);
        head.Left.Right = new TreeNode(4);
        head.Right.Left = new TreeNode(6);
        head.Right.Right = new TreeNode(8);
        head.Left.Left.Left = new TreeNode(1);
        PrintTree(head);
        Console.WriteLine(IsBst(head));

        Console.WriteLine("situation 1");
        var head1 = new TreeNode(7)
        {
            Left = new TreeNode(3),
            Right = new TreeNode(5)
        };
        head1.Left.Left = new TreeNode(2);
        head1.Left.Right = new TreeNode(4);
        head1.Right.Left = new TreeNode(6);
        head1.Right.Right = new TreeNode(8);
        head1.Left.Left.Left = new TreeNode(1);
        PrintTree(head1);
        Console.WriteLine(IsBst(head1));
        var res1 = RecoverTree2(head1);
        PrintTree(res1);
        Console.WriteLine(IsBst(res1));

        Console.WriteLine("situation 2");
        var head2 = new TreeNode(6)
        {
            Left = new TreeNode(3),
            Right = new TreeNode(7)
        };
        head2.Left.Left = new TreeNode(2);
        head2.Left.Right = new TreeNode(4);
        head2.Right.Left = new TreeNode(5);
        head2.Right.Right = new TreeNode(8);
        head2.Left.Left.Left = new TreeNode(1);
        PrintTree(head2);
        Console.WriteLine(IsBst(head2));
        var res2 = RecoverTree2(head2);
        PrintTree(res2);
        Console.WriteLine(IsBst(res2));

        Console.WriteLine("situation 3");
        var head3 = new TreeNode(8)
        {
            Left = new TreeNode(3),
            Right = new TreeNode(7)
        };
        head3.Left.Left = new TreeNode(2);
        head3.Left.Right = new TreeNode(4);
        head3.Right.Left = new TreeNode(6);
        head3.Right.Right = new TreeNode(5);
        head3.Left.Left.Left = new TreeNode(1);
        PrintTree(head3);
        Console.WriteLine(IsBst(head3));
        var res3 = RecoverTree2(head3);
        PrintTree(res3);
        Console.WriteLine(IsBst(res3));

        Console.WriteLine("situation 4");
        var head4 = new TreeNode(3)
        {
            Left = new TreeNode(5),
            Right = new TreeNode(7)
        };
        head4.Left.Left = new TreeNode(2);
        head4.Left.Right = new TreeNode(4);
        head4.Right.Left = new TreeNode(6);
        head4.Right.Right = new TreeNode(8);
        head4.Left.Left.Left = new TreeNode(1);
        PrintTree(head4);
        Console.WriteLine(IsBst(head4));
        var res4 = RecoverTree2(head4);
        PrintTree(res4);
        Console.WriteLine(IsBst(res4));

        Console.WriteLine("situation 5");
        var head5 = new TreeNode(2)
        {
            Left = new TreeNode(3),
            Right = new TreeNode(7)
        };
        head5.Left.Left = new TreeNode(5);
        head5.Left.Right = new TreeNode(4);
        head5.Right.Left = new TreeNode(6);
        head5.Right.Right = new TreeNode(8);
        head5.Left.Left.Left = new TreeNode(1);
        PrintTree(head5);
        Console.WriteLine(IsBst(head5));
        var res5 = RecoverTree2(head5);
        PrintTree(res5);
        Console.WriteLine(IsBst(res5));

        Console.WriteLine("situation 6");
        var head6 = new TreeNode(4)
        {
            Left = new TreeNode(3),
            Right = new TreeNode(7)
        };
        head6.Left.Left = new TreeNode(2);
        head6.Left.Right = new TreeNode(5);
        head6.Right.Left = new TreeNode(6);
        head6.Right.Right = new TreeNode(8);
        head6.Left.Left.Left = new TreeNode(1);
        PrintTree(head6);
        Console.WriteLine(IsBst(head6));
        var res6 = RecoverTree2(head6);
        PrintTree(res6);
        Console.WriteLine(IsBst(res6));

        Console.WriteLine("situation 7");
        var head7 = new TreeNode(5)
        {
            Left = new TreeNode(4),
            Right = new TreeNode(7)
        };
        head7.Left.Left = new TreeNode(2);
        head7.Left.Right = new TreeNode(3);
        head7.Right.Left = new TreeNode(6);
        head7.Right.Right = new TreeNode(8);
        head7.Left.Left.Left = new TreeNode(1);
        PrintTree(head7);
        Console.WriteLine(IsBst(head7));
        var res7 = RecoverTree2(head7);
        PrintTree(res7);
        Console.WriteLine(IsBst(res7));

        Console.WriteLine("situation 8");
        var head8 = new TreeNode(5)
        {
            Left = new TreeNode(3),
            Right = new TreeNode(8)
        };
        head8.Left.Left = new TreeNode(2);
        head8.Left.Right = new TreeNode(4);
        head8.Right.Left = new TreeNode(6);
        head8.Right.Right = new TreeNode(7);
        head8.Left.Left.Left = new TreeNode(1);
        PrintTree(head8);
        Console.WriteLine(IsBst(head8));
        var res8 = RecoverTree2(head8);
        PrintTree(res8);
        Console.WriteLine(IsBst(res8));

        Console.WriteLine("situation 9");
        var head9 = new TreeNode(5)
        {
            Left = new TreeNode(2),
            Right = new TreeNode(7)
        };
        head9.Left.Left = new TreeNode(3);
        head9.Left.Right = new TreeNode(4);
        head9.Right.Left = new TreeNode(6);
        head9.Right.Right = new TreeNode(8);
        head9.Left.Left.Left = new TreeNode(1);
        PrintTree(head9);
        Console.WriteLine(IsBst(head9));
        var res9 = RecoverTree2(head9);
        PrintTree(res9);
        Console.WriteLine(IsBst(res9));

        Console.WriteLine("situation 10");
        var head10 = new TreeNode(5)
        {
            Left = new TreeNode(3),
            Right = new TreeNode(6)
        };
        head10.Left.Left = new TreeNode(2);
        head10.Left.Right = new TreeNode(4);
        head10.Right.Left = new TreeNode(7);
        head10.Right.Right = new TreeNode(8);
        head10.Left.Left.Left = new TreeNode(1);
        PrintTree(head10);
        Console.WriteLine(IsBst(head10));
        var res10 = RecoverTree2(head10);
        PrintTree(res10);
        Console.WriteLine(IsBst(res10));

        Console.WriteLine("situation 11");
        var head11 = new TreeNode(5)
        {
            Left = new TreeNode(3),
            Right = new TreeNode(7)
        };
        head11.Left.Left = new TreeNode(6);
        head11.Left.Right = new TreeNode(4);
        head11.Right.Left = new TreeNode(2);
        head11.Right.Right = new TreeNode(8);
        head11.Left.Left.Left = new TreeNode(1);
        PrintTree(head11);
        Console.WriteLine(IsBst(head11));
        var res11 = RecoverTree2(head11);
        PrintTree(res11);
        Console.WriteLine(IsBst(res11));

        Console.WriteLine("situation 12");
        var head12 = new TreeNode(5)
        {
            Left = new TreeNode(3),
            Right = new TreeNode(7)
        };
        head12.Left.Left = new TreeNode(8);
        head12.Left.Right = new TreeNode(4);
        head12.Right.Left = new TreeNode(6);
        head12.Right.Right = new TreeNode(2);
        head12.Left.Left.Left = new TreeNode(1);
        PrintTree(head12);
        Console.WriteLine(IsBst(head12));
        var res12 = RecoverTree2(head12);
        PrintTree(res12);
        Console.WriteLine(IsBst(res12));

        Console.WriteLine("situation 13");
        var head13 = new TreeNode(5)
        {
            Left = new TreeNode(3),
            Right = new TreeNode(7)
        };
        head13.Left.Left = new TreeNode(2);
        head13.Left.Right = new TreeNode(6);
        head13.Right.Left = new TreeNode(4);
        head13.Right.Right = new TreeNode(8);
        head13.Left.Left.Left = new TreeNode(1);
        PrintTree(head13);
        Console.WriteLine(IsBst(head13));
        var res13 = RecoverTree2(head13);
        PrintTree(res13);
        Console.WriteLine(IsBst(res13));

        Console.WriteLine("situation 14");
        var head14 = new TreeNode(5)
        {
            Left = new TreeNode(3),
            Right = new TreeNode(7)
        };
        head14.Left.Left = new TreeNode(2);
        head14.Left.Right = new TreeNode(8);
        head14.Right.Left = new TreeNode(6);
        head14.Right.Right = new TreeNode(4);
        head14.Left.Left.Left = new TreeNode(1);
        PrintTree(head14);
        Console.WriteLine(IsBst(head14));
        var res14 = RecoverTree2(head14);
        PrintTree(res14);
        Console.WriteLine(IsBst(res14));
    }

    // 不要提交这个类
    public class TreeNode(int v)
    {
        public TreeNode? Left;
        public TreeNode? Right;
        public int Val = v;
    }
}