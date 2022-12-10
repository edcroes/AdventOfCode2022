namespace AoC2022.Day10;

public class Day10 : IMDay
{
    public string FilePath { private get; init; } = "Day10\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var device = await GetExecutedDevice();

        return (new[] { 20, 60, 100, 140, 180, 220 })
            .Select(device.GetSignalStrength)
            .Sum()
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var device = await GetExecutedDevice();
        return device.GetDisplay();
    }

    private async Task<DeviceCpu> GetExecutedDevice()
    {
        var input = await GetInput();
        var instructions = input.Select(CpuInstruction.Parse);

        DeviceCpu device = new();
        device.Execute(instructions);

        return device;
    }

    private async Task<string[]> GetInput() =>
        await FileParser.ReadLinesAsString(FilePath);
}
