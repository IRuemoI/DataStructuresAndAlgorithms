namespace Algorithms.Lesson35;

public class AvlNode<TK, TV> where TK : IComparable<TK>, IEquatable<TK>
{
    public readonly TK K;
    public int H;
    public AvlNode<TK, TV>? LeftChild;
    public AvlNode<TK, TV>? RightChild;
    public TV V;

    public AvlNode(TK key, TV value)
    {
        K = key;
        V = value;
        H = 1;
    }
}