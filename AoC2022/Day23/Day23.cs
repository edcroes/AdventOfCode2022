namespace AoC2022.Day23;

public class Day23 : IMDay
{
    public string FilePath { private get; init; } = "Day23\\input.txt";

    private static readonly Movement[] _directions = new Movement[]
    {
        new('N', new(0, -1), new Point[] { new(-1, -1), new(0, -1), new(1, -1) }),
        new('S', new(0, 1), new Point[] { new(-1, 1), new(0, 1), new(1, 1) }),
        new('W', new(-1, 0), new Point[] { new(-1, -1), new(-1, 0), new(-1, 1) }),
        new('E', new(1, 0), new Point[] { new(1, -1), new(1, 0), new(1, 1) })
    };

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetMap();

        for (var round = 1; round <= 10; round++)
        {
            _ = MoveElvesToFinalPosition(round, map);
        }

        var space = map.GetBoundingRectangle();
        var result = (space.Size.Width + 1) * (space.Size.Height + 1) - map.Points.Count;

        return result.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetMap();

        var round = 0;
        while (!MoveElvesToFinalPosition(++round, map));

        return round.ToString();
    }

    private static bool MoveElvesToFinalPosition(int round, PointMap<bool> map)
    {
        Dictionary<Point, Point> _nextPositions = new();
        var elvesInCorrectPosition = 0;

        var start = (round - 1) % _directions.Length;
        foreach (var elf in map.Points)
        {
            if (map.NumberOfStraightAndDiagonalNeighborsThatMatch(elf, true) > 0)
            {
                for (var i = 0; i < _directions.Length; i++)
                {
                    var direction = _directions[(start + i) % _directions.Length];
                    if (direction.Checks.All(p => !map.GetValueOrDefault(elf.Add(p))))
                    {
                        var newPoint = elf.Add(direction.Move);
                        var otherWasPresent = _nextPositions.Remove(newPoint);
                        
                        if (!otherWasPresent)
                            _nextPositions.Add(newPoint, elf);
                        
                        break;
                    }
                }
            }
            else
            {
                elvesInCorrectPosition++;
            }
        }

        foreach (var newPoint in _nextPositions.Keys)
        {
            map.RemoveValue(_nextPositions[newPoint]);
            map.SetValue(newPoint, true);
        }

        return elvesInCorrectPosition == map.Points.Count;
    }

    private async Task<PointMap<bool>> GetMap()
    {
        PointMap<bool> map = new();
        var input = await GetInput();

        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input.Min(i => i.Length); x++)
            {
                if (input[y][x] == '#')
                {
                    map.SetValue(x, y, true);
                }
            }
        }

        return map;
    }

    private async Task<char[][]> GetInput() =>
        await FileParser.ReadLinesAsCharArray(FilePath);

    private record struct Movement(char Direction, Point Move, Point[] Checks);
}
