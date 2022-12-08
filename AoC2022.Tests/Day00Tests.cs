namespace AoC2022.Tests;

[TestClass]
public class Day00Tests : DayTestsWithTestDataBase<Day00.Day00>
{
    public Day00Tests() : base(
        "38",
        string.Empty + Environment.NewLine +
        "X     X   XXX     XXX  XXX  XXXXX   XXXX  X   X" + Environment.NewLine +
        "XX   XX  X   X   X      X   X      X   X  X   X" + Environment.NewLine +
        "X X X X  X   X  X       X   X      X      X   X" + Environment.NewLine +
        "X  X  X  XXXXX  X  XX   X    XXX   X      XXXXX" + Environment.NewLine +
        "X     X  X   X  X   X   X       X  X      X   X" + Environment.NewLine +
        "X     X  X   X   X  X   X       X  X   X  X   X" + Environment.NewLine +
        "X     X  X   X    XXX  XXX  XXXX    XXX   X   X" + Environment.NewLine,
        "12",
        string.Empty + Environment.NewLine +
        "          X" + Environment.NewLine +
        "         X " + Environment.NewLine +
        "XXXXXXX X  " + Environment.NewLine
    ) { }
}