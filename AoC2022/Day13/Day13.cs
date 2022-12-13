namespace AoC2022.Day13;

public class Day13 : IMDay
{
    public string FilePath { private get; init; } = "Day13\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        var rightOrderSum = Enumerable.Range(1, input.Length)
            .Where(i => input[i - 1][0].CompareTo(input[i - 1][1]) <= 0)
            .Sum(i => i);
        
        return rightOrderSum.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput();

        var divider1 = ParseList("[[2]]");
        var divider2 = ParseList("[[6]]");

        var ordered = input
            .SelectMany(l => l)
            .Union(new[] { divider1, divider2 })
            .OrderBy(l => l)
            .ToArray();

        var result = (ordered.IndexOf(divider1) + 1) * (ordered.IndexOf(divider2) + 1);

       return result.ToString();
    }

    private async Task<ListItemList[][]> GetInput() =>
        (await FileParser.ReadBlocksAsStringArray(FilePath))
            .Select(b => b
                .Select(l => ParseList(l))
                .ToArray())
            .ToArray();

    private static ListItemList ParseList(string line) =>
        ParseList(line
            .Replace("[", ",[,")
            .Replace("]", ",],")
            .Split(',', StringSplitOptions.RemoveEmptyEntries));

    private static ListItemList ParseList(string[] parts)
    {
        // Should have used:
        // dynamic signal = JsonConvert.DeserializeObject(line);
        // Way easier
        ListItemList current = new();

        for (var i = 1; i < parts.Length - 1; i++)
        {
            var value = parts[i];
            if (value == "[")
            {
                var closingIndex = parts.IndexOfClosingTag(i, "[", "]");
                current.Add(ParseList(parts[i..(closingIndex +1)]));
                i = closingIndex;
            }
            else
            {
                current.Add(new IntListItem(int.Parse(parts[i])));
            }
        }

        return current;
    }
}