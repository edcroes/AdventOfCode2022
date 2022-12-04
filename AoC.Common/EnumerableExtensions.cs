namespace AoC.Common;

public static class EnumerableExtensions
{
    public static IEnumerable<T> IntersectMany<T>(this IEnumerable<IEnumerable<T>> sources) =>
        sources
            .Skip(1)
            .Aggregate(sources.First(), (result, next) => result.Intersect(next));
}
