namespace AoC.Common.Graphs;

public class WeightedGraph<T> where T : IEdge
{
    public readonly Dictionary<T, List<T>> _edges = new();

    public IReadOnlyList<T> Edges => _edges.Keys.ToList();

    public void AddEdge(T from, T to)
    {
        if (!_edges.TryGetValue(from, out List<T>? fromList) || !fromList.Contains(to))
            _edges.AddOrUpdate(from, to);

        if (!_edges.TryGetValue(to, out List<T>? toList) || !toList.Contains(from))
            _edges.AddOrUpdate(to, from);
    }

    public void AddDirectedEdge(T from, T to)
    {
        if (!_edges.TryGetValue(from, out List<T>? fromList) || !fromList.Contains(to))
            _edges.AddOrUpdate(from, to);
    }

    public int GetShortestPath(T from, T to)
    {
        Dictionary<T, int> currentCostPerEdge = new();
        HashSet<T> visitedPoints = new();
        PriorityQueue<T, int> openPositions = new();
        openPositions.Enqueue(from, 0);

        while (currentCostPerEdge.GetValueOrDefault(to, 0) == 0)
        {
            var edge = openPositions.Dequeue();
            var cost = currentCostPerEdge.GetValueOrDefault(edge, 0);

            visitedPoints.Add(edge);
            var nextEdges = _edges[edge].Where(e => !visitedPoints.Contains(e));

            foreach (var nextEdge in nextEdges)
            {
                var nextCost = nextEdge.Weight + cost;
                var currentCost = currentCostPerEdge.GetValueOrDefault(nextEdge, 0);
                if (currentCost == 0 || nextCost < currentCost)
                {
                    currentCostPerEdge.AddOrUpdate(nextEdge, nextCost);
                    openPositions.Enqueue(nextEdge, nextCost);
                }
            }
        }

        return currentCostPerEdge[to];
    }

    public List<List<T>> GetAllPathsWithAllEdges(T start, T to, int maxWeight)
    {
        List<List<T>> finishedPaths = new();
        List<List<T>> openPaths = new() { new() { start } };

        for (var i = 0; i < _edges.Count - 1; i++)
        {
            List<List<T>> newPaths = new();
            foreach (var path in openPaths)
            {
                var newEdges = _edges[path[^1]].Where(e => !path.Contains(e));
                foreach (var newEdge in newEdges)
                {
                    List<T> newPath = new(path) { newEdge };

                    if (newPath.Sum(e => e.Weight) < maxWeight)
                    {
                        if (to.Equals(newEdge))
                            finishedPaths.Add(newPath);
                        else
                            newPaths.Add(newPath);
                    }

                    
                }
            }
            openPaths = newPaths;
        }

        return finishedPaths;
    }
}