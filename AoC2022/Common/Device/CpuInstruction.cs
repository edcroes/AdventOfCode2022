namespace AoC2022.Common.Device;

public record struct CpuInstruction(string OpCode, int Value)
{
    public static CpuInstruction Parse(string line)
    {
        var (opCode, value) = line.Split(' ');
        return new(opCode!, value.IsNotNullOrWhitespace() ? int.Parse(value) : 0);
    }
};