namespace AoC2022.Day06;

public class Day06 : IMDay
{
    public string FilePath { private get; init; } = "Day06\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        return GetUniqueRangeMarker(input, 4).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput();
        return GetUniqueRangeMarker(input, 14).ToString();
    }

    private static int GetUniqueRangeMarker(string value, int numberOfUniqueCharacters)
    {
        for (var i = numberOfUniqueCharacters; i <= value.Length; i++)
        {
            var option = value[(i - numberOfUniqueCharacters)..i];
            if (option.Distinct().Count() == numberOfUniqueCharacters)
            {
                return i;
            }
        }

        return -1;
    }

    private async Task<string> GetInput() =>
        (await File.ReadAllTextAsync(FilePath)).Trim();
}
