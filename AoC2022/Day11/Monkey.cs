namespace AoC2022.Day11;

public record class Monkey(
    int Id,
    Func<long, long> GetNewValue,
    Func<long, long> LowerWorryLevel,
    long DivisibleBy,
    int DivisbleByTrueReceiver,
    int DivisbleByFalseReceiver,
    Func<int, Monkey> GetMonkeyById)
{
    public long NumberOfInspectedItems { get; private set; }

    public List<long> Items { get; } = new();

    public void InspectAllItems()
    {
        var newItems = Items.Select(i => LowerWorryLevel(GetNewValue(i))).ToArray();
        NumberOfInspectedItems += newItems.Length;
        Items.Clear();
        Items.AddRange(newItems);
    }

    public void CatchItem(long item) =>
        Items.Add(item);

    public void ThrowAllItems()
    {
        foreach (var item in Items)
        {
            var receiver = item % DivisibleBy == 0 ? DivisbleByTrueReceiver : DivisbleByFalseReceiver;
            GetMonkeyById(receiver).CatchItem(item);
        }
        Items.Clear();
    }

    public static Monkey Parse(string[] input, Func<int, Monkey> getMonkeyById, Func<long, long> lowerWorryLevel)
    {
        var id = int.Parse(input[0].Split(' ')[1].Trim(':'));
        var startingItems = input[1]["  Starting items: ".Length..].ToLongArray(", ");
        var operation = "old => " + input[2]["  Operation: new = ".Length..];
        var divisibleBy = long.Parse(input[3]["  Test: divisible by ".Length..]);
        var trueReceiver = int.Parse(input[4]["    If true: throw to monkey ".Length..]);
        var falseReceiver = int.Parse(input[5]["    If false: throw to monkey ".Length..]);

        var getNewValue = ExpressionParser.ParseSimpleMathExpression<long>(operation);

        Monkey monkey = new(id, getNewValue, lowerWorryLevel, divisibleBy, trueReceiver, falseReceiver, getMonkeyById);
        monkey.Items.AddRange(startingItems);

        return monkey;
    }
}
