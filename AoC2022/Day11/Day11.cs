namespace AoC2022.Day11;

public class Day11 : IMDay
{
    private long _leastCommonMultiple;

    public string FilePath { private get; init; } = "Day11\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var monkeys = await GetMonkeys(i => i / 3);
        for (var round = 0; round < 20; round++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.InspectAllItems();
                monkey.ThrowAllItems();
            }
        }

        return monkeys
            .Select(m => m.NumberOfInspectedItems)
            .OrderByDescending(i => i)
            .Take(2)
            .Aggregate((left, right) => left * right)
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var monkeys = await GetMonkeys(LowerWorryLevelPart2);
        _leastCommonMultiple = AoCMath.LeastCommonMultiple(monkeys.Select(m => m.DivisibleBy));

        for (var round = 0; round < 10000; round++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.InspectAllItems();
                monkey.ThrowAllItems();
            }
        }

        return monkeys
            .Select(m => m.NumberOfInspectedItems)
            .OrderByDescending(i => i)
            .Take(2)
            .Aggregate((left, right) => left * right)
            .ToString();
    }

    private long LowerWorryLevelPart2(long item) => item % _leastCommonMultiple;

    private async Task<Monkey[]> GetMonkeys(Func<long, long> lowerWorryLevel)
    {
        List<Monkey> monkeys = new();
        monkeys.AddRange(
            (await FileParser.ReadBlocksAsStringArray(FilePath))
            .Select(b => Monkey.Parse(b, id => monkeys[id], lowerWorryLevel))
            .ToArray());

        return monkeys.ToArray();
    }
}
