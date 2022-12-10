using AoC2022.Common.Device.Instructions;

namespace AoC2022.Common.Device;

public class DeviceCpu
{
    private const int ScreenWidth = 40;

    private readonly Dictionary<int, int> _registerXLog = new();
    private readonly InstructionSet _instructionSet = InstructionSet.DefaultSet;

    public Dictionary<char, int> Registers { get; } = new() { { 'X', 1 } };
    public int Cycle { get; private set; }

    public void Execute(IEnumerable<CpuInstruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            Execute(instruction);
        }
    }

    private void Execute(CpuInstruction instruction)
    {
        _instructionSet
            .GetInstruction(instruction.OpCode)
            .Execute(this, instruction.Value);
    }

    public void Tick()
    {
        Cycle++;
        _registerXLog.Add(Cycle, Registers['X']);
    }

    public int GetSignalStrength(int cycle) =>
        cycle * _registerXLog[cycle];

    public string GetDisplay()
    {
        StringBuilder display = new();
        display.AppendLine();

        for (var y = 0; y < _registerXLog.Count / ScreenWidth; y++)
        {
            for (var x = 0; x < ScreenWidth; x++)
            {
                var cycle = y * ScreenWidth + x + 1;
                var currentSpriteValue = _registerXLog[cycle];
                char pixel = x >= currentSpriteValue - 1 && x <= currentSpriteValue + 1 ? '#' : '.';
                display.Append(pixel);

            }
            display.AppendLine();
        }

        return display.ToString().TrimEnd();
    }
}
