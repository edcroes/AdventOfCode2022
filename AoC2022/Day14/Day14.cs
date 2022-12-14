namespace AoC2022.Day14;

public class Day14 : IMDay
{
    private const char Rock = '#';
    private const char Sand = 'o';
    private const char Air = '.';
    private const char SandStart = '+';

    public string FilePath { private get; init; } = "Day14\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        var map = CreateMap(input);
        var sandStartingPoint = map.First((p, v) => v == SandStart);
        
        var sandFallen = 0;
        while (MeasureUnitOfSandFall(map, sandStartingPoint) != -1)
        {
            sandFallen++;
        }

        return sandFallen.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInputPart2();
        var map = CreateMap(input);
        var sandStartingPoint = map.First((p, v) => v == SandStart);

        var sandFallen = 0;
        while (MeasureUnitOfSandFall(map, sandStartingPoint) > 0)
        {
            sandFallen++;
        }

        return (sandFallen + 1).ToString();
    }

    private static int MeasureUnitOfSandFall(Map<char> map, Point from)
    {
        var time = 0;
        Point next;

        while((next = GetNextPoint(map, from)) != from)
        {
            time++;

            if (next.Y >= map.SizeY)
                return -1;

            from = next;
        }

        map.SetValue(from, Sand);

        return time;
    }

    private static Point GetNextPoint(Map<char> map, Point currentPosition)
    {
        var nextPositions = new Point[]
        {
            new (currentPosition.X, currentPosition.Y + 1),
            new (currentPosition.X - 1, currentPosition.Y + 1),
            new (currentPosition.X + 1, currentPosition.Y + 1)
        };
        
        foreach (var next in nextPositions)
        {
            if (map.GetValueOrDefault(next, Air) == Air)
                return next;
        }

        return currentPosition;
    }

    private static Map<char> CreateMap(Point[][] rocks)
    {
        var minX = rocks.SelectMany(p => p).Min(p => p.X);
        var maxX = rocks.SelectMany(p => p).Max(p => p.X);
        var maxY = rocks.SelectMany(p => p).Max(p => p.Y);
        Point transform = new(minX * -1, 0);

        Map<char> map = new(maxX - minX + 1, maxY + 1);
        map.FillWith('.');

        foreach (var formation in rocks)
        {
            for (var i = 0; i < formation.Length - 1; i++)
            {
                var from = formation[i].Add(transform);
                var to = formation[i + 1].Add(transform);
                map.SetLine(from, to, Rock);
            }
        }

        map.SetValue(500 + transform.X, 0, SandStart);

        return map;
    }

    private async Task<Point[][]> GetInputPart2()
    {
        const int addY = 2;
        var input = await GetInput();

        var maxY = addY + input.SelectMany(p => p).Max(p => p.Y);
        var startX = 500 - maxY;
        var endX = 500 + maxY;

        var newInput = new Point[input.Length + 1][];
        Array.Copy(input, newInput, input.Length);
        newInput[^1] = new Point[] { new(startX, maxY), new(endX, maxY) };

        return newInput;
    }

    private async Task<Point[][]> GetInput() =>
        (await FileParser.ReadLinesAsStringArray(FilePath, " -> "))
            .Select(ParseLine)
            .ToArray();

    private Point[] ParseLine(string[] line) =>
        line
            .Select(p => new Point(int.Parse(p.Split(',')[0]), int.Parse(p.Split(",")[1])))
            .ToArray();
}
