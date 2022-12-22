namespace AoC2022.Day21;

public class Day21 : IMDay
{
    public string FilePath { private get; init; } = "Day21\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (knownValues, monkeys) = await ParseMonkeys();

        DetermineAllValues(knownValues, monkeys);

        return knownValues["root"].ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        const string human = "humn";
        var (knownValues, monkeys) = await ParseMonkeys();
        
        var root = monkeys.Single(m => m.AnswerName == "root");
        monkeys.Remove(root);

        var knowns = knownValues.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        DetermineAllValues(knowns, monkeys.ToList());
        var diffInitial = knowns[root.Left] - knowns[root.Right];

        knowns = knownValues.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        knowns[human] = knowns[human] + 10;
        DetermineAllValues(knowns, monkeys.ToList());
        var newDiff = Math.Abs(knowns[root.Left] - knowns[root.Right]);

        var multiplier = newDiff < Math.Abs(diffInitial) ? 1 : -1;

        var humn = knownValues[human];
        var value = 1000000000000L;

        while (newDiff != 0)
        {
            knowns = knownValues.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            humn += value * multiplier;
            knowns[human] = humn;

            DetermineAllValues(knowns, monkeys.ToList());
            var diff = knowns[root.Left] - knowns[root.Right];
            
            if ((diffInitial > 0 && diff >= 0) || (diffInitial < 0 && diff <= 0))
            {
                newDiff = diff;
            }
            else
            {
                humn -= value * multiplier;
                value /= 10;
            }
        }

        return humn.ToString();
    }

    private static void DetermineAllValues(Dictionary<string, long> knownValues, List<Monkey> monkeys)
    {
        while (monkeys.Count > 0)
        {
            var currentMonkeys = monkeys
                .Where(m => knownValues.ContainsKey(m.Left) && knownValues.ContainsKey(m.Right))
                .ToArray();

            foreach (var monkey in currentMonkeys)
            {
                monkeys.Remove(monkey);
                var left = knownValues[monkey.Left];
                var right = knownValues[monkey.Right];
                var answer = monkey.Operation(left, right);
                knownValues.Add(monkey.AnswerName, answer);
            }
        }
    }

    private async Task<string[][]> GetInput() =>
        await FileParser.ReadLinesAsStringArray(FilePath, ": ");

    private async Task<(Dictionary<string, long>, List<Monkey>)> ParseMonkeys()
    {
        var lines = await GetInput();
        Dictionary<string, long> knownValues = new();
        List<Monkey> monkeys = new();

        foreach (var line in lines)
        {
            if (int.TryParse(line[1], out int answer))
            {
                knownValues.Add(line[0], answer);
            }
            else
            {
                var (left, @operator, right) = line[1].Split(' ');
                var func = $"({left}, {right}) => {left} {@operator} {right}".ParseSimpleMathExpressionWithTwoInputs<long>();
                monkeys.Add(new(left!, right!, line[0], func));
            }
        }

        return (knownValues, monkeys);
    }

    private record struct Monkey(string Left, string Right, string AnswerName, Func<long, long, long> Operation);
}
