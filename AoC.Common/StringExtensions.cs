using System.Diagnostics.CodeAnalysis;

namespace AoC.Common;

public static class StringExtensions
{
    public static bool IsNotNullOrEmpty([AllowNull][NotNullWhen(true)] this string value) =>
        !string.IsNullOrEmpty(value);

    public static bool IsNullOrEmpty([AllowNull][NotNullWhen(false)] this string value) =>
        string.IsNullOrEmpty(value);

    public static bool IsNotNullOrWhitespace([AllowNull][NotNullWhen(true)] this string value) =>
        !string.IsNullOrWhiteSpace(value);

    public static bool IsNullOrWhitespace([AllowNull][NotNullWhen(false)] this string value) =>
        string.IsNullOrWhiteSpace(value);

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

    public static int IndexOfClosingTag(this string value, int openTagIndex, char openTag, char closingTag) =>
        value.ToCharArray().IndexOfClosingTag(openTagIndex, openTag, closingTag);
}