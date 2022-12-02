namespace AoC.Common.Collections;

public class LinkedArray<T>
{
    private readonly T[] _items;

    public LinkedArray(IEnumerable<T> items)
    {
        _items = items.ToArray();
    }

    public T this[int index]
    {
        get => _items[GetActualIndex(index)];
        set => _items[GetActualIndex(index)] = value;
    }

    public T GetPrevious(int current) =>
        this[current - 1];

    public T GetNext(int current) =>
        this[current + 1];

    public T GetPrevious(T current) =>
        GetPrevious(Array.IndexOf(_items, current));

    public T GetNext(T current) =>
        GetNext(Array.IndexOf(_items, current));

    private int GetActualIndex(int index)
    {
        index %= _items.Length;
        if (index < 0)
            index += _items.Length;

        return index;
    }
}
