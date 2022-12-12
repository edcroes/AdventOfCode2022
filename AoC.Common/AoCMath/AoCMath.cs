namespace AoC.Common.AoCMath;

public static class AoCMath
{
    public static long Factorial(int value)
    {
        var fact = 1L;
        for (int next = value; next > 0; next--)
        {
            fact *= next;
        }
        return fact;
    }

    public static long GreatestCommonDivisor(long a, long b)
    {
        if (a < 0 || b < 0)
        {
            throw new ArgumentException("Both a and b should be greater than or equal to 0");
        }

        if (a == 0)
        {
            return b;
        }

        while (b != 0)
        {
            var newA = b;
            b = a % b;
            a = newA;
        }

        return a;
    }

    public static long GreatestCommonDivisor(IEnumerable<long> values) =>
        values.Aggregate(GreatestCommonDivisor);

    public static long LeastCommonMultiple(long a, long b) =>
        a * b / GreatestCommonDivisor(a, b);

    public static long LeastCommonMultiple(IEnumerable<long> values) =>
        values.Aggregate(LeastCommonMultiple);
}