namespace AoC2022.Day04;

public class Day04 : IMDay
{
    private record struct SectionRange(int Start, int End)
    {
        public SectionRange(string input) :
            this(int.Parse(input.Split("-")[0]), int.Parse(input.Split("-")[1]))
        {
        }
    }

    public string FilePath { private get; init; } = "Day04\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        return input
            .Select(l => l.Select(a => new SectionRange(a)))
            .Select(l => l.Select(a => Enumerable.Range(a.Start, a.End - a.Start + 1).ToArray()))
            .Select(l => HasCompleteOverlap(l.First(), l.Last()))
            .Count(o => o)
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput();
        return input
            .Select(l => l.Select(a => new SectionRange(a)))
            .Select(l => l.Select(a => Enumerable.Range(a.Start, a.End - a.Start + 1).ToArray()))
            .Select(l => HasOverlap(l.First(), l.Last()))
            .Count(o => o)
            .ToString();
    }

    private static bool HasCompleteOverlap(int[] left, int[] right) =>
        left.Intersect(right).Count() == Math.Min(left.Length, right.Length);

    private static bool HasOverlap(int[] left, int[] right) =>
        left.Intersect(right).Any();

    private async Task<string[][]> GetInput() =>
        await FileParser.ReadLinesAsStringArray(FilePath, ",");
}
