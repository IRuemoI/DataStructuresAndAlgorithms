namespace Common.DataStructures.Graph;

public class Edge(int weight, Dot? from, Dot? to)
{
    //来向节点
    public readonly Dot? From = from;

    //去向节点
    public readonly Dot? To = to;

    //权重
    public readonly int Weight = weight;
}