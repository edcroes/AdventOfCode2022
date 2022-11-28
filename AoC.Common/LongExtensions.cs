namespace AoC.Common;

public static class LongExtensions
{
    public static int GetNthDigit(this long value, int n) =>
        (int)(value / (long)Math.Pow(10, n) % 10);

    public static long SetNthDigit(this long value, int n, int to) =>
         value + ((to - value.GetNthDigit(n)) * (long)Math.Pow(10, n));
}