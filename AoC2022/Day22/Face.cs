namespace AoC2022.Day22;

public class Face
{
    private readonly FaceReference?[] _references = new FaceReference?[4];

    public FaceReference? Right
    {
        get => _references[(int)Side.Right];
        set => _references[(int)Side.Right] = value;
    }

    public FaceReference? Left
    {
        get => _references[(int)Side.Left];
        set => _references[(int)Side.Left] = value;
    }

    public FaceReference? Top
    {
        get => _references[(int)Side.Top];
        set => _references[(int)Side.Top] = value;
    }

    public FaceReference? Bottom
    {
        get => _references[(int)Side.Bottom];
        set => _references[(int)Side.Bottom] = value;
    }

    public required Point LeftUpperCorner { get; init; }

    public required int Size { get; init; }

    public bool Contains(Point point) =>
        point.X >= LeftUpperCorner.X && point.X < LeftUpperCorner.X + Size &&
        point.Y >= LeftUpperCorner.Y && point.Y < LeftUpperCorner.Y + Size;

    public void Set(Side side, FaceReference faceReference) =>
        _references[(int)side] = faceReference;

    public FaceReference? Get(Side? side) =>
        side.HasValue ? _references[(int)side] : null;
}