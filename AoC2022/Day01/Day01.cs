namespace AoC2022.Day01;

public class Day01 : IMDay
{
    public string FilePath { private get; init; } = "Day01\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var caloriesPerElf = await GetCalories();
        var totalCaloriesPerElf = caloriesPerElf.Select(c => c.Sum());

        return totalCaloriesPerElf.Max().ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var caloriesPerElf = await GetCalories();
        var topThreeElves = caloriesPerElf
            .Select(c => c.Sum())
            .OrderByDescending(e => e)
            .Take(3);

        return topThreeElves.Sum().ToString();
    }

    private async Task<int[][]> GetCalories() =>
        await FileParser.ReadBlocksAsIntArray(FilePath);
}