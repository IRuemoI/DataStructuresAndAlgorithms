namespace Common.DataStructures.Graph;

public class Graph(GraphType graphType = GraphType.Directed)
{
    //图中所有节点标签与节点的字典
    public readonly Dictionary<int, Dot?> Dots = new();

    //图中所有边的集合
    public readonly HashSet<Edge> Edges = new();

    public GraphType GraphType { get; } = graphType;
}