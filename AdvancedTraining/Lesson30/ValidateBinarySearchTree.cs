namespace AdvancedTraining.Lesson30;

public class ValidateBinarySearchTree //Problem_0098
{
    public virtual bool IsValidBst(TreeNode? root)
    {
        if (root == null) return true;
        var cur = root;
        int? pre = null;
        var ans = true;
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

            if (pre != null && pre.Value >= cur.Val) ans = false;
            pre = cur.Val;
            cur = cur.Right;
        }

        return ans;
    }

    public class TreeNode
    {
        internal TreeNode? Left;
        internal TreeNode? Right;
        internal int Val;
    }
}