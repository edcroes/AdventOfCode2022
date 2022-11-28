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
}