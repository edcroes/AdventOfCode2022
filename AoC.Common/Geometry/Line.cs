namespace AoC.Common.Geometry;

public readonly struct Line
{
    public Point From { get; init; }
    public Point To { get; init; }

    public Line(Point from, Point to)
    {
        From = from;
        To = to;
    }

    public double Length
    {
        get
        {
            var lengthX = Math.Abs(To.X - From.X);
            var lengthY = Math.Abs(To.Y - From.Y);
            if (lengthY == 0) return lengthX;
            if (lengthX == 0) return lengthY;
            return Math.Sqrt(lengthX * lengthX + lengthY * lengthY);
        }
    }

    public double Angle
    {
        get
        {
            var angle = Math.Atan2(To.Y - From.Y, To.X - From.X);
            return (angle >= 0 ? angle : 2 * Math.PI + angle) * 360 / (2 * Math.PI);
        }
    }

    public PointF? GetIntersectionWithLine(Line other)
    {
        var thisA = From.X - To.X;
        var thisB = From.Y - To.Y;
        var thisC = From.X * To.Y - From.Y * To.X;

        var otherA = other.From.X - other.To.X;
        var otherB = other.From.Y - other.To.Y;
        var otherC = other.From.X * other.To.Y - other.From.Y * other.To.X;

        var determinant = thisA * otherB - thisB * otherA;

        if (determinant == 0)
        {
            return null;
        }

        var x = (thisC * otherA - thisA * otherC) / (float)determinant;
        var y = (thisC * otherB - thisB * otherC) / (float)determinant;

        return new PointF(x, y);
    }

    public PointF? GetIntersectionWithLineSegment(Line other)
    {
        var intersectionAt = GetIntersectionWithLine(other);
        if (intersectionAt == null)
        {
            return null;
        }

        return IsPointInLineRectangle(intersectionAt.Value) && other.IsPointInLineRectangle(intersectionAt.Value)
            ? intersectionAt
            : null;
    }

    public IEnumerable<Point> GetLinePoints()
    {
        if (From.X != To.X && From.Y != To.Y && Math.Abs(From.Y - To.Y) != Math.Abs(From.X - To.X))
        {
            throw new InvalidOperationException("Only points for horizontal, vertical or 45 degree lines can be determined");
        }

        List<Point> points = new() { From };
        var xStep = From.X < To.X ? 1 : From.X > To.X ? -1 : 0;
        var yStep = From.Y < To.Y ? 1 : From.Y > To.Y ? -1 : 0;

        var nextPoint = From;
        while (nextPoint != To)
        {
            nextPoint = new Point(nextPoint.X + xStep, nextPoint.Y + yStep);
            points.Add(nextPoint);
        }

        return points;
    }

    private bool IsPointInLineRectangle(PointF point) =>
            point.X >= Math.Min(From.X, To.X) &&
            point.X <= Math.Max(From.X, To.X) &&
            point.Y >= Math.Min(From.Y, To.Y) &&
            point.Y <= Math.Max(From.Y, To.Y);

    public override string ToString() =>
        $"{From.X},{From.Y} -> {To.X},{To.Y}";
}