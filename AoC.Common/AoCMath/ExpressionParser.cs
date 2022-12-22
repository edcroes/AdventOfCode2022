using System.Linq.Expressions;
using System.Numerics;

namespace AoC.Common.AoCMath;

public static class ExpressionParser
{
    public static Func<T, T> ParseSimpleMathExpressionWithOneInput<T>(this string expression) where T : INumber<T> =>
        expression.ParseSimpleMathExpression<Func<T, T>, T>();

    public static Func<T, T, T> ParseSimpleMathExpressionWithTwoInputs<T>(this string expression) where T : INumber<T> =>
        expression.ParseSimpleMathExpression<Func<T, T, T>, T>();

    private static TFunc ParseSimpleMathExpression<TFunc, T>(this string expression) where T : INumber<T>
    {
        var (parameters, left, @operator, right) = expression.SplitSimpleMathExpression();

        var parameterExpressions = parameters.Select(p => Expression.Parameter(typeof(T), p)).ToArray();

        Expression leftVariable = T.TryParse(left, null, out T? leftValue)
            ? Expression.Constant(leftValue)
            : parameterExpressions.Single(p => p.Name == left);

        Expression rightVariable = T.TryParse(right, null, out T? rightValue)
            ? Expression.Constant(rightValue)
            : parameterExpressions.Single(p => p.Name == right);

        Expression operation = @operator switch
        {
            "+" => Expression.Add(leftVariable, rightVariable),
            "-" => Expression.Subtract(leftVariable, rightVariable),
            "/" => Expression.Divide(leftVariable, rightVariable),
            "*" => Expression.Multiply(leftVariable, rightVariable),
            "%" => Expression.Modulo(leftVariable, rightVariable),
            _ => throw new NotSupportedException($"Operator '{@operator}' is not a supported operator")
        };

        var lambda = Expression.Lambda<TFunc>(operation, parameterExpressions);
        return lambda.Compile();
    }

    private static (string[], string, string, string) SplitSimpleMathExpression(this string expression)
    {
        var (parametersValue, body) = expression.Split("=>", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (parametersValue.IsNullOrWhitespace())
            throw new ArgumentNullException(nameof(expression), "No parameter name was supplied for the input parameter");

        if (body.IsNullOrWhitespace())
            throw new ArgumentNullException(nameof(expression), "No body was supplied");

        var parameters = parametersValue.Trim('(', ')').Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (parameters.Length == 0)
            throw new ArgumentNullException(nameof(expression), "No parameter name was supplied for the input parameter");

        var (left, @operator, right) = body.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (left.IsNullOrWhitespace())
            throw new ArgumentNullException(nameof(expression), "No left argument was specified in the body");

        if (right.IsNullOrWhitespace())
            throw new ArgumentNullException(nameof(expression), "No right argument was specified in the body");

        if (@operator.IsNullOrWhitespace())
            throw new ArgumentNullException(nameof(expression), "No operator was specified in the body");

        return (parameters, left, @operator, right);
    }
}
