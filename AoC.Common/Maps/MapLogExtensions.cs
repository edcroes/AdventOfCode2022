﻿using System.Text;

namespace AoC.Common.Maps;
public static class MapLogExtensions
{
    public static void DumpMapToConsole<T>(this Map<T> map) =>
        Console.WriteLine(map.DumpMapToString());

    public static async Task DumpMapToFile<T>(this Map<T> map, string filePath)
    {
        var mapDump = map.DumpMapToString();
        await File.WriteAllTextAsync(filePath, mapDump);
    }

    public static string DumpMapToString<T>(this Map<T> map)
    {
        StringBuilder builder = new();
        for (var y = 0; y < map.SizeY; y++)
        {
            for (var x = 0; x < map.SizeX; x++)
            {
                builder.Append(map.GetValue(x, y));
            }
            builder.AppendLine();
        }

        return builder.ToString().Trim();
    }
}
