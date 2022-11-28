namespace AoC2022;

public interface IMDay
{
    string FilePath { init; }
    Task<string> GetAnswerPart1();
    Task<string> GetAnswerPart2();
}
