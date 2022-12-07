namespace AoC2022.Day07;

public class Container
{
    required public string Name { get; init; }
    public Container? Parent { get; init; }
    public List<Container> Containers { get; } = new();
    public List<Leaf> Leaves { get; } = new();

    public long Size => Containers.Sum(c => c.Size) + Leaves.Sum(l => l.Size);

    public override string ToString() => Name;
}