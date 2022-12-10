namespace AoC2022.Common.Device.Instructions;

public interface IInstruction
{
    string Name { get; init; }
    void Execute(DeviceCpu cpu, int parameter);
}
