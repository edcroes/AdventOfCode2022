namespace AoC2022.Common.Device.Instructions;

public class NoopInstruction : IInstruction
{
    public string Name { get; init; } = "noop";

    public void Execute(DeviceCpu cpu, int parameter)
    {
        cpu.Tick();
    }
}
