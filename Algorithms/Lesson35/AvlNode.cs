namespace Algorithms.Lesson35;

public class AvlNode<TK, TV>(TK key, TV value)
    where TK : IComparable<TK>, IEquatable<TK>
{
    public readonly TK K = key;
    public int H = 1;
    public AvlNode<TK, TV>? LeftChild;
    public AvlNode<TK, TV>? RightChild;
    public TV V = value;
}