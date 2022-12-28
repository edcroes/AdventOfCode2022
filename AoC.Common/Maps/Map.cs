namespace AoC.Common.Maps;

public class Map<T>
{
    private T[,] _map;

    public int SizeX => _map.GetLength(1);
    public int SizeY => _map.GetLength(0);

    public Map(int x, int y)
    {
        _map = new T[y, x];
    }

    public Map(T[][] grid)
    {
        _map = new T[grid.Length, grid[0].Length];

        for (var y = 0; y < grid.Length; y++)
        {
            for (var x = 0; x < grid[0].Length; x++)
            {
                SetValue(x, y, grid[y][x]);
            }
        }
    }

    public Map(T[,] grid)
    {
        _map = new T[grid.GetLength(0), grid.GetLength(1)];
        Array.Copy(grid, _map, grid.Length);
    }

    public T? GetValueOrDefault(Point point, T? defaultValue = default) =>
        GetValueOrDefault(point.X, point.Y, defaultValue);

    public T? GetValueOrDefault(int x, int y, T? defaultValue = default)
    {
        if (x < 0 || x >= SizeX || y < 0 || y >= SizeY)
        {
            return defaultValue;
        }

        return GetValue(x, y);
    }

    public T GetValue(Point point) => GetValue(point.X, point.Y);

    public T GetValue(int x, int y) => _map[y, x];

    public void SetValue(Point location, T value) => SetValue(location.X, location.Y, value);

    public void SetValue(int x, int y, T value) => _map[y, x] = value;

    public T[] GetLine(int fromX, int fromY, int toX, int toY)
    {
        var row = new List<T>();

        var moveY = fromY == toY ? 0 : fromY > toY ? -1 : 1;
        var moveX = fromX == toX ? 0 : fromX > toX ? -1 : 1;

        do
        {
            row.Add(_map[fromY, fromX]);
            fromY += moveY;
            fromX += moveX;
        }
        while (fromX <= toX && moveX == 1 || fromX >= toX && moveX == -1 || fromY <= toY && moveY == 1 || fromY >= toY && moveY == -1);

        return row.ToArray();
    }

    public void SetLine(Point from, Point to, T value) =>
        SetLine(from.X, from.Y, to.X, to.Y, value);

    public void SetLine(int fromX, int fromY, int toX, int toY, T value)
    {
        var moveY = fromY == toY ? 0 : fromY > toY ? -1 : 1;
        var moveX = fromX == toX ? 0 : fromX > toX ? -1 : 1;

        do
        {
            SetValue(fromX, fromY, value);
            fromY += moveY;
            fromX += moveX;
        }
        while (fromX <= toX && moveX == 1 || fromX >= toX && moveX == -1 || fromY <= toY && moveY == 1 || fromY >= toY && moveY == -1);
    }

    public void RotateRight()
    {
        var newMap = new T[SizeX, SizeY];

        for (var y = 0; y < SizeY; y++)
        {
            for (var x = 0; x < SizeX; x++)
            {
                newMap[x, SizeY - 1 - y] = _map[y, x];
            }
        }

        _map = newMap;
    }

    public void RotateLeft()
    {
        RotateRight();
        RotateRight();
        RotateRight();
    }

    public Map<T> MirrorHorizontal()
    {
        var newMap = new T[SizeY, SizeX];

        for (var y = 0; y < SizeY; y++)
        {
            for (var x = 0; x < SizeX; x++)
            {
                newMap[y, SizeX - 1 - x] = _map[y, x];
            }
        }

        _map = newMap;
        return this;
    }

    public Map<T> MirrorVertical()
    {
        MirrorHorizontal();
        RotateRight();
        RotateRight();

        return this;
    }

    public void DistributeChaos(T aliveValue, Func<bool, int, T> getNewValue)
    {
        DistributeChaos(aliveValue, (map, point) => NumberOfStraightAndDiagonalNeighborsThatMatch(map, point, aliveValue), getNewValue);
    }

    public void DistributeChaos(T aliveValue, Func<Map<T>, Point, int> getNumberOfNeighborsThatMatch, Func<bool, int, T> getNewValue)
    {
        var pointsToChange = new Dictionary<Point, T>();

        for (int y = 0; y < SizeY; y++)
        {
            for (int x = 0; x < SizeX; x++)
            {
                var currentPoint = new Point(x, y);
                var currentValue = GetValue(currentPoint.X, currentPoint.Y);
                var matches = getNumberOfNeighborsThatMatch(this, currentPoint);

                var newValue = getNewValue(aliveValue.Equals(currentValue), matches);
                if (!newValue.Equals(currentValue))
                {
                    pointsToChange.Add(currentPoint, newValue);
                }
            }
        }

        foreach (var point in pointsToChange.Keys)
        {
            SetValue(point.X, point.Y, pointsToChange[point]);
        }
    }

    public IEnumerable<Point> GetStraightNeighbors(Point point)
    {
        var neighbors = new Point[]
        {
                new Point(point.X - 1, point.Y),
                new Point(point.X, point.Y - 1),
                new Point(point.X + 1, point.Y),
                new Point(point.X, point.Y + 1)
        };

        return neighbors.Where(p => Contains(p));
    }

    public IEnumerable<Point> GetStraightAndDiagonalNeighbors(Point point)
    {
        List<Point> neighbors = new();

        for (int y = Math.Max(point.Y - 1, 0); y <= point.Y + 1 && y < SizeY; y++)
        {
            for (int x = Math.Max(point.X - 1, 0); x <= point.X + 1 && x < SizeX; x++)
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

    public int NumberOfStraightNeighborsThatMatch(Point point, Func<T, T?, bool> matcher, T? outOfBoundsValue = default)
    {
        var neighbors = new Point[]
        {
                new Point(point.X - 1, point.Y),
                new Point(point.X, point.Y - 1),
                new Point(point.X + 1, point.Y),
                new Point(point.X, point.Y + 1)
        };

        return neighbors.Count(n => matcher(GetValue(point), GetValueOrDefault(n.X, n.Y, outOfBoundsValue)));
    }

    public int NumberOfStraightNeighborsThatMatch(Point point, T valueToMatch)
    {
        var neighbors = new Point[]
        {
                new Point(point.X - 1, point.Y),
                new Point(point.X, point.Y - 1),
                new Point(point.X + 1, point.Y),
                new Point(point.X, point.Y + 1)
        };

        return neighbors.Count(n => valueToMatch.Equals(GetValueOrDefault(n.X, n.Y)));
    }

    public int NumberOfStraightAndDiagonalNeighborsThatMatch(Point point, T valueToMatch) =>
        NumberOfStraightAndDiagonalNeighborsThatMatch(this, point, valueToMatch);

    private static int NumberOfStraightAndDiagonalNeighborsThatMatch(Map<T> map, Point point, T valueToMatch)
    {
        var numberOfMatches = 0;
        for (int y = Math.Max(point.Y - 1, 0); y <= point.Y + 1 && y < map.SizeY; y++)
        {
            for (int x = Math.Max(point.X - 1, 0); x <= point.X + 1 && x < map.SizeX; x++)
            {
                if (y == point.Y && x == point.X)
                {
                    continue;
                }

                if (map.GetValue(x, y).Equals(valueToMatch))
                {
                    numberOfMatches++;
                }
            }
        }

        return numberOfMatches;
    }

    public void CopyTo(Map<T> otherMap, Point startingPoint)
    {
        if (otherMap is null || startingPoint.X + SizeX > otherMap.SizeX || startingPoint.Y + SizeY > otherMap.SizeY)
        {
            throw new IndexOutOfRangeException("The map does not fit the destination");
        }

        for (int y = 0; y < SizeY; y++)
        {
            for (int x = 0; x < SizeX; x++)
            {
                otherMap.SetValue(x + startingPoint.X, y + startingPoint.Y, GetValue(x, y));
            }
        }
    }

    public T[] ToFlatArray()
    {
        var result = new T[SizeX * SizeY];

        for (int y = 0; y < SizeY; y++)
        {
            for (int x = 0; x < SizeX; x++)
            {
                result[y * SizeX + x] = GetValue(x, y);
            }
        }

        return result;
    }

    public T[,] To2DArray() => (T[,])_map.Clone();

    public bool Contains(T value)
    {
        for (int y = 0; y < SizeY; y++)
        {
            for (int x = 0; x < SizeX; x++)
            {
                if (GetValue(x, y).Equals(value))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool Contains(Point point) => point.X >= 0 && point.X < SizeX && point.Y >= 0 && point.Y < SizeY;

    public Map<T> Clone()
    {
        var newMapArray = new T[SizeY, SizeX];
        Array.Copy(_map, newMapArray, _map.Length);
        return new(newMapArray);
    }

    public void FillWith(T defaultValue)
    {
        for (int y = 0; y < SizeY; y++)
        {
            for (int x = 0; x < SizeX; x++)
            {
                SetValue(x, y, defaultValue);
            }
        }
    }
}
