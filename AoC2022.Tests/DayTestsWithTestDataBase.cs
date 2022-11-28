namespace AoC2022.Tests;

public class DayTestsWithTestDataBase<T> : DayTestsBase<T> where T : IMDay, new()
{
    private readonly string _expectedAnswerPart1TestData;
    private readonly string _expectedAnswerPart2TestData;
    private readonly IMDay _dayWithTestDataToTest = new T() { FilePath = $"TestData\\{typeof(T).Name}-testinput.txt" };

    public DayTestsWithTestDataBase(
        string expectedAnswerPart1,
        string expectedAnswerPart2,
        string expectedAnswerPart1TestData,
        string expectedAnswerPart2TestData
        ) : base(expectedAnswerPart1, expectedAnswerPart2)
    {
        _expectedAnswerPart1TestData = expectedAnswerPart1TestData;
        _expectedAnswerPart2TestData = expectedAnswerPart2TestData;
    }

    [TestMethod]
    public async Task Part1WithTestDataTest()
    {
        var answerPart1 = await _dayWithTestDataToTest.GetAnswerPart1();
        Assert.AreEqual(_expectedAnswerPart1TestData, answerPart1);
    }

    [TestMethod]
    public async Task Part2WithTestDataTest()
    {
        var answerPart2 = await _dayWithTestDataToTest.GetAnswerPart2();
        Assert.AreEqual(_expectedAnswerPart2TestData, answerPart2);
    }
}
