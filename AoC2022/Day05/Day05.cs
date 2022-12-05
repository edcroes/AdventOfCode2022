using AoC.Common;

namespace AoC2022.Day05;

public class Day05 : IMDay
{
    private record struct Movement(int Move, int From, int To);

    public string FilePath { private get; init; } = "Day05\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (stacks, moves) = await ParseInputFile();

        foreach (var move in moves)
        {
            for (var step = 0; step < move.Move; step++)
            {
                var crate = stacks[move.From - 1].Pop();
                stacks[move.To - 1].Push(crate);
            }
        }

        return string.Join(string.Empty, stacks.Select(s => s.Peek()));
    }

    public async Task<string> GetAnswerPart2()
    {
        var (stacks, moves) = await ParseInputFile();

        foreach (var move in moves)
        {
            var cratesToMove = Enumerable.Range(0, move.Move)
                .Select(m => stacks[move.From - 1].Pop())
                .Reverse();
            foreach (var crate in cratesToMove)
            {
                stacks[move.To - 1].Push(crate);
            }
        }

        return string.Join(string.Empty, stacks.Select(s => s.Peek()));
    }

    private async Task<(Stack<char>[] stacks, IEnumerable<Movement>)> ParseInputFile()
    {
        var input = await GetInput();
        var crates = input[0].Reverse().ToArray();
        var stacks = crates[0]
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(c => new Stack<char>())
            .ToArray();

        foreach (var row in crates[1..])
        {
            for (var rowNumber = 0; rowNumber < stacks.Length; rowNumber++)
            {
                var crate = row[rowNumber * 4 + 1];
                if (crate != ' ')
                {
                    stacks[rowNumber].Push(crate);
                }
            }
        }

        var moves = input[1].Select(ParseMove);

        return (stacks, moves);
    }

    private static Movement ParseMove(string movement)
    {
        var splits = movement.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        return new Movement(int.Parse(splits[1]), int.Parse(splits[3]), int.Parse(splits[5]));
    }

    private async Task<string[][]> GetInput() =>
        await FileParser.ReadBlocksAsStringArray(FilePath);
}
