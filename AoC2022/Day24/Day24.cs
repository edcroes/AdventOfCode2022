namespace AoC2022.Day24;

public class Day24 : IMDay
{
    public string FilePath { private get; init; } = "Day24\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetBlizzardMap();
        Point start = new(0, -1);
        Point end = new(map.SizeX - 1, map.SizeY - 1);

        var minute = MoveToEnd(map, start, end);

        return minute.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetBlizzardMap();
        Point start = new(0, -1);
        Point end = new(map.SizeX - 1, map.SizeY - 1);

        var minute = MoveToEnd(map, start, end);
        minute += MoveToEnd(map, new(map.SizeX - 1, map.SizeY), new(0, 0));
        minute += MoveToEnd(map, start, end);

        return minute.ToString();
    }

    private static int MoveToEnd(Map<Blizzard> map, Point start, Point end)
    {
        HashSet<Point> currentPoints = new() { start };
        var minute = 0;

        while (true)
        {
            minute++;

            HashSet<Point> newCurrentPoints = new();
            GetNextMap(map);

            if (currentPoints.Contains(end))
                break;

            foreach (var point in currentPoints)
            {
                var neighbors = map.GetStraightNeighbors(point);
                var nextPoints = neighbors.Union(new[] { point }).Where(n => map.GetValueOrDefault(n, Blizzard.None) == Blizzard.None);

                foreach (var next in nextPoints)
                {
                    newCurrentPoints.Add(next);
                }
            }

            currentPoints = newCurrentPoints;
        }

        return minute;
    }

    private static void GetNextMap(Map<Blizzard> map)
    {
        Map<Blizzard> original = map.Clone();
        original.ForEach((point, blizzard) =>
        {
            var newValue = Blizzard.None;
            Point left = new((point.X - 1 + map.SizeX) % map.SizeX, point.Y);
            Point right = new((point.X + 1) % map.SizeX, point.Y);
            Point up = new(point.X, (point.Y - 1 + map.SizeY) % map.SizeY);
            Point down = new(point.X, (point.Y + 1) % map.SizeY);

            newValue |= original.GetValue(left) & Blizzard.Right;
            newValue |= original.GetValue(right) & Blizzard.Left;
            newValue |= original.GetValue(up) & Blizzard.Down;
            newValue |= original.GetValue(down) & Blizzard.Up;

            map.SetValue(point, newValue);
        });
    }

    private async Task<char[][]> GetInput() =>
        await FileParser.ReadLinesAsCharArray(FilePath);

    private async Task<Map<Blizzard>> GetBlizzardMap()
    {
        var input = await GetInput();
        Map<Blizzard> map = new(input.Max(l => l.Length) - 2, input.Length - 2);

        for (var y = 1; y < input.Length - 1; y++)
        {
            for (var x = 1; x < input.Max(l => l.Length) - 1; x++)
            {
                var value = input[y][x] switch
                {
                    '^' => Blizzard.Up,
                    '>' => Blizzard.Right,
                    'v' => Blizzard.Down,
                    '<' => Blizzard.Left,
                    _ => Blizzard.None
                };

                map.SetValue(x - 1, y - 1, value);
            }
        }

        return map;
    }

    [Flags]
    private enum Blizzard
    {
        None = 0,
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8
    }
}
