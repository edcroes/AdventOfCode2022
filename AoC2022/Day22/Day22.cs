namespace AoC2022.Day22;

public partial class Day22 : IMDay
{
    private const char Wall = '#';
    private const char Wrap = ' ';
    private const char Path = '.';

    private static readonly Point[] _movement = new Point[]
    {
        new(1, 0),
        new(0, 1),
        new(-1, 0),
        new(0, -1)
    };

    public string FilePath { private get; init; } = "Day22\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (map, instructions) = await GetInput();

        var currentPosition = map.First((p, v) => v == Path);
        (currentPosition, var move) = GetEndPosition(map, instructions, currentPosition, (move, curPos) => GetWrappedNextPosition(map, move, curPos));
        
        var answer = GetAnswer(currentPosition, move);
        return answer.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var (map, instructions) = await GetInput();

        CubeFolding<char> cubeFolding = new() { VoidValue = Wrap };
        var faces = cubeFolding.GetFacesFromMap(map);
        
        var currentPosition = map.First((p, v) => v == Path);
        (currentPosition, var move) = GetEndPosition(map, instructions, currentPosition, (move, curPos) => GetWrappedNextPositionOnCube(faces, move, curPos));

        var answer = GetAnswer(currentPosition, move);
        return answer.ToString();
    }

    private static int GetAnswer(Point currentPosition, Point move) =>
        1000 * (currentPosition.Y + 1) + 4 * (currentPosition.X + 1) + _movement.IndexOf(move);

    private static (Point position, Point move) GetEndPosition(Map<char> map, Instruction[] instructions, Point currentPosition, Func<Point, Point, (Point position, Point move)> getNextPosition)
    {
        var move = _movement[0];

        foreach (var instruction in instructions)
        {
            if (instruction.Turn)
            {
                move = instruction.Distance == -1
                    ? GetPreviousMovement(move)
                    : GetNextMovement(move);
            }
            else
            {
                for (var i = 0; i < instruction.Distance; i++)
                {
                    var next = currentPosition.Add(move);
                    var nextMove = move;
                    if (map.GetValueOrDefault(next, Wrap) == Wrap)
                    {
                        (next, nextMove) = getNextPosition(move, currentPosition);
                    }

                    if (map.GetValue(next) == Wall)
                        break;

                    currentPosition = next;
                    move = nextMove;
                }
            }
        }

        return (currentPosition, move);
    }

    private static (Point position, Point move) GetWrappedNextPosition(Map<char> map, Point move, Point currentPosition)
    {
        var next = currentPosition.Add(move);
        next = move switch
        {
            { X: 1 } => new(Enumerable.Range(0, currentPosition.X).First(x => map.GetValue(x, next.Y) != Wrap), next.Y),
            { X: -1 } => new(Enumerable.Range(currentPosition.X, map.SizeX - currentPosition.X).Last(x => map.GetValue(x, next.Y) != Wrap), next.Y),
            { Y: 1 } => new(next.X, Enumerable.Range(0, currentPosition.Y).First(y => map.GetValue(next.X, y) != Wrap)),
            { Y: -1 } => new(next.X, Enumerable.Range(currentPosition.Y, map.SizeY - currentPosition.Y).Last(y => map.GetValue(next.X, y) != Wrap))
        };

        return (next, move);
    }

    private static (Point next, Point move) GetWrappedNextPositionOnCube(List<Face> faces, Point move, Point currentPosition)
    {
        var nextMove = move;
        var next = currentPosition;

        var face = faces.Single(f => f.Contains(currentPosition));
        var side = (Side)_movement.IndexOf(move);

        var otherFace = face.Get(side)!;
        var fromZero = currentPosition.Subtract(face.LeftUpperCorner);

        if (side.GetNext() == otherFace.Side)
        {
            nextMove = GetPreviousMovement(move);
            fromZero = face.RotateLeft(currentPosition).Subtract(face.LeftUpperCorner);
        }
        else if (side.GetPrevious() == otherFace.Side)
        {
            nextMove = GetNextMovement(move);
            fromZero = face.RotateRight(currentPosition).Subtract(face.LeftUpperCorner);
        }
        else if (otherFace.Side == side)
        {
            nextMove = GetNextMovement(GetNextMovement(move));
        }

        if (IsOppositeSideAndLeftOrRight(side, otherFace.Side) ||
            IsSameSideAndTopOrBottom(side, otherFace.Side) ||
            IsSideOneRotationAwayAndOtherIsRightOrLeft(side, otherFace.Side))
        {
            next = new(otherFace.Face.LeftUpperCorner.X + (face.Size - 1 - fromZero.X), otherFace.Face.LeftUpperCorner.Y + fromZero.Y);
        }
        else
        {
            next = new(otherFace.Face.LeftUpperCorner.X + fromZero.X, otherFace.Face.LeftUpperCorner.Y + (face.Size - 1 - fromZero.Y));
        }

        return (next, nextMove);
    }

    private static bool IsSideOneRotationAwayAndOtherIsRightOrLeft(Side left, Side right) =>
        (right == left.GetNext() || right == left.GetPrevious()) && right is Side.Right or Side.Left;

    private static bool IsSameSideAndTopOrBottom(Side left, Side right) =>
        right == left && left is Side.Top or Side.Bottom;

    private static bool IsOppositeSideAndLeftOrRight(Side left, Side right) =>
        right == left.GetNext().GetNext() && left is Side.Right or Side.Left;

    private static Point GetNextMovement(Point move) =>
        _movement[(_movement.IndexOf(move) + 1) % _movement.Length];

    private static Point GetPreviousMovement(Point move) =>
        _movement[(_movement.IndexOf(move) + _movement.Length - 1) % _movement.Length];

    private async Task<(Map<char>, Instruction[])> GetInput()
    {
        var (mapInput, instructionsInput) = await FileParser.ReadBlocksAsStringArray(FilePath);

        var map = ParseMap(mapInput!);
        var instructions = ParseInstructions(instructionsInput.Single());

        return (map, instructions);
    }

    private static Map<char> ParseMap(string[] lines)
    {
        var mapLines = lines.Select(l => l.ToCharArray()).ToArray();
        var sizeX = mapLines.Max(l => l.Length);

        var map = new char[mapLines.Length][];

        for (var i = 0; i < mapLines.Length; i++)
        {
            var line = new char[sizeX];
            Array.Copy(mapLines[i], line, mapLines[i].Length);

            for (var j = mapLines[i].Length; j < sizeX; j++)
            {
                line[j] = Wrap;
            }
            map[i] = line;
        }

        return new(map);
    }

    private static Instruction[] ParseInstructions(string line)
    {
        List<Instruction> instructions = new();

        int currentValue = 0;

        for (var i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]))
            {
                currentValue = currentValue * 10 + int.Parse(line[i].ToString());
            }
            else
            {
                if (currentValue > 0)
                {
                    instructions.Add(new(false, currentValue));
                    currentValue = 0;
                }

                var turn = line[i] switch
                {
                    'R' => 1,
                    'L' => -1,
                    _ => throw new NotSupportedException("That's an odd turn")
                };
                instructions.Add(new(true, turn));
            }
        }

        if (currentValue > 0)
        {
            instructions.Add(new(false, currentValue));
        }

        return instructions.ToArray();
    }

    private record struct Instruction(bool Turn, int Distance);
}