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

        var result = (Array.IndexOf(ordered, divider1) + 1) * (Array.IndexOf(ordered, divider2) + 1);

       return result.ToString();
    }

    private async Task<ListItemList[][]> GetInput() =>
        (await FileParser.ReadBlocksAsStringArray(FilePath))
            .Select(b => b
                .Select(l => ParseList(l))
                .ToArray())
            .ToArray();

    private static ListItemList ParseList(string line)
    {
        // Should have used:
        // dynamic signal = JsonConvert.DeserializeObject(line);
        // Way easier

        ListItemList current = new();
        if (line.Length < 2 || line[0] != '[' || line[^1] != ']')
            throw new ArgumentException($"Invalid list '{line}'", nameof(line));

        var depth = 0;
        var newList = string.Empty;

        for (var i = 1; i < line.Length - 1; i++)
        {
            var value = line[i];

            if (value == '[')
            {
                depth++;
            }
            else if (value == ']')
            {
                depth--;
            }
            
            if (depth == 0)
            {
                if (Char.IsNumber(value))
                {
                    var intValue = value - 48;
                    if (Char.IsNumber(line[i + 1]))
                    {
                        intValue *= 10;
                        intValue += line[i + 1] - 48;
                        i++;
                    }
                    current.Add(new IntListItem(intValue));
                }
                else if(value != ',')
                {
                    newList += value;
                    current.Add(ParseList(newList));
                    newList = string.Empty;
                }
            }
            else
            {
                newList += value;
            }
        }

        return current;
    }
}