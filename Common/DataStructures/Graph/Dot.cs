namespace Common.DataStructures.Graph;

// 点结构的描述
public class Dot(int label)
{
    public readonly List<Edge> BelongEdgeList = [];

    //标签
    public readonly int Label = label;

    public readonly List<Dot?> PointToList = [];

    //入度
    public int InDegree = 0;

    //出度
    public int OutDegree = 0;
}