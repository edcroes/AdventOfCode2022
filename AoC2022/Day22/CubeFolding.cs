using System.Diagnostics.CodeAnalysis;

namespace AoC2022.Day22;

public class CubeFolding<T>
{
    [NotNull]
    public required T VoidValue { get; init; }

    public List<Face> GetFacesFromMap(Map<T> map)
    {
        List<Face> faces = new();

        var faceSize = GetRectangleSize(map);
        for (var y = 0; y < map.SizeY; y += faceSize)
        {
            for (var x = 0; x < map.SizeX; x += faceSize)
            {
                if (!VoidValue.Equals(map.GetValue(x, y)))
                {
                    faces.Add(new() { LeftUpperCorner = new(x, y), Size = faceSize });
                }
            }
        }

        faces.ForEach(f => GetDirectFaceReferences(f, faces));
        while (faces.Any(f => f.Left is null || f.Right is null || f.Top is null || f.Bottom is null))
        {
            faces.ForEach(GetIndirectFaceReferences);
        }

        return faces;
    }

    private static void GetDirectFaceReferences(Face face, List<Face> faces)
    {
        Dictionary<Side, Point> movement = new()
        {
            { Side.Right, new(face.Size, 0) },
            { Side.Bottom, new(0, face.Size) },
            { Side.Left, new(-face.Size, 0) },
            { Side.Top, new(0, -face.Size) }
        };

        foreach (Side side in Enum.GetValues(typeof(Side)))
        {
            if (face.Get(side) is null)
            {
                var other = faces.FirstOrDefault(f => f.LeftUpperCorner == face.LeftUpperCorner.Add(movement[side]));
                if (other is not null)
                {
                    SetConnectedSides(face, side, other, side.GetPrevious().GetPrevious());
                }
            }
        }
    }

    /// <summary>
    /// Tries to get diagonal neighbors for the side.
    /// So for side ~~ it tries to find one of the two possible : sides.
    /// 
    /// So if ~~ is Top then:
    /// +--+  +--+
    /// |  :  :  |
    /// +--+~~+--+
    /// |  |  |  |
    /// +--+--+--+
    /// 
    /// If ~~ is Right then:
    /// +--+--+
    /// |  |  |
    /// +--+..+
    /// |  ~
    /// +--+..+
    /// |  |  |
    /// +--+--+
    /// </summary>
    private static void GetIndirectFaceReferences(Face face)
    {
        foreach (Side side in Enum.GetValues(typeof(Side)))
        {
            if (face.Get(side) is null)
            {
                FaceReference? other = null;
                Side? otherSide = null;
                var previous = face.Get(side.GetPrevious());
                var next = face.Get(side.GetNext());

                if (previous is not null)
                {
                    other = previous.Face.Get(previous.Side.GetPrevious());
                    otherSide = other?.Side.GetPrevious();

                }
                else if (next is not null)
                {
                    other = next.Face.Get(next.Side.GetNext());
                    otherSide = other?.Side.GetNext();
                }

                if (other is not null && otherSide is not null)
                {
                    SetConnectedSides(face, side, other.Face, otherSide.Value);
                }
            }
        }
    }

    private static void SetConnectedSides(Face face, Side side, Face other, Side otherSide)
    {
        //Console.WriteLine($"{face.LeftUpperCorner} {side} -> {other.LeftUpperCorner} {otherSide}");
        face.Set(side, new() { Face = other, Side = otherSide });
        other.Set(otherSide, new() { Face = face, Side = side });
    }

    private static int GetRectangleSize(Map<T> map) =>
        map switch
        {
            Map<char> m when (double)m.SizeX / m.SizeY == 0.4 => m.SizeX / 2, // 2 by 5
            Map<char> m when (double)m.SizeY / m.SizeX == 0.4 => m.SizeY / 2, // 5 by 2
            Map<char> m when (double)m.SizeX / m.SizeY == 0.75 => m.SizeX / 3, // 3 by 4
            Map<char> m when (double)m.SizeY / m.SizeX == 0.75 => m.SizeY / 3, // 4 by 3
            _ => throw new NotSupportedException("This is not a supported cube layout")
        };
}
