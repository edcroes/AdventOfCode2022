using System.Text.RegularExpressions;

namespace AoC.Common;

public static class MatchExtensions
{
    public static int GetInt(this Match match, string groupName) =>
        int.Parse(match.Groups[groupName].Value);

    public static string GetString(this Match match, string groupName) =>
        match.Groups[groupName].Value;

    public static string[] GetStringArray(this Match match, string groupName, string separator) =>
        match.Groups[groupName].Value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
}