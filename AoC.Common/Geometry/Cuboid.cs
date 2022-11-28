namespace AoC.Common.Geometry;

public readonly struct Cuboid : IEquatable<Cuboid>
{
    public Point3D From { get; }
    public Point3D To { get; }

    public long CubeCount =>
        (To.X - From.X + 1L) *
        (To.Y - From.Y + 1L) *
        (To.Z - From.Z + 1L);

    public Cuboid(Point3D from, Point3D to)
    {
        From = new(Math.Min(from.X, to.X), Math.Min(from.Y, to.Y), Math.Min(from.Z, to.Z));
        To = new(Math.Max(from.X, to.X), Math.Max(from.Y, to.Y), Math.Max(from.Z, to.Z));
    }

    public static Cuboid Empty => new(Point3D.Empty, Point3D.Empty);

    public Cuboid Intersect(Cuboid other)
    {
        if (!HasOverlapWith(other))
        {
            return Empty;
        }

        var from = new Point3D(
            Math.Max(From.X, other.From.X),
            Math.Max(From.Y, other.From.Y),
            Math.Max(From.Z, other.From.Z)
        );
        var to = new Point3D(
            Math.Min(To.X, other.To.X),
            Math.Min(To.Y, other.To.Y),
            Math.Min(To.Z, other.To.Z)
        );

        return new(from, to);
    }

    public bool HasOverlapWith(Cuboid other) =>
        From.X <= other.To.X && To.X >= other.From.X &&
        From.Y <= other.To.Y && To.Y >= other.From.Y &&
        From.Z <= other.To.Z && To.Z >= other.From.Z;

    public bool Contains(Cuboid other) =>
        From.X <= other.From.X && To.X >= other.To.X &&
        From.Y <= other.From.Y && To.Y >= other.To.Y &&
        From.Z <= other.From.Z && To.Z >= other.To.Z;

    public List<Cuboid> Explode(Cuboid other)
    {
        List<Cuboid> subCuboids = new();
        if (!HasOverlapWith(other))
        {
            subCuboids.Add(this);
            return subCuboids;
        }

        if (!Contains(other))
        {
            other = Intersect(other);
        }

        if (this == other)
        {
            return subCuboids;
        }

        var remainder = this;

        if (remainder.From.X < other.From.X)
        {
            (var left, remainder) = remainder.SliceLeftOfX(other.From.X);
            subCuboids.Add(left);
        }

        if (other.To.X < remainder.To.X)
        {
            (remainder, var right) = remainder.SliceLeftOfX(other.To.X + 1);
            subCuboids.Add(right);
        }

        if (remainder.From.Y < other.From.Y)
        {
            (var left, remainder) = remainder.SliceLeftOfY(other.From.Y);
            subCuboids.Add(left);
        }

        if (other.To.Y < remainder.To.Y)
        {
            (remainder, var right) = remainder.SliceLeftOfY(other.To.Y + 1);
            subCuboids.Add(right);
        }

        if (remainder.From.Z < other.From.Z)
        {
            (var left, remainder) = remainder.SliceLeftOfZ(other.From.Z);
            subCuboids.Add(left);
        }

        if (other.To.Z < remainder.To.Z)
        {
            (remainder, var right) = remainder.SliceLeftOfZ(other.To.Z + 1);
            subCuboids.Add(right);
        }

        if (remainder != other)
        {
            throw new InvalidOperationException("That sucks... the remaining cube should be the cube to explode");
        }

        return subCuboids;
    }

    public (Cuboid, Cuboid) SliceLeftOfX(int x)
    {
        var leftTo = new Point3D(x - 1, To.Y, To.Z);
        var left = new Cuboid(From, leftTo);

        var rightFrom = new Point3D(x, From.Y, From.Z);
        var right = new Cuboid(rightFrom, To);

        return (left, right);
    }

    public (Cuboid, Cuboid) SliceLeftOfY(int y)
    {
        var leftTo = new Point3D(To.X, y - 1, To.Z);
        var left = new Cuboid(From, leftTo);

        var rightFrom = new Point3D(From.X, y, From.Z);
        var right = new Cuboid(rightFrom, To);

        return (left, right);
    }

    public (Cuboid, Cuboid) SliceLeftOfZ(int z)
    {
        var leftTo = new Point3D(To.X, To.Y, z - 1);
        var left = new Cuboid(From, leftTo);

        var rightFrom = new Point3D(From.X, From.Y, z);
        var right = new Cuboid(rightFrom, To);

        return (left, right);
    }

    public override bool Equals(object? obj) =>
        obj is Cuboid cube && Equals(cube);

    public bool Equals(Cuboid other) =>
        From == other.From && To == other.To;

    public override int GetHashCode() =>
        HashCode.Combine(From, To);

    public static bool operator ==(Cuboid left, Cuboid right) =>
        left.Equals(right);

    public static bool operator !=(Cuboid left, Cuboid right) =>
        !left.Equals(right);
}