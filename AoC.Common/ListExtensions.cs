namespace AoC.Common;

public static class ListExtensions
{
    public static void RemoveRange<T>(this List<T> list, IEnumerable<T> itemsToRemove)
    {
        foreach (var item in itemsToRemove)
        {
            list.Remove(item);
        }
    }

    public static void AddIfNotContains<T>(this List<T> list, T item)
    {
        if (!list.Contains(item))
        {
            list.Add(item);
        }
    }
}