namespace AoC2022.Day02;

public class Day02 : IMDay
{
    private static readonly Dictionary<char, Hand> _handTranslation = new()
    {
        { 'A', Hand.Rock },
        { 'B', Hand.Paper},
        { 'C', Hand.Scissors },
        { 'X', Hand.Rock },
        { 'Y', Hand.Paper },
        { 'Z', Hand.Scissors }
    };

    private static readonly LinkedArray<Hand> _hands = new(new []
    {
        Hand.Rock,
        Hand.Paper,
        Hand.Scissors
    });

    public string FilePath { private get; init; } = "Day02\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = (await GetInput())
            .Select(l => new { Left = _handTranslation[l[0]], Right = _handTranslation[l[1]] });

        return input
            .Select(i => GetScore(i.Left, i.Right))
            .Sum()
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = (await GetInput())
            .Select(i => new { Left = _handTranslation[i[0]], Right = GetOwnHand(i[0], i[1]) });

        return input
            .Select(i => GetScore(i.Left, i.Right))
            .Sum()
            .ToString();
    }

    private static Hand GetOwnHand(char left, char outcome)
    {
        var leftHand = _handTranslation[left];
        return outcome switch
        {
            'X' => _hands.GetPrevious(leftHand),
            'Y' => leftHand,
            'Z' => _hands.GetNext(leftHand),
            _ => throw new NotSupportedException("That's a wierd outcome")
        };
    }

    private static int GetScore(Hand left, Hand right)
    {
        var outcomeScore = left == right
            ? 3
            : _hands.GetPrevious(right) == left
                ? 6
                : 0;

        return outcomeScore + (int)right;
    }

    private async Task<char[][]> GetInput() =>
        await FileParser.ReadLinesAsCharArray(FilePath, " ");
}

public enum Hand
{
    Rock = 1,
    Paper = 2,
    Scissors = 3
}