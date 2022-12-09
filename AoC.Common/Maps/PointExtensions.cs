namespace AoC.Common.Maps;

public static class PointExtensions
{
    public static int GetManhattenDistance(this Point point, Point other) =>
        Math.Abs(point.X - other.X) + Math.Abs(point.Y - other.Y);

    public static Point Add(this Point left, Point right) =>
        new(left.X + right.X, left.Y + right.Y);

    public static Point Subtract(this Point left, Point right) =>
        new(left.X - right.X, left.Y - right.Y);

    public static bool IsTouching(this Point point, Point other) =>
        Math.Abs(point.X - other.X) <= 1 && Math.Abs(point.Y - other.Y) <= 1;
}
