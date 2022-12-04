namespace AoC.Common;

public static class StringExtensions
{
    public static bool IsNotNullOrEmpty(this string value) =>
        !string.IsNullOrEmpty(value);

    public static string[] SplitOnNewLine(this string value) =>
        value
            .Replace("\r", string.Empty)
            .Split("\n", StringSplitOptions.RemoveEmptyEntries);

    public static string[] SplitOnTwoNewLines(this string value) =>
        value
            .Replace("\r", string.Empty)
            .Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

    public static (string, string) SplitInHalf(this string value) =>
        (value[..(value.Length / 2)], value[(value.Length / 2)..]);
}