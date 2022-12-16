namespace AoC2022.Day15;

public partial class Day15 : IMDay
{
    public string FilePath { private get; init; } = "Day15\\input.txt";

    private static readonly Regex _lineRegex = GetRegex();
    [GeneratedRegex("^Sensor at x=(?<sensorX>-?\\d+), y=(?<sensorY>-?\\d+): closest beacon is at x=(?<beaconX>-?\\d+), y=(?<beaconY>-?\\d+)$")]
    private static partial Regex GetRegex();

    public async Task<string> GetAnswerPart1()
    {
        var (extraInput, sensors) = await GetInput();
        var y = extraInput.Part1Y;

        var beaconCountOnRow = sensors
            .Select(s => s.Beacon)
            .Where(b => b.Y == y)
            .Distinct()
            .Count();
        
        var sensorCountOnRow = sensors
            .Select(s => s.Location)
            .Where(s => s.Y == y)
            .Count();

        var lines = GetOccupiedLinesOn(y, sensors);
        var occupiedLocations = lines.Select(l => l.Length + 1).Sum();

        return (occupiedLocations - beaconCountOnRow - sensorCountOnRow).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var (extraInput, sensors) = await GetInput();
        var max = extraInput.Part2Max;

        Point? result = null;
        Parallel.For(0, max + 1, (y, state) =>
        {
            var lines = GetOccupiedLinesOn(y, sensors).Where(l => l.To.X >= 0 && l.From.X <= max).ToArray();
            if (lines.Length != 1)
            {
                result = new(lines[0].To.X + 1, y);
                state.Stop();
            }
        });

        if (!result.HasValue)
            throw new Exception("That's strange");

        return (result.Value.X * 4000000L + result.Value.Y).ToString();
    }

    private static List<Line> GetOccupiedLinesOn(int y, IEnumerable<Sensor> sensors)
    {
        var sensorData = sensors
            .Where(s => y >= s.MinY && y <= s.MaxY)
            .Select(s => new { minX = s.GetMinXOn(y), maxX = s.GetMaxXOn(y) })
            .OrderBy(s => s.minX)
            .ToArray();

        List<Line> lines = new();

        var currentLineStart = sensorData.First().minX;
        var currentLineEnd = sensorData.First().maxX;

        foreach (var sensor in sensorData.Skip(1))
        {
            if (sensor.minX <= currentLineEnd)
            {
                currentLineEnd = Math.Max(sensor.maxX, currentLineEnd);
            }
            else
            {
                lines.Add(new(new(currentLineStart, y), new(currentLineEnd, y)));
                currentLineStart = sensor.minX;
                currentLineEnd = sensor.maxX;
            }
        }

        lines.Add(new(new(currentLineStart, y), new(currentLineEnd, y)));

        return lines;
    }

    private async Task<(ExtraDayInput, Sensor[])> GetInput()
    {
        var lines = await FileParser.ReadLinesAsString(FilePath);
        var extraInput = ParseExtraDayInput(lines[0]);
        var sensors = lines
            .Skip(1)
            .Select(ParseSensor)
            .ToArray();

        return (extraInput, sensors);
    }

    private static ExtraDayInput ParseExtraDayInput(string line)
    {
        var (first, second) = line.Split(',');
        return new(int.Parse(first.Split('=')[1]), int.Parse(second.Split('=')[1]));
    }


    private static Sensor ParseSensor(string line)
    {
        var match = _lineRegex.Match(line);
        Point sensor = new(match.GetInt("sensorX"), match.GetInt("sensorY"));
        Point beacon = new(match.GetInt("beaconX"), match.GetInt("beaconY"));

        return new(sensor, beacon);
    }

    private record struct ExtraDayInput(int Part1Y, int Part2Max);

    private record struct Sensor(Point Location, Point Beacon)
    {
        private int _beaconDistance = 0;

        public int MinX => Location.X - GetBeaconDistance();
        public int MaxX => Location.X + GetBeaconDistance();
        public int MinY => Location.Y - GetBeaconDistance();
        public int MaxY => Location.Y + GetBeaconDistance();

        public int GetMinXOn(int y) =>
            Location.X - (GetBeaconDistance() - Math.Abs(Location.Y - y));

        public int GetMaxXOn(int y) =>
            Location.X + (GetBeaconDistance() - Math.Abs(Location.Y - y));        

        public bool IsInBoundary(Point point) =>
            GetBeaconDistance() >= point.GetManhattenDistance(Location);

        public int GetBeaconDistance() =>
            _beaconDistance == 0 ? _beaconDistance = Location.GetManhattenDistance(Beacon) : _beaconDistance;
    };
}
