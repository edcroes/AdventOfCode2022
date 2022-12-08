namespace AoC2022.Day00;

/// <summary>
/// Not part of adventofcode.com, but a nice puzzle for lunch from https://aoc.infi.nl/2022
/// </summary>
public class Day00 : IMDay
{
    public record struct Instruction(string Type, int Value);

    public string FilePath { private get; init; } = "Day00\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var instructions = await GetInstructions();
        var allSteps = WalkTheWalk(instructions);

        return allSteps.Last().GetManhattenDistance(new Point(0, 0)).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var instructions = await GetInstructions();
        var allSteps = WalkTheWalk(instructions);

        return DrawPointsHumanReadable(allSteps);
    }

    private static List<Point> WalkTheWalk(Instruction[] instructions)
    {
        Point currentPosition = new(0, 0);
        int orientation = 0;
        List<Point> allSteps = new() { currentPosition };

        foreach (var instruction in instructions)
        {
            if (instruction.Type == "draai")
            {
                orientation = (orientation + instruction.Value) % 360;
                if (orientation < 0) orientation += 360;
            }
            else
            {
                var (length, times) = instruction.Type switch
                {
                    "spring" => (instruction.Value, 1),
                    _ => (1, instruction.Value)
                };
                
                for (var i = 0; i < times; i++)
                {
                    var move = orientation switch
                    {
                        0 => new Point(0, -length),
                        45 => new Point(length, -length),
                        90 => new Point(length, 0),
                        135 => new Point(length, length),
                        180 => new Point(0, length),
                        225 => new Point(-length, length),
                        270 => new Point(-length, 0),
                        315 => new Point(-length, -length),
                        _ => throw new Exception($"Whoops, {orientation} is not a valid value for orientation")
                    };
                    currentPosition = new Point(currentPosition.X + move.X, currentPosition.Y + move.Y);
                    allSteps.Add(currentPosition);
                }
            }
        }

        return allSteps;
    }

    private static string DrawPointsHumanReadable(List<Point> allPoints)
    {
        StringBuilder builder = new();
        builder.AppendLine();

        var moveX = allPoints.Min(p => p.Y) * -1;
        var moveY = allPoints.Min(p => p.X) * -1;
        var maxX = allPoints.Max(p => p.Y + moveX);
        var maxY = allPoints.Max(p => p.X + moveY);

        for (var x = 0; x <= maxX; x++)
        {
            for (var y = 0; y <= maxY; y++)
            {
                var draw = allPoints.Contains(new Point(y - moveY, x - moveX)) ? "X" : " ";
                builder.Append(draw);
            }
            builder.AppendLine();
        }

        return builder.ToString();

    }

    private async Task<Instruction[]> GetInstructions() =>
        (await FileParser.ReadLinesAsString(FilePath))
            .Select(ParseInstruction)
            .ToArray();

    private static Instruction ParseInstruction(string line)
    {
        var (type, value) = line.Split(' ');
        return new(type!, int.Parse(value!));
    }
}
