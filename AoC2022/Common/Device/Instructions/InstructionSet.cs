namespace AoC2022.Common.Device.Instructions;

public class InstructionSet
{
    private readonly IInstruction[] _instructions;

    private InstructionSet(IEnumerable<IInstruction> instructions)
    {
        _instructions = instructions.ToArray();
    }

    public bool SupportsInstruction(string name) =>
        _instructions.Any(i => i.Name == name);

    public IInstruction GetInstruction(string name) =>
        _instructions.FirstOrDefault(i => i.Name == name) ?? throw new NotSupportedException($"OpCode {name} is not supported");

    public static InstructionSet DefaultSet { get; } = new InstructionSet(new IInstruction[]
    {
        new NoopInstruction(),
        new AddInstruction()
    });
}
