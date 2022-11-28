﻿namespace AoC.Common.Maps;

public static class MapGraphExtensions
{
    public static int GetShortestPath(this Map<int> map, Point toPoint) =>
        map.GetShortestPath(new Point(0, 0), toPoint);

    public static int GetShortestPath(this Map<int> map, Point fromPoint, Point toPoint)
    {
        var currentCostPerPoint = new Map<int>(map.SizeX, map.SizeY);
        HashSet<Point> visitedPoints = new();
        PriorityQueue<Point, int> openPositions = new();
        openPositions.Enqueue(fromPoint, 0);

        while (currentCostPerPoint.GetValue(toPoint) == 0)
        {
            var point = openPositions.Dequeue();
            var cost = currentCostPerPoint.GetValue(point);

            visitedPoints.Add(point);
            var nextPoints = map
                .GetStraightNeighbors(point)
                .Where(p => !visitedPoints.Contains(p));

            foreach (var nextPoint in nextPoints)
            {
                var nextCost = map.GetValue(nextPoint) + cost;
                var currentCost = currentCostPerPoint.GetValue(nextPoint);
                if (currentCost == 0 || nextCost < currentCost)
                {
                    currentCostPerPoint.SetValue(nextPoint, nextCost);
                    openPositions.Enqueue(nextPoint, nextCost);
                }
            }
        }

        return currentCostPerPoint.GetValue(toPoint);
    }
}