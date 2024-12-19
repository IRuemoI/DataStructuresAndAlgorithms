namespace Common.DataStructures.Graph;

public static class GraphGenerator
{
    // matrix 所有的边
    // N*3 的矩阵
    // [weight:权重, from:来向节点索引，to:去向节点索引]
    // 
    // [ 5 , 0 , 7]
    // [ 3 , 0,  1]

    public static Graph CreateGraph(int[,] matrix, GraphType graphType)
    {
        var graph = new Graph(graphType);
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            // 拿到每一条边， matrix[i] 
            var weight = matrix[i, 0];
            var from = matrix[i, 1];
            var to = matrix[i, 2];
            if (!graph.Dots.ContainsKey(from)) graph.Dots.Add(from, new Dot(from));
            if (!graph.Dots.ContainsKey(to)) graph.Dots.Add(to, new Dot(to));

            var fromDot = graph.Dots[from];
            var toDot = graph.Dots[to];

            switch (graph.GraphType)
            {
                case GraphType.Directed:
                    var newDirectedEdge = new Edge(weight, fromDot, toDot);
                    fromDot?.PointToList.Add(toDot);
                    if (fromDot != null && toDot != null)
                    {
                        fromDot.OutDegree++;
                        toDot.InDegree++;
                        fromDot.BelongEdgeList.Add(newDirectedEdge);
                    }

                    graph.Edges.Add(newDirectedEdge);
                    break;
                case GraphType.Undirected:
                    var newDirectedEdge1 = new Edge(weight, fromDot, toDot);
                    var newDirectedEdge2 = new Edge(weight, toDot, fromDot);
                    fromDot?.PointToList.Add(toDot);
                    toDot?.PointToList.Add(fromDot);
                    if (fromDot != null && toDot != null)
                    {
                        fromDot.OutDegree++;
                        fromDot.InDegree++;

                        toDot.OutDegree++;
                        toDot.InDegree++;

                        toDot.BelongEdgeList.Add(newDirectedEdge2);
                        fromDot.BelongEdgeList.Add(newDirectedEdge1);
                    }

                    graph.Edges.Add(newDirectedEdge1);
                    graph.Edges.Add(newDirectedEdge2);
                    break;
                default:
                    throw new AggregateException("未指定图类型");
            }
        }

        return graph;
    }
}