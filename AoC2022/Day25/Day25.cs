namespace AoC2022.Day25;

public partial class Day25 : IMDay
{
    public string FilePath { private get; init; } = "Day25\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        SnafuNumber result = input.Select(SnafuNumber.Parse).Sum(n => n.Number);

        return result.ToString();
    }

    public Task<string> GetAnswerPart2()
    {
        return Task.FromResult("No part 2 today :)");
    }

    private async Task<string[]> GetInput() =>
        await FileParser.ReadLinesAsString(FilePath);

    private partial record struct SnafuNumber(long Number)
    {
        private static readonly Dictionary<char, int> _translation = new()
        {
            { '2', 2 },
            { '1', 1 },
            { '0', 0 },
            { '-', -1 },
            { '=', -2 }
        };

        private static readonly Regex _validNumberRegex = GetRegex();
        private string Snafu { get; set; }

        public static SnafuNumber Parse(string s)
        {
            if (!_validNumberRegex.IsMatch(s))
                throw new FormatException("Input string was not in a correct format");

            var current = 1L;
            var result = 0L;
            foreach (var part in s.Reverse())
            {
                result += _translation[part] * current;
                current *= 5;
            }

            return new(result) { Snafu = s };
        }

        public override string ToString() =>
            Snafu ??= GetSnafuNumber();

        private string GetSnafuNumber()
        {
            string result = string.Empty;
            var current = 1L;
            while((current * 2 + current / 2) < Number)
                current *= 5;

            var remaining = Number;
            while (current > 0)
            {
                var multiplier = remaining < 0 ? -1 : 1;
                if (current / 2 >= Math.Abs(remaining))
                    result += '0';
                else if (current + (current / 2) < Math.Abs(remaining))
                {
                    result += remaining < 0 ? '=' : '2';
                    remaining -= 2 * current * multiplier;
                }
                else
                {
                    result += remaining < 0 ? '-' : '1';
                    remaining -= current * multiplier;
                }

                current /= 5;
            }

            return result;
        }

        public static implicit operator SnafuNumber(long number) =>
            new(number);

        [GeneratedRegex("^[012=-]+$")]
        private static partial Regex GetRegex();
    }
}
