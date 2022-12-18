namespace AoC2022.Day18;

public class Day18 : IMDay
{
    private const char Air = '.';
    private const char Droplet = 'X';
    private const char TrappedAir = 'O';

    public string FilePath { private get; init; } = "Day18\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (map, cubes) = await GetMapWithCubes();

        var totalSurfaceArea = cubes.Sum(p => 6 - map.NumberOfStraightNeighborsThatMatch(p, Droplet));
        return totalSurfaceArea.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var (map, cubes) = await GetMapWithCubes();

        var remaining = map
            .Where((p, v) => !cubes.Contains(p))
            .ToList();

        int previousRoundAir = int.MaxValue;
        while (remaining.Count > 0 && previousRoundAir > 0)
        {
            var currentRoundAir = 0;
            List<Point3D> newRemaining = new();

            foreach(var current in remaining)
            {
                if (map.NumberOfStraightNeighborsThatMatchWithoutBorders(current, Air, Air) > 0)
                {
                    map.SetValue(current, '.');
                    currentRoundAir++;
                }
                else if (
                    map.NumberOfStraightNeighborsThatMatch(current, Droplet) +
                    map.NumberOfStraightNeighborsThatMatch(current, TrappedAir) == 6)
                {
                    map.SetValue(current, TrappedAir);
                }
                else
                {
                    newRemaining.Add(current);
                }
            }

            previousRoundAir = currentRoundAir;
            remaining = newRemaining;
        }

        var totalSurfaceArea = cubes.Sum(p => map.NumberOfStraightNeighborsThatMatchWithoutBorders(p, Air, Air));
        return totalSurfaceArea.ToString();
    }

    private async Task<(Map3D<char>, Point3D[])> GetMapWithCubes()
    {
        var input = await GetInput();

        Map3D<char> map = new(
            input.Max(p => p.X) + 1,
            input.Max(p => p.Y) + 1,
            input.Max(p => p.Z) + 1);

        foreach (var point in input)
        {
            map.SetValue(point, 'X');
        }

        return (map, input);
    }

    private async Task<Point3D[]> GetInput() =>
        (await FileParser.ReadLinesAsPoint3D(FilePath, ","));
}
