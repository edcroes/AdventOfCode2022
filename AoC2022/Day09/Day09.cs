namespace AoC2022.Day09;

public class Day09 : IMDay
{
    private record struct Instruction(char Type, int Value);
    public string FilePath { private get; init; } = "Day09\\input.txt";

    private static readonly Dictionary<char, Point> _movement = new()
    {
        { 'U', new(0, 1) },
        { 'R', new(1, 0) },
        { 'D', new(0, -1) },
        { 'L', new(-1, 0) }
    };

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInstructions();
        List<Point> allTailPoints = new() { new(0, 0) };

        Point head = new(0, 0), tail = new(0, 0);

        foreach (var instruction in input)
        {
            for (var step = 0; step < instruction.Value; step++)
            {
                head = head.Add(_movement[instruction.Type]);
                tail = GetNextTailPosition(head, tail);
                allTailPoints.AddIfNotContains(tail);
            }
        }

        return allTailPoints.Count.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInstructions();
        List<Point> allTailPoints = new() { new(0, 0) };
        var rope = new Point[10];

        foreach (var instruction in input)
        {
            for (var step = 0; step < instruction.Value; step++)
            {
                rope[0] = rope[0].Add(_movement[instruction.Type]);

                for (var knot = 1; knot < rope.Length; knot++)
                {
                    rope[knot] = GetNextTailPosition(rope[knot - 1], rope[knot]);
                    if (knot == rope.Length - 1)
                    {
                        allTailPoints.AddIfNotContains(rope[knot]);
                    }
                }
            }
        }

        return allTailPoints.Count.ToString();
    }

    private static Point GetNextTailPosition(Point head, Point tail)
    {
        if (!head.IsTouching(tail))
        {
            var diff = head.Subtract(tail);

            var moveX = diff.X < 0 ? -1 : 1;
            var moveY = diff.Y < 0 ? -1 : 1;
            tail = diff switch
            {
                { X: < 0 or > 0, Y: < 0 or > 0 } => tail.Add(new(moveX, moveY)),
                { X: < 0 or > 0 } => tail.Add(new(moveX, 0)),
                _ => tail.Add(new(0, moveY))
            };
        }

        return tail;
    }

    private async Task<Instruction[]> GetInstructions() =>
        (await FileParser.ReadLinesAsString(FilePath))
            .Select(ParseInstruction)
            .ToArray();

    private static Instruction ParseInstruction(string line)
    {
        var (type, value) = line.Split(' ');
        return new(type![0], int.Parse(value!));
    }
}
