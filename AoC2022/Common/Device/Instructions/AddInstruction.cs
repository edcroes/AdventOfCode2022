namespace AoC2022.Common.Device.Instructions;

public class AddInstruction : IInstruction
{
    public string Name { get; init; } = "addx";

    public void Execute(DeviceCpu cpu, int parameter)
    {
        cpu.Tick();
        cpu.Tick();
        cpu.Registers['X'] += parameter;
    }
}
