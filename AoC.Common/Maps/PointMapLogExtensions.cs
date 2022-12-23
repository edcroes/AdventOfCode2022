using System.Text;

namespace AoC.Common.Maps;

public static class PointMapLogExtensions
{
    public static void DumpMapToConsole<T>(this PointMap<T> map, Func<T, char> mapToChar) =>
        Console.WriteLine(map.DumpMapToString(mapToChar));

    public static async Task DumpMapToFile<T>(this PointMap<T> map, string filePath, Func<T, char> mapToChar)
    {
        var mapDump = map.DumpMapToString(mapToChar);
        await File.WriteAllTextAsync(filePath, mapDump);
    }

    public static string DumpMapToString<T>(this PointMap<T> map, Func<T, char> mapToChar)
    {
        var bounds = map.GetBoundingRectangle();
        StringBuilder builder = new();

        for (var y = bounds.Y; y <= bounds.Size.Height; y++)
        {
            for (var x = bounds.X; x <= bounds.Size.Width; x++)
            {
                builder.Append(mapToChar(map.GetValueOrDefault(x, y)));
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }
}
