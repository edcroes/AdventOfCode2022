namespace AoC.Common;

public static class EnumExtenions
{
    public static T GetPrevious<T>(this T value) where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Length;
        var enumValue = (value.ToInt() - 1 + values) % values;
        return enumValue.ToEnum<T>();
    }

    public static T GetNext<T>(this T value) where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Length;
        var enumValue = (value.ToInt() + 1) % values;
        return enumValue.ToEnum<T>();
    }

    private static int ToInt<T>(this T value) where T : Enum =>
        (int)(object)value;

    private static T ToEnum<T>(this int value) where T : Enum =>
        (T)(object)value;
}
