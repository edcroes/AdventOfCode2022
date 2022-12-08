namespace AoC.Common.Maps;

public static class PointExtensions
{
    public static int GetManhattenDistance(this Point point, Point other) =>
        Math.Abs(point.X - other.X) + Math.Abs(point.Y - other.Y);
}
