using System.Runtime.InteropServices;

namespace AoC2022.Day22;
public static class FaceExtensions
{
    public static Point RotateLeft(this Face face, Point point)
    {
        var moveToZero = point.Subtract(face.LeftUpperCorner);
        Point newPointFromZero = new(moveToZero.Y, face.Size - 1 - moveToZero.X);
        return newPointFromZero.Add(face.LeftUpperCorner);
    }

    public static Point RotateRight(this Face face, Point point)
    {
        var moveToZero = point.Subtract(face.LeftUpperCorner);
        Point newPointFromZero = new(face.Size - 1 - moveToZero.Y, moveToZero.X);
        return newPointFromZero.Add(face.LeftUpperCorner);
    }

    public static void DumpToBitmap<T>(this IEnumerable<Face> faces, string filePath, Map<T> map)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return;

        Color[] colors = new[]
        {
            Color.Black,
            Color.Red,
            Color.Green,
            Color.Yellow,
            Color.Orange,
            Color.Blue,
            Color.LightBlue,
            Color.LightGray,
            Color.LightGreen,
            Color.Magenta,
            Color.SandyBrown,
            Color.Purple
        };

        Dictionary<Face, List<Side>> drawnSides = new();
        foreach (var face in faces)
        {
            drawnSides.Add(face, new());
        }

        using Bitmap image = new(map.SizeX, map.SizeY);
        using var graphics = Graphics.FromImage(image);

        var colorIndex = 0;
        foreach (var face in faces)
        {
            foreach (Side side in Enum.GetValues(typeof(Side)))
            {
                if (!drawnSides[face].Contains(side))
                {
                    var other = face.Get(side);

                    graphics.DrawLine(colors[colorIndex], face, side);
                    graphics.DrawLine(colors[colorIndex], other!.Face, other.Side);
                    colorIndex++;

                    drawnSides[face].Add(side);
                    drawnSides[other.Face].Add(other.Side);
                }
            }
        }

        image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
    }

    private static void DrawLine(this Graphics graphics, Color color, Face face, Side side)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return;

        Console.WriteLine($"Draw {face.LeftUpperCorner} {side}");

        using Pen pen = new(color, 1);
        Point start = side switch
        {
            Side.Top => new(face.LeftUpperCorner.X + 1, face.LeftUpperCorner.Y),
            Side.Bottom => new(face.LeftUpperCorner.X + 1, face.LeftUpperCorner.Y + face.Size - 1),
            Side.Left => new(face.LeftUpperCorner.X, face.LeftUpperCorner.Y + 1),
            Side.Right => new(face.LeftUpperCorner.X + face.Size - 1, face.LeftUpperCorner.Y + 1)
        };

        Point end = side switch
        {
            Side.Top => new(face.LeftUpperCorner.X + face.Size - 2, face.LeftUpperCorner.Y),
            Side.Bottom => new(face.LeftUpperCorner.X + face.Size - 2, face.LeftUpperCorner.Y + face.Size - 1),
            Side.Left => new(face.LeftUpperCorner.X, face.LeftUpperCorner.Y + face.Size - 2),
            Side.Right => new(face.LeftUpperCorner.X + face.Size - 1, face.LeftUpperCorner.Y + face.Size - 2)
        };

        graphics.DrawLine(pen, start, end);
    }
}
