namespace AoC.Common.Maps;

public struct Point3D : IEquatable<Point3D>
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public Point3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static Point3D Empty => new(0, 0, 0);

    public override bool Equals(object? obj)
    {
        return obj is Point3D p && Equals(p);
    }

    public bool Equals(Point3D other) => X == other.X && Y == other.Y && Z == other.Z;

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public override string ToString() => $"({X}, {Y}, {Z})";

    public static bool operator ==(Point3D left, Point3D right) => left.Equals(right);

    public static bool operator !=(Point3D left, Point3D right) => !left.Equals(right);

    public static Point3D operator +(Point3D left, Point3D right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

    public static Point3D operator -(Point3D left, Point3D right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
}