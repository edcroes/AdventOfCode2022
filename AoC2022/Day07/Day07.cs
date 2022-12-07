namespace AoC2022.Day07;

public class Day07 : IMDay
{
    public string FilePath { private get; init; } = "Day07\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        var root = GetTree(input);

        return GetContainers(root, c => c.Size <= 100000L)
            .Sum(c => c.Size)
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput();
        var root = GetTree(input);

        var currentFreeSpace = 70000000 - root.Size;
        var spaceNeeded = 30000000 - currentFreeSpace;
        return GetContainers(root, c => c.Size >= spaceNeeded)
            .Min(c => c.Size)
            .ToString();
    }

    private static List<Container> GetContainers(Container current, Func<Container, bool> match)
    {
        var subContainers = current.Containers.SelectMany(c => GetContainers(c, match)).ToList();
        if (match(current))
            subContainers.Add(current);

        return subContainers;
    }

    private static Container GetTree(string[] input)
    {
        var root = new Container { Name = "/" };
        var current = root;

        foreach (var line in input)
        {
            if (line.StartsWith("$"))
            {
                var (_, command, dir) = line.Split(' ');
                if (command == "cd")
                {
                    current = dir switch
                    {
                        "/" => root,
                        ".." => current!.Parent,
                        _ => current!.Containers.Single(c => c.Name == dir)
                    };
                }
            }
            else
            {
                var (left, name) = line.Split(' ');
                if (left == "dir")
                {
                    if (!current!.Containers.Any(c => c.Name == name))
                    {
                        Container dir = new() { Name = name!, Parent = current };
                        current.Containers.Add(dir);
                    }
                }
                else
                {
                    var size = long.Parse(left!);

                    if (!current!.Leaves.Any(c => c.Name == name))
                    {
                        Leaf file = new(name!, size);
                        current.Leaves.Add(file);
                    }
                }
            }
        }

        return root;
    }

    private async Task<string[]> GetInput() =>
        await FileParser.ReadLinesAsString(FilePath);
}