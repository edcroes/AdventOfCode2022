namespace AoC.Common.Maps;

public class PointMap<T>
{
    private readonly Dictionary<Point, T> _points = new();

    public PointMap() { }

    public PointMap(T[][] values, bool insertDefaultValue = false)
    {
        for (var y = 0; y < values.Length; y++)
        {
            for (var x = 0; x < values.Min(l => l.Length); x++)
            {
                if ((values[y][x] != null && !values[y][x]!.Equals(default)) || insertDefaultValue)
                {
                    SetValue(new(x, y), values[y][x]);
                }
            }
        }
    }

    public List<Point> Points =>
        _points.Keys.ToList();

    public T GetValue(int x, int y) =>
        GetValue(new(x, y));

    public T GetValue(Point point) =>
        _points.TryGetValue(point, out T? value) ? value : throw new ArgumentOutOfRangeException(nameof(point));

    public T? GetValueOrDefault(int x, int y, T defaultValue = default) =>
        GetValueOrDefault(new(x, y), defaultValue);

    public T? GetValueOrDefault(Point point, T defaultValue = default) =>
        _points.TryGetValue(point, out T? value) ? value : defaultValue;

    public void SetValue(int x, int y, T value) =>
        SetValue(new(x, y), value);

    public void SetValue(Point point, T value) =>
        _points.AddOrSet(point, value);

    public void RemoveValue(Point point) =>
        _points.Remove(point);

    public IEnumerable<Point> GetStraightAndDiagonalNeighbors(Point point)
    {
        List<Point> neighbors = new();

        for (int y = point.Y - 1; y <= point.Y + 1; y++)
        {
            for (int x = point.X - 1; x <= point.X + 1; x++)
            {
                if (y == point.Y && x == point.X)
                {
                    continue;
                }

                neighbors.Add(new Point(x, y));
            }
        }

        return neighbors;
    }

    public int NumberOfStraightAndDiagonalNeighborsThatMatch(Point point, T valueToMatch) =>
        NumberOfStraightAndDiagonalNeighborsThatMatch(point, p => _points.TryGetValue(p, out T? value) && valueToMatch.Equals(value));

    public int NumberOfStraightAndDiagonalNeighborsThatMatch(Point point, Func<Point, bool> getMatch) =>
        GetStraightAndDiagonalNeighbors(point).Count(getMatch);

    public Rectangle GetBoundingRectangle()
    {
        var minX = _points.Keys.Min(p => p.X);
        var maxX = _points.Keys.Max(p => p.X);
        var minY = _points.Keys.Min(p => p.Y);
        var maxY = _points.Keys.Max(p => p.Y);

        return new(minX, minY, maxX - minX, maxY - minY);
    }
}
