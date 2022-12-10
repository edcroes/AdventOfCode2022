namespace AoC2022.Tests;

[TestClass]
public class Day10Tests : DayTestsWithTestDataBase<Day10.Day10>
{
    public Day10Tests() : base(
        "14920",
        Environment.NewLine +
        "###..#..#..##...##...##..###..#..#.####." + Environment.NewLine +
        "#..#.#..#.#..#.#..#.#..#.#..#.#..#....#." + Environment.NewLine +
        "###..#..#.#....#..#.#....###..#..#...#.." + Environment.NewLine +
        "#..#.#..#.#....####.#....#..#.#..#..#..." + Environment.NewLine +
        "#..#.#..#.#..#.#..#.#..#.#..#.#..#.#...." + Environment.NewLine +
        "###...##...##..#..#..##..###...##..####.",
        "13140",
        Environment.NewLine +
        "##..##..##..##..##..##..##..##..##..##.." + Environment.NewLine +
        "###...###...###...###...###...###...###." + Environment.NewLine +
        "####....####....####....####....####...." + Environment.NewLine +
        "#####.....#####.....#####.....#####....." + Environment.NewLine +
        "######......######......######......####" + Environment.NewLine +
        "#######.......#######.......#######....."
    ) { }
}