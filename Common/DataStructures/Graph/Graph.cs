namespace Common.DataStructures.Graph;

public class Graph
{
    //图中所有节点标签与节点的字典
    public readonly Dictionary<int, Dot?> Dots;

    //图中所有边的集合
    public readonly HashSet<Edge> Edges;

    public Graph(GraphType graphType = GraphType.Directed)
    {
        GraphType = graphType;
        Dots = new Dictionary<int, Dot?>();
        Edges = new HashSet<Edge>();
    }

    public GraphType GraphType { get; }
}