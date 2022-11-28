namespace AoC.Common.Maps;

public class Space3D
{
    private readonly List<Point3D> _points = new();
    private int[] _cornerPointIndeces = Array.Empty<int>();

    public string Name { get; init; } = "unnamed";

    public Point3D Center { get; private set; } = new(0, 0, 0);

    public Space3D() { }

    public Space3D(IEnumerable<Point3D> points)
    {
        AddRange(points);
    }

    public IReadOnlyList<Point3D> Points => _points;
    public IReadOnlyList<Point3D> CornerPoints => _cornerPointIndeces.Select(i => _points[i]).ToList();

    public void Add(Point3D point)
    {
        AddPoint(point);
        ReindexCorners();
    }

    public void AddRange(IEnumerable<Point3D> points)
    {
        foreach (var point in points)
        {
            AddPoint(point);
        }

        ReindexCorners();
    }

    public Space3D RotateX()
    {
        for (var i = 0; i < _points.Count; i++)
        {
            _points[i] = _points[i].RotateX();
        }
        Center = Center.RotateX();

        return this;
    }

    public Space3D RotateY()
    {
        for (var i = 0; i < _points.Count; i++)
        {
            _points[i] = _points[i].RotateY();
        }
        Center = Center.RotateY();

        return this;
    }

    public Space3D RotateZ()
    {
        for (var i = 0; i < _points.Count; i++)
        {
            _points[i] = _points[i].RotateZ();
        }
        Center = Center.RotateZ();

        return this;
    }

    public Space3D MoveBy(Point3D moveBy)
    {
        for (var i = 0; i < _points.Count; i++)
        {
            _points[i] = _points[i].MoveBy(moveBy);
        }
        Center = Center.MoveBy(moveBy);

        return this;
    }

    private void AddPoint(Point3D point)
    {
        if (!_points.Contains(point))
        {
            _points.Add(point);
        }
    }

    private void ReindexCorners()
    {
        var cornerPoints = new[] {
            _points.Where(p => (p - Center).X < 0 && (p - Center).Y < 0 && (p - Center).Z < 0).OrderBy(p => p.GetManhattenDistance(Center)).LastOrDefault(),
            _points.Where(p => (p - Center).X < 0 && (p - Center).Y < 0 && (p - Center).Z > 0).OrderBy(p => p.GetManhattenDistance(Center)).LastOrDefault(),
            _points.Where(p => (p - Center).X < 0 && (p - Center).Y > 0 && (p - Center).Z < 0).OrderBy(p => p.GetManhattenDistance(Center)).LastOrDefault(),
            _points.Where(p => (p - Center).X > 0 && (p - Center).Y < 0 && (p - Center).Z < 0).OrderBy(p => p.GetManhattenDistance(Center)).LastOrDefault(),
            _points.Where(p => (p - Center).X < 0 && (p - Center).Y > 0 && (p - Center).Z > 0).OrderBy(p => p.GetManhattenDistance(Center)).LastOrDefault(),
            _points.Where(p => (p - Center).X > 0 && (p - Center).Y < 0 && (p - Center).Z > 0).OrderBy(p => p.GetManhattenDistance(Center)).LastOrDefault(),
            _points.Where(p => (p - Center).X > 0 && (p - Center).Y > 0 && (p - Center).Z < 0).OrderBy(p => p.GetManhattenDistance(Center)).LastOrDefault(),
            _points.Where(p => (p - Center).X > 0 && (p - Center).Y > 0 && (p - Center).Z > 0).OrderBy(p => p.GetManhattenDistance(Center)).LastOrDefault()
        };

        _cornerPointIndeces = cornerPoints
            .Where(p => p != default)
            .Select(p => _points.IndexOf(p))
            .ToArray();
    }
}