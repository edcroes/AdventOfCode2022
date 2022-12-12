namespace AoC2022.Day12;

public class Day12 : IMDay
{
    public string FilePath { private get; init; } = "Day12\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();
        var start = map.First((p, v) => v == 'S');

        return GetShortestPath(map, start).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetInput();
        map.SetValue(map.First((p, v) => v == 'S'), 'a');
        var lowestPoints = map.Where((p, v) => v == 'a');
        var currentShortestPath = int.MaxValue;

        foreach (var start in lowestPoints)
        {
            var shortestPath = GetShortestPath(map, start);
            currentShortestPath = Math.Min(shortestPath, currentShortestPath);
        }

        return currentShortestPath.ToString();
    }

    private static int GetShortestPath(Map<char> map, Point start)
    {
        var end = map.First((p, v) => v == 'E');

        var origStartValue = map.GetValue(start);
        var origEndValue = map.GetValue(end);

        map.SetValue(start, 'a');
        map.SetValue(end, 'z');

        var shortest = map.GetShortestPath(start, end, CanMoveTo);

        map.SetValue(start, origStartValue);
        map.SetValue(end, origEndValue);

        return shortest;
    }

    private static bool CanMoveTo(Map<char> map, Point from, Point to)
    {
        var fromValue = map.GetValue(from);
        var toValue = map.GetValue(to);

        return toValue <= fromValue + 1;
    }

    private async Task<Map<char>> GetInput() =>
        new(await FileParser.ReadLinesAsCharArray(FilePath));
}
