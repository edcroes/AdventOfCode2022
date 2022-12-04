using AoC.Common;

namespace AoC2022.Day03;

public class Day03 : IMDay
{
    public string FilePath { private get; init; } = "Day03\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        var answer = input
            .Select(l => l.SplitInHalf())
            .Select(rs => GetWronglyPlacedItem(rs.Item1, rs.Item2))
            .Sum(GetPriority);

        return answer.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var groups = (await GetInput())
            .SplitInBlocksOf(3);

        return groups
            .Select(GetBadgeFromGroup)
            .Sum(GetPriority)
            .ToString();
    }

    private static char GetWronglyPlacedItem(string left, string right) =>
        left.ToCharArray().Distinct().Single(c => right.Contains(c));

    private static int GetPriority(char c) =>
        (int)c < 96 ? (int)c - 38 : (int)c - 96;

    private static char GetBadgeFromGroup(string[] elfs) =>
        elfs
            .Select(e => e.ToCharArray())
            .IntersectMany()
            .Distinct()
            .Single();

    private async Task<string[]> GetInput() =>
        await FileParser.ReadLinesAsString(FilePath);
}
