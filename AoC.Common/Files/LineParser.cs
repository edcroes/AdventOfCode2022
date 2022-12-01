namespace AoC.Common.Files;

public static class LineParser
{
    public static int[] ToIntArray(this string line) =>
        line
            .ToCharArray()
            .Select(c => int.Parse(c.ToString()))
            .ToArray();

    public static int[] ToIntArray(this string line, string separator) =>
        line
            .Split(separator, StringSplitOptions.RemoveEmptyEntries)
            .Select(c => int.Parse(c.ToString()))
            .ToArray();

    public static bool[] ToBoolArray(this string line, char trueValue) =>
        line
            .ToCharArray()
            .Select(c => c == trueValue)
            .ToArray();
}
