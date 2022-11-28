namespace AoC.Common.Maps;

public static class Point3DExtensions
{
    public static Point3D MirrorX(this Point3D point) => new(point.X * -1, point.Y, point.Z);

    public static Point3D MirrorY(this Point3D point) => new(point.X, point.Y * -1, point.Z);

    public static Point3D MirrorZ(this Point3D point) => new(point.X, point.Y, point.Z * -1);

    public static Point3D RotateX(this Point3D point) => new(point.Y, point.X * -1, point.Z);

    public static Point3D RotateY(this Point3D point) => new(point.Z * -1, point.Y, point.X);

    public static Point3D RotateZ(this Point3D point) => new(point.X, point.Z * -1, point.Y);

    public static Point3D MoveBy(this Point3D point, Point3D moveBy) => point + moveBy;

    public static int GetManhattenDistance(this Point3D point, Point3D other) => Math.Abs(point.X - other.X) + Math.Abs(point.Y - other.Y) + Math.Abs(point.Z - other.Z);
}