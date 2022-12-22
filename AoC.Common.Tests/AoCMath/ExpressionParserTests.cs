namespace AoC.Common.Tests.AoCMath;

[TestClass]
public class ExpressionParserTests
{
    [TestMethod]
    public void ShouldParseAddCorrectWithLeftParameterAndRightConstant()
    {
        var func = ExpressionParser.ParseSimpleMathExpressionWithOneInput<int>("i => i + 10");
        Assert.AreEqual(15, func(5));
    }

    [TestMethod]
    public void ShouldParseAddCorrectWithLeftConstantAndRightParameter()
    {
        var func = ExpressionParser.ParseSimpleMathExpressionWithOneInput<int>("i => 10 + i");
        Assert.AreEqual(15, func(5));
    }

    [TestMethod]
    public void ShouldParseAddCorrectWithLeftParameterAndRightParameter()
    {
        var func = ExpressionParser.ParseSimpleMathExpressionWithOneInput<int>("i => i + i");
        Assert.AreEqual(10, func(5));
    }

    [TestMethod]
    public void ShouldParseSubtractCorrectWithLeftParameterAndRightConstant()
    {
        var func = ExpressionParser.ParseSimpleMathExpressionWithOneInput<int>("i => i - 10");
        Assert.AreEqual(-5, func(5));
    }

    [TestMethod]
    public void ShouldParseSubtractCorrectWithLeftConstantAndRightParameter()
    {
        var func = ExpressionParser.ParseSimpleMathExpressionWithOneInput<int>("i => 10 - i");
        Assert.AreEqual(5, func(5));
    }

    [TestMethod]
    public void ShouldParseSubtractCorrectWithLeftParameterAndRightParameter()
    {
        var func = ExpressionParser.ParseSimpleMathExpressionWithOneInput<int>("i => i - i");
        Assert.AreEqual(0, func(5));
    }

    [TestMethod]
    public void ShouldParseDivideCorrectWithLeftParameterAndRightConstant()
    {
        var func = ExpressionParser.ParseSimpleMathExpressionWithOneInput<int>("i => i / 10");
        Assert.AreEqual(5, func(50));
    }

    [TestMethod]
    public void ShouldParseDivideCorrectWithLeftConstantAndRightParameter()
    {
        var func = ExpressionParser.ParseSimpleMathExpressionWithOneInput<int>("i => 10 / i");
        Assert.AreEqual(2, func(5));
    }

    [TestMethod]
    public void ShouldParseDivideCorrectWithLeftParameterAndRightParameter()
    {
        var func = ExpressionParser.ParseSimpleMathExpressionWithOneInput<int>("i => i / i");
        Assert.AreEqual(1, func(5));
    }

    [TestMethod]
    public void ShouldParseMultiplyCorrectWithLeftParameterAndRightConstant()
    {
        var func = ExpressionParser.ParseSimpleMathExpressionWithOneInput<int>("i => i * 10");
        Assert.AreEqual(500, func(50));
    }

    [TestMethod]
    public void ShouldParseMultiplyCorrectWithLeftConstantAndRightParameter()
    {
        var func = ExpressionParser.ParseSimpleMathExpressionWithOneInput<int>("i => 10 * i");
        Assert.AreEqual(50, func(5));
    }

    [TestMethod]
    public void ShouldParseMultiplyCorrectWithLeftParameterAndRightParameter()
    {
        var func = ExpressionParser.ParseSimpleMathExpressionWithOneInput<int>("i => i * i");
        Assert.AreEqual(25, func(5));
    }

    [TestMethod]
    public void ShouldParseModuloCorrectWithLeftParameterAndRightConstant()
    {
        var func = ExpressionParser.ParseSimpleMathExpressionWithOneInput<int>("i => i % 10");
        Assert.AreEqual(3, func(523));
    }

    [TestMethod]
    public void ShouldParseModuloCorrectWithLeftConstantAndRightParameter()
    {
        var func = ExpressionParser.ParseSimpleMathExpressionWithOneInput<int>("i => 10 % i");
        Assert.AreEqual(1, func(3));
    }

    [TestMethod]
    public void ShouldParseModuloCorrectWithLeftParameterAndRightParameter()
    {
        var func = ExpressionParser.ParseSimpleMathExpressionWithOneInput<int>("i => i % i");
        Assert.AreEqual(0, func(5));
    }
}