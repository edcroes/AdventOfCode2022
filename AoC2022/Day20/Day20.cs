namespace AoC2022.Day20;

public class Day20 : IMDay
{
    public string FilePath { private get; init; } = "Day20\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        List<BoxedValue<long>> ordered = new(input);
        
        MixNumbers(input, ordered);

        var answer = GetFinalAnswer(input);
        return answer.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        const long decryptionKey = 811589153;
        var input = await GetInput();
        List<BoxedValue<long>> ordered = new(input);

        foreach (var value in input)
        {
            value.Value *= decryptionKey;
        }

        for (var i = 0; i < 10; i++)
        {
            MixNumbers(input, ordered);
        }

        var answer = GetFinalAnswer(input);
        return answer.ToString();
    }

    private static void MixNumbers(List<BoxedValue<long>> input, List<BoxedValue<long>> order)
    {
        foreach (var value in order)
        {
            var index = input.IndexOf(value);
            input.RemoveAt(index);
            long move = (index + value) % input.Count;

            while (move < 0)
                move += input.Count;

            input.Insert((int)move, value);
        }
    }

    private static long GetFinalAnswer(List<BoxedValue<long>> input)
    {
        var zero = input.Single(l => l == 0);
        var zeroIndex = input.IndexOf(zero);

        var after1000Index = (zeroIndex + 1000) % input.Count;
        var after2000Index = (zeroIndex + 2000) % input.Count;
        var after3000Index = (zeroIndex + 3000) % input.Count;

        return input[after1000Index] + input[after2000Index] + input[after3000Index];
    }

    private async Task<List<BoxedValue<long>>> GetInput() =>
        (await FileParser.ReadLinesAsLong(FilePath))
            .Select(l => new BoxedValue<long> { Value = l })
            .ToList();
}
