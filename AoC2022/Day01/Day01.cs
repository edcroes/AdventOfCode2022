namespace AoC2022.Day01;

public class Day01 : IMDay
{
    public string FilePath { private get; init; } = "Day01\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        return "TODO";
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput();
        return "TODO";
    }

    private async Task<int[]> GetInput() =>
        (await File.ReadAllLinesAsync(FilePath))
            .Where(l => !string.IsNullOrEmpty(l))
            .Select(l => int.Parse(l))
            .ToArray();
}