namespace AoC.Common;

public static class ArrayExtensions
{
    public static T[][] Split<T>(this T[] source, params T[] separators)
    {
        var parts = new List<T[]>();
        var remainder = (T[])source.Clone();

        int index;
        while ((index = remainder.IndexOfMultiple(separators)) > -1)
        {
            var left = remainder.Take(index).ToArray();

            if (left.Length > 0)
            {
                parts.Add(left);
            }
            remainder = remainder.Skip(index + 1).ToArray();
        }

        if (remainder.Length > 0)
        {
            parts.Add(remainder);
        }

        return parts.ToArray();
    }

    public static T[] Replace<T>(this T[] source, T[] oldValue, T[] newValue)
    {
        var newArray = (T[])source.Clone();

        int index;
        while ((index = newArray.IndexOf(oldValue)) > -1)
        {
            var newSize = newArray.Length - oldValue.Length + newValue.Length;
            var replacement = new T[newSize];

            if (index > 0)
            {
                Array.Copy(newArray, 0, replacement, 0, index);
            }

            Array.Copy(newValue, 0, replacement, index, newValue.Length);

            if (index + oldValue.Length < newArray.Length)
            {
                Array.Copy(newArray, index + oldValue.Length, replacement, index + newValue.Length, newArray.Length - index - oldValue.Length);
            }

            newArray = replacement;
        }

        return newArray;
    }

    public static int IndexOfMultiple<T>(this T[] source, params T[] values)
    {
        if (source == null || values == null || source.Length == 0 || values.Length == 0)
        {
            return -1;
        }

        var firstIndex = -1;

        foreach (var value in values)
        {
            var index = Array.IndexOf(source, value);

            if (index > -1)
            {
                firstIndex = firstIndex == -1 ? index : Math.Min(firstIndex, index);
            }
        }

        return firstIndex;
    }

    public static int IndexOf<T>(this T[] source, T[] value)
    {
        if (source == null || value == null || source.Length == 0 || source.Length < value.Length)
        {
            return -1;
        }

        var sourceIndex = 0;
        var valueIndex = 0;

        while (sourceIndex < source.Length && valueIndex < value.Length)
        {
            if (source[sourceIndex].Equals(value[valueIndex]))
            {
                sourceIndex++;
                valueIndex++;

                if (valueIndex == value.Length)
                {
                    return sourceIndex - valueIndex;
                }
            }
            else
            {
                sourceIndex -= valueIndex - 1;
                valueIndex = 0;
            }
        }

        return -1;
    }

    public static int IndexOf<T>(this T[] array, T value) =>
        Array.IndexOf(array, value);

    public static T[][] SplitInBlocksOf<T>(this T[] source, int blockSize)
    {
        if (source == null || source.Length == 0 || blockSize < 2)
        {
            return Array.Empty<T[]>();
        }

        if (source.Length % blockSize != 0)
        {
            throw new NotSupportedException("Arrays can only be split in blocks of equal sizes. Source length mod blockSize should be zero.");
        }

        var array = new T[source.Length / blockSize][];

        for (var i = 0; i < source.Length / blockSize; i++)
        {
            var start = i * blockSize;
            array[i] = source[start..(start + blockSize)];
        }

        return array;
    }

    public static int IndexOfClosingTag<T>(this T[] source, int openTagIndex, T openTag, T closingTag)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        if (openTagIndex >= source.Length || openTagIndex < 0)
            throw new IndexOutOfRangeException(nameof(openTagIndex));

        var depth = 0;
        for (var i = openTagIndex + 1; i < source.Length; i++)
        {
            if (source[i].Equals(openTag))
                depth++;
            else if (source[i].Equals(closingTag))
            {
                if (depth == 0)
                    return i;
                depth--;
            }
        }

        return -1;
    }

    public static void Deconstruct<T>(this T[] source, out T? value1, out T? value2)
    {
        value1 = source.ElementAtOrDefault(0);
        value2 = source.ElementAtOrDefault(1);
    }

    public static void Deconstruct<T>(this T[] source, out T? value1, out T? value2, out T? value3)
    {
        value1 = source.ElementAtOrDefault(0);
        value2 = source.ElementAtOrDefault(1);
        value3 = source.ElementAtOrDefault(2);
    }

    public static void Deconstruct<T>(this T[] source, out T? value1, out T? value2, out T? value3, out T? value4)
    {
        value1 = source.ElementAtOrDefault(0);
        value2 = source.ElementAtOrDefault(1);
        value3 = source.ElementAtOrDefault(2);
        value4 = source.ElementAtOrDefault(3);
    }
}