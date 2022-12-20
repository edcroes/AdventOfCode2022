namespace AoC.Common;

public class BoxedValue<T> where T : struct
{
    public required T Value { get; set; }

    public static implicit operator T(BoxedValue<T> box) => box.Value;

    public static explicit operator BoxedValue<T>(T value) => new() { Value = value };
}
