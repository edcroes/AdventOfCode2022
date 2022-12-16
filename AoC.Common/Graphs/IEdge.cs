namespace AoC.Common.Graphs;
public interface IEdge : IEquatable<IEdge>
{
    int Weight { get; }
}
