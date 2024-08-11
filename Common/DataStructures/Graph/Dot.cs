namespace Common.DataStructures.Graph;

// 点结构的描述
public class Dot
{
    public readonly List<Edge> BelongEdgeList;

    //标签
    public readonly int Label;

    public readonly List<Dot?> PointToList;

    //入度
    public int InDegree;

    //出度
    public int OutDegree;

    public Dot(int label)
    {
        Label = label;
        InDegree = 0;
        OutDegree = 0;
        PointToList = new List<Dot?>();
        BelongEdgeList = new List<Edge>();
    }
}