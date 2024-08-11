namespace Common.DataStructures.Graph;

public class Edge
{
    //来向节点
    public readonly Dot? From;

    //去向节点
    public readonly Dot? To;

    //权重
    public readonly int Weight;

    public Edge(int weight, Dot? from, Dot? to)
    {
        Weight = weight;
        From = from;
        To = to;
    }
}