namespace AoC2022.Day08;

public class Day08 : IMDay
{
    public string FilePath { private get; init; } = "Day08\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var trees = await GetInput();
        var treeCount = trees.SizeY * 2 + trees.SizeX * 2 - 4;

        for (var x = 1; x < trees.SizeX - 1; x++)
        {
            for (var y = 1; y < trees.SizeY - 1; y++)
            {
                if (trees.GetLine(x - 1, y, 0, y).All(t => t < trees.GetValue(x, y)) ||
                    trees.GetLine(x + 1, y, trees.SizeX - 1, y).All(t => t < trees.GetValue(x, y)) ||
                    trees.GetLine(x, y - 1, x, 0).All(t => t < trees.GetValue(x, y)) ||
                    trees.GetLine(x, y + 1, x, trees.SizeY - 1).All(t => t < trees.GetValue(x, y)))
                {
                    treeCount++;
                }
            }
        }

        return treeCount.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var trees = await GetInput();
        var maxTreeScenicScore = 0;

        for (var x = 1; x < trees.SizeX - 1; x++)
        {
            for (var y = 1; y < trees.SizeY - 1; y++)
            {
                var height = trees.GetValue(x, y);
                var currentScenicScore =
                    GetLineScore(trees.GetLine(x - 1, y, 0, y), height) *
                    GetLineScore(trees.GetLine(x + 1, y, trees.SizeX - 1, y), height) *
                    GetLineScore(trees.GetLine(x, y - 1, x, 0), height) *
                    GetLineScore(trees.GetLine(x, y + 1, x, trees.SizeY - 1), height);

                maxTreeScenicScore = Math.Max(maxTreeScenicScore, currentScenicScore);
            }
        }

        return maxTreeScenicScore.ToString();
    }

    private static int GetLineScore(int[] line, int height)
    {
        var score = 0;

        foreach (var tree in line)
        {
            score++;
            if (tree >= height)
                break;
        }

        return score;
    }

    private async Task<Map<int>> GetInput() =>
        new Map<int>(await FileParser.ReadLinesAsIntArray(FilePath));
}
