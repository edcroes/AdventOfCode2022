namespace AoC.Common.Maps;

public static class Map3DExtensions
{
    public static IEnumerable<Point3D> Where<T>(this Map3D<T> map, Func<Point3D, T, bool> predicate)
    {
        List<Point3D> points = new();

        for (var z = 0; z < map.SizeZ; z++)
        {
            for (var y = 0; y < map.SizeY; y++)
            {
                for (var x = 0; x < map.SizeX; x++)
                {
                    Point3D point = new(x, y, z);
                    if (predicate(point, map.GetValue(point)))
                    {
                        points.Add(point);
                    }
                }
            }
        }

        return points;
    }

    public static bool All<T>(this Map3D<T> map, Func<Point3D, T, bool> predicate)
    {
        for (var z = 0; z < map.SizeZ; z++)
        {
            for (var y = 0; y < map.SizeY; y++)
            {
                for (var x = 0; x < map.SizeX; x++)
                {
                    Point3D point = new(x, y, z);
                    if (!predicate(point, map.GetValue(point)))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public static bool Any<T>(this Map3D<T> map, Func<Point3D, T, bool> predicate)
    {
        for (var z = 0; z < map.SizeZ; z++)
        {
            for (var y = 0; y < map.SizeY; y++)
            {
                for (var x = 0; x < map.SizeX; x++)
                {
                    Point3D point = new(x, y, z);
                    if (predicate(point, map.GetValue(point)))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
