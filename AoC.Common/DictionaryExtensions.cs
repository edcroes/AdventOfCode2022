namespace AoC.Common;

public static class DictionaryExtensions
{
    public static void AddOrSet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    public static void AddOrUpdate<T>(this IDictionary<T,int> dictionary, T key, int value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] += value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    public static void AddOrReplaceIfGreaterThan<T>(this IDictionary<T, int> dictionary, T key, int value)
    {
        if (dictionary.ContainsKey(key))
        {
            if (value > dictionary[key])
            {
                dictionary[key] = value;
            }
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    public static void AddOrUpdate<T>(this IDictionary<T, long> dictionary, T key, long value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] += value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary, TKey key, IEnumerable<TValue> values)
    {
        foreach (var value in values)
        {
            dictionary.AddOrUpdate(key, value);
        }
    }

    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key].Add(value);
        }
        else
        {
            dictionary.Add(key, new List<TValue> { value });
        }
    }

    public static void RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> valuesToRemove)
    {
        foreach (var valueToRemove in valuesToRemove)
        {
            dictionary.Remove(valueToRemove);
        }
    }

    public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue @default) =>
        dictionary.TryGetValue(key, out TValue? value) ? value : @default;
}